using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ApiRoot
{
    public List<ApiData> API { get; set; }
}

public class ApiData
{
    public string index { get; set; }
    public string address { get; set; }
    public string desc { get; set; }
}

public class CaptchaClass 
{
    public string message { get; set; }
}
public class LoginInfo
{
    public string id { get; set; }
    public string token { get; set; }
    // "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJvcmdyb2xlIjp0cnVlLCJpc3MiOiJuZW9pZG0uY29tIiwiaWQiOiJzcDJ1c2VyMSIsInNjb3BlcyI6WyJBQ0NPVU5UX0NSVUQiLCJERVZJQ0VfQ1JVRCIsIkZPVEFfQ1JVRCIsIkxFVkVMX0NSVUQiLCJMV00yTV9DUlVEIiwiT0JKRUNUX0NSVUQiLCJPUkdBTklaQVRJT05fT1dORVIiLCJSVUxFX0NSVUQiLCJXSURHRVRfQ1JVRCJdLCJleHAiOjE3MTA4MjgwMDEsImlhdCI6MTcxMDgyNDQwMSwib3JnSWQiOjE2NTV9.I1QmfF_P61mWkcQTlaiB7XBMkWHLB-fP_S9zabfE_Vo",
    public string refreshtoken { get; set; }        
    //"refreshtoken": "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJuZW9pZG0uY29tIiwicmVmcmVzaCI6InJlZnJlc2giLCJpZCI6InNwMnVzZXIxIiwiZXhwIjoxNzEwODMxNjAxLCJpYXQiOjE3MTA4MjQ0MDF9.ZpHxFpb3aCb60qBNJR5K8N9zm41pH_Hgq1LKVdEtLb8",
    public string imagePath { get; set; }           //"imagePath": null,
    public string orgName { get; set; }             //"orgName": "인천공항",
    public string accountImageType { get; set; }    //"accountImageType": null,
    public string accountImagePreset { get; set; }  //"accountImagePreset": null,
    public string accountImagePath { get; set; }    //"accountImagePath": null,
    public string domain { get; set; }              //"domain": null,
    public string pwTobeChanged { get; set; }       //"pwTobeChanged": false
}
public enum API
{
    
    api_base,
    api_captcha,
    api_login,
    api_token,
    api_account_levels,
    api_account_userId,
    api_account_userId_myinfo,
    api_organization_organizationId_childhierarchy,
    api_organization_orgId,
    api_organization_orgId_users,
    api_device,

    api_device_listwithresources,
    api_device_deviceId,
    api_eventrecord_device,
    api_eventrecord_device_deviceId_list,
    api_deviceutil_place_stats_orgId,
    api_device_list_place_orgId,
    api_datarecord_latest,
    api_datarecord_deviceId_latest,
    api_datarecord_deviceId_history,
    api_dts_event_deviceId_stat,

    api_dts_event_deviceType_top,
    api_dts_device_deviceId_history,
    api_dts_event_placestat,
    api_device_device_infolist,
    api_device_deviceId_prevent_count,

    api_device_write,
    api_device_act,
    api_device_poweron
}

public class RSA_Key
{
    public string responseCode { get; set; }
    public string responseMessage { get; set; }
    public RSA_Data data { get; set; }
}

public class RSA_Data
{
    public string modulus { get; set; }
    public string exponent { get; set; }
}

public class UserInfo
{
    public string authId { get; set; }
    public string customerId { get; set; }
    public string loginId { get; set; }
    public string result { get; set; }
    public string token { get; set; }
    public string aes { get; set; }
    public string topmenuList { get; set; }
    public string userId { get; set; }
    public string userNm { get; set; }
}
