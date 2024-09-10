using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRootController2 : MonoBehaviour
{
    Transform cameraRoot;

    [SerializeField]
    Transform mainCamera;

    [SerializeField]
    bool isMove;

    
    public float MoveSpeed = 0.1f;
    public float ZoomSpeed = 0.1f;
    public float minDis = 200f;
    public float maxDis = 2000f;
    public float distance;
    public float limit_y;

    Vector3 basicRot;
    private void Awake()
    {
        cameraRoot = this.transform;
        basicRot = transform.localRotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            MouseUpdate();

        }
    }
    private void LateUpdate()
    {
        Vector3 pos = cameraRoot.position;
        pos.y = limit_y;
        cameraRoot.position = pos;
    }
    void MouseUpdate()
    {
        distance = Vector3.Distance(cameraRoot.transform.position, mainCamera.transform.position);

        if (Input.GetMouseButton(1))
        {
            
            float rotationX = Input.GetAxisRaw("Mouse X") * 4f;
            float rotationY = Input.GetAxisRaw("Mouse Y") * -4f;
            //rotationY = Mathf.Clamp(rotationY, -15, 60);

            cameraRoot.transform.localEulerAngles += new Vector3(rotationY, rotationX, 0);
            float AngleX = cameraRoot.transform.localEulerAngles.x;

            //if (AngleX >= 350f || (AngleX < 0 && AngleX > -10)) AngleX = 350f;
            //else if (AngleX <= 300f || AngleX <= -60) AngleX = 300f;

            //AngleX= Mathf.Clamp(AngleX,-15,60);  
            cameraRoot.transform.localEulerAngles = new Vector3(AngleX, cameraRoot.transform.localEulerAngles.y, cameraRoot.transform.localEulerAngles.z);

        }

    }

    public void SetMainCamera(Transform target)
    {
        transform.SetParent(target.parent);
        mainCamera = target;
    }
    public void SetPos(Vector3 worldPos)
    {
        cameraRoot.transform.position = worldPos;

    }
    
    public void RotateTarget(Transform target)
    {
        cameraRoot.transform.LookAt(target);
    }
    public void SetMoveOnOff(bool isOn)
    {
        isMove = isOn;
    }
    public void SetActiveOnOff(bool isOn)
    {
        gameObject.SetActive(isOn);
    }
    public void SetBasicRot()
    {
        cameraRoot.transform.localEulerAngles = basicRot;
    }
}
