using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISOPController : MonoBehaviour
{
    [SerializeField]
    GameObject contents;

    [SerializeField]
    Button exit_btn;

    [Header("LEFT")]
    [SerializeField]
    List<SOPRowController> rowList;

    [Header("RIGHT")]
    [SerializeField]
    List<TMP_Text> scenario_txts;
    [SerializeField]
    List<Image> scenario_imgs;


    [Header("Resources")]
    [SerializeField]
    Sprite[] arrows_sps;

    
    // Start is called before the first frame update
    void Start()
    {
        SetContentsOnOff(false);

        for(int i = 0; i < rowList.Count; i++)
        { 
            DeviceType type = DeviceType.BRT+i;

            rowList[i].AddToggleAction(x => 
            {
                if (x)
                {
                    RowToggleAction(type);
                }            
            });
        }
    }

    public void SetContentsOnOff(bool isOn)
    {
        contents.SetActive(isOn);
        if (isOn)
        {
            rowList[0].row_tg.isOn = true;
        }

    }

    void RowToggleAction(DeviceType type)
    {
        DebugScrollView.Instance.Print("row : " + type.ToString());
        
    }
}
