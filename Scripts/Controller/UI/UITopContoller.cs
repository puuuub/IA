using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;


public class UITopContoller : MonoBehaviour
{
    [SerializeField]
    GameObject logo;
    [SerializeField]
    Button logo_btn;

    [SerializeField]
    GameObject title;


    [SerializeField]
    GameObject contents;    //logo제외 로그인 화면에서 사용해서


    [SerializeField]
    GameObject area;
    [SerializeField]
    TMP_Text area_txt;

    [SerializeField]
    GameObject id;
    [SerializeField]
    TMP_Text id_txt;

    [SerializeField]
    Button split_btn;


    
    [SerializeField]
    Toggle event_tg;
    //이벤트 리스트 panel
    [SerializeField]
    UIEventPanelController uiEventPanelController;


    [SerializeField]
    Toggle list_tg;
    //장비 타입별 POI 리스트 panel
    [SerializeField]
    UIPOIListPanelController uipoiListPanelController;



    [SerializeField]
    Toggle setting_tg;

    [SerializeField]
    Button exit_btn;


    float titlePosX = 240f;
    // Start is called before the first frame update
    void Start()
    {
        logo_btn.onClick.AddListener(LogoAction);

        exit_btn.onClick.AddListener(ExitBtnAction);

        list_tg.onValueChanged.AddListener(uipoiListPanelController.SetContentsOnOff);
        event_tg.onValueChanged.AddListener(uiEventPanelController.SetContentsOnOff);
        split_btn.onClick.AddListener(SplitButtonAction);

        uiEventPanelController.AddMouseOverAction(delegate { CameraManager.Instance.SetRootMoveOnOff(false); }, 
            delegate { CameraManager.Instance.SetRootMoveOnOff(true); });

        SetContentsOnOff(false);
        SetTitlePos(true);
    }

    public void LogoAction()
    {
        if (!UISideMenuManager.Instance.LayerInvoke(0)) 
        {
            MainManager.Instance.SetArea(AIRPORTAREA.ALL);
        }
    }
    public void SetLogout()
    {
        uiEventPanelController.ResetEvnetRow();

        event_tg.isOn = false;
        list_tg.isOn = false;
        
        LogoAction();

    }

    void ExitBtnAction()
    {
        //Application.Quit();
        CommonPopup.ins.SetMode(CommonPopup.Mode.YES_NO);
        CommonPopup.ins.SetText("종료하시겠습니까?");
        CommonPopup.ins.SetYesButtonText("로그아웃");
        CommonPopup.ins.AddYesAction(delegate {
            MainManager.Instance.LogoutAction();
            CommonPopup.ins.HidePopUp();
        });
        CommonPopup.ins.SetNoButtonText("종료");
        CommonPopup.ins.AddNoAction(delegate { Application.Quit(); });
        CommonPopup.ins.ShowPopUp();


    }

    void SplitButtonAction()
    {
        MainManager.Instance.SetSplitScreenOnOff(true);
    }

    public void SetContentsOnOff(bool isOn)
    {
        contents.SetActive(isOn);
    }

    public void SetAreaText(string text)
    {
        area_txt.text = text;

    }
    public void SetIdText(string text)
    {
        id_txt.text = text;
    }

    public void SetTitlePos(bool isLogin)
    {
        if (!isLogin)
        {
            title.transform.SetParent(contents.transform);
            title.transform.localPosition = Vector3.zero;
        }
        else
        {
            title.transform.SetParent(logo.transform);
            
            Vector3 temp = title.transform.localPosition;
            temp.x = titlePosX;
            title.transform.localPosition = temp; 
        }
    }

    public void SetPOIListPanel(List<int> list)
    {
        uipoiListPanelController.SetUnused(list);
    }

    public void SetPOIListAllToggle(UnityAction<bool> act)
    {
        uipoiListPanelController.all_act = act;
    }

    public void SetPOIListRowAction(UnityAction<DeviceType> act)
    {
        uipoiListPanelController.rowAction = act;
    }
    public void AddEventRow(string[] txts, UnityAction act)
    {
        uiEventPanelController.AddEventRow(txts, act);
    }

    
}
