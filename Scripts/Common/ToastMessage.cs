using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToastMessage : SingletonMonoBehaviour<ToastMessage>
{
    GameObject Contents;
    Tween ToastTween;
    public void SetToastMessage(string content, Color col)
    {
        if (ToastTween != null)
        {
            ToastTween.Kill();
        }
        Contents.SetActive(true);
        Text tempText = Contents.GetComponentInChildren<Text>();
        Image bg = Contents.GetComponentInChildren<Image>();
        bg.color = Color.white;
        Contents.GetComponentInChildren<Text>().text = content;
        tempText.color = col;
        ToastTween = JistUtil.Fade(Contents, 1.0f, 0.0f, 1.0f, 3.0f);
        ToastTween.OnComplete(delegate { Contents.SetActive(false); });
    }

    private void Awake()
    {
        Contents = CommonUtility.FindChildObject("Content", transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
