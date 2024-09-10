using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Security.Cryptography.X509Certificates;
//using System.Diagnostics;

public class WebReqHeader
{
    public string name;
    public string value;
    public WebReqHeader(string name_, string value_)
    {
        name = name_;
        value = value_;
    }
}

class WebRequestUtil : SingletonClass<WebRequestUtil>
{
    public List<WebReqHeader> HeaderList = new List<WebReqHeader>();

    // 응답 실패시
    protected List<UnityAction> FailActionList = new List<UnityAction>();
    // 응답은 성공 결과는 에러
    protected List<UnityAction> ResultFailActionList = new List<UnityAction>();
    // 응답은 성공 결과도 성공
    protected List<UnityAction> SuccessActionList = new List<UnityAction>();
    

    string resultStr;

    bool enableBlokingWait = true;
    bool enableErrPopup = true;
    bool enableAutoRetry = true;
    bool _isUsedWebRequestManager = true;

    // 통신중 진행중 아이콘 표시
    public bool EnableBlockingWait { get { return enableBlokingWait; } set { enableBlokingWait = value; } }
    // 요청실패하면 팝업으로 알림
    public bool EnableErrPopup { get { return enableErrPopup; } set { enableErrPopup = value; } }
    // 요청실패하면 자동으로 재시도
    public bool EnableAutoRetry { get { return enableAutoRetry; } set { enableAutoRetry = value; } }

    public string ResultString { get { return resultStr; } }

    // WebRequestManager(Queue를 사용)를 통해 요청한 것인가?
    public bool IsUsedWebRequestManager { get { return _isUsedWebRequestManager; } set { _isUsedWebRequestManager = value; } }


    public WebRequestUtil()
    {
    }

    /// <summary>
    /// 일반적인 네트워크처리가 아닌경우(매니저사용하지않고 독립적으로 사용할때)
    /// </summary>
    /// <param name="useManager">매니저 사용유무 독립적으로 사용하면 false로 </param>
    public WebRequestUtil(bool useManager)
    {
        IsUsedWebRequestManager = useManager;
    }

    public override void Init()
    {
        FailActionList.Clear();
        ResultFailActionList.Clear();
        SuccessActionList.Clear();
        JistUtil.CheckLine();
        resultStr = "";
    }

    public void Post(MonoBehaviour mbh, string myUrl, WWWForm form = null)
    {

        if (enableBlokingWait)
        {
            //BusyWating.ins.ShowWithCount();
        }
        mbh.StartCoroutine(ResquestPost(myUrl, form));
    }

    public void PostJsonForm(MonoBehaviour mbh, string myUrl, string form = null)
    {

        if (enableBlokingWait)
        {
            //BusyWating.ins.ShowWithCount();
        }
        mbh.StartCoroutine(ResquestPost(myUrl, form));
    }

    public void Post(MonoBehaviour mbh, string myUrl, string form = null)
    {

        if (enableBlokingWait)
        {
            //BusyWating.ins.ShowWithCount();
        }
        mbh.StartCoroutine(ResquestPost(myUrl, form));
    }

    public void Get(MonoBehaviour mbh, string myUrl)
    {

        if (enableBlokingWait)
        {
            //BusyWating.ins.ShowWithCount();
        }
        mbh.StartCoroutine(RequestGet(myUrl));
    }


    IEnumerator ResquestPost(string myUrl, WWWForm form = null)
    {
        //JistUtil.CheckLine();
        using (var uwr = UnityWebRequest.Post(myUrl, form))
        //using (var uwr = new UnityWebRequest(myPath, UnityWebRequest.kHttpVerbPOST))
        {
            if (WebRequestManager.ActionQueue.Count > 0 && IsUsedWebRequestManager)
                WebRequestManager.Instance.RequestInProgress = true;

            uwr.SetRequestHeader("Content-Type", "application/json");
            uwr.certificateHandler = new AcceptCeritificates();

            //uwr.uploadHandler = new UploadHandlerRaw(data);
            //uwr.SetRequestHeader("Content-Type", "application/json");
            //JistUtil.CheckLine();
            yield return uwr.SendWebRequest();
            //JistUtil.CheckLine();
            RequestResultProc(uwr);
        }
    }

    IEnumerator ResquestPost(string myUrl, string form = null)
    {
        //JistUtil.CheckLine();
        using (var uwr = UnityWebRequest.Post(myUrl, form))
        //using (var uwr = new UnityWebRequest(myPath, UnityWebRequest.kHttpVerbPOST))
        {
            if (WebRequestManager.ActionQueue.Count > 0 && IsUsedWebRequestManager)
                WebRequestManager.Instance.RequestInProgress = true;

            if (form != "")
            {
                byte[] raw = System.Text.Encoding.UTF8.GetBytes(form);
                uwr.uploadHandler = new UploadHandlerRaw(raw);

            }
            uwr.SetRequestHeader("Content-Type", "application/json");
            uwr.certificateHandler = new AcceptCeritificates();

            foreach (WebReqHeader wr in HeaderList)
            {
                uwr.SetRequestHeader(wr.name, wr.value);
            }

            //JistUtil.CheckLine();
            yield return uwr.SendWebRequest();
            //JistUtil.CheckLine();
            RequestResultProc(uwr, form);
        }
    }

