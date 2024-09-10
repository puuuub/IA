using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRootController : MonoBehaviour
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

    private void Awake()
    {
        cameraRoot = this.transform;

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

            if (AngleX >= 350f || (AngleX < 0 && AngleX > -10)) AngleX = 350f;
            else if (AngleX <= 300f || AngleX <= -60) AngleX = 300f;

            //AngleX= Mathf.Clamp(AngleX,-15,60);  
            cameraRoot.transform.localEulerAngles = new Vector3(AngleX, cameraRoot.transform.localEulerAngles.y, cameraRoot.transform.localEulerAngles.z);

        }
        if (Input.GetMouseButton(0))
        {
            

            float ratio = (distance - minDis) / maxDis;
            MoveSpeed  = (ratio * 0.5f) + 0.05f;

            float posX = Input.GetAxis("Mouse Horizontal") * MoveSpeed * 4;
            float posZ = Input.GetAxis("Mouse Vertical") * MoveSpeed * 4;

            cameraRoot.transform.position += (cameraRoot.transform.right * posX) + (cameraRoot.transform.forward* posZ);
            
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            float ratio = (distance - minDis) / maxDis;
            ZoomSpeed = (ratio * 450f) + 50f;

            float rotationY = Input.GetAxisRaw("Mouse ScrollWheel") * ZoomSpeed;
            //float distance_ = Vector3.Distance(cameraRoot.transform.position, mainCamera.transform.position);
            if (distance < minDis && rotationY > 0) { Debug.Log("Min:" + distance); return; }
            else if (distance > maxDis && rotationY < 0) { Debug.Log("Max:" + distance); return; }
            else
            {
                mainCamera.transform.Translate(new Vector3(0, 0, rotationY));//localPosition-= (new Vector3(0, 0,rotationY));
            }                                                                //Vector3(25.6000061,771.200012,-1411)Vector3(25.6000061,746.98999,-1369.06006)
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
}
