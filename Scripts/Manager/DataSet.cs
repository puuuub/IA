using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public enum APITYPE
{
    LOGIN,
    DEVICE,
    EVENT
}
public enum DeviceType
{
    BRT,    //���ε�ϴܸ���
    DPK,    //������Ű����ũ
    DPG,    //���������İ���Ʈ
    PVK,    //����Ȯ��Ű����ũ
    SBG,    //������������Ʈ
    BDG,    //���ι������԰���Ʈ
    BEG,    //�����������԰���Ʈ
    ATG,    //ȯ�°���Ʈ
    TSG     //ȯ�½º��Ȱ���Ʈ
}

public enum AIRPORTAREA
{
    ALL,    //��ü
    T1,     //T1
    CA,     //ȯ����
    T2      //T2
}

public enum AIRPORTAREA2
{
    CKI,    //üũ��
    DEP,    //�ⱹ��
    TRS,    //ȯ��
    GTE,     //ž�±�
    NONE
}

//�ѱ� ǥ�ÿ�
public enum SIDEMENUKOR
{
    üũ��,
    �ⱹ��,
    ȯ��,
    ž�±�,
    ����
}

public enum SUBAREA
{
    //CKI
    ���Ϸ���A,
    ���Ϸ���B,
    ���Ϸ���C,
    ���Ϸ���D,
    ���Ϸ���E,
    ���Ϸ���F,
    ���Ϸ���G,
    ���Ϸ���H,
    ���Ϸ���J,
    ���Ϸ���K,
    ���Ϸ���L,
    ���Ϸ���M,
    ���Ϸ���N,

    //DEP
    �ⱹ��1��,
    �ⱹ��2��,
    �ⱹ��3��,
    �ⱹ��4��,
    �ⱹ��5��,
    �ⱹ��6��,


    //GTE(ž�±��� ���Ƽ� �߾�,����,�������� �ѹ� �� ����)
    ž�±��߾�,
    ž�±�����,
    ž�±�����,

    ž�±�10��,
    ž�±�15��,
    ž�±�17��,
    ž�±�21��,
    ž�±�23��,
    ž�±�36��,
    ž�±�45��,

    ž�±�232��,
    ž�±�247��,
    ž�±�248��,
    ž�±�254��,
    ž�±�255��,
    ž�±�265��,
    ž�±�266��,
    ž�±�267��,

    ž�±�114��,
    ž�±�122��,

    ����,
    ����,

    NONE
}





public class DeviceInfo
{
    public string timecreated { get; set; }
    public string timeupdated { get; set; }
    public string deviceId { get; set; }        //����̽� ID
    public string deviceName { get; set; }      //�ٺ�ƽ� �̸�
    public string description { get; set; }
    public int organizationId { get; set; }     //��� ID
    public string organizationName { get; set; }    //��� �̸�
    public int lifeTimeInSec { get; set; }      //������Ÿ��
    public bool enable { get; set; }            //�������
    public string status { get; set; }          //�������
    public List<Deviceuser> deviceUsers { get; set; }   //��������
    public string createUserId { get; set; }        //����̽�������
    public List<string> tagList { get; set; }       //�±�
    public bool initAllClientData { get; set; }
    public object[] extProps { get; set; }          //??
    public Subprop subProp { get; set; }            //�˶�����?
    public string linkPayload { get; set; }         //���̷ε�
    public string registrationDate { get; set; }  //Registration �Ͻ�
    public string registrationId { get; set; }      //Registration ID
    public string rootPath { get; set; }            //��Ʈ�н�
    public string lastUpdate { get; set; }        //���������Ͻ�
    public string manufacturer { get; set; }        //������
    public string modelNumber { get; set; }         //�𵨹�ȣ
    public string serialNumber { get; set; }        //�ø����ȣ
    public string firmwareVersion { get; set; }     //�߿������
    public string lastFotaStatus { get; set; }
    public string lastFotaDetail { get; set; }
    public string latitude { get; set; }            //����
    public string longitude { get; set; }           //�浵
}

