using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using ChartAndGraph;


public class UIDashBoardController : MonoBehaviour
{
    [SerializeField]
    GameObject contents;


    [SerializeField]
    Toggle[] deviceType_tgs;

    [SerializeField]
    TMP_Text[] total_txts;

    [SerializeField]
    Image[] oper_imgs;
    [SerializeField]
    TMP_Text[] oper_txts;

    [SerializeField]
    Image[] auto_imgs;
    [SerializeField]
    TMP_Text[] auto_txts;

    [SerializeField]
    Image[] abnormal_imgs;
    [SerializeField]
    TMP_Text[] abnormal_txts;

    [SerializeField]
    Image[] nOper_imgs;
    [SerializeField]
    TMP_Text[] nOper_txts;

    const int CHARTSIZE = 4;


    [SerializeField]
    BarChart[] barCharts;
    [SerializeField]
    VerticalAxis[] barChartVerticals;


    const string GROUP = "Group";

    // Start is called before the first frame update
    void Start()
    {
        
        SetContentsOnOff(false);
    }
    public void SetContentsOnOff(bool isOn)
    {
        contents.SetActive(isOn);
        if (isOn)
        {
            deviceType_tgs[0].isOn = true;
        }


    }


    public void SetDeviceTypeToggleAction(int idx, UnityAction act)
    {
        deviceType_tgs[idx].onValueChanged.AddListener(x =>
        {
            if (x)
            {
                act.Invoke();
            }
        });
    }
    public void SetDashBoard(int[] totals, int[] opers, int[] autos, int[] abnormals)
    {
        for(int i = 0; i < CHARTSIZE; i++)
        {
            if(totals[i] > 0)
            {
                total_txts[i].text = totals[i].ToString() + StaticText.SetSize(17, StaticText.SIK);

                oper_imgs[i].fillAmount = ((float)opers[i] / (float)totals[i]); // > 0 ? (opers[i] / totals[i]) : 0.01f;
                oper_txts[i].text = opers[i].ToString() + StaticText.SetSize(17, StaticText.SIK);

                auto_imgs[i].fillAmount = ((float)opers[i] + autos[i]) / (float)totals[i]; // > 0 ? (opers[i] + autos[i]) / totals[i] : 0.02f;
                auto_txts[i].text = autos[i].ToString() + StaticText.SetSize(17, StaticText.SIK);

                abnormal_imgs[i].fillAmount = ((float)opers[i] + autos[i] + abnormals[i]) / (float)totals[i]; // > 0 ? (opers[i] + autos[i] + abnormals[i]) / totals[i] : 0.03f;
                abnormal_txts[i].text = abnormals[i].ToString() + StaticText.SetSize(17, StaticText.SIK);

                //nOper_imgs[i].fillAmount = 1f;
                nOper_txts[i].text = (totals[i] - (opers[i] + autos[i] + abnormals[i])).ToString() + StaticText.SetSize(17, StaticText.SIK);

            }
            else
            {
                total_txts[i].text = StaticText.ZERO + StaticText.SetSize(17, StaticText.SIK);
                
                oper_imgs[i].fillAmount = 0f;
                oper_txts[i].text = StaticText.ZERO + StaticText.SetSize(17, StaticText.SIK);

                auto_imgs[i].fillAmount = 0f;
                auto_txts[i].text = StaticText.ZERO + StaticText.SetSize(17, StaticText.SIK);

                abnormal_imgs[i].fillAmount = 0f;
                abnormal_txts[i].text = StaticText.ZERO + StaticText.SetSize(17, StaticText.SIK);

                //nOper_imgs[i].fillAmount = 1f;
                nOper_txts[i].text = StaticText.ZERO + StaticText.SetSize(17, StaticText.SIK);

            }

        }

    }

    public void SetDashBoardChart(int idx, Dictionary<string, double> valueDic, int max)
    {
        BarChart targetChart = barCharts[idx];
        VerticalAxis targetVertical = barChartVerticals[idx];
        
        targetChart.DataSource.StartBatch();
        targetChart.DataSource.ClearValues();


        foreach (string category in valueDic.Keys)
        {
            targetChart.DataSource.SetValue(category, GROUP, valueDic[category]);
        }

        targetChart.DataSource.MaxValue = max;

        targetVertical.SubDivisions.Total = max / 10 < 2 ? 2 : max / 10;

        targetChart.DataSource.EndBatch();
    }
}
