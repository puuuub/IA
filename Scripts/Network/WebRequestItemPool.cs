using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

class LoginForm
{
    public string userid;
    public string password;
    public string captcha;
}
public class WebRequestItemPool : SingletonClass<WebRequestItemPool>
{
    float startTime;
    
    bool OneChance = true;

    RSA_Key CurrentRSA_key;

    UserInfo LoginUser_info;
    bool IsLogin = false;


    const string Authorization = "Authorization";
    const string bear = "Bearer";

    public UserInfo GetUserInfo()
    {
        return LoginUser_info;
    }
    // RSA 암호화
    public string RSAEncrypt(string getValue, string pubKey)
    {
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(pubKey);
            //암호화할 문자열을 UFT8인코딩
            byte[] inbuf = (new UTF8Encoding()).GetBytes(getValue);

            byte[] bytText = new byte[getValue.Length];

            for (int i = 0; i < getValue.Length; i++)
            {
                bytText[i] = Convert.ToByte(getValue[i]);
            }
            byte[] bytEncText = rsa.Encrypt(bytText, false);
            return BitConverter.ToString(bytEncText).Replace("-", string.Empty); ;
        }
    }


    public static byte[] HexStringToByteArray(string hexString)
    {
        MemoryStream stream = new MemoryStream(hexString.Length / 2);
        for (int i = default(int); i < hexString.Length; i += 2)
        {
            stream.WriteByte(byte.Parse(hexString.Substring(i, 2), System.Globalization.NumberStyles.AllowHexSpecifier));
        }
        return stream.ToArray();
    }


    public static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
    {
        try
        {
            byte[] encryptedData;
            //Create a new instance of RSACryptoServiceProvider.
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {

                //Import the RSA Key information. This only needs
                //toinclude the public key information.
                RSA.ImportParameters(RSAKeyInfo);

                //Encrypt the passed byte array and specify OAEP padding.  
                //OAEP padding is only available on Microsoft Windows XP or
                //later.  
                encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
            }
            return encryptedData;
        }
        //Catch and display a CryptographicException  
        //to the console.
        catch (CryptographicException e)
        {
            DebugScrollView.Instance.Print(e.Message);

            return null;
        }
    }

    public void EnqueueRSA()
    {
        WebRequestManager.Instance.EnqueueAction(delegate { RequestPostRSA(); });
    }

    void RequestPostRSA()
    {
    //    WebRequestUtil.Instance.EnableBlockingWait = true;
    //    WWWForm form = null;
    //    //WebRequestUtil.Instance.Post(WebRequestManager.Instance, "http://aliandev.iptime.org:38111/mnc/rsa/getRsaInfo", form);
    //    WebRequestUtil.Instance.Post(WebRequestManager.Instance, NetProtocol.BaseURL 
    //        + WebRequestManager.Instance.Urls.ApiUrl[((int)NetProtocol.API_URL.rsa_getRsaInfo)].path, form);
    //    WebRequestUtil.Instance.AddSuccessAction(UpdateRSA);
    }


    void UpdateRSA()
    {
        CurrentRSA_key = JsonUtil.JsonToObject<RSA_Key>(WebRequestUtil.Instance.ResultString);
    }


    public void EnqueueLogin(string id, string pw, string captCha)
    {
        string publicKeyText;
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            RSAParameters publicKey = new RSAParameters();
            
            // 서버에서 받은 RSA정보인 modulus(길이256의 문자열)을 128바이트의 16진수 문자열로 변환
            // 서버에서 이런 프로세스를 거치는지 알려주지 않으면 클라이언트에서 알 수가 없다.
            publicKey.Modulus = HexStringToByteArray(CurrentRSA_key.data.modulus);
            // 16진수를 만들기위해 길이를 짝수로 조정
            if(CurrentRSA_key.data.exponent.Length % 2 != 0)
            {
                CurrentRSA_key.data.exponent = "0" + CurrentRSA_key.data.exponent;
            }
            publicKey.Exponent = HexStringToByteArray(CurrentRSA_key.data.exponent);//Convert.FromBase64String("10001"); //JAVA에서 생성한 값
            rsa.ImportParameters(publicKey);
            publicKeyText = rsa.ToXmlString(false);
        }

        string crypLoginId = RSAEncrypt(id, publicKeyText);
        string crypUserPwd = RSAEncrypt(pw, publicKeyText);

        WebRequestManager.Instance.EnqueueAction(delegate { RequestPostLogin(crypLoginId, crypUserPwd, captCha); });
    }

    public void RquestWeatherApi(DateTime date)
    {
        // WebRequestManager를 이용한 요청이 아님
        WebRequestUtil wru = new WebRequestUtil(false);
        wru.EnableBlockingWait = false;
        string wheaterBaseURL = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getUltraSrtFcst?";
        string wheaterKey = "jt7E6fVTyX1CTX2UMUQ%2Fy8SqFwecATm%2BZSxEXbicTOXtz5JinfcRRnZZA4D3HDjw3Ni8uwdwV6ShGBRNg3DFfg%3D%3D";
        DateTime dt = date;

        wru.Get(WebRequestManager.Instance, wheaterBaseURL+ "serviceKey="+ wheaterKey + "&pageNo=1&numOfRows=100&dataType=JSON&base_date="+dt.ToString("yyyyMMdd")+"&base_time=" + dt.AddHours(-1f).ToString("HHmm")+ "&nx=58&ny=126"); //query 처리
        wru.AddSuccessAction(delegate
        {
            //mainManager.SetWeather(wru.ResultString);
        });
        wru.AddFailAction(delegate {
            //mainManager.WeatherFail();
        });
    }


    public void RequestPostLogin(string id, string pw, string captCha)
    {
        WebRequestUtil.Instance.EnableBlockingWait = true;
        WebRequestManager.Instance.curAPIType = APITYPE.LOGIN;
        
        LoginForm lf = new LoginForm();
        lf.userid = id;
        lf.password = pw;
        lf.captcha = captCha;

        string formJson = JsonUtil.ObjectToJsonEx(lf);

        Guid uuid = Guid.NewGuid();
        WebRequestUtil.Instance.HeaderList.Add(new WebReqHeader("tranId", uuid.ToString()));
        WebRequestUtil.Instance.Post(WebRequestManager.Instance
            , WebRequestManager.Instance.Urls.API[(int)API.api_base].address + WebRequestManager.Instance.Urls.API[((int)API.api_login)].address,
            formJson);
        WebRequestUtil.Instance.AddSuccessAction(delegate 
        {
            WebRequestManager.Instance.SetLoginInfo(WebRequestUtil.Instance.ResultString, id);
            MainManager.Instance.SuccessLogin();
            //DebugScrollView.Instance.Print(WebRequestUtil.Instance.ResultString);
        });
    }


    public void RequestDeviceInfo()
    {
        WebRequestUtil wru = new WebRequestUtil();
        WebRequestManager.Instance.curAPIType = APITYPE.DEVICE;

        string emptyResources = "{ \"uris\": []}";

        Guid uuid = Guid.NewGuid();
        wru.HeaderList.Add(new WebReqHeader("tranId", uuid.ToString()));
        wru.HeaderList.Add(new WebReqHeader(Authorization, bear + WebRequestManager.Instance.curLoginInfo.token));
        wru.Post(WebRequestManager.Instance
            , WebRequestManager.Instance.Urls.API[(int)API.api_base].address + WebRequestManager.Instance.Urls.API[((int)API.api_device_listwithresources)].address,
            emptyResources);
        wru.AddSuccessAction(delegate
        {
            MainManager.Instance.SuccessDeviceInfo(wru.ResultString);
            //DebugScrollView.Instance.Print(WebRequestUtil.Instance.ResultString);
        });
    }

    public void RequestDeviceRecordLatest(string objId, string json, DeviceType type)
    {
        WebRequestUtil.Instance.EnableBlockingWait = true;
        WebRequestManager.Instance.curAPIType = APITYPE.DEVICE;

        Guid uuid = Guid.NewGuid();
        WebRequestUtil.Instance.HeaderList.Add(new WebReqHeader("tranId", uuid.ToString()));
        WebRequestUtil.Instance.HeaderList.Add(new WebReqHeader(Authorization, bear + WebRequestManager.Instance.curLoginInfo.token));
        WebRequestUtil.Instance.Post(WebRequestManager.Instance
            , WebRequestManager.Instance.Urls.API[(int)API.api_base].address + WebRequestManager.Instance.Urls.API[((int)API.api_datarecord_deviceId_latest)].address,
            json);
        WebRequestUtil.Instance.AddSuccessAction(delegate
        {
            MainManager.Instance.SuccessDeviceRecord(objId, type, WebRequestUtil.Instance.ResultString);
            //DebugScrollView.Instance.Print(WebRequestUtil.Instance.ResultString);
        });
    }
    public void RequestCaptcha()
    {
        WebRequestUtil.Instance.EnableBlockingWait = true;
        Guid uuid = Guid.NewGuid();

        WebRequestUtil.Instance.HeaderList.Add(new WebReqHeader("tranId", uuid.ToString()));
        WebRequestUtil.Instance.Get(WebRequestManager.Instance, WebRequestManager.Instance.Urls.API[(int)API.api_base].address + WebRequestManager.Instance.Urls.API[(int)API.api_captcha].address);
        WebRequestUtil.Instance.AddSuccessAction(delegate 
        {
            CaptchaClass captcha = JsonUtil.JsonToObject<CaptchaClass>(WebRequestUtil.Instance.ResultString);
            MainManager.Instance.SuccessCaptcha(captcha.message);
        });


    }

    public void RequestTokenRefresh()
    {
        WebRequestUtil wru = new WebRequestUtil();

        WebRequestManager.Instance.curAPIType = APITYPE.LOGIN;
       
        
        Guid uuid = Guid.NewGuid();
        wru.HeaderList.Add(new WebReqHeader(Authorization, bear + WebRequestManager.Instance.curLoginInfo.refreshtoken));
        wru.HeaderList.Add(new WebReqHeader("tranId", uuid.ToString()));
        wru.Post(WebRequestManager.Instance
            , WebRequestManager.Instance.Urls.API[(int)API.api_base].address + WebRequestManager.Instance.Urls.API[((int)API.api_token)].address, "");
        wru.AddSuccessAction(delegate
        {
            WebRequestManager.Instance.RefreshLoginInfo(wru.ResultString);

        });
    }

    public void RequestDeviceAct(string deviceId, string objId, string instanceId, string resourcesId, Action successAct)
    {

        WebRequestUtil wru = new WebRequestUtil();

        WebRequestManager.Instance.curAPIType = APITYPE.DEVICE;

        string pathParameter = deviceId + StaticText.SLASH + objId + StaticText.SLASH + instanceId + StaticText.SLASH + resourcesId;
        

        Guid uuid = Guid.NewGuid();
        wru.HeaderList.Add(new WebReqHeader(Authorization, bear + WebRequestManager.Instance.curLoginInfo.refreshtoken));
        wru.HeaderList.Add(new WebReqHeader("tranId", uuid.ToString()));
        wru.Post(WebRequestManager.Instance
            , WebRequestManager.Instance.Urls.API[(int)API.api_base].address + WebRequestManager.Instance.Urls.API[((int)API.api_device_act)].address+ pathParameter, "");
        wru.AddSuccessAction(delegate
        {
            successAct.Invoke();
        });
    }
    public void RequestDevicePlace(string id)
    {
        WebRequestUtil wru = new WebRequestUtil();
        WebRequestManager.Instance.curAPIType = APITYPE.DEVICE;

        WebRequestUtil.Instance.EnableBlockingWait = true;
        Guid uuid = Guid.NewGuid();

        wru.HeaderList.Add(new WebReqHeader("tranId", uuid.ToString()));
        wru.HeaderList.Add(new WebReqHeader(Authorization, bear + WebRequestManager.Instance.curLoginInfo.token));
        wru.Get(WebRequestManager.Instance, 
            WebRequestManager.Instance.Urls.API[(int)API.api_base].address + WebRequestManager.Instance.Urls.API[(int)API.api_deviceutil_place_stats_orgId].address+id);
        wru.AddSuccessAction(delegate
        {
            MainManager.Instance.SuccessDevicePlace(id, wru.ResultString);
        });

    }
    public void RequestAccountInfo()
    {
        WebRequestUtil wru = new WebRequestUtil();
        WebRequestManager.Instance.curAPIType = APITYPE.LOGIN;

        WebRequestUtil.Instance.EnableBlockingWait = true;
        Guid uuid = Guid.NewGuid();

        wru.HeaderList.Add(new WebReqHeader("tranId", uuid.ToString()));
        wru.HeaderList.Add(new WebReqHeader(Authorization, bear + WebRequestManager.Instance.curLoginInfo.token));
        wru.Get(WebRequestManager.Instance,
            WebRequestManager.Instance.Urls.API[(int)API.api_base].address + WebRequestManager.Instance.Urls.API[(int)API.api_account_userId_myinfo].address + WebRequestManager.Instance.curLoginInfo.id + "/myinfo");
        wru.AddSuccessAction(delegate
        {
            MainManager.Instance.SuccessAccountInfo(wru.ResultString);
        });

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="deviceType"></param>
    /// <param name="date">"yyyy-MM-dd HH:mm:ss"</param>
    public void RequestDeviceErrorTop(string deviceType, string date, Action<string> successAction)
    {
        WebRequestUtil wru = new WebRequestUtil();

        WebRequestUtil.Instance.EnableBlockingWait = true;
        Guid uuid = Guid.NewGuid();

        string apiParameter = "/" + deviceType + "/top?statTime=" + date + "&topCnt=15&statType=hour";

        wru.HeaderList.Add(new WebReqHeader("tranId", uuid.ToString()));
        wru.HeaderList.Add(new WebReqHeader(Authorization, bear + WebRequestManager.Instance.curLoginInfo.token));
        wru.Get(WebRequestManager.Instance,
            WebRequestManager.Instance.Urls.API[(int)API.api_base].address + WebRequestManager.Instance.Urls.API[(int)API.api_dts_event_deviceType_top].address+ apiParameter);
        wru.AddSuccessAction(delegate
        {
            successAction.Invoke(wru.ResultString);
        });
    }

    public void RequestDeviceInfoList()
    {
        WebRequestUtil wru = new WebRequestUtil();

        WebRequestManager.Instance.curAPIType = APITYPE.DEVICE;

        WebRequestUtil.Instance.EnableBlockingWait = true;
        Guid uuid = Guid.NewGuid();

        string apiParameter = "?organizationId=1655&size=15&sort=DESC&sortCol=CT_DOOR_COUNT";

        wru.HeaderList.Add(new WebReqHeader("tranId", uuid.ToString()));
        wru.HeaderList.Add(new WebReqHeader(Authorization, bear + WebRequestManager.Instance.curLoginInfo.token));
        wru.Get(WebRequestManager.Instance,
            WebRequestManager.Instance.Urls.API[(int)API.api_base].address + WebRequestManager.Instance.Urls.API[(int)API.api_device_device_infolist].address + apiParameter);
        wru.AddSuccessAction(delegate
        {
            MainManager.Instance.SuccessDeviceInfoList(wru.ResultString);
        });
    }


    public void RequestDeviceDoorCount(string orgId)
    {
        WebRequestUtil wru = new WebRequestUtil();

        string apiParameter = "?organizationCodeL2="+orgId+"&range1=0&range2=500000&range3=800000&range4=1000000";
        WebRequestManager.Instance.curAPIType = APITYPE.DEVICE;

        WebRequestUtil.Instance.EnableBlockingWait = true;
        Guid uuid = Guid.NewGuid();

        wru.HeaderList.Add(new WebReqHeader("tranId", uuid.ToString()));
        wru.HeaderList.Add(new WebReqHeader(Authorization, bear + WebRequestManager.Instance.curLoginInfo.token));
        wru.Get(WebRequestManager.Instance,
            WebRequestManager.Instance.Urls.API[(int)API.api_base].address 
            + WebRequestManager.Instance.Urls.API[(int)API.api_device_deviceId_prevent_count].address
            + apiParameter);
        wru.AddSuccessAction(delegate
        {
            MainManager.Instance.SuccessDeviceDoorCount(orgId, wru.ResultString);
        });
    }


    void CheckLogin()
    {
        EnqueueUserInfo();
    }

    public void SendControlApi(string jsonbase, List<string> JsonList)
    {
        for(int i = 0; i < JsonList.Count; i++)
        {
            DebugScrollView.Instance.Print("SendControlApi_" + i);
            EnqueueControlApi(jsonbase + JsonList[i]);
        }
    }
    void EnqueueControlApi(string bodydata)
    {
        DebugScrollView.Instance.Print("EnqueueControlApi!! " );
        WebRequestManager.Instance.EnqueueAction(delegate { RequestPostControlApi(bodydata); });
    }
    void RequestPostControlApi(string bodydata)
    {
        //WebRequestUtil.Instance.EnableBlockingWait = false;
        //// 헤더처리
        //WebRequestUtil.Instance.HeaderList.Add(new WebReqHeader("accessMode", "client"));
        //WebRequestUtil.Instance.HeaderList.Add(new WebReqHeader("accessToken", LoginUser_info.token));
        //WebRequestUtil.Instance.HeaderList.Add(new WebReqHeader("serviceType", "gigasafe"));

        ////WWWForm form = null;
        ////WebRequestUtil.Instance.Post(WebRequestManager.Instance, "http://aliandev.iptime.org:38111/mnc/login/sessionIdCheck", "");  //form => ""
        //Debug.Log("WebRequestItemPool : " + bodydata);
        //WebRequestUtil.Instance.Post(WebRequestManager.Instance, NetProtocol.DigtalTweenBaseURL
        //    + WebRequestManager.Instance.Urls.ApiUrl[((int)NetProtocol.API_URL.dt_legacy_legacyCtrlComm)].path, bodydata);
        
    }

    
    public void EnqueueUserInfo()
    {
        WebRequestManager.Instance.EnqueueAction(delegate { RequestPostUserInfo(); });
    }
    void RequestPostUserInfo()
    {
        WebRequestUtil.Instance.EnableBlockingWait = true;
        //WWWForm form = null;
        //WebRequestUtil.Instance.Post(WebRequestManager.Instance, "http://aliandev.iptime.org:38111/mnc/login/sessionIdCheck", "");  //form => ""
        WebRequestUtil.Instance.Post(WebRequestManager.Instance, WebRequestManager.Instance.Urls.API[(int)API.api_base].address
            + WebRequestManager.Instance.Urls.API[((int)NetProtocol.API_URL.login_sessionIdCheck)].address, "");  //form => ""
        WebRequestUtil.Instance.AddSuccessAction(UpdateUserInfo);
    }
    
    public void EnqueueTest()
    {
        WebRequestManager.Instance.EnqueueAction(delegate { RquestTest(); });
    }


    public void RquestTest()
    {
        // 헤더처리
        WebRequestUtil.Instance.HeaderList.Add(new WebReqHeader("accessMode", "client"));
        WebRequestUtil.Instance.HeaderList.Add(new WebReqHeader("accessToken", LoginUser_info.token));
        WebRequestUtil.Instance.HeaderList.Add(new WebReqHeader("serviceType", "gigasafe"));
        WebRequestUtil.Instance.EnableBlockingWait = true;
        //WWWForm form = null;
        WebRequestUtil.Instance.Get(WebRequestManager.Instance, WebRequestManager.Instance.Urls.API[(int)API.api_base].address
            + WebRequestManager.Instance.Urls.API[((int)NetProtocol.API_URL.dt_getSiteInfoList)].address
            + "?authId=" + LoginUser_info.authId); //query 처리
    }

    void UpdateUserInfo()
    {
        LoginUser_info = JsonUtil.JsonToObject<UserInfo>(WebRequestUtil.Instance.ResultString);
        OneChance = true;
        //LoginView.Instance.StartCoroutine(OnUpdateCheckToken());
        //CameraMoveToController.Instance.LoginMove();
        IsLogin = true;
        //GetAllSensorApi();
    }
    
    IEnumerator OnUpdateCheckToken()
    {
        float interval = 10;

        startTime = Time.time - interval; // 시작하자 마자 바로 체크
        //startTime = Time.time; // 시작후 interval초후 체크
        //RequestGet();
        while (true)
        {
            yield return null;
            float deltaTime = Time.time - startTime;
            
            if (OneChance && deltaTime > interval) // 10초간격
            {
                RequestCheckToken();
                OneChance = false;
            }
        }
    }

    void RequestCheckToken()
    {
        // 헤더에 셋팅
        WebReqHeader wh = new WebReqHeader("token", LoginUser_info.token);
        
        // WebRequestManager를 이용한 요청이 아님
        WebRequestUtil tokenReq = new WebRequestUtil(false);
        tokenReq.EnableBlockingWait = false;

        DebugScrollView.PrintEx("token : " + LoginUser_info.token);
        tokenReq.HeaderList.Add(wh);
        tokenReq.Post(WebRequestManager.Instance, WebRequestManager.Instance.Urls.API[(int)API.api_base].address
            + WebRequestManager.Instance.Urls.API[((int)NetProtocol.API_URL.login_chkToken)].address, "");
        tokenReq.AddSuccessAction(UpdateCheckToken);
        tokenReq.AddFailAction(Retry);
        tokenReq.AddConnectResultErrorAction(Retry);   
    }

    void UpdateCheckToken()
    {
        DebugScrollView.PrintEx("UpdateCheckToken");
        startTime = Time.time;
        OneChance = true;
    }

    void Retry()
    {
        startTime += 10; // 10초후 재요청
        OneChance = true;
    }

    public void Logout()
    {
        IsLogin = false;
        OneChance = false;
        //LoginView.Instance.StopCoroutine(OnUpdateCheckToken());
        //WebSocketController.Instance.Disconnect();
        
    }
}
