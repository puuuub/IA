using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubareaObjectController : BasicObjectController
{
    public SUBAREA targetArea;

    private void Awake()
    {
        if (outline == null)
        {
            Renderer[] rens = GetComponentsInChildren<Renderer>();
            foreach (Renderer ren in rens)
            {
                outlineList.Add(ren.gameObject.AddComponent<cakeslice.Outline>());
            }
        }
    }

    private void Start()
    {
        SetOutlineNum(1);

        SetOutLineOnOff(false);
    }

}
