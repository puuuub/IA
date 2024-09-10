using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : SingletonMonoBehaviour<EventManager>
{
    Coroutine curCoroutine;

    //WaitForSeconds wfs_2 = new WaitForSeconds(2f);
    WaitForSeconds wfs_3 = new WaitForSeconds(3f);


    public void EventAction(EventClass eventData)
    {
        if(curCoroutine != null)
        {
            StopCoroutine(curCoroutine);
        }

        DeviceInfoRok infoRok = DataManager.Instance.GetDeviceInfoRok(eventData.deviceId);

        if (infoRok == null)
        {
            CommonPopup.ins.SetMode(CommonPopup.Mode.CONFIRM);
            CommonPopup.ins.SetText("등록되지 않은 디바이스 입니다.");
            CommonPopup.ins.AddMyAction(CommonPopup.ins.HidePopUp);
            CommonPopup.ins.ShowPopUp();

            return;
        }
        else
        {
            curCoroutine = StartCoroutine(EventCoroutine(infoRok));
        }

    }

    IEnumerator EventCoroutine(DeviceInfoRok infoRok)
    {
        
        //필요 데이터로 치환
        AIRPORTAREA area;
        if (infoRok.org1.Equals(AIRPORTAREA.T1.ToString()))
        {
            area = AIRPORTAREA.T1;
        }
        else if (infoRok.org1.Equals(AIRPORTAREA.T2.ToString()))
        {
            area = AIRPORTAREA.T2;
        }
        else
        {
            area = AIRPORTAREA.CA;
        }

        AIRPORTAREA2 area2;
        SUBAREA subArea;
        if (infoRok.org2.Equals(AIRPORTAREA2.CKI.ToString()))
        {
            area2 = AIRPORTAREA2.CKI;
            
        }
        else if (infoRok.org2.Equals(AIRPORTAREA2.DEP.ToString()))
        {
            area2 = AIRPORTAREA2.DEP;
        }
        else if (infoRok.org2.Equals(AIRPORTAREA2.TRS.ToString()))
        {
            area2 = AIRPORTAREA2.TRS;
        }
        else
        {
            area2 = AIRPORTAREA2.GTE;
        }

        if (!area2.Equals(AIRPORTAREA2.GTE))
        {
            subArea = DataManager.Instance.GetSubareaByPart(area2, infoRok.org3_);
        }
        else
        {
            subArea = DataManager.Instance.GetSubareaByPart2(area, area2, infoRok.org3_);
        }
        
        if (!subArea.Equals(SUBAREA.NONE))
        {
            //자동 카메라 움직임
            CameraManager.Instance.SetRootMoveOnOff(false);
            CameraManager.Instance.SetRaycastOnOff(false);
            CameraManager.Instance.SetVirCam(area, subArea);
            UIPOIManager.Instance.SetPOIAreaRoot(area);
            CameraManager.Instance.SetCameraOutlineAniOnOff(true);

            UIManager.Instance.SetTopArea(area.ToString() + StaticText.BAR + (SIDEMENUKOR.체크인 + (int)area2).ToString() + StaticText.BAR + subArea.ToString());

            if (!area2.Equals(AIRPORTAREA2.GTE))
            {
                ObjectManager.Instance.SetSubareaOutlineOn(area, subArea);
            }
            else
            {
                ObjectManager.Instance.SetSubareaOutlineOn(area, DataManager.Instance.GetSubareaByPart(area2, infoRok.org3_));
            }

            yield return wfs_3;

            ObjectManager.Instance.SetSubareaOutlineOff();

            UIManager.Instance.SetPOIOff();
            
            CameraManager.Instance.SetVirCam(infoRok.deviceId);
            
            var deviceController = ObjectManager.Instance.GetDeviceController(infoRok.deviceId);

            deviceController.SetOutLineOnOff(true);
            deviceController.SetOutlineNum(1);
            
            yield return wfs_3;
 
            deviceController.SetOutLineOnOff(false);
            deviceController.SetOutlineNum(0);

            MainManager.Instance.ObjectClick(deviceController.gameObject);


            CameraManager.Instance.SetRaycastOnOff(true);
            ObjectManager.Instance.SetDeviceOutlineInter(PoiMapping.GetIdList(infoRok.deviceId));
            
            CameraManager.Instance.SetCameraOutlineAniOnOff(false);

        }


    }
}
