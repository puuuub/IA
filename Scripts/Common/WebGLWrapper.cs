using UnityEngine;
using System.Runtime.InteropServices;

public class WebGLWrapper : SingletonMonoBehaviour<WebGLWrapper>
{
#if ((!UNITY_EDITOR) && (UNITY_WEBGL))
    [DllImport("__Internal")]
    private static extern void LoadWebView(string str);

    [DllImport("__Internal")]
    private static extern void OpenWindow(string url);

    [DllImport("__Internal")]
    private static extern bool IsMobileDevice();

    [DllImport("__Internal")]
    private static extern string GetUserAgent();
#endif

    //[DllImport("__Internal")]
    //private static extern void Hello();

    //[DllImport("__Internal")]
    //private static extern void HelloString(string str);

    //[DllImport("__Internal")]
    //private static extern void PrintFloatArray(float[] array, int size);

    //[DllImport("__Internal")]
    //private static extern int AddNumbers(int x, int y);

    //[DllImport("__Internal")]
    //private static extern string StringReturnValueFunction();

    //[DllImport("__Internal")]
    //private static extern void BindWebGLTexture(int texture);

    bool isiPhone;

    public void OpenUrl(string url)
    {
#if ((!UNITY_EDITOR) && (UNITY_WEBGL))
        OpenWindow(url);
#else
        Application.OpenURL(url);
#endif
    }

    // MonoBehaviour의 Start 이후의 메소드에서 호출 Awake 에서 호출시 WebGL빌드에서 에러 팝업 메세지 창 뜸
    public bool IsMobile()
    {
#if ((!UNITY_EDITOR) && (UNITY_WEBGL))
        JistUtil.CheckLine();
        bool ret = IsMobileDevice();
        JistUtil.CheckLine(ret.ToString());
        return ret;
#elif UNITY_ANDROID || UNITY_IOS
        return true;
#else
        return false;
#endif
    }

    public bool IsiPhone()
    {
        return isiPhone;
    }


    public string GetWebGLUserAgent()
    {
#if ((!UNITY_EDITOR) && (UNITY_WEBGL))
        JistUtil.CheckLine();
        string ret =  GetUserAgent();
        JistUtil.CheckLine(ret);
        return ret;
#else
        return "";
#endif
    }

    void Start()
    {
        string ag = GetWebGLUserAgent();
        isiPhone = ag.ToLower().Contains("iphone");

        //Hello();

        //HelloString("This is a string.");

        //float[] myArray = new float[10];
        //PrintFloatArray(myArray, myArray.Length);

        //int result = AddNumbers(5, 7);
        //Debug.Log(result);

        //Debug.Log(StringReturnValueFunction());

        //var texture = new Texture2D(0, 0, TextureFormat.ARGB32, false);
        //BindWebGLTexture(texture.GetNativeTextureID());
    }
}