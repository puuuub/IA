using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;


public class UILayerController : MonoBehaviour
{
    [SerializeField]
    List<Toggle> layerTgList;
    [SerializeField]
    List<TMP_Text> layerTxtList;

    UnityAction layerAllOffAction;


    // Start is called before the first frame update
    void Start()
    {
        
        for(int i = 0; i < layerTgList.Count; i++)
        {
            SetToggleBaisc(layerTgList[i], i);
        }

    }

    void SetToggleBaisc(Toggle tg, int i)
    {
        tg.onValueChanged.AddListener(x => 
        {
            if (x)
            {
                layerTxtList[i].fontStyle = FontStyles.Bold;
            }
            else
            {
                layerTxtList[i].fontStyle = FontStyles.Normal;

            }
        });
    }
    public void SetLayerToggleAction(int i, UnityAction act)
    {
        layerTgList[i].onValueChanged.AddListener(x=> 
        {
            if (x)
            {
                act.Invoke();
            }
            else
            {
                if(layerAllOffAction!=null)
                    layerAllOffAction.Invoke();
            }
        });
    }
    public void SetLayerAllOffAction(UnityAction act)
    {
        layerAllOffAction = act;
    }

    public void SetContentOnOff(bool isOn)
    {
        gameObject.SetActive(isOn);
    }

    public bool LayerToggleInvoke(int idx)
    {
        if (layerTgList[idx].isOn)
        {
            return false;
        }
        else
        {
            layerTgList[idx].isOn = true;
            return true;
        }
    }
}
