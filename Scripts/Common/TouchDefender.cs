using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TouchDefender : MonoBehaviour
{
    private static TouchDefender _ins = null;

    public static TouchDefender ins
    {
        get
        {
            if (_ins == null)
            {
                _ins = FindObjectOfType(typeof(TouchDefender)) as TouchDefender;
                if (_ins == null)
                {
#if !RELEASE
                    Debug.LogError("Error, Fail to get the TouchDefender instance");
#endif
                }
            }
            return _ins;
        }
    }

    public GameObject Content;
    GameObject BgObj;
    GameObject Bg3DObj;
    Color BaseColor;

    public bool IsOn { get; private set; }
    
    public void FadeIn(float duration, bool blocking = false)
    {
        SetEnable(true);
        //Bg3DObj.GetComponent<Image>().color = Color.black;
        Tween t = Bg3DObj.GetComponent<Image>().DOBlendableColor(new Color(0.0f, 0.0f, 0.0f, 0.0f), duration);
        t.OnComplete(delegate { Bg3DObj.GetComponent<Image>().color = BaseColor; SetEnable(blocking); });
    }

    public void FadeIn(float duration, TweenCallback action)
    {
        SetEnable(true);
        //Bg3DObj.GetComponent<Image>().color = Color.black;
        Tween t = Bg3DObj.GetComponent<Image>().DOBlendableColor(new Color(0.0f, 0.0f, 0.0f, 0.0f), duration);
        t.OnComplete(action);
    }

    public void FadeOut(float duration)
    {
        SetEnable(true);
        Bg3DObj.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        Tween t = Bg3DObj.GetComponent<Image>().DOBlendableColor(new Color(0.0f, 0.0f, 0.0f, 1.0f), duration);
        //t.OnComplete(delegate { Bg3DObj.GetComponent<Image>().color = baseCol; });
    }

    public void FadeOut(float duration, TweenCallback action)
    {
        SetEnable(true);
        Bg3DObj.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        Tween t = Bg3DObj.GetComponent<Image>().DOBlendableColor(new Color(0.0f, 0.0f, 0.0f, 1.0f), duration);
        t.OnComplete(action);
    }

    public void SetNoBlockingState()
    {
        Bg3DObj.GetComponent<Image>().color = BaseColor; 
        SetEnable(false);
    }

    private void Awake()
    {
        Content = CommonUtility.FindChildObject("Content", transform);
        BgObj = CommonUtility.FindChildObject("Bg", transform);
        Bg3DObj = CommonUtility.FindChildObject("Bg3D", transform);
        BaseColor = Bg3DObj.GetComponent<Image>().color;
        Bg3DObj.GetComponent<Image>().color = new Color(0.1372549f, 0.1215686f, 0.1254902f, 1.0f);
        //SetEnable(true);

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void SetEnable(bool enable)
    {
        IsOn = enable;
        Content.SetActive(enable);
        //Bg3DObj.SetActive(enable);
    }

    public void SetEnableDark(bool enable)
    {
        BgObj.SetActive(enable);
    }
}
