using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MyDebugData
{
    public string URL;
    public string SOS_URL;
    public string SOSOff_Url;
    public string HRRange_URL;
    public string MqttURL;
}

[DefaultExecutionOrder(-1)]
public class DebugScrollView : SingletonMonoBehaviour<DebugScrollView>
{
    public static Text DebugText;
    public static RectTransform DebugTextParentRectTransForm;
    public static Scrollbar DebugScrollbarVertical;
    public GameObject ContentRoot;

    public InputField TestInput;
    public InputField SOSURLInput;
    public InputField SOSOffURLInput;
    public InputField HRRangeURLInput;
    public InputField MqttURLInput;

    private void Awake()
    {
        ContentRoot = CommonUtility.FindChildObject("ContentRoot", transform);
        GameObject scrollView = CommonUtility.FindChildObject("Scroll View", transform);
        DebugText = CommonUtility.FindChildObject("Text", scrollView.transform).GetComponent<Text>();
        DebugTextParentRectTransForm = DebugText.transform.parent.GetComponent<RectTransform>();

        DebugScrollbarVertical = CommonUtility.FindChildObject("Scrollbar Vertical", scrollView.transform).GetComponent<Scrollbar>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //if (SceneManager.GetActiveScene().name == "loginTest")
        //{
        //    WebGLWrapper.Instance.GetWebGLUserAgent();
        //    JistUtil.CheckLine(Application.platform.ToString());
        //    if (WebGLWrapper.Instance.IsMobile())
        //    {
        //        UIManager.Instance.ZoomScrollbar.gameObject.SetActive(false);
        //    }
        //}
        //SocketIOShvv.ins.OnPrintDebugMsg += OnPrintConnectionDebug;
#if !USE_DEBUG_SCROLL_VIEW
        GetComponent<Lean.Touch.LeanFingerTap>().enabled = false;
        //gameObject.SetActive(false);
#endif
    }



    // Update is called once per frame
    void Update()
    {
        if (ContentRoot.activeSelf && Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {

            if (TestInput != null &&  TestInput.text != "")
            {
                MyDebugData data = new MyDebugData();
                //data.URL = TestInput.text;
                //DemoController.Instance.MyUrl = data.URL;
                
                //data.SOS_URL = SOSURLInput.text;
                //DemoController.Instance.SOSUrl = data.SOS_URL;

                //data.SOSOff_Url = SOSOffURLInput.text;
                //DemoController.Instance.SOSOffUrl = data.SOSOff_Url;

                //data.HRRange_URL = HRRangeURLInput.text;
                //DemoController.Instance.HRRangeUrl = data.HRRange_URL;

                //data.MqttURL = MqttURLInput.text;
                //DemoController.Instance.HRRangeUrl = data.MqttURL;

                JsonUtil.SaveJsonData(data, "MyData");
            }
        }
    }

    public void ChangeActivate()
    {
        ContentRoot.gameObject.SetActive(!ContentRoot.gameObject.activeSelf);
    }

    //public void OnPrintConnectionDebug(string msg)
    //{
    //    int fontSize = DebugText.fontSize;
    //    DebugText.text += "\n" + msg;
    //    // 줄넘김 체크
    //    string[] separatingStrings = { "\n" };
    //    string[] lines = msg.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
    //    int lineCnt = lines.Length;
   
    //    Vector2 size = DebugText.rectTransform.sizeDelta;
    //    Vector2 newSize = new Vector2(size.x, size.y + fontSize * lineCnt);
    //    DebugText.rectTransform.sizeDelta = newSize;
    //    DebugTextParentRectTransForm.sizeDelta = newSize;
    //    DebugScrollbarVertical.value = 0;
    //}

    static void OnPrintConnectionDebug(string msg)
    {
        int fontSize = DebugText.fontSize;
        if (msg.Length > 500)
            msg = msg.Substring(0, 500);

        // 줄넘김 체크
        string[] separatingStrings = { "\n" };
        string[] lines = msg.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
        int lineCnt = lines.Length;

        int totalLineCnt = DebugText.text.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries).Length;

  
        // 100라인 넘으면 위의 라인 지움 -> Text UI texture 사이즈 제한 오류 막기
        if (totalLineCnt > 50)
        {
            for (int k = 0; k < lineCnt; k++)
            {
                int deletEnd = DebugText.text.IndexOf("\n", 0, DebugText.text.Length);
                DebugText.text = DebugText.text.Substring(deletEnd + 1, DebugText.text.Length - deletEnd - 1);
            }
        }
        else
        {
            Vector2 size = DebugText.rectTransform.sizeDelta;
            Vector2 newSize = new Vector2(size.x, size.y + fontSize * lineCnt);
            DebugText.rectTransform.sizeDelta = newSize;
            DebugTextParentRectTransForm.sizeDelta = newSize;
            DebugScrollbarVertical.value = 0;
        }

        DebugText.text += "\n" + msg;
    }

    public void Print(string msg)
    {
#if UNITY_EDITOR
        Debug.Log(msg);
#elif USE_MONO_PRINT
        print(msg);
#endif
#if USE_DEBUG_SCROLL_VIEW
        OnPrintConnectionDebug(msg);
#endif
    }

    public static void PrintEx(string msg)
    {
#if UNITY_EDITOR
        Debug.Log(msg);
#elif USE_MONO_PRINT
        print(msg);
#endif
#if USE_DEBUG_SCROLL_VIEW
        OnPrintConnectionDebug(msg);
#endif
    }
}
