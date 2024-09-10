using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UIEventRowController : MonoBehaviour
{

    [SerializeField]
    TMP_Text[] text_txt;

    [SerializeField]
    Button row_btn;

    public void SetContentsOn(string[] txts, UnityAction act)
    {
        gameObject.SetActive(true);

        for(int i=0;i< text_txt.Length; i++)
        {
            if(i < txts.Length)
                text_txt[i].text = txts[i];
        }

        if (row_btn != null)
        {
            row_btn.onClick.RemoveAllListeners();
            row_btn.onClick.AddListener(act);

        }
    }

    public void SetContentsOff()
    {
        row_btn.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }

}
