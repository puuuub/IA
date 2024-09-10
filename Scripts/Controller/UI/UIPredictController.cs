using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChartAndGraph;

public class UIPredictController : MonoBehaviour
{
    [SerializeField]
    List<UIPredictGaugeController> gaugeList;

    [SerializeField]
    BarChart barChart;

    
    [SerializeField]
    List<UIEventRowController> rowList;

    List<DeviceType> chartGroup;
    List<string> chartCate;

    Dictionary<string, Dictionary<string, double>> chartValDic;

    const int GAUGEMAX = 1000000;

    // Start is called before the first frame update
    void Start()
    {
        chartGroup = new List<DeviceType>();
        chartGroup.Add(DeviceType.BEG);
        chartGroup.Add(DeviceType.BDG);
        chartGroup.Add(DeviceType.DPG);
        chartGroup.Add(DeviceType.SBG);
        chartGroup.Add(DeviceType.ATG);
        chartGroup.Add(DeviceType.TSG);

        chartCate = new List<string>();
        chartCate.Add("Cate1");
        chartCate.Add("Cate2");
        chartCate.Add("Cate3");
        chartCate.Add("Cate4");

        chartValDic = new Dictionary<string, Dictionary<string, double>>();



        SetContentsOff();
    }

    public void SetContentsOn()
    {
        gameObject.SetActive(true);

        //gauge
        var datas = DataManager.Instance.deviceInfoList.content;
        
        for(int i = 0; i < 5; i++)
        {
            if (i < datas.Count)
            {
                string[] orgNames = datas[i].orgNames.Split(StaticText.SLASH);
                string deviceKor = string.Empty;
                for(DeviceType j = DeviceType.BRT; j <= DeviceType.TSG; j++)
                {
                    if (j.ToString().Equals(datas[i].tag))
                    {
                        deviceKor = MenuTree.GetDeviceTypeKor(j);
                    }
                }
                string orgName = orgNames[1] + StaticText.BAR + orgNames[orgNames.Length - 1] + StaticText.BAR + deviceKor;

                float rotY = (float)datas[i].ctDoorCount / GAUGEMAX * (-180f);

                gaugeList[i].SetContents(string.Format("{0:N0}", datas[i].ctDoorCount), orgName, rotY);
            }
            else
            {
                gaugeList[i].SetContentsOff();
            }
        }


        //chart
        //chartValDic reset
        chartValDic.Clear();
        for (int i = 0; i < chartGroup.Count; i++)
        {
            chartValDic.Add(chartGroup[i].ToString(), new Dictionary<string, double>());
            for (int j = 0; j < chartCate.Count; j++)
            {
                chartValDic[chartGroup[i].ToString()].Add(chartCate[j], 0);
            }
        }
        
        var doorDic = DataManager.Instance.deviceDoorDic;
        
        foreach(string key in doorDic.Keys)
        {
            for(int i = 0; i < chartGroup.Count; i++)
            {
                var data = doorDic[key].Find(x => x.deviceTag.Equals(chartGroup[i].ToString()));
                if(data != null)
                {
                    chartValDic[chartGroup[i].ToString()][chartCate[0]] += data.doorRange1;
                    chartValDic[chartGroup[i].ToString()][chartCate[1]] += data.doorRange2;
                    chartValDic[chartGroup[i].ToString()][chartCate[2]] += data.doorRange3;
                    chartValDic[chartGroup[i].ToString()][chartCate[3]] += data.doorRange4;
                }

            }
        }

        SetChartOn(chartValDic);


        //table
        for(int i = 0; i < 10; i++)
        {
            int dataIdx = i + 5;
            if(dataIdx < datas.Count)
            {
                string[] txts = new string[4];
                txts[0] = datas[dataIdx].deviceId;

                string deviceKor = string.Empty;
                for (DeviceType j = DeviceType.BRT; j <= DeviceType.TSG; j++)
                {
                    if (j.ToString().Equals(datas[dataIdx].tag))
                    {
                        deviceKor = MenuTree.GetDeviceTypeKor(j);
                    }
                }
                txts[1] = deviceKor;

                string[] orgNames = datas[dataIdx].orgNames.Split(StaticText.SLASH);
                txts[2] = orgNames[1] + StaticText.SLASH + orgNames[orgNames.Length - 1];
                txts[3] = string.Format("{0:N0}", datas[dataIdx].ctDoorCount);

                rowList[i].SetContentsOn(txts, ActionNull);

            }
            else
            {
                rowList[i].SetContentsOff();
            }
        }
    }

    public void SetContentsOff()
    {
        gameObject.SetActive(false);

    }

    public void SetChartOn(Dictionary<string, Dictionary<string, double>> valueDic)
    {
        gameObject.SetActive(true);

        //barChart.ViewType = BarChart.BarType.Stacked;

        barChart.DataSource.StartBatch();

        barChart.DataSource.ClearValues();
        //barChart.DataSource.ClearCategories();
        //barChart.DataSource.ClearGroups();

        int max = 0;

        foreach (string group in valueDic.Keys)
        {
            foreach (string category in valueDic[group].Keys)
            {
                if(max < valueDic[group][category])
                {
                    max = (int)valueDic[group][category];
                }
                barChart.DataSource.SetValue(category, group, valueDic[group][category]);
            }
        }
        barChart.DataSource.MaxValue = ((max / 10) + 1) * 10;

        barChart.DataSource.EndBatch();
    }

    void ActionNull()
    {

    }
}
