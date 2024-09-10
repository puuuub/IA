using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POIBasic : MonoBehaviour
{
    public Camera targetCamera;
    public Transform targetTr;
    public Vector3 subTargetPos = Vector3.zero;   //보조 움직임?
    public bool isBasicSet { get; set; } = false;    //기본셋팅
    public RectTransform rectTransform;

    /// <summary>
    /// insert into LateUpdate()
    /// </summary>
    public virtual void SetTransformPos()
    {
        if (targetCamera == null || targetTr == null) return;

        transform.position = targetCamera.WorldToScreenPoint(targetTr.position + subTargetPos) ;
    }

    public virtual void SetTargetCamera(Camera cam)
    {
        targetCamera = cam;
    }

    public virtual void SetTargetTransform(Transform tr)
    {
        targetTr = tr;
    }
    public virtual void SetSubTargetPos(Vector3 pos)
    {
        subTargetPos = pos;
    }
    
    public void SetActive(bool isOn)
    {
        gameObject.SetActive(isOn);
    }
}
