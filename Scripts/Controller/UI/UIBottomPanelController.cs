using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIBottomPanelController : MonoBehaviour
{

    [SerializeField]
    ToggleGroup bottomToggleGroup;

    [SerializeField]
    Toggle dashBoard_tg;
    [SerializeField]
    Toggle sop_tg;
    [SerializeField]
    Toggle predict_tg;

    public UIDashBoardController uiDashBoardController;
    
    //화면분할용
    public UIDashBoardController uiDashBoardMINIController;

    [SerializeField]
    UISOPController uiSopController;
    [SerializeField]
    UIPredictController uiPredictController;

    const string ALLDEVICE = "등록단말";
    const string OPERDEVICE = "운영단말";
    const string AUTODEVICE = "자동모드";
    const string ABNORMALDEVICE = "비정상종료";
    const string NONOPERDEVICE = "미운영단말";


    // Start is called before the first frame update
    void Start()
    {
        dashBoard_tg.onValueChanged.AddListener(SetBottomGroupToggleBasicAction);
        dashBoard_tg.onValueChanged.AddListener(uiDashBoardController.SetContentsOnOff);
        sop_tg.onValueChanged.AddListener(SetBottomGroupToggleBasicAction);
        sop_tg.onValueChanged.AddListener(uiSopController.SetContentsOnOff);
        predict_tg.onValueChanged.AddListener(SetBottomGroupToggleBasicAction);
        predict_tg.onValueChanged.AddListener(x =>
        {
            if (x)
            {
                uiPredictController.SetContentsOn();
            }
            else
            {
                uiPredictController.SetContentsOff();

            }

        });

        SetDashboard();

        SetContentsOnOff(false);
    }

    void SetBottomGroupToggleBasicAction(bool isOn)
    {
        if (isOn)
        {
            CameraManager.Instance.SetRootMoveOnOff(false);
        }
        else
        {
            if (!bottomToggleGroup.AnyTogglesOn())
            {
                CameraManager.Instance.SetRootMoveOnOff(true);

            }
        }
    }
    public void SetContentsOnOff(bool isOn)
    {
        if (!isOn)
        {
            bottomToggleGroup.SetAllTogglesOff();
        }
        gameObject.SetActive(isOn);

    }
    
    void SetDashboard()
    {
        
        for (int i= 0; i <= (int)DeviceType.TSG; i++)
        {
            DeviceType dt = DeviceType.BRT + i;
            uiDashBoardController.SetDeviceTypeToggleAction((int)dt, delegate
            {
                SetDashBoardToggle(uiDashBoardController, dt);
            });

            uiDashBoardMINIController.SetDeviceTypeToggleAction((int)dt, delegate
            {
                SetDashBoardToggle(uiDashBoardMINIController, dt);
            });   
        }
    }

    public void SetDashBoardToggle(UIDashBoardController targetController, DeviceType dt)
    {

        int[] totals = new int[4];
        int[] opers = new int[4];
        int[] autos = new int[4];
        int[] abnormals = new int[4];

        var devicePlaceDic = DataManager.Instance.devicePlaceDic;
        var datas  = DataManager.Instance.deviceResList.FindAll(x => x.deviceType.Equals(dt.ToString()));
        
        List<string> deviceTypeCode = new List<string>();
        for(int i=0;i< datas.Count; i++)
        {
            deviceTypeCode.Add(datas[i].deviceTypeCode);

        }

        var dicDatas = devicePlaceDic[StaticText.T1Code].details.FindAll(x => deviceTypeCode.Contains(x.type.ToString()));
        DevicePlace t1Device;
        if (dicDatas.Count > 1)
        {
            t1Device = DevicePlace.Add(dicDatas[0], dicDatas[1]);
        }
        else if (dicDatas.Count > 0)
        {
            t1Device = dicDatas[0];
        }
        else
        {
            t1Device = new DevicePlace();
        }

        dicDatas = devicePlaceDic[StaticText.CACode].details.FindAll(x => deviceTypeCode.Contains(x.type.ToString()));
        DevicePlace caDevice;
        if (dicDatas.Count > 1)
        {
            caDevice = DevicePlace.Add(dicDatas[0], dicDatas[1]);
        }
        else if (dicDatas.Count > 0)
        {
            caDevice = dicDatas[0];
        }
        else
        {
            caDevice = new DevicePlace();
        }

        dicDatas = devicePlaceDic[StaticText.T2Code].details.FindAll(x => deviceTypeCode.Contains(x.type.ToString()));
        DevicePlace t2Device;
        if (dicDatas.Count > 1)
        {
            t2Device = DevicePlace.Add(dicDatas[0], dicDatas[1]);
        }
        else if (dicDatas.Count > 0)
        {
            t2Device = dicDatas[0];
        }
        else
        {
            t2Device = new DevicePlace();
        }


        totals[0] = t1Device.all_cnt + caDevice.all_cnt + t2Device.all_cnt;

        totals[1] = t1Device.all_cnt;
        totals[2] = caDevice.all_cnt;
        totals[3] = t2Device.all_cnt;

        opers[0] = t1Device.operating_cnt + caDevice.operating_cnt + t2Device.operating_cnt;

        opers[1] = t1Device.operating_cnt;
        opers[2] = caDevice.operating_cnt;
        opers[3] = t2Device.operating_cnt;


        autos[0] = t1Device.auto_operation_mode_cnt + caDevice.auto_operation_mode_cnt + t2Device.auto_operation_mode_cnt;

        autos[1] = t1Device.auto_operation_mode_cnt;
        autos[2] = caDevice.auto_operation_mode_cnt;
        autos[3] = t2Device.auto_operation_mode_cnt;


        abnormals[0] = t1Device.abnormal_termination_cnt + caDevice.abnormal_termination_cnt + t2Device.abnormal_termination_cnt;

        abnormals[1] = t1Device.abnormal_termination_cnt;
        abnormals[2] = caDevice.abnormal_termination_cnt;
        abnormals[3] = t2Device.abnormal_termination_cnt;

        uiDashBoardController.SetDashBoard(totals, opers, autos, abnormals);



        
        Dictionary<string, double> chartAllDic = new Dictionary<string, double>();
        chartAllDic.Add(ALLDEVICE, t1Device.all_cnt + caDevice.all_cnt + t2Device.all_cnt);
        chartAllDic.Add(OPERDEVICE, t1Device.operating_cnt + caDevice.operating_cnt + t2Device.operating_cnt);
        chartAllDic.Add(AUTODEVICE, t1Device.auto_operation_mode_cnt + caDevice.auto_operation_mode_cnt + t2Device.auto_operation_mode_cnt);
        chartAllDic.Add(ABNORMALDEVICE, t1Device.abnormal_termination_cnt + caDevice.abnormal_termination_cnt + t2Device.abnormal_termination_cnt);
        chartAllDic.Add(NONOPERDEVICE, (t1Device.all_cnt + caDevice.all_cnt + t2Device.all_cnt) - (t1Device.operating_cnt + caDevice.operating_cnt + t2Device.operating_cnt));

        targetController.SetDashBoardChart(0, chartAllDic, GetChartMax(t1Device.all_cnt + caDevice.all_cnt + t2Device.all_cnt));

        Dictionary<string, double> chartT1Dic = new Dictionary<string, double>();
        chartT1Dic.Add(ALLDEVICE, t1Device.all_cnt);
        chartT1Dic.Add(OPERDEVICE, t1Device.operating_cnt);
        chartT1Dic.Add(AUTODEVICE, t1Device.auto_operation_mode_cnt);
        chartT1Dic.Add(ABNORMALDEVICE, t1Device.abnormal_termination_cnt);
        chartT1Dic.Add(NONOPERDEVICE, t1Device.all_cnt - t1Device.operating_cnt);

        targetController.SetDashBoardChart(1, chartT1Dic, GetChartMax(t1Device.all_cnt));

        Dictionary<string, double> chartCADic = new Dictionary<string, double>();
        chartCADic.Add(ALLDEVICE, caDevice.all_cnt);
        chartCADic.Add(OPERDEVICE, caDevice.operating_cnt);
        chartCADic.Add(AUTODEVICE, caDevice.auto_operation_mode_cnt);
        chartCADic.Add(ABNORMALDEVICE, caDevice.abnormal_termination_cnt);
        chartCADic.Add(NONOPERDEVICE, caDevice.all_cnt - caDevice.operating_cnt);

        targetController.SetDashBoardChart(2, chartCADic, GetChartMax(caDevice.all_cnt));

        Dictionary<string, double> chartT2Dic = new Dictionary<string, double>();
        chartT2Dic.Add(ALLDEVICE, t2Device.all_cnt);
        chartT2Dic.Add(OPERDEVICE, t2Device.operating_cnt);
        chartT2Dic.Add(AUTODEVICE, t2Device.auto_operation_mode_cnt);
        chartT2Dic.Add(ABNORMALDEVICE, t2Device.abnormal_termination_cnt);
        chartT2Dic.Add(NONOPERDEVICE, t2Device.all_cnt - t2Device.operating_cnt);

        targetController.SetDashBoardChart(3, chartT2Dic, GetChartMax(t2Device.all_cnt));
    }

    int GetChartMax(int val)
    {
        int result = 10;
        while(val > result)
        {
            result += 10;
        }


        return result;
    }

 
    public void SetDashboardMINIOnOff(bool isOn)
    {
        uiDashBoardMINIController.SetContentsOnOff(isOn);
    }
}
