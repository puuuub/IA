using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceObjectController : BasicObjectController
{
    [SerializeField]
    DeviceType dType;

    public string objId;

    public bool interactiveOutline { get; set; }

    private void Awake()
    {
        if(outline == null)
        {
            Renderer[] rens = GetComponentsInChildren<Renderer>();
            foreach(Renderer ren in rens)
            {
                outlineList.Add(ren.gameObject.AddComponent<cakeslice.Outline>());
            }
        }
    }

    private void Start()
    {
        interactiveOutline = false;

        SetOutLineOnOff(false);
    }

    public DeviceType GetDeviceType()
    {
        return dType;
    }

    /// <summary>
    /// when interactive on
    /// used mouseOver
    /// </summary>
    public void SetDeviceOutlineOnOff(bool isOn)
    {
        if (interactiveOutline)
        {
            SetOutLineOnOff(isOn);
        }
    }
}
