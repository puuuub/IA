using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VircamController : MonoBehaviour
{

    [SerializeField]
    Vector3 oriWorldPos;
    [SerializeField]
    Quaternion oriRotate;

    [SerializeField]
    Transform oriParent;

    private void OnEnable()
    {
        oriWorldPos = transform.position;
        oriRotate = transform.rotation;

    }
    
    public void PosReset()
    {
        transform.position = oriWorldPos;
        transform.rotation = oriRotate;
    }

    public Vector3 GetPlaneVector()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, float.MaxValue, CineMachineManager.Instance.GetZeroPlaneMask()))
        {
            return hit.point;
        }

        return Vector3.zero;
    }
    public void SetObjectOnOff(bool isOn)
    {
        gameObject.SetActive(isOn);
        if (!isOn)
        {
            if (transform.parent != oriParent)
            {
                SetParent(oriParent);
            }
            if(transform.position != oriWorldPos)
            {
                PosReset();
            }
        }
    }
   
    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }
}