public class Subprop
{
    public string propstr4 { get; set; }    //�μ�
    public string propstr5 { get; set; }    //����ó
    public string propstr1 { get; set; }    //���������� �˶� (N:����, E:����������)
    public int propnum1 { get; set; }   //cpu �˶� (0:����, 1:�߻�)
    public int propnum2 { get; set; }   //�޸� �˶� (0:����, 1:�߻�)
    public int propnum3 { get; set; }   //�������Ƚ�� �˶� (0:����, 1:�߻�)
}

public class Deviceuser
{
    public string userid { get; set; }      //����� ID
    public string userName { get; set; }    //����� �̸�
    public string roleScope { get; set; }   //����� role
}



public class DeviceResourceRoot
{
    public List<DeviceRecord> records { get; set; }
}
public class DeviceRecord
{
    public string uri { get; set; }

    public object value { get; set; }
    //public object Value { get; set; }
    public string type { get; set; }
    public long time { get; set; }
    public string deviceId { get; set; }

}


public class DeviceResources
{
    public string deviceId { get; set; }
    public string bsType { get; set; }
    public string DeviceName { get; set; }
    public int tsActive { get; set; }            //�ܸ��� ����(0=�����, 1=���)
    public int tsFace { get; set; }              //�ȸ��νı� �̻� ����(0=����, 1=������)
    public int tsPsprt { get; set; }            //�����ǵ��� �̻� ����(0=����, 1=������)
    public int tsBd { get; set; }               //ž�±Ǹ����� �̻� ����(0=����, 1=������)
    public int tsDoor { get; set; }             //����Ʈ���� �̻� ����(0=����, 1=������)
    public int tsCPU { get; set; }              //CPU
    public int tsMem { get; set; }              //Memory
    public int opModeCode { get; set; }         //����(0=�ڵ�, 1=����)
    public float tsThreshold { get; set; }       //�����Ӱ谪
    public int tsDoorStatus { get; set; }       //�������(0=����, 1=����)
    public int cmDoorOnOff { get; set; }        //�����/����(0=�����, 1=�������)
    public int ctDoorCount { get; set; }        //���������Ƚ��
    public string cmRegReqNo { get; set; }       //(���ε�ϱ�)����/ž�±� ��Ͽ�û��ȣ
    public int tsFireAlarm { get; set; }        //ȭ��˶�����(0=Off, 1=On)
    public int cmFireAlarm { get; set; }        //ȭ��˶��˸�(0=�˶�����, 1=�˶��߻�)
    public int tsResOnly { get; set; }          //������ ����(0=�Ϲݻ���ڿ�,1=����������)
    public int tsNonResAllow { get; set; }      //�� ������ ��� ���(0=������, 1=���)
    public string tsMacAddr { get; set; }        //�ܸ��� Mac Address
    public string tsCarrierCode { get; set; }    //�װ��� IATA Code(for BDG,BEG)
    public string tsIpAddr { get; set; }        //�ܸ��� IP Address
    public string cmPowerOff { get; set; }      //���� OFF ����(���� ��� �ȵ�)
    public int tsSensor { get; set; }           //����Ʈ���� �̻� ����(0=����, 1=������)


    public DeviceResources()
    {
        
        bsType = null;
        DeviceName = null;
        tsActive = -1;
        tsFace = -1;
        tsPsprt = -1;
        tsBd = -1;
        tsDoor = -1;
        tsCPU = -1;
        tsMem = -1;
        opModeCode = -1;

        tsThreshold = -1;
        tsDoorStatus = -1;
        cmDoorOnOff = -1;
        ctDoorCount = -1;
        tsFireAlarm = -1;
        cmFireAlarm = -1;
        tsResOnly = -1;
        tsNonResAllow = -1;
        tsSensor = -1;
    }

