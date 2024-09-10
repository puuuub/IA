using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RootApiUrl
{
    public ApiUrl[] ApiUrl { get; set; }
}

public class ApiUrl
{
    public int index { get; set; }
    public string path { get; set; }
}

public class NetProtocol
{
    public enum SERVER_MODE
    {
        LIVE,
        TEST,
        NUM_OF_MODE
    }
    public enum ServerConnection
    {
        LOGIN,
        GET_ALL_SEAWOMAN,
        WEATHER_UPDATE,
        GET_ALL_NOTICE,
        ERROR_URL,
        NUM_OF_CONNECTION
    }

    public static string BaseURL = "http://aliandev.iptime.org:38111/mnc";

    public static string DigtalTweenBaseURL = "http://aliandev.iptime.org:38115/api_dt";

    public enum API_URL
    {

        BASE_URL,
        DT_BASE_URL,

        rsa_getRsaInfo,
        login_captcha,
        login_loginProc,
        login_sessionIdCheck,
        login_chkToken,




        dt_getCurSensorEvent,
        dt_getEvetHistList,
        dt_getEvetInfo,
        dt_getRealTimeChartData,
        dt_getSensorInfo,
        dt_getSiteInfoList,
        dt_getSocEvThrs,
        dt_legacy_getCtrlYn,
        dt_legacy_getIotSensing,
        dt_legacy_getIotSensingHist,
        dt_legacy_getLegacyAccess,
        dt_legacy_getLegacyAccessHist,
        dt_legacy_getLegacyCctv,
        dt_legacy_getLegacyCtrlHist,
        dt_legacy_getLegacyDpm,
        dt_legacy_getLegacyDpmHist,
        dt_legacy_getLegacyDpmMapping,
        dt_legacy_getLegacyEhp,
        dt_legacy_getLegacyEhpHist,
        dt_legacy_getLegacyElevator,
        dt_legacy_getLegacyElevatorHist,
        dt_legacy_getLegacyEmr,
        dt_legacy_getLegacyEmrHist,
        dt_legacy_getLegacyEscalator,
        dt_legacy_getLegacyEscalatorHist,
        dt_legacy_getLegacyFaAnyang,
        dt_legacy_getLegacyFaAnyangHist,
        dt_legacy_getLegacyFaAnyangMerge,
        dt_legacy_getLegacyFaIncheon,
        dt_legacy_getLegacyFaIncheonHist,
        dt_legacy_getLegacyFaSuncheon,
        dt_legacy_getLegacyFaSuncheonHist,
        dt_legacy_getLegacyFaYeosu,
        dt_legacy_getLegacyFaYeosuHist,
        dt_legacy_getLegacyFaYeosuMerge,
        dt_legacy_getLegacyFire,
        dt_legacy_getLegacyFireHist,
        dt_legacy_getLegacyFloor,
        dt_legacy_getLegacyGeothermal,
        dt_legacy_getLegacyGeothermalHist,
        dt_legacy_getLegacyLight,
        dt_legacy_getLegacyLightHist,
        dt_legacy_getLegacyNp,
        dt_legacy_getLegacyNpHist,
        dt_legacy_getLegacyObj,
        dt_legacy_getLegacyObjGroup,
        dt_legacy_getLegacyParking,
        dt_legacy_getLegacyParkingHist,
        dt_legacy_getLegacyPool,
        dt_legacy_getLegacyPoolHist,
        dt_legacy_getLegacyPower,
        dt_legacy_getLegacyPowerHist,
        dt_legacy_getLegacySolar,
        dt_legacy_getLegacySolarHist,
        dt_legacy_getLegacyTelemeter,
        dt_legacy_getLegacyTelemeterHist,
        dt_legacy_getRailRobot,
        dt_legacy_getRailRobotHist,
        dt_legacy_getRailRobotSensing,
        dt_legacy_getRailRobotSensingHist,
        dt_legacy_legacyCtrlComm,
        dt_legacy_sop_sms,
        dt_simul_getElecSafeForePtimePopupMsg,
        dt_simul_getElecSafeForeSimu,
        dt_simul_getElecSafeForeSimuDtl,
        dt_simul_getElecSafeForeSimuDtlBySensorId,
        dt_simul_getElecSafeForeSimuDtlGraph,
        dt_simul_getElpwUseOptzSimu,
        dt_simul_getElpwUseOptzSimuAnalysis,
        dt_simul_getElpwUseOptzSimuDtl,
        dt_simul_getElpwUseOptzSimuPopupMsg,
        dt_simul_getFireDfsnPtimePopupMsg,
        dt_simul_getFireDfsnSimu,
        dt_simul_getFireDfsnSimuToJson,
        dt_simul_getIfcdDfsnPtimePopupMsg,
        dt_simul_getIfcdDfsnSimu,
        dt_simul_getIfcdDfsnSimuToJson,
        dt_simul_getSimuDesc,
        dt_legacy_getLegacyFaCheongju,
        dt_legacy_getLegacyFaCheongjuHist,
        dt_getEvetHistCnt

    }







