using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class SOPRowController : MonoBehaviour
{
    public Toggle row_tg;

    [SerializeField]
    TMP_Text row_txt;

    // Start is called before the first frame update
    void Start()
    {
        row_tg.onValueChanged.AddListener(ToggleBasicAction);
    }

    public void AddToggleAction(UnityAction<bool> act)
    {
        row_tg.onValueChanged.AddListener(act);
    }
    void ToggleBasicAction(bool isOn)
    {
        if (isOn)
        {
            row_txt.fontStyle = FontStyles.Bold;
        }
        else
        {
            row_txt.fontStyle = FontStyles.Normal;
        }
    }
    
}
