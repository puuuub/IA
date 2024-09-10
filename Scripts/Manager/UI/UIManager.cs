using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMonoBehaviour<UIManager>
{

    //UI 전체 Manager

    //로그인 화면
    [SerializeField]
    UILoginBGController uiLoginController;

    //상단 아이콘들 위주(+ 이벤트 리스트 panel, 장비 타입별 POI 리스트 panel)
    [SerializeField]
    UITopContoller uiTopController;

    //왼쪽 구역, 메뉴 선택
    [SerializeField]
    UISideMenuManager uiSideMenuManger; //layer 포함

    //전체 POI
    [SerializeField]
    UIPOIManager uiPoiManager;

    //장비 클릭 - 상세 창
    [SerializeField]
    UIDevicePopupController uiDevicePopupController;


    //하단 중앙3개 메뉴
    [SerializeField]
    UIBottomPanelController uiBottomPanelController;

    //하단 우측 구역별 운영 상태 Panel
    [SerializeField]
    UIDeviceChartPanelController uiDeviceChartPanelController;

    //이벤트 팝업
    [SerializeField]
    UIEventPopupPanelController uiEventPopupController;


    [SerializeField]
    Canvas uiCanvas;
    public Canvas UICanvas { get { return uiCanvas; } }
    // Start is called before the first frame update
    void Start()
    {
        uiDevicePopupController.SetPopupOffClickAction(MainManager.Instance.DevicePopupOffClickAction);
        uiDevicePopupController.SetPopupRestartClickAction(MainManager.Instance.DevicePopupRestartClickAction);

        uiTopController.SetPOIListAllToggle(SetPOIListAllAction);
        uiTopController.SetPOIListRowAction(SetPOIListRowClickAction);
    }


    public void SetDevicePanelOn(DeviceType type, DeviceResources res)
    {
        uiDevicePopupController.SetContentsOn(type, res);
        
    }
    public void SetDevicePanelOff()
    {
        uiDevicePopupController.SetContentsOff();
        MainManager.Instance.SetSelectedDeiveId(StaticText.EMPTY);
        ObjectManager.Instance.SetPopupDeviceOutlineOff();
    }
   
    public void LogoutAction()
    {
        uiLoginController.SetContentsOn(false);

        uiTopController.SetContentsOnOff(false);
        uiTopController.SetTitlePos(true);
        uiTopController.SetLogout();

        uiSideMenuManger.SetLogout();


        uiBottomPanelController.SetContentsOnOff(false);


        uiDeviceChartPanelController.SetContentsOnOff(false);
    }
    public void SuccessCaptcha(string base64)
    {
        uiLoginController.CaptchaSuccess(base64);
    }

    public void LoginSuccess(string area)
    {
        uiLoginController.SetContentsOff(false);


        uiSideMenuManger.SetLogin();

        uiTopController.SetContentsOnOff(true);
        uiTopController.SetTitlePos(false);
        uiTopController.SetAreaText(area);

        
        uiBottomPanelController.SetContentsOnOff(true);

    }

    public void SetTopId(string txt)
    {
        uiTopController.SetIdText(txt);
    }
    public void SetArea(AIRPORTAREA area)
    {
        uiTopController.SetAreaText(area.ToString());

        uiSideMenuManger.SetSideMenu(area);
        uiPoiManager.SetPOIAreaRoot(area);

        SetDevicePanelOff();
        SetPOIMenuAll();
    }

    public void SetSideMenuSubContents(AIRPORTAREA area, AIRPORTAREA2 sideMenu, List<SUBAREA> subList, List<SUBAREA> subInteractList)
    {
        uiSideMenuManger.SetSubContentsList(sideMenu, subList, subInteractList);
        SetPOIMenuOn(area, sideMenu);
    }
    public void SetTopArea(string area)
    {
        uiTopController.SetAreaText(area);
    }
    public void SetPOIOff()
    {
        uiPoiManager.SetPoiRootOnOff(false);
    }
    public void SetPOIMenuOn(AIRPORTAREA area, AIRPORTAREA2 sideMenu)
    {
        uiPoiManager.SetPOIMenuRoot(MainManager.Instance.curArea, MenuTree.GetDeviceTypes(area, sideMenu));
    }
    public void SetPOIListPanel(AIRPORTAREA area, AIRPORTAREA2 sideMenu)
    {
        List<DeviceType> targetTypes = MenuTree.GetDeviceTypes(area, sideMenu);
        if (targetTypes != null)
        {
            uiTopController.SetPOIListPanel(targetTypes.ConvertAll(x => (int)x));
        }
        else
        {
            uiTopController.SetPOIListPanel(null);
        }
    }
    public void SetPOIMenuAll()
    {
        uiPoiManager.SetPOIMenuRoot(MainManager.Instance.curArea, MenuTree.GetDeviceTypes(MainManager.Instance.curArea, AIRPORTAREA2.NONE));
    }

    public void SetSplitScreenOnOff(bool isOn)
    {
        uiBottomPanelController.SetDashboardMINIOnOff(isOn);
        uiTopController.LogoAction();
        uiTopController.SetContentsOnOff(!isOn);

        uiSideMenuManger.SetContentsOnOff(!isOn);
        uiBottomPanelController.SetContentsOnOff(!isOn);
        uiDeviceChartPanelController.SetContentsOnOff(!isOn);

        uiPoiManager.SetPoiSplitOnOff(isOn);
    }

   
    public void DeviceChartPanelOn(string orgId)
    {
        int idx;
        if (orgId.Equals(StaticText.T1Code))
        {
            idx = 0;
        }
        else if (orgId.Equals(StaticText.T2Code))
        {
            idx = 2;
        }
        else
        {
            idx = 1;
        }

        float percent = DataManager.Instance.GetOperationDevicePercent(orgId);

        uiDeviceChartPanelController.SetContentsOn(idx, percent);
    }

    void SetPOIListAllAction(bool x)
    {
        if (x)
        {
            SetPOIMenuOn(MainManager.Instance.curArea, MainManager.Instance.curMenu);
        }
    }

    public void SetPOIListRowClickAction(DeviceType type)
    {
        
        uiPoiManager.SetPOIMenuRoot(MainManager.Instance.curArea, type);
    }
    public void FirstSetDashBoard()
    {
        uiBottomPanelController.SetDashBoardToggle(uiBottomPanelController.uiDashBoardController, DeviceType.BRT);
        uiBottomPanelController.SetDashBoardToggle(uiBottomPanelController.uiDashBoardMINIController, DeviceType.BRT);
    }

    public void ReceiveEvent()
    {

        var eventData = DataManager.Instance.curEventData;

        string dateTime = eventData.timeCreated.Replace("T", StaticText.ENTER);
        dateTime = dateTime.Substring(0, eventData.timeCreated.IndexOf('.'));


        string[] row_txts = new string[4];
        row_txts[0] = dateTime;
        row_txts[1] = eventData.organizationCodeL2 + StaticText.EMPTY + eventData.organizationName;
        row_txts[2] = eventData.deviceType;
        row_txts[3] = DataManager.Instance.GetEventTypeKor(eventData.eventTag);
        uiTopController.AddEventRow(row_txts, delegate { EventManager.Instance.EventAction(eventData); });


        string[] con_txts = new string[4];
        con_txts[0] = DataManager.Instance.GetEventTypeKor(eventData.eventTag);
        con_txts[1] = eventData.organizationCodeL2 + StaticText.EMPTY + eventData.organizationName + StaticText.EMPTY + eventData.deviceType;
        con_txts[2] = dateTime.Replace(StaticText.ENTER, StaticText.EMPTY);
        uiEventPopupController.SetContentsOn(con_txts, delegate { EventManager.Instance.EventAction(eventData); });

    }

    public bool GetDevicePopupActive()
    {
        return uiDevicePopupController.isActive;
    }

    public void SetDevicePopupResClickAction(DeviceType type, int idx)
    {
        Color col = DataManager.Instance.GetResourcesColor(idx);
        
        ObjectManager.Instance.SetPopupDeviceOutlineOn(type, idx, col);
    }

    public void SetDevicePopupResOffAction(DeviceType type)
    {
        ObjectManager.Instance.SetPopupDeviceOutlineOff(type);
    }
}
