using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using TMPro;
using DG.Tweening;

public class UILoginBGController : MonoBehaviour
{
    [SerializeField]
    GameObject contents;

    [SerializeField]
    GameObject info;

    [SerializeField]
    TMP_InputField id_if;
    [SerializeField]
    TMP_InputField pw_if;
    [SerializeField]
    TMP_InputField sn_if;       //보안숫자

    [SerializeField]
    Image capcha_img;
    [SerializeField]
    Button capchaReset_btn;
    [SerializeField]
    Image reset_img;
    [SerializeField]
    Sprite[] reset_sps;


    [SerializeField]
    Button login_btn;

    [SerializeField]
    Button exit_btn;

    EventSystem system;

    bool isOn = true;

    const float DURATION = 0.5f;
    const string IDERROR = "아이디를 입력하십시오.";
    const string PWERROR = "비밀번호를 입력하십시오.";
    const string CAPCHAERROR = "보안숫자를 입력하십시오.";

    // Start is called before the first frame update
    void Start()
    {
        system = EventSystem.current;

        capchaReset_btn.onClick.AddListener(MainManager.Instance.UIDownLoadCapchaImg);
        login_btn.onClick.AddListener(LoginAction);
        capchaReset_btn.gameObject.GetComponent<MouseEvent2>().AddEnterAction(delegate { reset_img.sprite = reset_sps[1]; });
        capchaReset_btn.gameObject.GetComponent<MouseEvent2>().AddExitAction(delegate { reset_img.sprite = reset_sps[0]; });
        
        exit_btn.onClick.AddListener(ExitBtnAction);
        
        info.SetActive(false);

        //SetContentsOn(true);
    }

    void Update()
    {
        if (isOn)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                LoginAction();
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Selectable next;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    next = system.currentSelectedGameObject?.GetComponent<Selectable>().FindSelectableOnUp();
                }
                else
                {
                    next = system.currentSelectedGameObject?.GetComponent<Selectable>().FindSelectableOnDown();
                }

                if (next != null)
                {

                    InputField inputfield = next.GetComponent<InputField>();
                    if (inputfield != null) inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret

                    system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
                }
                else
                {
                    id_if.OnPointerClick(new PointerEventData(system));
                }

            }
        }
    }

    void ExitBtnAction()
    {
        //Application.Quit();
        CommonPopup.ins.SetMode(CommonPopup.Mode.CONFIRM);
        CommonPopup.ins.SetText("종료하시겠습니까?");
        CommonPopup.ins.AddMyAction(delegate { Application.Quit(); });
        CommonPopup.ins.ShowPopUp();

    }
    public void SetInfoOn()
    {
        id_if.text = "sp2user1";
        pw_if.text = "sp2user1";
        sn_if.text = "";
        
        MainManager.Instance.UIDownLoadCapchaImg();

        info.SetActive(true);

        JistUtil.Fade(info, 0f, 1f, 1f);

    }

    public void SetContentsOn(bool isImme)
    {
        isOn = true;

        //#if UNITY_EDITOR
        id_if.text = "sp2user1";
        pw_if.text = "sp2user1";
        sn_if.text = "";

        //#else
        //        id_if.text = "";
        //        pw_if.text = "";
        //        sn_if.text = "";
        //#endif

        MainManager.Instance.UIDownLoadCapchaImg();

        contents.SetActive(true);
        if (isImme)
        {
            JistUtil.Fade(contents, 0f, 1f, 0f);
        }
        else
        {
            JistUtil.Fade(contents, 0f, 1f, DURATION);
        }
    }

    public void SetContentsOff(bool isImme)
    {
        isOn = false;
        if (isImme)
        {
            JistUtil.Fade(contents, 1f, 0f, 0f);
            contents.SetActive(false);
        }
        else
        {
            Sequence sq = DOTween.Sequence();
            sq = JistUtil.Fade(contents, 1f, 0f, DURATION);
            sq.OnComplete(delegate { contents.SetActive(false); });
        }
    }
    void LoginAction()
    {
        string id = id_if.text;
        string pw = pw_if.text;
        string captcha = sn_if.text;


        if (id.Trim().Equals(string.Empty))
        {
            CommonPopup.ins.SetMode(CommonPopup.Mode.CONFIRM);
            CommonPopup.ins.SetText(IDERROR);
            CommonPopup.ins.AddMyAction(CommonPopup.ins.HidePopUp);
            CommonPopup.ins.ShowPopUp();
        }
        else if (pw.Trim().Equals(string.Empty))
        {
            CommonPopup.ins.SetMode(CommonPopup.Mode.CONFIRM);
            CommonPopup.ins.SetText(PWERROR);
            CommonPopup.ins.AddMyAction(CommonPopup.ins.HidePopUp);
            CommonPopup.ins.ShowPopUp();
        }
        else if (captcha.Trim().Equals(string.Empty))
        {
            CommonPopup.ins.SetMode(CommonPopup.Mode.CONFIRM);
            CommonPopup.ins.SetText(CAPCHAERROR);
            CommonPopup.ins.AddMyAction(CommonPopup.ins.HidePopUp);
            CommonPopup.ins.ShowPopUp();
        }
        else
        {

            byte[] bytes = Encoding.UTF8.GetBytes(pw);

            // 바이트들을 Base64로 변환
            string base64 = Convert.ToBase64String(bytes);

            MainManager.Instance.LoginAction(id, base64, captcha);

        }
    }

    public void CaptchaSuccess(string base64)
    {
       
        Texture2D tex = new Texture2D(200, 200);
        byte[] b = Convert.FromBase64String(base64);
        tex.LoadImage(b);
        
        capcha_img.sprite = Sprite.Create(tex, new Rect(Vector2.zero, new Vector2(tex.width, tex.height)), new Vector2(0.5f, 0.5f));

        capcha_img.SetNativeSize();


    }

}