    public DeviceResources(List<DeviceRecord> resList)
    {
        bsType = null;
        DeviceName = null;
        tsActive = -1;
        tsFace = -1;
        tsPsprt = -1;
        tsBd = -1;
        tsDoor = -1;
        tsCPU = -1;
        tsMem = -1;
        opModeCode = -1;

        tsThreshold = -1;
        tsDoorStatus = -1;
        cmDoorOnOff = -1;
        ctDoorCount = -1;
        tsFireAlarm = -1;
        cmFireAlarm = -1;
        tsResOnly = -1;
        tsNonResAllow = -1;
        tsSensor = -1;

        for (int i = 0; i < resList.Count; i++)
        {
            //���ڵ� �������� �ѹ����� ���� ������ ��
            //����̽� object���Ǽ� ����
            string[] parse = resList[i].uri.Split("/");
            SetResData(parse[2], resList[i].value);
        }
    }
    void SetResData(string idx_, object value)
    {
        int idx = int.Parse(idx_);
        switch (idx)
        {
            case 0:
                bsType = value.ToString();
                break;
            case 1:
                DeviceName = value.ToString();
                break;
            case 2:
                tsActive = Convert.ToInt32(value);
                break;
            case 3:
                tsFace = Convert.ToInt32(value);
                break;
            case 4:
                tsPsprt = Convert.ToInt32(value);
                break;
            case 5:
                tsBd = Convert.ToInt32(value);
                break;
            case 6:
                tsDoor = Convert.ToInt32(value);
                break;
            case 7:
                tsCPU = Convert.ToInt32(value);
                break;
            case 8:
                tsMem = Convert.ToInt32(value);
                break;
            case 9:
                opModeCode = Convert.ToInt32(value);
                break;
            case 10:
                tsThreshold = (float)Convert.ToDouble(value);
                break;
            case 11:
                tsDoorStatus = Convert.ToInt32(value);
                break;
            case 12:
                cmDoorOnOff = Convert.ToInt32(value);
                break;
            case 13:
                ctDoorCount = Convert.ToInt32(value);
                break;
            case 14:
                cmRegReqNo = value.ToString();
                break;
            case 15:
                tsFireAlarm = Convert.ToInt32(value);
                break;
            case 16:
                cmFireAlarm = Convert.ToInt32(value);
                break;
            case 17:
                tsResOnly = Convert.ToInt32(value);
                break;
            case 18:
                tsNonResAllow = Convert.ToInt32(value);
                break;
            case 19:
                tsMacAddr = value.ToString();
                break;
            case 20:
                tsCarrierCode = value.ToString();
                break;
            case 21:
                tsIpAddr = value.ToString();
                break;
            case 22:
                cmPowerOff = value.ToString();
                break;
            case 23:
                tsSensor = Convert.ToInt32(value);
                break;
        }
    }
}


public class DeviceUser
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string RoleScope { get; set; }
}

public class SubProp
{
    public string PropStr4 { get; set; }
    public string PropStr5 { get; set; }
}

public class ContentClass
{
    public DateTime TimeCreated { get; set; }
    public DateTime TimeUpdated { get; set; }
    public string DeviceId { get; set; }
    public string DeviceName { get; set; }
    public string Description { get; set; }
    public int OrganizationId { get; set; }
    public string OrganizationName { get; set; }
    public int LifeTimeInSec { get; set; }
    public bool Enable { get; set; }
    public string Status { get; set; }
    public List<DeviceUser> DeviceUsers { get; set; }
    public string CreateUserId { get; set; }
    public List<string> TagList { get; set; }
    public bool InitAllClientData { get; set; }
    public List<object> ExtProps { get; set; }
    public SubProp SubProp { get; set; }
}

public class SortClass
{
    public bool Unsorted { get; set; }
    public bool Sorted { get; set; }
}

public class PageClass
{
    public SortClass Sort { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int Offset { get; set; }
    public bool Unpaged { get; set; }
    public bool Paged { get; set; }
}

public class DeviceListRoot
{
    public List<DeviceInfo> content { get; set; }
    public PageClass pageable { get; set; }
    public bool Last { get; set; }
    public int TotalElements { get; set; }
    public int TotalPages { get; set; }
    public SortClass Sort { get; set; }
    public bool First { get; set; }
    public int NumberOfElements { get; set; }
    public int Size { get; set; }
    public int Number { get; set; }
}

public class DeviceResList
{
    public string deviceType { get; set; }
    public string deviceTypeCode { get; set; }

