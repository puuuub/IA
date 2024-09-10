using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class BasicObjectController : MonoBehaviour
{
    [SerializeField]
    public Transform poiTarget;

    public Outline outline;
    public List<Outline> outlineList;


    public void SetOutLineOnOff(bool isOn)
    {
        if(outline != null)
        {
            outline.enabled = isOn;
        }
        else
        {
            if (outlineList == null) return;

            foreach(Outline line in outlineList)
            {
                line.enabled = isOn;
            }
        }

    }
    public Transform GetPoiTarget()
    {
        if(poiTarget != null)
        {
            return poiTarget;
        }
        else
        {
            return transform;
        }
    }

    public void SetOutlineNum(int num = 0)
    {
        if(outline != null)
        {
            outline.color = num;
        }
        else
        {
            for(int i = 0; i < outlineList.Count; i++)
            {
                outlineList[i].color = num;
            }
        }
    }
}