    IEnumerator RequestGet(string myUrl)
    {
        //JistUtil.CheckLine();
        using (var uwr = UnityWebRequest.Get(myUrl))
        {
            if (WebRequestManager.ActionQueue.Count > 0 && IsUsedWebRequestManager)
                WebRequestManager.Instance.RequestInProgress = true;

            //uwr.uploadHandler = new UploadHandlerRaw(data);
            uwr.SetRequestHeader("Content-Type", "application/json");
            uwr.certificateHandler = new AcceptCeritificates();
            
            foreach (WebReqHeader wr in HeaderList)
            {
                uwr.SetRequestHeader(wr.name, wr.value);
            }
            //JistUtil.CheckLine();
            // 타임아웃 테스트
            //uwr.timeout = 2;
            yield return uwr.SendWebRequest();
            //JistUtil.CheckLine();

            RequestResultProc(uwr);
        }
    }

    void RequestResultProc(UnityWebRequest uwr, string form = null)
    {
        //응답을 받지 못한 실패
        if (uwr.result == UnityWebRequest.Result.ConnectionError
                || uwr.result == UnityWebRequest.Result.ProtocolError
                )
        {
            if (WebRequestManager.Instance.curLoginInfo != null &&  uwr.responseCode.Equals(401))
            {
                //토큰만료
                //토큰 갱신 필요
                WebRequestItemPool.Instance.RequestTokenRefresh();
            }
            else
            {
                //토큰 갱신 이외의 문제

                DebugScrollView.Instance.Print("Response Error!  " + uwr.url + "  :  " + form);
                UnityEngine.Debug.LogError(uwr.error + " : " + uwr.url);
                JistUtil.CheckLine();


                string errorMessage = WebRequestManager.Instance.GetErrorMessage(uwr);
                CommonPopup.ins.ShowPopUp();
                CommonPopup.ins.SetText(errorMessage);


                CommonPopup.ins.SetMode(CommonPopup.Mode.CONFIRM);
                CommonPopup.ins.AddMyAction(delegate
                {
                    CommonPopup.ins.HidePopUp();
                    Init();
                });

            }


        }// 응답 성공
        else
        {
            JistUtil.CheckLine();
            DebugScrollView.Instance.Print("Response Success!  " + uwr.url);
            DebugScrollView.Instance.Print(uwr.downloadHandler.text);
            try
            {
                bool result = WebRequestManager.Instance.SuccessRequest(uwr.downloadHandler.text);
                if (result)
                {
                    resultStr = uwr.downloadHandler.text;
                    // 성공시 액션
                    JistUtil.instance.EventCall(SuccessActionList);
                }
                else
                {
                    WebRequestManager.Instance.FinishActionQ();
                }
                Init();

               
            }
            catch(JsonReaderException e)
            {
                //결과가 json이 아닐때
                resultStr = uwr.downloadHandler.text;
                // 성공시 액션
                JistUtil.instance.EventCall(SuccessActionList);


                if (IsUsedWebRequestManager)
                {
                    WebRequestManager.Instance.FinishActionQ();
                }
                Init();
            }

            
        }
        HeaderList.Clear();
        
        uwr.Dispose();

        //JistUtil.CheckLine();

        if (enableBlokingWait)
        {
            //BusyWating.ins.HideWithCount();
        }
    }

    public void AddFailAction(UnityAction act)
    {
        JistUtil.instance.AddMyAction(act, FailActionList);
    }

    public void AddConnectResultErrorAction(UnityAction act)
    {
        JistUtil.instance.AddMyAction(act, ResultFailActionList);
        JistUtil.CheckLine("ResultFailActionList.Count " + ResultFailActionList.Count.ToString() + " "+ IsUsedWebRequestManager);
    }

    public void AddSuccessAction(UnityAction act)
    {
        JistUtil.instance.AddMyAction(act, SuccessActionList);
    }

    public void NoTryPrePopup(GameObject content)
    {
        // 응답실패
        AddFailAction(delegate { content.SetActive(false); });
        AddFailAction(delegate { CommonPopup.ins.PopPopupStack(); });
        // 응답결과 에러
        AddConnectResultErrorAction(delegate { content.SetActive(false); });
        AddConnectResultErrorAction(delegate { CommonPopup.ins.PopPopupStack(); });
    }


    public void Upload(MonoBehaviour mbh, string from, string to)
    {

        if (enableBlokingWait)
        {
            //BusyWating.ins.ShowWithCount();
        }
        mbh.StartCoroutine(UploadFileData( from, to));
    }

    // https://docs.unity3d.com/ScriptReference/Networking.UploadHandlerFile.html

    IEnumerator UploadFileData(string from, string to)
    {
        using (var uwr = new UnityWebRequest(to, UnityWebRequest.kHttpVerbPUT))
        {
            uwr.uploadHandler = new UploadHandlerFile(from/*"/path/to/file"*/);
            yield return uwr.SendWebRequest();
            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
                UnityEngine.Debug.LogError(uwr.error);
            else
            {
                // file data successfully sent
                UnityEngine.Debug.Log("Upload Success !");
            }
        }
    }
}
public class AcceptCeritificates : CertificateHandler
{
    private static string PUB_KEY;

    protected override bool ValidateCertificate(byte[] certificateData)
    {

        //X509Certificate2 certificate = new X509Certificate2(certificateData);
        //string pk = certificate.GetPublicKeyString();
        //if (pk.Equals(PUB_KEY))
        //    return true;
        //return false;

        return true;

    }
}