    public string deviceVersion { get; set; }
    public List<string> uris { get; set; }
}

[Serializable]
public class RequestDeviceRes
{
    public List<string> deviceIds { get; set; }
    public List<string> uris { get; set; }
}


public class DevicePlaceRoot
{
    public int total_count { get; set; }
    public List<DevicePlace> details { get; set; }
}

public class DevicePlace
{
    public int type { get; set; }
    public int all_cnt { get; set; }
    public int online_cnt { get; set; }
    public int abnormal_termination_cnt { get; set; }
    public int operating_cnt { get; set; }
    public int auto_operation_mode_cnt { get; set; }
    public int cpu_alarm_cnt { get; set; }
    public int mem_alarm_cnt { get; set; }
    public int facemodule_alarm_cnt { get; set; }
    public int passportmodule_alarm_cnt { get; set; }
    public int boardingpassmodule_alarm_cnt { get; set; }
    public int doormodule_alarm_cnt { get; set; }
    public int doordurability_alarm_cnt { get; set; }
    public int sensor_alarm_cnt { get; set; }

    public static DevicePlace Add(DevicePlace a, DevicePlace b)
    {
        DevicePlace result = new DevicePlace();
        result.all_cnt = a.all_cnt + b.all_cnt;
        result.online_cnt = a.online_cnt + b.online_cnt;
        result.abnormal_termination_cnt = a.abnormal_termination_cnt + b.abnormal_termination_cnt;
        result.operating_cnt = a.operating_cnt + b.operating_cnt;
        result.auto_operation_mode_cnt = a.auto_operation_mode_cnt + b.auto_operation_mode_cnt;
        result.cpu_alarm_cnt = a.cpu_alarm_cnt + b.cpu_alarm_cnt;
        result.mem_alarm_cnt = a.mem_alarm_cnt + b.mem_alarm_cnt;
        result.facemodule_alarm_cnt = a.facemodule_alarm_cnt + b.facemodule_alarm_cnt;
        result.passportmodule_alarm_cnt = a.passportmodule_alarm_cnt + b.passportmodule_alarm_cnt;
        result.boardingpassmodule_alarm_cnt = a.boardingpassmodule_alarm_cnt + b.boardingpassmodule_alarm_cnt;
        result.doormodule_alarm_cnt = a.doormodule_alarm_cnt + b.doormodule_alarm_cnt;
        result.doordurability_alarm_cnt = a.doordurability_alarm_cnt + b.doordurability_alarm_cnt;
        result.sensor_alarm_cnt = a.sensor_alarm_cnt + b.sensor_alarm_cnt;



        return result;
    }
}


public class EventTypeClass
{
    public string eventType { get; set; }
    public string eventTypeKor { get; set; }
}

public class LoginResponseClass
{
    public string timestamp { get; set; }
    public int status { get; set; }
    public string error { get; set; }
    public string message { get; set; }
    public string path { get; set; }

}

public class EventClass
{
    public string deviceId { get; set; }
    public string deviceName { get; set; }
    public string eventTag { get; set; }
    public string eventType { get; set; }
    public object value { get; set; }
    public string valueType { get; set; }

