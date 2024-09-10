using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestManager : SingletonMonoBehaviour<WebRequestManager>
{
    // 통신 작업풀관리
    public static Queue<Action> ActionQueue = new Queue<Action>();
    bool _inProgress;
    public bool RequestInProgress { get { return _inProgress; } set { _inProgress = value; } }
    public static float downloadstart;
    
    Action CurrentAction;

    /// <summary>
    /// StreamingAsset/AlianAPIData.json
    /// 사용 API 관리
    /// API 추가시 enum API도 추가해야함
    /// </summary>
    public ApiRoot Urls;
    
    
    public LoginInfo curLoginInfo { get; set; }
    
    public AccountInfo accountInfo { get; set; }

    //api 문제시 처리를 위한 구분
    public APITYPE curAPIType { get; set; }

    const string LOGINERROR ="로그인 정보를 확인하십시오.";
    const string APIERROR = "네트워크 문제가 발생했습니다.\n잠시후 다시 시도해주십시오.";
    const string NOAUTH = "계정의 조회 권한이 없습니다.\n계정을 확인하십시오.";
    const string SERVERERROR = "서버 문제가 있습니다.\n잠시후 다시 시도해주십시오.";

    const string CAPCHAERROR = "보안숫자를 확인하십시오.";
    const string INCORRECTCAPCHA = "Incorrect captcha";


    public void EnqueueAction(Action act)
    {
        ActionQueue.Enqueue(act);
        DebugScrollView.Instance.Print("ActionQueue.Count : " + ActionQueue.Count);
        if (ActionQueue.Count == 1)
        {
            StartCoroutine(UpdateQueue());
        }
    }


    IEnumerator UpdateQueue()
    {
        //BusyWating.ins.ShowWithCount();
        while (ActionQueue.Count > 0)
        {
            // WebRequestUtil에서 Coroutine이 끝나고 로그인 하면
            if (!RequestInProgress/* && LoginPopUp.IsLogOn*/)
            {
                downloadstart = Time.time;
                //print("download start " + downloadstart);
                //큐에서 가장 위에 있는 항목을 반환한다
                Action act = CurrentAction = ActionQueue.Peek();
                act.Invoke();

            }
            yield return null; // new WaitForSeconds(1.0f);
        }
        //BusyWating.ins.HideWithCount();
    }

    private void Awake()
    {
        Urls = JsonUtil.LoadJsonData<ApiRoot>("AlianAPIData");

        //NetProtocol.BaseURL = WebRequestManager.Instance.Urls.ApiUrl[((int)NetProtocol.API_URL.BASE_URL)].path;
        //NetProtocol.DigtalTweenBaseURL = WebRequestManager.Instance.Urls.ApiUrl[((int)NetProtocol.API_URL.DT_BASE_URL)].path;
    }

    public void FinishActionQ()
    {
        if(ActionQueue.Count > 0 && RequestInProgress)
            ActionQueue.Dequeue();

        RequestInProgress = false;
    }

    public void SetLoginInfo(string apiresult, string id)
    {
        curLoginInfo = JsonUtil.JsonToObject<LoginInfo>(apiresult);
        curLoginInfo.id = id;
    }

    public void RefreshLoginInfo(string apiResult)
    {
        LoginInfo info = JsonUtil.JsonToObject<LoginInfo>(apiResult);
        curLoginInfo.token = info.token;
        curLoginInfo.refreshtoken = info.refreshtoken;

    }
    public string GetErrorMessage(UnityWebRequest uwr)
    {
        long error = uwr.responseCode;

        if (error.Equals(500))
        {
            return SERVERERROR;
        }
        else
        {
            if (curAPIType.Equals(APITYPE.LOGIN))
            {
                if (error.Equals(400))
                {
                    LoginResponseClass temp = JsonUtil.JsonToObject<LoginResponseClass>(uwr.downloadHandler.text);
                    if (temp.message.Equals(INCORRECTCAPCHA))
                    {
                        return CAPCHAERROR;
                    }
                    else
                    {
                        return LOGINERROR;
                    }
                }
                else if (error.Equals(401))
                {
                    return LOGINERROR;
                }
            }
            else if (curAPIType.Equals(APITYPE.DEVICE))
            {
                if (error.Equals(403))
                {
                    return NOAUTH;
                }
            }

        }
        return APIERROR;
    }

    public bool SuccessRequest(string apiResult)
    {
        //데이터 검증
        if (curAPIType.Equals(APITYPE.LOGIN))
        {

            return true;
        }
        else if(curAPIType.Equals(APITYPE.DEVICE))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetAccountInfo(string apiResult)
    {
        accountInfo = JsonUtil.JsonToObject<AccountInfo>(apiResult);
    }
}
