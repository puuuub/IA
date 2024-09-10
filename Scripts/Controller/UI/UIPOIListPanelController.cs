using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UIPOIListPanelController : MonoBehaviour
{
    [SerializeField]
    UIListPanelRowController[] rowControllers;
    public UnityAction<DeviceType> rowAction { get; set; }
    
    [SerializeField]
    Image[] unused_imgs;

    [SerializeField]
    Toggle all_tg;
    public UnityAction<bool> all_act { get; set; }

    // Start is called before the first frame update
    void Start()
    {

        all_tg.onValueChanged.AddListener(all_act);
        all_tg.onValueChanged.AddListener(x=>
        {
            if (x)
            {
                for (int i = 0; i < rowControllers.Length; i++)
                {
                    rowControllers[i].SetTextColor(Color.white);
                }
            }
        });

        for (int i = 0; i < rowControllers.Length; i++)
        {
            int idx = i;
            rowControllers[idx].AddRowClickAction(delegate 
            {
                all_tg.isOn = false;
                if (rowAction != null)
                {
                    rowAction.Invoke(DeviceType.BRT + idx);
                }
                for(int j = 0; j < rowControllers.Length; j++)
                {
                    if (rowControllers[j].isActive)
                    {
                        if(j == idx)
                        {
                            rowControllers[j].SetTextColor(Color.yellow);
                        }
                        else
                        {
                            rowControllers[j].SetTextColor(Color.white);
                        }
                    }
                }
            });
        }

        SetContentsOnOff(false);
    }
    
    public void SetAllToggleInter(bool isOn)
    {
        all_tg.interactable = isOn;
    }

    public void SetContentsOnOff(bool isOn)
    {
        gameObject.SetActive(isOn);
    }

    public void SetUnused(List<int> used)
    {
        if(used != null)
        {
            for (int i = 0; i < rowControllers.Length; i++)
            {
                if (used.Contains(i))
                {
                    rowControllers[i].SetRowInterect(true);
                }
                else
                {
                    rowControllers[i].SetRowInterect(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < rowControllers.Length; i++)
            {
                rowControllers[i].SetRowInterect(false);    
            }
        }
    }
}
