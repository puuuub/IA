using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;


public class UISideMenuController : MonoBehaviour
{
    [SerializeField]
    ToggleGroup sideMenuGroup;
    [SerializeField]
    List<Toggle> sideMenuList;

    [SerializeField]
    GameObject subContents;
    [SerializeField]
    ToggleGroup subRowGroup;
    [SerializeField]
    List<Toggle> subRow_tgs;
    [SerializeField]
    List<TMP_Text> subRow_txts;

    UnityAction sideMenuAllOffAction;

    const int SUBROWSIZE = 13;
    Vector3 subContentsPos;
    Color subRowTextOffCol;

    // Start is called before the first frame update
    void Start()
    {
        subContentsPos = new Vector3(150f, 15f, 0f);
        
        for (int i = 0; i < subRow_tgs.Count; i++)
        {
            int idx = i;
            subRow_tgs[idx].onValueChanged.AddListener(x => 
            {
                subRow_txts[idx].color = x ? Color.white : Color.red;
            });
        }

        ColorUtility.TryParseHtmlString("#B3B3B3", out subRowTextOffCol);
    }

    public void SetSideMenuToggleAction(int idx, UnityAction act)
    {
        sideMenuList[idx].onValueChanged.AddListener(x=>
        {
            if (x)
            {
                act.Invoke();
            }
            else
            {
                sideMenuAllOffAction.Invoke();
            }
        });

        
    }

    public void SetSiceMenuAllOffAction(UnityAction act)
    {
        sideMenuAllOffAction = act;
    }
    public void SetSubContents(int rootIdx, List<string> subMenuTitles, List<string> subInteracts, List<UnityAction> subRowAction, UnityAction allOffAction)
    {
        if(subMenuTitles.Count > 0)
        {
            subContents.SetActive(true);
            subContents.transform.SetParent(sideMenuList[rootIdx].transform);
            subContents.transform.localPosition = subContentsPos;

            subRowGroup.SetAllTogglesOff();
            
            foreach(Toggle tg in subRow_tgs)
            {
                tg.onValueChanged.RemoveAllListeners();
            }

            for (int i = 0; i < SUBROWSIZE; i++)
            {
                if (i < subMenuTitles.Count)
                {
                    subRow_tgs[i].gameObject.SetActive(true);
                    subRow_txts[i].text = subMenuTitles[i];
                    if (subInteracts.Contains(subMenuTitles[i]))
                    {
                        subRow_txts[i].color = Color.white;
                    }
                    else
                    {
                        subRow_txts[i].color = subRowTextOffCol;
                    }

                    int idx = i;
                    subRow_tgs[i].onValueChanged.AddListener(x =>
                    {
                        if (x)
                        {
                            subRowAction[idx].Invoke();
                        }
                        else
                        {
                            if (!subRowGroup.AnyTogglesOn())
                                allOffAction.Invoke();
                        }
                    });
                }
                else
                {
                    subRow_tgs[i].gameObject.SetActive(false);
                }
            }

        }
        else
        {
            SetSubContentsOff();
        }

    }

    public void SetSubContentsOff()
    {
        subContents.SetActive(false);
    }

    public void SetSideMenu(List<int> onMenuList = null)
    {
        //메뉴 변경하면서 켜기
        gameObject.SetActive(true);
        if (onMenuList == null)
        {
            for (int i = 0; i < sideMenuList.Count; i++)
            {
                sideMenuList[i].gameObject.SetActive(false);    
            }
        }
        else
        {
            for (int i = 0; i < sideMenuList.Count; i++)
            {
                if (onMenuList.Contains(i))
                {
                    sideMenuList[i].gameObject.SetActive(true);
                }
                else
                {
                    sideMenuList[i].gameObject.SetActive(false);
                }
            }
        }

    }

    public void SetSideMenuToggleOff()
    {
        sideMenuGroup.SetAllTogglesOff();
    }
    
    public void SetContentsOn()
    {
        //단순히 켜기
        gameObject.SetActive(true);
    }
    public void SetContentsOff()
    {
        gameObject.SetActive(false);
    }

}
