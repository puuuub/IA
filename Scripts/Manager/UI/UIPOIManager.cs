using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPOIManager : SingletonMonoBehaviour<UIPOIManager>
{

    [SerializeField]
    GameObject poiRoot;

    [SerializeField]
    List<GameObject> poiRootList;   //T1, CA, T2

    [SerializeField]
    List<GameObject> t1PoiDeviceRootList;   //brt, dpk, dpg, pvk, sbg, beg, bdg, atg, tsg
    [SerializeField]
    List<GameObject> caPoiDeviceRootList;
    [SerializeField]
    List<GameObject> t2PoiDeviceRootList;

    [SerializeField]
    List<UIPOIController> poiList;

    [Header("Image Resource")]
    [SerializeField]
    List<Sprite> poiBg_sps;
    [SerializeField]
    List<Sprite> smallPoi_sps;

    public float POISMALLDIS = 300f;
    
    const string LANE = "Lane";
    // Start is called before the first frame update
    void Start()
    {

        CreatePoi();
    }

    void CreatePoi()
    {
        int idx = 0;
        List<POITargetController> targetList = ObjectManager.Instance.T1POITargetList;
        
        for(int i = 0; i < targetList.Count; i++)
        {
            var deviceType = targetList[i].deviceType;

            poiList[idx].SetTargetTransform(targetList[i].transform);
            poiList[idx].SetSubTargetPos(targetList[i].subPos);
            poiList[idx].transform.SetParent(t1PoiDeviceRootList[(int)deviceType].transform);
            poiList[idx].SetDeviceType(deviceType, MenuTree.GetDeviceTypeKor(deviceType));
            poiList[idx].SetPOIImage(poiBg_sps[(int)deviceType], smallPoi_sps[(int)deviceType]);
            poiList[idx].SetLaneText(targetList[i].deviceCnt.ToString() + LANE);
            poiList[idx].area = AIRPORTAREA.T1;

            string  id = targetList[i].targetId;
            poiList[idx].AddButtonAction(delegate { POIClickAction(id); });
            
            idx++;
        }

        targetList = ObjectManager.Instance.T2POITargetList;

        for (int i = 0; i < targetList.Count; i++)
        {
            var deviceType = targetList[i].deviceType;

            poiList[idx].SetTargetTransform(targetList[i].transform);
            poiList[idx].SetSubTargetPos(targetList[i].subPos);
            poiList[idx].transform.SetParent(t2PoiDeviceRootList[(int)deviceType].transform);
            poiList[idx].SetDeviceType(deviceType, MenuTree.GetDeviceTypeKor(deviceType));
            poiList[idx].SetPOIImage(poiBg_sps[(int)deviceType], smallPoi_sps[(int)deviceType]);
            poiList[idx].SetLaneText(targetList[i].deviceCnt.ToString() + LANE);
            poiList[idx].area = AIRPORTAREA.T2;

            string id = targetList[i].targetId;
            poiList[idx].AddButtonAction(delegate { POIClickAction(id); });

            idx++;
        }
        targetList = ObjectManager.Instance.CAPOITargetList;

        for (int i = 0; i < targetList.Count; i++)
        {
            var deviceType = targetList[i].deviceType;

            poiList[idx].SetTargetTransform(targetList[i].transform);
            poiList[idx].SetSubTargetPos(targetList[i].subPos);
            poiList[idx].transform.SetParent(caPoiDeviceRootList[(int)deviceType].transform);
            poiList[idx].SetDeviceType(deviceType, MenuTree.GetDeviceTypeKor(deviceType));
            poiList[idx].SetPOIImage(poiBg_sps[(int)deviceType], smallPoi_sps[(int)deviceType]);
            poiList[idx].SetLaneText(targetList[i].deviceCnt.ToString() + LANE);
            poiList[idx].area = AIRPORTAREA.CA;

            string id = targetList[i].targetId;
            poiList[idx].AddButtonAction(delegate { POIClickAction(id); });

            idx++;
        }
    }

    void POIClickAction(string targetId)
    {

        MainManager.Instance.ClickPOIAction(targetId);
    }

    public void SetPOIAreaRoot(AIRPORTAREA area)
    {
        SetPoiRootOnOff(true);

        if (area.Equals(AIRPORTAREA.ALL))
        {
            for(int i = 0; i < poiRootList.Count; i++)
            {
                poiRootList[i].SetActive(false);
            }
        }
        else
        {
            for(int i = 0; i < poiRootList.Count; i++)
            {
                if((int)area-1 == i)
                {
                    poiRootList[i].SetActive(true);
                }
                else
                {
                    poiRootList[i].SetActive(false);
                }
            }
        }
    }
    
    public void SetPOIMenuRoot(AIRPORTAREA area, List<DeviceType> deviceList)
    {
        SetPoiRootOnOff(true);

        List<GameObject> targetList;
        if (area.Equals(AIRPORTAREA.T1))
        {
            targetList = t1PoiDeviceRootList;
        }
        else if (area.Equals(AIRPORTAREA.CA))
        {
            targetList = caPoiDeviceRootList;
        }
        else if (area.Equals(AIRPORTAREA.T2))
        {
            targetList = t2PoiDeviceRootList;
        }
        else
        {
            return;
        }

        if (deviceList != null)
        {
            var intList = deviceList.ConvertAll(x => (int)x);
            for (int i = 0; i < targetList.Count; i++)
            {
                if (intList.Contains(i))
                {
                    targetList[i].SetActive(true);
                }
                else
                {
                    targetList[i].SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < targetList.Count; i++)
            {
                targetList[i].SetActive(false);
            }
        }
    }
    public void SetPOIMenuRoot(AIRPORTAREA area, DeviceType deviceType)
    {
        SetPoiRootOnOff(true);

        List<GameObject> targetList;
        if (area.Equals(AIRPORTAREA.T1))
        {
            targetList = t1PoiDeviceRootList;
        }
        else if (area.Equals(AIRPORTAREA.CA))
        {
            targetList = caPoiDeviceRootList;
        }
        else if (area.Equals(AIRPORTAREA.T2))
        {
            targetList = t2PoiDeviceRootList;
        }
        else
        {
            return;
        }

        for (int i = 0; i < targetList.Count; i++)
        {
            if ((int)deviceType==i)
            {
                targetList[i].SetActive(true);
            }
            else
            {
                targetList[i].SetActive(false);
            }
        }
    }


    public void SetPoiRootOnOff(bool isOn)
    {
        poiRoot.SetActive(isOn);
    }

    public void SetPoiSplitOnOff(bool isOn)
    {
        

        if (isOn)
        {
            
            for (int i = 0; i < poiList.Count; i++)
            {
                poiList[i].SetTargetCamera(CameraManager.Instance.GetSplitCamera((int)(poiList[i].area - 1)));
                poiList[i].SetButtonInteraction(false);
            }
        }
        else
        {
            for (int i = 0; i < poiList.Count; i++)
            {
                poiList[i].SetTargetCamera(CameraManager.Instance.GetMainCamera());
                poiList[i].SetButtonInteraction(true);
            }
        }

        
        for (int i = 0; i < poiRootList.Count; i++)
        {
            poiRootList[i].SetActive(isOn);
        }

        for (int i = 0; i < t1PoiDeviceRootList.Count; i++)
        {
            t1PoiDeviceRootList[i].SetActive(isOn);
        }
        for (int i = 0; i < t1PoiDeviceRootList.Count; i++)
        {
            caPoiDeviceRootList[i].SetActive(isOn);
        }
        for (int i = 0; i < t1PoiDeviceRootList.Count; i++)
        {
            t2PoiDeviceRootList[i].SetActive(isOn);
        }
    }
}
