using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : SingletonMonoBehaviour<MainManager>
{

    [SerializeField]
    UIManager uiManager;

    [SerializeField]
    CameraManager cameraManager;


    [SerializeField]
    ObjectManager objectManager;

    [SerializeField]
    DataManager dataManager;

    [SerializeField]
    WebSocketManager websocketManager;

    [SerializeField]
    TokenRefreshController tokenRefreshController;

    [SerializeField]
    EventManager eventManager;


    //기본 구조 
    //AIRPORTAREA - AIRPORTAREA2 -SUBAREA
    //T1,T2,CA,ALL - CKI,DEP,TRS,GTE - SUBAREA
    public AIRPORTAREA curArea;
    public AIRPORTAREA2 curMenu;
    public SIDEMENUKOR sideMenuKor = SIDEMENUKOR.체크인;

    public bool isActive;

    public const float HIDEDURATION = 0.5f;

    [SerializeField]
    string selectedDeviceId;

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        cameraManager.SetCineAllOff();
        
        SetArea(AIRPORTAREA.ALL);
    }


    
    public void LoginAction(string id, string pw, string captcha)
    {
        WebRequestItemPool.Instance.RequestPostLogin(id, pw, captcha);
    }

    public void UIDownLoadCapchaImg()
    {
        WebRequestItemPool.Instance.RequestCaptcha();
    }

    public void LogoutAction()
    {
        //전체뷰, 전체 셋팅   
        SetArea(AIRPORTAREA.ALL);

        tokenRefreshController.EndTokenRefresh();

        websocketManager.SetDisconnect();

        cameraManager.SetRootMoveOnOff(false);

        uiManager.LogoutAction();
        isActive = false;

    }


    public void SetArea(AIRPORTAREA area)
    {
        curArea = area;
        uiManager.SetArea(area);
        
        cameraManager.SetDevicePopupCameraOnOff(false);

        cameraManager.SetVirCam(area);
        cameraManager.SetRootMoveOnOff(true);
        uiManager.SetPOIListPanel(curArea, curMenu);

    }

    public void SuccessCaptcha(string base64)
    {
        uiManager.SuccessCaptcha(base64);
    }
    public void SuccessLogin()
    {
        isActive = true;

        //WebRequestItemPool.Instance.RequestDeviceInfo();
        WebRequestItemPool.Instance.RequestAccountInfo();

        dataManager.ResetDevicePlaceDic();
        WebRequestItemPool.Instance.RequestDevicePlace(StaticText.T1Code);
        WebRequestItemPool.Instance.RequestDevicePlace(StaticText.T2Code);
        WebRequestItemPool.Instance.RequestDevicePlace(StaticText.CACode);

        //CO = CA 데이터는 CO 표출은 CA
        WebRequestItemPool.Instance.RequestDeviceDoorCount("T1");
        WebRequestItemPool.Instance.RequestDeviceDoorCount("T2");
        WebRequestItemPool.Instance.RequestDeviceDoorCount("CO");

        //dataManager.ResetDeviceErrorTop();
        //WebRequestItemPool.Instance.RequestDeviceErrorTop(DeviceType.BDG.ToString(), DateTime.Now.AddDays(-1).ToString(StaticText.DATEFORMAT_0));
        //WebRequestItemPool.Instance.RequestDeviceErrorTop(DeviceType.BEG.ToString(), DateTime.Now.AddDays(-1).ToString(StaticText.DATEFORMAT_0));
        //WebRequestItemPool.Instance.RequestDeviceErrorTop(DeviceType.SBG.ToString(), DateTime.Now.AddDays(-1).ToString(StaticText.DATEFORMAT_0));
        //WebRequestItemPool.Instance.RequestDeviceErrorTop(DeviceType.DPG.ToString(), DateTime.Now.AddDays(-1).ToString(StaticText.DATEFORMAT_0));

        WebRequestItemPool.Instance.RequestDeviceInfoList();

        

        tokenRefreshController.StartTokenRefresh();

        websocketManager.WebsocketConnect();

        uiManager.LoginSuccess(curArea.ToString());
        //상단 id로 세팅
        //uiManager.SetTopId(WebRequestManager.Instance.curLoginInfo.id);
        
        cameraManager.SetRootMoveOnOff(true);

    }
    public void SuccessAccountInfo(string apiResult)
    {
        WebRequestManager.Instance.SetAccountInfo(apiResult);
        
        //상단 유저이름으로 세팅
        uiManager.SetTopId(WebRequestManager.Instance.accountInfo.userName);
    }


    public void SuccessDevicePlace(string orgId, string apiResult)
    {
        dataManager.SetDevicePlaceDic(orgId, apiResult);
        uiManager.DeviceChartPanelOn(orgId);
    }

    

    public void SuccessDeviceInfoList(string apiResult)
    {
        dataManager.SetDeviceInfoList(apiResult);
    }
    public void SuccessDeviceDoorCount(string orgId, string apiResult)
    {
        dataManager.SetDeviceDoorDic(orgId, apiResult);
    }

    public void SuccessDeviceInfo(string apiResult)
    {
        dataManager.SetDeviceInfo(apiResult);
    }
    public void SelectSideMenu(AIRPORTAREA2 sideMenu)
    {
        curMenu = sideMenu;
        List<SUBAREA> subList;

        switch (curMenu)
        {
            case AIRPORTAREA2.CKI:
                subList = MenuTree.GetCKISubArea(curArea);
                break;
            case AIRPORTAREA2.DEP:
                subList = MenuTree.GetDEPSubArea(curArea);
                break;
            case AIRPORTAREA2.GTE:
                subList = MenuTree.GetGTESubArea(curArea);
                break;
            default:
                subList = new List<SUBAREA>();
                break;
        }

        uiManager.SetSideMenuSubContents(curArea, sideMenu, subList, MenuTree.GetSubInteractList(curArea));
        uiManager.SetTopArea(curArea.ToString() + StaticText.BAR + (sideMenuKor + (int)curMenu).ToString());
        
        uiManager.SetDevicePanelOff();
        cameraManager.SetDevicePopupCameraOnOff(false);

        uiManager.SetPOIListPanel(curArea, curMenu);

        cameraManager.SetVirCam(curArea, curMenu);
        cameraManager.SetRootMoveOnOff(true);
        cameraManager.SetRaycastOnOff(false);

        objectManager.SetDeviceOutlineInter();

    }

    public void SelectSideMenuSubRow(SUBAREA area)
    {
        //사이드 서브메뉴 선택
        Debug.Log(area.ToString());
        cameraManager.SetVirCam(curArea, area);
        cameraManager.SetRaycastOnOff(false);
        cameraManager.SetRootMoveOnOff(true);

        uiManager.SetPOIMenuOn(curArea, curMenu);
        uiManager.SetPOIListPanel(curArea, curMenu);
        uiManager.SetTopArea(curArea.ToString() + StaticText.BAR + (sideMenuKor + (int)curMenu).ToString() + StaticText.BAR + area.ToString());
        uiManager.SetDevicePanelOff();
        cameraManager.SetDevicePopupCameraOnOff(false);


        objectManager.SetDeviceOutlineInter();

    }

    public void SideMenuSubAllOff()
    {
        //사이드 서브메뉴 선택 없을때
        DebugScrollView.Instance.Print("사이드서브메뉴 선택X");
        cameraManager.SetVirCam(curArea, curMenu);
        cameraManager.SetRaycastOnOff(false);
        cameraManager.SetRootMoveOnOff(true);


        uiManager.SetPOIMenuOn(curArea, curMenu);
        uiManager.SetPOIListPanel(curArea, curMenu);
        uiManager.SetTopArea(curArea.ToString() + StaticText.BAR + (sideMenuKor +(int)curMenu).ToString());
        uiManager.SetDevicePanelOff();
        cameraManager.SetDevicePopupCameraOnOff(false);


        objectManager.SetDeviceOutlineInter();


    }
    public void SideMenuAllOff()
    {
        DebugScrollView.Instance.Print("사이드메뉴 선택X");
        curMenu = AIRPORTAREA2.NONE;

        cameraManager.SetVirCam(curArea);
        cameraManager.SetRaycastOnOff(false);
        cameraManager.SetRootMoveOnOff(true);


        uiManager.SetPOIMenuAll();
        uiManager.SetTopArea(curArea.ToString());

        uiManager.SetDevicePanelOff();
        cameraManager.SetDevicePopupCameraOnOff(false);

        uiManager.SetPOIListPanel(curArea, curMenu);

        objectManager.SetDeviceOutlineInter();

    }

    public void ClickPOIAction(string targetId)
    {
        //카메라 이동 및 오브젝트 마우스 오버 아웃라인 활성화
        cameraManager.SetRaycastOnOff(true);
        cameraManager.SetVirCam(targetId);
        cameraManager.SetRootMoveOnOff(false);

        uiManager.SetPOIOff();

        objectManager.SetDeviceOutlineInter(PoiMapping.GetIdList(targetId));

        DeviceInfoRok info = dataManager.GetDeviceInfoRok(targetId);

        uiManager.SetTopArea(info.org1 + StaticText.BAR + info.org2Kor + StaticText.BAR + info.org3_);

    }

    public void HitRaycast(GameObject target)
    {
        if (uiManager.GetDevicePopupActive())
        {
            return;
        }

        //mouse over
        DeviceObjectController controller;
        if (!target.TryGetComponent(out controller))
        {
            controller = target.GetComponentInParent<DeviceObjectController>();
            if (controller != null)
            {
                DebugScrollView.Instance.Print(controller.objId);
            }
        }

        if (controller != null)
        {
            objectManager.SetDeviceOutline(controller);
        }
        

    }

    public void ObjectClick(GameObject target)
    {
        if (uiManager.GetDevicePopupActive())
        {
            return;
        }

        DeviceObjectController controller;
        if (!target.TryGetComponent(out  controller))
        {
            controller = target.GetComponentInParent<DeviceObjectController>();
        }

        if (controller != null)
        {
            DebugScrollView.Instance.Print(controller.objId);
            //            string json = "{ "
            //+ "\"deviceIds\": [\"ICN1CA03PT01\"], "
            //+ "\"uris\": [\"38001/0/0\",\"38001/0/1\",\"38001/0/2\",\"38001/0/7\",\"38001/0/8\"]}";
            SetSelectedDeiveId(controller.objId);

            string json_ = dataManager.GetRequestDeviceResJson(controller.GetDeviceType(), controller.objId);
            WebRequestItemPool.Instance.RequestDeviceRecordLatest(controller.objId, json_, controller.GetDeviceType());

        }

    }
    public void SuccessDeviceRecord(string objId, DeviceType type, string apiResult)
    {
        DeviceResources request = dataManager.SetDeviceResource(objId, apiResult);
        uiManager.SetDevicePanelOn(type, request);
        cameraManager.SetDevicePopupCameraOnOff(true);
        objectManager.SetPopupDeviceOn(type);
    }

    public void NoHitRaycast()
    {
        objectManager.SetDeviceOutline();
    }

    public void ReceiveEvent()
    {
        uiManager.ReceiveEvent();
        
    }

   
    public void DevicePopupOffClickAction()
    {
        CommonPopup.ins.SetMode(CommonPopup.Mode.YES_NO);
        CommonPopup.ins.SetText("<color=red>(경고)</color>디바이스 운영을 중지 하시겠습니까?");
        CommonPopup.ins.AddYesAction(CommonPopup.ins.HidePopUp);
        CommonPopup.ins.AddYesAction(RequestDeviceOffAction);
        CommonPopup.ins.AddNoAction(CommonPopup.ins.HidePopUp);
        CommonPopup.ins.ShowPopUp();

    }
    public void DevicePopupRestartClickAction()
    {
        CommonPopup.ins.SetMode(CommonPopup.Mode.YES_NO);
        CommonPopup.ins.SetText("디바이스를 재시작 하시겠습니까?");
        CommonPopup.ins.AddYesAction(CommonPopup.ins.HidePopUp);
        CommonPopup.ins.AddYesAction(RequestDeviceResetAction);
        CommonPopup.ins.AddNoAction(CommonPopup.ins.HidePopUp);
        CommonPopup.ins.ShowPopUp();

    }

    public void SetSplitScreenOnOff(bool isOn)
    {
        uiManager.SetSplitScreenOnOff(isOn);
        cameraManager.SetSplitScreenOnOff(isOn);
        cameraManager.SetRootMoveOnOff(!isOn);
    }
    public void SetSelectedDeiveId(string deviceId)
    {
        selectedDeviceId = deviceId;
    }

    public void RequestDeviceOffAction()
    {
        if (selectedDeviceId != StaticText.EMPTY)
        {
            //디바이스 종료
            //{38003/0/22}
            DeviceInfoRok data = DataManager.Instance.GetDeviceInfoRok(selectedDeviceId);

            string objId = DataManager.Instance.deviceResList.Find(x => x.deviceType.Equals(data.deviceType) && x.deviceVersion.Equals(data.deviceVersion)).deviceTypeCode;
            WebRequestItemPool.Instance.RequestDeviceAct(selectedDeviceId, objId, StaticText.ZERO, "22", delegate { });
        }
    }

    public void RequestDeviceResetAction()
    {
        if (selectedDeviceId != StaticText.EMPTY)
        {
            //디바이스 재시작

            WebRequestItemPool.Instance.RequestDeviceAct(selectedDeviceId, "3", StaticText.ZERO, "4", delegate { });

        }
    }
}
