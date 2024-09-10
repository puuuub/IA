using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class DataManager : SingletonMonoBehaviour<DataManager>
{

    //DeviceInfoRoot deviceInfoRoot;
    DeviceListRoot deviceInfoRoot;
    List<DeviceRecord> recordList;

    //EventTypeData.json
    List<EventTypeClass> eventTypeList;

    public EventClass curEventData { get; set; }

    /// <summary>
    /// ���Ÿ��+������ ��û ���ҽ�(uris)
    /// </summary>
    public List<DeviceResList> deviceResList { get; set; }

    //��ú��忡 ���Ǵ� ������
    public Dictionary<string, DevicePlaceRoot> devicePlaceDic { get; set; }

    //��������
    //<orgId,<deviceType, count>> 
    public Dictionary<string, List<DeviceDoorCount>> deviceDoorDic{get;set;}

    //������� ������̺�
    List<DeviceInfoRok> deviceTableRok;

    

    //IF_DTS_028(����̽� ��Ȳ ��ȸ)
    public DeviceInfoListRoot deviceInfoList { get; set; }

    public DeviceResources curDeviceRes { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        SetDeviceResList();
        SetEventTypeData();
        SetDevicePlaceInfo();


        BasicSetDeviceDoorDic();

    }

    void SetDevicePlaceInfo()
    {
        deviceTableRok = JsonUtil.LoadJsonData<List<DeviceInfoRok>>("DeviceInfoTableRok");
    }
    void BasicSetDeviceDoorDic()
    {
        deviceDoorDic = new Dictionary<string, List<DeviceDoorCount>>();

    }

  

    public void SetDeviceInfoList(string apiResult)
    {
        deviceInfoList = JsonUtil.JsonToObject<DeviceInfoListRoot>(apiResult);
    }

  
    public void SetDeviceDoorDic(string orgId, string apiResult)
    {
        List<DeviceDoorCount> datas = JsonUtil.JsonToObject<List<DeviceDoorCount>>(apiResult);

        if (deviceDoorDic.ContainsKey(orgId))
        {
            deviceDoorDic[orgId] = datas;
        }
        else
        {
            deviceDoorDic.Add(orgId, datas);
        }
    }

    public void ResetDevicePlaceDic()
    {
        if(devicePlaceDic != null)
        {
            devicePlaceDic.Clear();
        }
        devicePlaceDic = new Dictionary<string, DevicePlaceRoot>();
    }
    
    public void SetDevicePlaceDic(string orgId, string apiResult)
    {
        devicePlaceDic.Add(orgId, JsonUtil.JsonToObject<DevicePlaceRoot>(apiResult));

        if (devicePlaceDic.Count.Equals(3))
        {
            UIManager.Instance.FirstSetDashBoard();
        }
    }

    void SetDeviceResList()
    {
        deviceResList = JsonUtil.LoadJsonData<List<DeviceResList>>("DeviceResources");
    }
    void SetEventTypeData()
    {
        eventTypeList = JsonUtil.LoadJsonData<List<EventTypeClass>>("EventTypeData");
    }
    public string GetRequestDeviceResJson(DeviceType type, string objId)
    {
        RequestDeviceRes req = new RequestDeviceRes();
        req.deviceIds = new List<string>();
        req.deviceIds.Add(objId);

        DeviceInfoRok info = GetDeviceInfoRok(objId);
        

        req.uris = deviceResList.Find(x => x.deviceType.Equals(type.ToString()) && x.deviceVersion.Equals(info.deviceVersion)).uris;

        return JsonUtil.ObjectToJson(req);
    }
    public void SetDeviceInfo(string apiResult)
    {
        //deviceInfoRoot = JsonUtil.JsonToObject<DeviceInfoRoot>(apiResult);
        deviceInfoRoot = JsonUtil.JsonToObject<DeviceListRoot>(apiResult);
        
    }

    public DeviceResources SetDeviceResource(string deviceId, string apiResult)
    {

        //recordList = JsonConvert.DeserializeObject<List<DeviceRecord>>(apiResult);
        recordList = JsonUtil.JsonToObject<List<DeviceRecord>>(apiResult);
        if (recordList !=null && recordList.Count > 0)
        {
            curDeviceRes = new DeviceResources(recordList);
        }
        else
        {
            curDeviceRes = new DeviceResources();
        }
        curDeviceRes.deviceId = deviceId;

        return curDeviceRes;

    }


    public DeviceInfo GetDeviceInfo(string deviceId)
    {
        return deviceInfoRoot.content.Find(x => x.deviceId.Equals(deviceId));
    }

    public DeviceInfoRok GetDeviceInfoRok(string deviceId)
    {
        return deviceTableRok.Find(x => x.deviceId.Equals(deviceId));
    }

    //������ ����̽� ���
    public float GetOperationDevicePercent(string orgId)
    {
        int total = 0;
        int oper = 0;
        
        
        for (int i=0;i< deviceResList.Count; i++)
        {
            var data = devicePlaceDic[orgId].details.Find(x => x.type.Equals(int.Parse(deviceResList[i].deviceTypeCode)));
            total += data.all_cnt;
            oper += data.operating_cnt;
        }

        return (float)oper / (float)total;
    }
    
    //�̺�Ʈ ��ȿ�� üũ
    public bool CheckReceiveEvent(string wsResult)
    {
        try
        {
            string data = wsResult.Substring(wsResult.IndexOf('{'), wsResult.Length - wsResult.IndexOf('{'));
            DebugScrollView.Instance.Print("data : " + data);
            
            curEventData = JsonUtil.JsonToObject<EventClass>(data);

            if (curEventData.eventType.Equals(EventRule.RULE_EVENT_BACKTONORMAL.ToString()))
            {//RULE_EVENT �϶���
                return false;
            }

           
        }
        catch (Exception e)
        {
            DebugScrollView.Instance.Print("wsResult : " + wsResult);
            DebugScrollView.Instance.Print("Event Error");
            return false;
        }


        
        return true;
    }

    /// <summary>
    /// �̺�Ʈ ���� - �ѱ�
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public string GetEventTypeKor(string type)
    {
        string result = string.Empty;
        var eventType = eventTypeList.Find(x => x.eventType.Equals(type));
        
        if(eventType != null)
        {
            result = eventType.eventTypeKor;
        }
        return result;
    }
    public void EventTest()
    {
        curEventData = JsonUtil.LoadJsonData<EventClass>("TestEvent");

        //curEventData = new EventClass();
        //curEventData.timeCreated = "2024-07-19T10:39:30.002";
        //curEventData.deviceId = "MDSTEST_DPG1";
        //curEventData.deviceName = "������������Ʈ";
        //curEventData.eventTag = "TAG_DOORCOUNT";
        //curEventData.eventType = "RULE_EVENT_BACKTONORMAL";
        //curEventData.value = "600000";
        //curEventData.valueType = "INTEGER";
        //curEventData.deviceType = "SBG";
        //curEventData.deviceTypeVersion = "";
        //curEventData.organizationId = 626;
        //curEventData.organizationName = "ž�±�21";
        //curEventData.organizationCodeL2 = "T1";
        //curEventData.organizationCodeL3 = "GTE";
        //curEventData.organizationCodeL4 = "021";
        //curEventData.condition = "600000 >= 800000";
    }

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="area2"></param>
    /// <param name="part"></param>
    /// <returns></returns>
    public SUBAREA GetSubareaByPart(AIRPORTAREA2 area2, string part)
    {

        if (area2.Equals(AIRPORTAREA2.CKI))
        {
            for (SUBAREA ck = SUBAREA.���Ϸ���A; ck < SUBAREA.�ⱹ��1��; ck++)
            {
                if (ck.ToString().Contains(part))
                {
                    return ck;
                }
            }
        }
        else if (area2.Equals(AIRPORTAREA2.DEP))
        {
            for (SUBAREA ck = SUBAREA.�ⱹ��1��; ck < SUBAREA.ž�±�10��; ck++)
            {
                if (ck.ToString().Contains(part))
                {
                    return ck;
                }
            }

        }
        else if (area2.Equals(AIRPORTAREA2.GTE))
        {
            for (SUBAREA ck = SUBAREA.ž�±�10��; ck < SUBAREA.����; ck++)
            {
                if (ck.ToString().Contains(part))
                {
                    return ck;
                }
            }
        }
        else
        {
            if (part.Equals("W"))
            {
                return SUBAREA.����;
            }
            else
            {
                return SUBAREA.����;
            }
        }

        return SUBAREA.NONE;
    }

    public SUBAREA GetSubareaByPart2(AIRPORTAREA area, AIRPORTAREA2 area2, string part)
    {
        //���� ���� ������
        
        if (area2.Equals(AIRPORTAREA2.GTE))
        {
            SUBAREA target = SUBAREA.NONE;
            
            for (SUBAREA ck = SUBAREA.ž�±�10��; ck < SUBAREA.����; ck++)
            {
                if (ck.ToString().Contains(part))
                {
                    target = ck;
                    continue;
                }
            }

            //return ž�±� ����, ���� 
            foreach (SUBAREA key in MenuTree.GTETree[area].Keys)
            {
                if (MenuTree.GTETree[area][key].Contains(target))
                {
                    return key;
                }
            }
        }
        
        return SUBAREA.NONE;
    }

    public Color GetResourcesColor(int idx)
    {
        Color result = Color.white;
        int targetValue = 1;
        //(0 = ����, 1 = ������)

        if (idx == 0)
        {
            targetValue = curDeviceRes.tsFace;
        }
        else if (idx == 1)
        {
            targetValue = curDeviceRes.tsBd;
        }
        else if (idx == 2)
        {
            targetValue = curDeviceRes.tsPsprt;
        }
        else if (idx == 3)
        {
            targetValue = curDeviceRes.tsDoor;
        }

        if (targetValue.Equals(0))
        {
            result = Color.green;
        }
        else if (targetValue.Equals(1))
        {
            result = Color.red;
        }
        result.a = 0.2f;

        return result;
    }
}
