//#define LOCAL_TEST

using System;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP.SocketIO;
using System.Net;

public class SocketIOShvv : MonoBehaviour
{
    //[SerializeField]
    // !!! Start() 에서 유니티에디터 Inspector 창에 설정한 값을 다시 변경된다 !!!
    // 그래서 NonSerialized 로 변경
    [NonSerialized]
    public string address = "http://192.168.0.32:3000/socket.io/";

    /// <summary>
    /// The Socket.IO manager instance.
    /// </summary>
    private SocketManager Manager;

    float PreUpdateTime;

    System.DateTime TaxiStart;

    string ClientIpAddress;

    private bool _IsDirectConneting;
    public bool IsDirectConneting
    {
        get { return _IsDirectConneting; }
        set { _IsDirectConneting = value; }
    }


    public System.Action OnConnectSuccessActoin { get; set; }
    public System.Action OnConnectFailActoin { get; set; }
    public System.Action<string> OnPrintDebugMsg { get; set; }

    private static SocketIOShvv _ins = null;
    public static SocketIOShvv ins
    {
        get
        {
            if (_ins == null)
            {
                _ins = FindObjectOfType(typeof(SocketIOShvv)) as SocketIOShvv;
                if (_ins == null)
                {
#if !RELEASE
                    Debug.LogError("Error, Fail to get the SocketIOShvv instance");
#endif
                }
            }
            return _ins;
        }
    }


    // 인스턴스 내용 긁어오기
    public static string GetListContent<T>(List<T> list)
    {
        string outPut = "";
        foreach (T temp in list)
        {
            outPut += ("\n" + JistUtil.PrintClassInfo<T>(temp) + "\n");
        }
        return outPut;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (IsDirectConneting)
        {
            // 이 컴포넌트 활성화시 서버에 접속 한다.
            OnConnect();
        }
#if !UNITY_WEBGL
        //// WebGL 빌드에서는 hostname이 "emscripten"로 나옴
        //// WebGL: 브라우저 스크립트와 상호작용을 참고하자
        //// https://docs.unity3d.com/kr/current/Manual/webgl-interactingwithbrowserscripting.html
        //String hostname = Dns.GetHostName();
        //print($"GetHostEntry({hostname}) returns:");

        //IPHostEntry host = Dns.GetHostEntry(hostname);

        //foreach (IPAddress address in host.AddressList)
        //{
        //    if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        //    {
        //        ClientIpAddress = address.ToString();
        //        print($"    {address}");
        //    }
        //}
#endif
        TaxiStart = System.DateTime.Now;
    }

    void OnDestroy()
    {
        if (this.Manager != null)
        {
            // Leaving this sample, close the socket
            this.Manager.Close();
            this.Manager = null;
        }
    }

    // 서버에 접속하기
    public void OnConnect()
    {
        PrintLog("Connecting...");

        // Create the Socket.IO manager
        Manager = new SocketManager(new Uri(address));

        // 접속 성공후 서버에서 보낸 이벤트메세지를 받을 콜백
        Manager.Socket.On(SocketIOEventTypes.Connect, (s, p, a) =>
        {
            PrintLog("Connected!");
            if (OnConnectSuccessActoin != null) OnConnectSuccessActoin();
            PreUpdateTime = Time.time;
            // 서버에 접속 완료 메세지 보내기(채널 정하기?)
            Manager.Socket.Emit("SmcwSocketStart", $"nowonCoross : {ClientIpAddress}");
        });

        // 접속 종료후 서버에서 보낸 이벤트메세지를 받을 콜백
        Manager.Socket.On(SocketIOEventTypes.Disconnect, (s, p, a) =>
        {
            PrintLog("Disconnected!");

        });
#if LOCAL_TEST
        // 로컬 테스트용 콜백
        Manager.Socket.On("taxiTotal", OnGetTotalTaxi);
#endif
        // 서버에서 받을 이벤트이름에 대한 콜백을 등록한다.
        Manager.Socket.On("SmcwList", OnSmcwList);


        // The argument will be an Error object.
        Manager.Socket.On(SocketIOEventTypes.Error, (socket, packet, args) => {
            Debug.LogError(string.Format("Error: {0}", args[0].ToString()));
            if (OnConnectFailActoin != null) OnConnectFailActoin();
        });
    }

#if LOCAL_TEST
    void OnGetTotalTaxi(Socket socket, Packet packet, params object[] args)
    {
        var data = args[0] as Dictionary<string, object>;

        var taxiData = data["T_DATA"] as string;

        PrintLog(string.Format("{0} taxiData", taxiData));
    }
#endif

    // 
    void OnSmcwList(Socket socket, Packet packet, params object[] args)
    {
        string jsonStirng = args[0].ToString();
        PrintLog(jsonStirng);

        if (NetworkDataManager.Instance.MovingObjectList != null)
        {
            NetworkDataManager.Instance.MovingObjectList.Clear();
            //PacketData.ins.WearWearSensorErrorList = null;
        }
        float elapsedTime = Time.time - PreUpdateTime;
        PreUpdateTime = Time.time;

        NetworkDataManager.Instance.MovingObjectList = JsonUtil.JsonToObject<List<MoveObjectPacket>>(jsonStirng);
        NetworkDataManager.Instance.ParseMoveObjectPacket();
        
        string content = GetListContent<MoveObjectPacket>(NetworkDataManager.Instance.MovingObjectList);
        PrintLog(string.Format("{0} {1}", "OnSmcwList", content));
    }



    public void OnCloseSocketManager()
    {
        this.Manager.Close();
    }


    void SendCheckPacket()
    {
        float gap = 5.0f;
        // gap 초에 한번 갱신
        System.TimeSpan interval = System.DateTime.Now - TaxiStart;
        if (interval.TotalSeconds >= gap)
        {
            TaxiStart = System.DateTime.Now;
            //Manager.Socket.Emit("CheckPacket", "CheckCheck");
            //print($"CheckPacket{TaxiStart}");
        }
    }

 #if LOCAL_TEST
    //클라이언트(프로그래머)가 만든 임시로컬 서버에서 taxiTotal메세지를 받기 위해 서버 메세지를 보냄 
    public void OnTaxiTotal(string textToSend)
    {
        Manager.Socket.Emit("taxiTotal", textToSend);

        PrintLog(string.Format("{0}: {1} : {2}", "Shovvel_Client", "OnTaxi_location_list", textToSend));
    }
#endif

    // Update is called once per frame
    void Update()
    {
        SendCheckPacket();
    }

    void PrintLog(string msg)
    {
        Debug.Log("socketIO shovv: " + msg);
        // 스크롤뷰에 로그찍기
#if USE_TEST_MODE
        if(OnPrintDebugMsg != null)
            OnPrintDebugMsg(msg);
#endif
    }

    //private string textAreaString = "text area";
    //void OnGUI()
    //{
    //    GUI.Label(new Rect(25, 25, 500, 20), textAreaString);
    //}
}
