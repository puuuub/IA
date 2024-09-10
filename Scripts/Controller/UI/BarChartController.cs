using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChartAndGraph;

public class BarChartController : MonoBehaviour
{


    [SerializeField]
    BarChart barChart;


    public void SetGraphOn(Dictionary<string, Dictionary<string, double>> valueDic)
    {
        gameObject.SetActive(true);
       
        //barChart.ViewType = BarChart.BarType.Stacked;

        barChart.DataSource.StartBatch();

        barChart.DataSource.ClearValues();
        //barChart.DataSource.ClearCategories();
        //barChart.DataSource.ClearGroups();


        foreach (string group in valueDic.Keys)
        {
            //barChart.DataSource.AddGroup(group);
           

            foreach(string category in valueDic[group].Keys)
            {                
                barChart.DataSource.SetValue(category, group, valueDic[group][category]);
            }
        }


        barChart.DataSource.EndBatch();
    }

   
}
