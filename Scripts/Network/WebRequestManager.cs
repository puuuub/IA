using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestManager : SingletonMonoBehaviour<WebRequestManager>
{
    // ��� �۾�Ǯ����
    public static Queue<Action> ActionQueue = new Queue<Action>();
    bool _inProgress;
    public bool RequestInProgress { get { return _inProgress; } set { _inProgress = value; } }
    public static float downloadstart;
    
    Action CurrentAction;

    /// <summary>
    /// StreamingAsset/AlianAPIData.json
    /// ��� API ����
    /// API �߰��� enum API�� �߰��ؾ���
    /// </summary>
    public ApiRoot Urls;
    
    
    public LoginInfo curLoginInfo { get; set; }
    
    public AccountInfo accountInfo { get; set; }

    //api ������ ó���� ���� ����
    public APITYPE curAPIType { get; set; }

    const string LOGINERROR ="�α��� ������ Ȯ���Ͻʽÿ�.";
    const string APIERROR = "��Ʈ��ũ ������ �߻��߽��ϴ�.\n����� �ٽ� �õ����ֽʽÿ�.";
    const string NOAUTH = "������ ��ȸ ������ �����ϴ�.\n������ Ȯ���Ͻʽÿ�.";
    const string SERVERERROR = "���� ������ �ֽ��ϴ�.\n����� �ٽ� �õ����ֽʽÿ�.";

    const string CAPCHAERROR = "���ȼ��ڸ� Ȯ���Ͻʽÿ�.";
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
            // WebRequestUtil���� Coroutine�� ������ �α��� �ϸ�
            if (!RequestInProgress/* && LoginPopUp.IsLogOn*/)
            {
                downloadstart = Time.time;
                //print("download start " + downloadstart);
                //ť���� ���� ���� �ִ� �׸��� ��ȯ�Ѵ�
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
        //������ ����
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
