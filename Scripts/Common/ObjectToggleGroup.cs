using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToggleGroup : MonoBehaviour
{
    List<GameObject> toggleList;

    Dictionary<GameObject, bool> valueList;
    Dictionary<GameObject, Func<bool, bool>> actionList;

    List<Func<bool>> allToggleOffActionList;

    public bool allowToggleOff { get; set; } = true;

    public void AddToggleList(GameObject go, Func<bool, bool> func)
    {
        if (toggleList == null)
        {
            toggleList = new List<GameObject>();
        }
        toggleList.Add(go);

        if (valueList == null)
        {
            valueList = new Dictionary<GameObject, bool>();
        }
        valueList.Add(go, false);

        if (actionList == null)
        {
            actionList = new Dictionary<GameObject, Func<bool, bool>>();
        }
        actionList.Add(go, func);

    }

   
    public void ClickAction(GameObject target)
    {
        if (!toggleList.Contains(target)) return;
        bool allOff = false;

        foreach (GameObject go in toggleList)
        {
            if (target.Equals(go))
            {
                if (allowToggleOff || !valueList[go])
                {
                    bool tmp = valueList[go];
                    valueList[go] = !tmp;
                    actionList[go](valueList[go]);
                }
            }
            else if (valueList[go])
            {
                actionList[go](valueList[go] = false);
            }
            allOff |= valueList[go];
        }

        if (!allOff && allToggleOffActionList != null)
        {
            foreach(Func<bool> func in allToggleOffActionList)
            {
                func.Invoke();
            }
        }

    }


    public void AddAllToggleOffAction(Func<bool> func)
    {
        if(allToggleOffActionList == null)
        {
            allToggleOffActionList = new List<Func<bool>>();
        }
        allToggleOffActionList.Add(func);
    }


    public void SetAllToggleOff()
    {
        foreach(GameObject go in toggleList)
        {
            if (valueList[go])
            {
                actionList[go](false);
            }
            valueList[go] = false;
        }
    }
    
 
}
