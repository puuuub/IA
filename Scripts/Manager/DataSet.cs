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
    BRT,    //유인등록단말기
    DPK,    //디파쳐키오스크
    DPG,    //셀프디파쳐게이트
    PVK,    //여객확인키오스크
    SBG,    //셀프보딩게이트
    BDG,    //유인백드랍진입게이트
    BEG,    //셀프백드랍진입게이트
    ATG,    //환승게이트
    TSG     //환승승보안게이트
}

public enum AIRPORTAREA
{
    ALL,    //전체
    T1,     //T1
    CA,     //환승장
    T2      //T2
}

public enum AIRPORTAREA2
{
    CKI,    //체크인
    DEP,    //출국장
    TRS,    //환승
    GTE,     //탑승구
    NONE
}

//한글 표시용
public enum SIDEMENUKOR
{
    체크인,
    출국장,
    환승,
    탑승구,
    없음
}

public enum SUBAREA
{
    //CKI
    아일랜드A,
    아일랜드B,
    아일랜드C,
    아일랜드D,
    아일랜드E,
    아일랜드F,
    아일랜드G,
    아일랜드H,
    아일랜드J,
    아일랜드K,
    아일랜드L,
    아일랜드M,
    아일랜드N,

    //DEP
    출국장1번,
    출국장2번,
    출국장3번,
    출국장4번,
    출국장5번,
    출국장6번,


    //GTE(탑승구는 많아서 중앙,동쪽,서쪽으로 한번 더 구분)
    탑승구중앙,
    탑승구동쪽,
    탑승구서쪽,

    탑승구10번,
    탑승구15번,
    탑승구17번,
    탑승구21번,
    탑승구23번,
    탑승구36번,
    탑승구45번,

    탑승구232번,
    탑승구247번,
    탑승구248번,
    탑승구254번,
    탑승구255번,
    탑승구265번,
    탑승구266번,
    탑승구267번,

    탑승구114번,
    탑승구122번,

    서측,
    동측,

    NONE
}





public class DeviceInfo
{
    public string timecreated { get; set; }
    public string timeupdated { get; set; }
    public string deviceId { get; set; }        //디바이스 ID
    public string deviceName { get; set; }      //다비아스 이름
    public string description { get; set; }
    public int organizationId { get; set; }     //장소 ID
    public string organizationName { get; set; }    //장소 이름
    public int lifeTimeInSec { get; set; }      //라이프타임
    public bool enable { get; set; }            //사용유무
    public string status { get; set; }          //현재상태
    public List<Deviceuser> deviceUsers { get; set; }   //유저정보
    public string createUserId { get; set; }        //디바이스생성자
    public List<string> tagList { get; set; }       //태그
    public bool initAllClientData { get; set; }
    public object[] extProps { get; set; }          //??
    public Subprop subProp { get; set; }            //알람유무?
    public string linkPayload { get; set; }         //페이로드
    public string registrationDate { get; set; }  //Registration 일시
    public string registrationId { get; set; }      //Registration ID
    public string rootPath { get; set; }            //루트패스
    public string lastUpdate { get; set; }        //최종갱신일시
    public string manufacturer { get; set; }        //제조사
    public string modelNumber { get; set; }         //모델번호
    public string serialNumber { get; set; }        //시리얼번호
    public string firmwareVersion { get; set; }     //펌웨어버전
    public string lastFotaStatus { get; set; }
    public string lastFotaDetail { get; set; }
    public string latitude { get; set; }            //위도
    public string longitude { get; set; }           //경도
}

public class Subprop
{
    public string propstr4 { get; set; }    //부서
    public string propstr5 { get; set; }    //연락처
    public string propstr1 { get; set; }    //비정상종료 알람 (N:정상, E:비정상종료)
    public int propnum1 { get; set; }   //cpu 알람 (0:정상, 1:발생)
    public int propnum2 { get; set; }   //메모리 알람 (0:정상, 1:발생)
    public int propnum3 { get; set; }   //도어계페횟수 알람 (0:정상, 1:발생)
}

public class Deviceuser
{
    public string userid { get; set; }      //사용자 ID
    public string userName { get; set; }    //사용자 이름
    public string roleScope { get; set; }   //사용자 role
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
    public int tsActive { get; set; }            //단말기 상태(0=운영중지, 1=운영중)
    public int tsFace { get; set; }              //안면인식기 이상 유무(0=정상, 1=비정상)
    public int tsPsprt { get; set; }            //여권판독기 이상 유무(0=정상, 1=비정상)
    public int tsBd { get; set; }               //탑승권리더기 이상 유무(0=정상, 1=비정상)
    public int tsDoor { get; set; }             //게이트도어 이상 유무(0=정상, 1=비정상)
    public int tsCPU { get; set; }              //CPU
    public int tsMem { get; set; }              //Memory
    public int opModeCode { get; set; }         //운영모드(0=자동, 1=수동)
    public float tsThreshold { get; set; }       //인증임계값
    public int tsDoorStatus { get; set; }       //도어상태(0=열림, 1=닫힘)
    public int cmDoorOnOff { get; set; }        //도어열림/닫힘(0=도어열림, 1=도어닫힘)
    public int ctDoorCount { get; set; }        //도어누적개폐횟수
    public string cmRegReqNo { get; set; }       //(유인등록기)여권/탑승권 등록요청번호
    public int tsFireAlarm { get; set; }        //화재알람상태(0=Off, 1=On)
    public int cmFireAlarm { get; set; }        //화재알람알림(0=알람해제, 1=알람발생)
    public int tsResOnly { get; set; }          //예약자 전용(0=일반사용자용,1=예약자전용)
    public int tsNonResAllow { get; set; }      //비 예약자 통과 허용(0=허용안함, 1=허용)
    public string tsMacAddr { get; set; }        //단말기 Mac Address
    public string tsCarrierCode { get; set; }    //항공사 IATA Code(for BDG,BEG)
    public string tsIpAddr { get; set; }        //단말기 IP Address
    public string cmPowerOff { get; set; }      //전원 OFF 실행(동작 취소 안됨)
    public int tsSensor { get; set; }           //게이트센서 이상 유무(0=정상, 1=비정상)


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
            //레코드 데이터의 넘버링에 맞춰 데이터 셋
            //디바이스 object정의서 참고
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
    public string org3 { get; set; }    //출국장 경우 숫자가 먼저옴
    public string org3_ { get; set; }   //출국장 숫자 뒤로
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