    public static string NoticeServerURL = "http://172.30.1.26:9001/";
    //public static string NoticeServerURL = "http://192.168.0.39:9010/";

#if TEST_SERVER
    public static SERVER_MODE MY_SERVER_MODE = SERVER_MODE.TEST;
#elif LIVE_SERVER
    public static SERVER_MODE MY_SERVER_MODE = SERVER_MODE.LIVE;
#else
    public static SERVER_MODE MY_SERVER_MODE = SERVER_MODE.TEST;
#endif

    public static string[,] ServerURL = new string[(int)SERVER_MODE.NUM_OF_MODE, (int)ServerConnection.NUM_OF_CONNECTION]
    {
        // LIVE 
        // HTTPS!!!!!!!!
        {
            "https://api.blackstonebelleforet.com/client/user/login",	        /*	LOGIN,	*/
            "http://192.168.0.39:9071/apiUnity/getData",                                    /*GET_ALL_SEAWOMAN*/
            "https://api.openweathermap.org/data/2.5/weather?lat=33.493379&lon=126.535004&lang=kr&units=metric&appid=28e0cf9418a8a6105f1a7e596e20e09e",	/*	WEATHER_UPDATE,	*/

            NoticeServerURL + "api/notice/getAllNoticeForClient",        /*	GET_ALL_NOTICE,	*/

            "ErrorTest"	                                                        /*	ERROR_URL,	*/
        },

        // TEST
        {
            "http://testapi.bsgrcc.com/client/user/login",	                    /*	LOGIN,	*/	
            "http://192.168.0.39:9071/apiUnity/getData",                                    /*GET_ALL_SEAWOMAN*/
            "http://api.openweathermap.org/data/2.5/weather?lat=33.493379&lon=126.535004&lang=kr&units=metric&appid=28e0cf9418a8a6105f1a7e596e20e09e",	/*	WEATHER_UPDATE,	*/	

            NoticeServerURL + "api/notice/getAllNoticeForClient",        /*	GET_ALL_NOTICE,	*/

            "ErrorTest"	                                                        /*	ERROR_URL,	*/
        },
    };

    //public static string GetConnectURL(ServerConnection con)
    //{
    //    string url = ServerURL[(int)MY_SERVER_MODE, (int)con];
    //    if (MY_SERVER_MODE == SERVER_MODE.LIVE)
    //    {
    //        // 날씨 정보 API를 제외한 실서버 API가 Https 프로코콜이 아니면 표시
    //        if (!url.Contains("https") && con != ServerConnection.WEATHER_UPDATE)
    //        {
    //            DebugScrollView.Instance.Print("Check it Now Live Server URL (https)!!!!!");
    //        }
    //    }
    //    return url;
    //}
}

public class ResponseResult
{
    public int resultCode;
    public string message;
    public string resultMsg;
    public string result = "SUCCESS"; // 아리안
    public string responseMessage = "SUCCESS"; // 아리안
}



