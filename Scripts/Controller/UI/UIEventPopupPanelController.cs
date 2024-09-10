using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class UIEventPopupPanelController : MonoBehaviour
{
    [SerializeField]
    GameObject contents;

    [SerializeField]
    TMP_Text[] contents_txts;

    [SerializeField]
    Button confirm_btn;
    [SerializeField]
    Button cancel_btn;


    // Start is called before the first frame update
    void Start()
    {
        cancel_btn.onClick.AddListener(SetContentsOff);
        SetContentsOff();
    }

    public void SetContentsOn(string[] txts, UnityAction confirmAct)
    {
        for(int i = 0; i < contents_txts.Length; i++)
        {
            if(i < txts.Length)
            {
                contents_txts[i].text = txts[i];
            }
        }

        confirm_btn.onClick.RemoveAllListeners();
        confirm_btn.onClick.AddListener(confirmAct);
        confirm_btn.onClick.AddListener(SetContentsOff);

        contents.SetActive(true);
    }


    public void SetContentsOff()
    {
        contents.SetActive(false);
    }

}