    public string timeCreated { get; set; }
    public string deviceType { get; set; }
    public string deviceTypeVersion { get; set; }
    public int organizationId { get; set; }
    public string organizationName { get; set; }
    public string organizationCodeL2 { get; set; }
    public string organizationCodeL3 { get; set; }
    public string organizationCodeL4 { get; set; }
    public string condition { get; set; }

}

public enum EventRule
{
    RULE_EVENT,
    RULE_EVENT_BACKTONORMAL
}



public class DeviceDoorCount
{
    public string deviceTag { get; set; }
    public int doorRange1 { get; set; }
    public int doorRange2 { get; set; }
    public int doorRange3 { get; set; }
    public int doorRange4 { get; set; }
    public int deviceTotalCnt { get; set; }
}

public class DeviceErrorTop
{
    public string stat_time { get; set; }
    public string device_id { get; set; }
    public int occur_cnt { get; set; }
    public int clear_cnt { get; set; }
}


public class DeviceInfoListRoot
{
    public int totalElements { get; set; }
    public int number { get; set; }
    public int size { get; set; }
    public List<DeviceInfoList> content { get; set; }
}

public class DeviceInfoList
{
    public string deviceId { get; set; }
    public string collectTime { get; set; }
    public string orgNames { get; set; }
    public string deviceName { get; set; }
    public int organizationId { get; set; }
    public string organizationName { get; set; }
    public string organizationCodeL2 { get; set; }
    public int lifeTimeInSec { get; set; }
    public string linkPayload { get; set; }
    public DateTime registrationDate { get; set; }
    public string registrationId { get; set; }
    public string rootPath { get; set; }
    public DateTime lastUpdate { get; set; }
    public bool enable { get; set; }
    public string status { get; set; }
    public string createUserId { get; set; }
    public string manufacturer { get; set; }
    public string modelNumber { get; set; }
    public string serialNumber { get; set; }
    public string firmwareVersion { get; set; }
    public string tag { get; set; }
    public string deviceTagVer { get; set; }
    public string versionType { get; set; }
    public DateTime timecreated { get; set; }
    public DateTime timeupdated { get; set; }
    public string description { get; set; }
    public string bsType { get; set; }
    public string bsDeviceName { get; set; }
    public int tsActive { get; set; }
    public int tsFace { get; set; }
    public int tsPsprt { get; set; }
    public int tsBd { get; set; }
    public int tsDoor { get; set; }
    public int tsCPU { get; set; }
    public int tsMem { get; set; }
    public int opModeCode { get; set; }
    public string tsThreshold { get; set; }
    public int tsDoorStatus { get; set; }
    public int ctDoorCount { get; set; }
    public int tsFireAlarm { get; set; }
    public string tsMacAddr { get; set; }
    public string tsIpAddr { get; set; }
    public int tsSensor { get; set; }
    public string subPropstr1 { get; set; }
    public string subPropstr4 { get; set; }
    public int subPropnum1 { get; set; }
    public int subPropnum2 { get; set; }
    public int subPropnum3 { get; set; }
    public int subPropnum4 { get; set; }
    public int subPropnum5 { get; set; }
    public int tsResOnly { get; set; }
    public int tsNonResAllow { get; set; }
    public int enclosureDoorStatus { get; set; }
    public string subPropstr5 { get; set; }
    public string tsCarrierCode { get; set; }
    public string latitude { get; set; }
    public string longitude { get; set; }
}


public class DeviceInfoRok
{
    public int idx { get; set; }
    public string deviceId { get; set; }
    public string org1 { get; set; }
    public string org2 { get; set; }
    public string org2Kor { get; set; }
    public string org3 { get; set; }    //�ⱹ�� ��� ���ڰ� ������
    public string org3_ { get; set; }   //�ⱹ�� ���� �ڷ�
    public string deviceType { get; set; }
    public string deviceVersion { get; set; }
}



public class AccountInfo
{
    public string userid { get; set; }
    public string userName { get; set; }
    public string mobile { get; set; }
    public string email { get; set; }
    public string description { get; set; }
    public DateTime timeCreated { get; set; }
    public DateTime timeUpdated { get; set; }
    public DateTime timelastlogin { get; set; }
    public int organizationId { get; set; }
    public string organizationName { get; set; }
    public string organizationDescription { get; set; }
    public string levelName { get; set; }
    public string levelId { get; set; }
    public List<Parentorglist> parentOrgList { get; set; }
    public bool enable { get; set; }
    public object[] extProps { get; set; }
}

public class Parentorglist
{
    public int id { get; set; }
    public string name { get; set; }
}