public class LoginData
{
    public int resultCode;
    public string message;
    public string resultMsg;
    //public string phone;
    public string handphone;
    //public string birth;
    //public string addrCity;
    //public string addrGu;
    //public string addrDetail;
    public string homeAddr1;
    public string homeAddr2;
    public string email;
    //public string memberType;
    public string type;
    public int userUid;
    public List<ActivityType> activityType;
}

public class ActivityType
{
    public string activityTypeId;
    public string activityInfo;
    public string activityState;
    public string activityName;
    public List<Activity> activityList;
}

public class Activity
{
    public string image;
    public string fileName;
    public int productUid;
    public string activityTypeId;
    public string productName;
    public int price;
    //public string priceWeekend;
    public int isWeekend; // 주말여부(0:주중, 1:주말)
    public string wmRef4; // 주말여부(D:주중, 1:E)
    public int discount;
    public string packageType; // 패키지 여부 (A: 일반상품, P: 패키지)
    public string productInfo;
}




public class PurchaseData
{
    public int resultCode;
    public string message;
    public List<PurchaseItem> purchaseList;
    public List<CouponItem> CouponList;
}

public class PurchaseItem
{
    public int purchaseUid;
    public string productName;
    //public int userUid;
    //public string moid;
    //public int productUid;
    //public int activityTypeId;
    //public int price;
    public int totalPrice;
    //public string ticketNo;
    public string ticketNumber;
    public string ticketCount;
    public string isUse;
    public string purchaseDate;
    public string purchaseState;
    public string couponNumber;
}

public class CouponItem
{
    public string couponName; // "벨포레 App 오픈 기념 1만원 할인 쿠폰",
    public string couponCode; // "welcome",
    public string couponNumber; // CP210427163420390
    public string isUse;
    public int discntAmt; // 10000
    public string endDate;
    public string discntType; // 정액할인
    public string userUid; // 1
}


public class PurchaseDetailData
{
    public int resultCode;
    public string message;
    public PurchaseDetailRecord purchaseDetail;
}

public class PurchaseDetailRecord
{
    public int purchaseUid;
    public string productName;
    public string moid; // 주문번호
    public int price;
    public int totalPrice; // 결제 금액
    public string ticketCount;
    public string ticketNumber;
    public string isUse;
    public string purchaseDate;
    public string useDate;
    public string purchaseState;
    public List<string> ticketCountList;
}

public class CouponDetailData
{
    public int resultCode;
    public string message;
    public CouponDetailRecord coupon;
}

public class CouponDetailRecord
{
    public string couponName; // "벨포레 App 오픈 기념 1만원 할인 쿠폰",
    public string couponCode; // "welcome",
    public int discntAmt; // 10000
    public string couponNumber; // CP210427163420390
    public string startDate;
    public string endDate;
    public string discntType; // 정액할인
    public string minAmt; //  최소주문 금액
    public string userUid; // 1
    public string activityType; // 엑티비티
}







public class BasketItemData
{
    public int userUid;
    public int price;
    public string productName;
    public int productUid;
    public int productQty;

    public string isCheck;
    public string isPay;
    public string orderKey;
    public int basketUid;
}

public class BasketList
{
    public int resultCode;
    public string message;
    public List<BasketItemData> basketList;
}



// 공지사항
public class NoticeData
{
    public int id;
    public string title;
    public string noticeText;
    public string attachementUrl;
    public string notice_url;
    public string notice_id;
}

public class NoticeList
{
    public int resultCode;
    public string message;
    public List<NoticeData> list;
}

//푸시
public class PushData
{
    public int id;
    public string title;
    public string pushText;
    public string noticeLink;
}


// Form 제작용 클래스 ==================
class OrderInfo
{
    public int productUid;
    //public int discount;
    public int price;
}
class UpdateBasketJson
{
    //public string resultCode;
    //public string message;
    public int userUid;
    public int productUid;
    public int productQty;
    //public int basketUid;
    //public List<BasketItemData> basketList;
}

class UpdateBasketCntJson
{
    public int productQty;
    public int basketUid;
}

class BasketJson
{
    //public string resultCode;
    //public string message;
    public int userUid;
}

class DeleteBasketItemJson
{
    public List<BasketUid> deleteList;
}

