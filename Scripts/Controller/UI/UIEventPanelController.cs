using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIEventPanelController : MonoBehaviour
{
    //우측상단 eventPanel

    [SerializeField]
    MouseEvent2 mouseEvent;
    
    [SerializeField]
    Transform rowRoot;

    [SerializeField]
    List<UIEventRowController> rowList;


    [SerializeField]
    List<Sprite> status_imgs;


    int eventIdx = 0;
    // Start is called before the first frame update
    void Start()
    {
        SetContentsOnOff(false);
    }

    public void AddEventRow(string[] texts, UnityAction act)
    {
        UIEventRowController row;

        if (eventIdx < rowList.Count)
        {
            row = rowList[eventIdx++];

        }
        else
        {
            eventIdx++;
            row = Instantiate(rowList[0].gameObject).GetComponent<UIEventRowController>();
            row.transform.SetParent(rowRoot);
            rowList.Add(row);
        }

        row.SetContentsOn(texts, act);
        row.transform.SetAsFirstSibling();

    }

    public void ResetEvnetRow()
    {
        for(int i=0;i< rowList.Count; i++)
        {
            rowList[i].SetContentsOff();
        }
    }
    public void SetContentsOnOff(bool isOn)
    {
        gameObject.SetActive(isOn);

    }
    public void AddMouseOverAction(UnityAction enterAct, UnityAction exitAct)
    {
        mouseEvent.AddEnterAction(enterAct);
        mouseEvent.AddExitAction(exitAct);

    }
}
