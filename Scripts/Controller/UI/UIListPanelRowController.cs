using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UIListPanelRowController : MonoBehaviour
{
    [SerializeField]
    Button row_btn;
    [SerializeField]
    TMP_Text[] row_txt;

    [SerializeField]
    GameObject unused_img;
    
    public bool isActive { get; set; }
    public void AddRowClickAction(UnityAction act)
    {
        row_btn.onClick.AddListener(act);
    }
    public void SetRowInterect(bool isOn)
    {
        isActive = isOn;
        row_btn.interactable = isOn;
        unused_img.SetActive(!isOn);
        SetTextColor(Color.white);
    }

    public void SetTextColor(Color col)
    {
        row_txt[0].color = col;
        row_txt[1].color = col;
    }
}