class BasketUid
{
    public int basketUid;
}
//==================================


public class GPSDataPacket
{
    public int resultCode;
    public string message;
    public List<MoveObjectPacket> gpsList;
}

public class GPSData
{
    public string familyPhone;
    public string lat;
    public string lng;
}

public class BusData
{
    public int resultCode;
    public string message;
    public List<BusLocation> busList;
}

public class BusLocation
{
    public string busName;
    public string lat;
    public string lng;
}


public class MoveObjectPacket
{
    public string familyPhone;
    public string familyName;
    public string CREATE_DATE;
    public string lng;
    public string ID;
    public string TYPE;
    public string lat;
    public string LONG;
    public string SIZE_X;
    public string SIZE_Y;
    public string SIZE_Z;
    public string OBJECT_TYPE;

    public float LAT_;
    public float LONG_;
}

public class MoveObjectEventPacket
{
    public string AREA_ID;
    public string CREATE_DATE;
    public string EVENT_NAME;

    public string EVENT_ID;
    public string ID;
    public string DEVICE_ID;
}

public class MoveObjList
{
    public int resultCode;
    public string message;
    public List<MoveObjectPacket> gpsList;
}


public class NetworkDataManager : SingletonClass<NetworkDataManager>
{
    public static Dictionary<string, int> AreaIndexDic = new Dictionary<string, int>();


    public static string[] AreaArray =
    {
        "제주시",
        "조천읍",
        "구좌읍",
        "성산읍",
        "표선면",
        "남원읍",
        "서귀포시",
        "중문",
        "안덕면",
        "대정읍",
        "한경면",
        "한림읍",
        "애월읍",
        "추자도",
        "우도",
    };

    public List<MoveObjectPacket> MovingObjectList;
    public List<MoveObjectEventPacket> MoveObjectEventList;
    public LoginData MyLoginData;
    public PurchaseData MyPurchaseData;
    public PurchaseDetailData MyPurchaseDetailData;
    public CouponDetailData MyCouponDetailData;

    public ResponseResult MyResponseResult;
    //public GPSDataPacket BusGPSPack;
    public MoveObjList FamilyGPSPack;
    public BasketList MyBasketList;

    public NoticeList AllNoticeList;
    public void ParseMoveObjectPacket()
    {
        for (int k = 0; k < MovingObjectList.Count; k++)
        {
            MovingObjectList[k].LAT_ = float.Parse(MovingObjectList[k].lat);
            MovingObjectList[k].LONG_ = float.Parse(MovingObjectList[k].lng);
        }
    }

    // 문자열 배열로 딕션너리 요소로 추가하기
    public static void InitAreaIndex()
    {
        foreach (var areaStr in AreaArray.Select((value, index) => (value, index)))
        {
            AreaIndexDic.Add(areaStr.value, areaStr.index);
        }
    }
}

public class NetWorkData : SingletonClass<NetWorkData>
{

    #region NetWorkArea

    ///******************** 서버에서 받은 데이터 저장 ********************/
    //public TaxiInfo TotalTaxiList;          //서버에서 받는 택시 정보 리스트
    //public List<TaxiLocation> TaxiLocList;  //실시간 택시 위치

    //public LogisInfo TotalLogisList;            //서버에서 받는 택배 정보 리스트
    //public List<LogisLocation> LogisLocList;    //실시간 택배 차량 위치

    //public List<WarehouseInfo> TotalWarehouseList; // 서버에서 받는 창고정보 리스트


    ///******************** KPI ********************/
    //public KpiTaxiToday KpiTaxiTodayInfo;                   //KPI 택시 총 이용현황
    //public List<KpiTaxiList> KpiTotalTaxiList;              //KPI 택시리스트(운행 통계, 고객 통계에서 사용)
    //public List<KpiTaxiDangerStats> KpiTaxiDangerStatsList; //KPI 택시 위험발생 통계

    //public KpiLogisToday KpiLogisTodayInfo;                 //KPI 택배 총 이용현황

    //public KpiWarehouseToday KpiWarehouseTodayInfo;         //KPI 창고 총 이용현황 
    #endregion
}