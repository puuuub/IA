using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : SingletonMonoBehaviour<ObjectManager>
{
    [SerializeField]
    List<POITargetController> t1PoiTargetList;
    public List<POITargetController> T1POITargetList { get { return t1PoiTargetList; } }

    [SerializeField]
    List<POITargetController> caPoiTargetList;
    public List<POITargetController> CAPOITargetList{   get { return caPoiTargetList; } }
    
    [SerializeField]
    List<POITargetController> t2PoiTargetList;
    public List<POITargetController> T2POITargetList{   get { return t2PoiTargetList; } }

    [SerializeField]
    List<DeviceObjectController> deviceControllerList;
    List<DeviceObjectController> outlienInterList;

    [SerializeField]
    List<SubareaObjectController> t1SubareaObjectControllerList;
    [SerializeField]
    List<SubareaObjectController> t2SubareaObjectControllerList;
    [SerializeField]
    List<SubareaObjectController> caSubareaObjectControllerList;
    SubareaObjectController exTargetSubareaController;


    [Header("DevicePopupPanel Object")]
    [SerializeField]
    List<DeviceObjectController2> popupDeviceList;

    public void SetDeviceOutlineInter(List<string> idList = null)
    {//마우스 오버 작동 셋팅
        if (outlienInterList != null)
        {
            for (int i = 0; i < outlienInterList.Count; i++)
            {
                outlienInterList[i].interactiveOutline = false;
            }
            outlienInterList.Clear();
        }



        if (idList != null)
        {
            if(outlienInterList != null)
            {
                for(int i=0;i< outlienInterList.Count; i++)
                {
                    outlienInterList[i].interactiveOutline = false;
                }
                outlienInterList.Clear();
            }

            outlienInterList = deviceControllerList.FindAll(x => idList.Contains(x.objId));
            for (int i = 0; i < outlienInterList.Count; i++)
            {
                outlienInterList[i].interactiveOutline = true;
            }

        }
    }

    public void SetDeviceOutline(DeviceObjectController target = null)
    {
        if(target == null)
        {
            for (int i = 0; i < outlienInterList.Count; i++)
            {
                outlienInterList[i].SetDeviceOutlineOnOff(false);
            }
        }
        else
        {
            for (int i = 0; i < outlienInterList.Count; i++)
            {
                if (outlienInterList[i].Equals(target))
                {
                    outlienInterList[i].SetDeviceOutlineOnOff(true);
                }
                else
                {
                    outlienInterList[i].SetDeviceOutlineOnOff(false);
                }
            }

        }
    }
    
    public DeviceObjectController GetDeviceController(string deviceId)
    {
        return deviceControllerList.Find(x => x.objId.Equals(deviceId));
    }


    public void SetSubareaOutlineOn(AIRPORTAREA area, SUBAREA subarea)
    {
        SetSubareaOutlineOff();

        if (area.Equals(AIRPORTAREA.T1))
        {
            exTargetSubareaController = t1SubareaObjectControllerList.Find(x => x.targetArea.Equals(subarea));
        }else if (area.Equals(AIRPORTAREA.T2))
        {
            exTargetSubareaController = t2SubareaObjectControllerList.Find(x => x.targetArea.Equals(subarea));
        }
        else
        {
            exTargetSubareaController = caSubareaObjectControllerList.Find(x => x.targetArea.Equals(subarea));
        }
        exTargetSubareaController.SetOutLineOnOff(true);
    }

    public void SetSubareaOutlineOff()
    {
        if (exTargetSubareaController != null)
            exTargetSubareaController.SetOutLineOnOff(false);
    }

    public void SetPopupDeviceOn(DeviceType type)
    {
        //오븝젝트 끌 필요없음
        //카메라 끄면 됨

        if (type.Equals(DeviceType.BRT))
        {
            popupDeviceList[0].SetContentsOnOff(true);
            popupDeviceList[1].SetContentsOnOff(false);
            popupDeviceList[2].SetContentsOnOff(false);
        }
        else if (type.Equals(DeviceType.DPK) || type.Equals(DeviceType.PVK))
        {
            popupDeviceList[0].SetContentsOnOff(false);
            popupDeviceList[1].SetContentsOnOff(true);
            popupDeviceList[2].SetContentsOnOff(false);

        }
        else if (type.Equals(DeviceType.DPG) || type.Equals(DeviceType.SBG)
            || type.Equals(DeviceType.BDG) || type.Equals(DeviceType.BEG))
        {
            popupDeviceList[0].SetContentsOnOff(false);
            popupDeviceList[1].SetContentsOnOff(false);
            popupDeviceList[2].SetContentsOnOff(true);

        }
    }
    
    public void SetPopupDeviceOutlineOn(DeviceType type, int idx, Color col)
    {
        if (type.Equals(DeviceType.BRT))
        {
            if(idx < 0)
            {
                popupDeviceList[0].SetAllOutlineOff();
            }
            else
            {
                popupDeviceList[0].SetPartOutlineOn(idx, col);
            }
        }
        else if (type.Equals(DeviceType.DPK) || type.Equals(DeviceType.PVK))
        {
            if (idx < 0)
            {
                popupDeviceList[1].SetAllOutlineOff();
            }
            else
            {
                popupDeviceList[1].SetPartOutlineOn(idx, col);
            }
        }
        else if (type.Equals(DeviceType.DPG) || type.Equals(DeviceType.SBG)
            || type.Equals(DeviceType.BDG) || type.Equals(DeviceType.BEG))
        {
            if (idx <0)
            {
                popupDeviceList[2].SetAllOutlineOff();
            }
            else
            {
                if (idx == 1)
                {//탑승권 리더기랑 여권판독기 하나로 사용중
                    idx = 2;
                }
                popupDeviceList[2].SetPartOutlineOn(idx, col);
            }
        }
    }
    public void SetPopupDeviceOutlineOff(DeviceType type)
    {
        if (type.Equals(DeviceType.BRT))
        {
            popupDeviceList[0].SetAllOutlineOff();
        }
        else if (type.Equals(DeviceType.DPK) || type.Equals(DeviceType.PVK))
        {
            popupDeviceList[1].SetAllOutlineOff();
        }
        else if (type.Equals(DeviceType.DPG) || type.Equals(DeviceType.SBG)
            || type.Equals(DeviceType.BDG) || type.Equals(DeviceType.BEG))
        {
            popupDeviceList[2].SetAllOutlineOff();
        }
    }
    public void SetPopupDeviceOutlineOff()
    {
        popupDeviceList[0].SetAllOutlineOff();
        popupDeviceList[1].SetAllOutlineOff();
        popupDeviceList[2].SetAllOutlineOff();
    }

}
