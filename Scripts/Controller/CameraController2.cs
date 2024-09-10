using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2 : MonoBehaviour
{
    
    Transform cameraRoot;
    
    [SerializeField]
    Transform mainCamera;

    public bool isMove;

    public float MoveSpeed = 0.1f;
    public float ZoomSpeed = 0.1f;
    public float minDis = 200f;
    public float maxDis = 2000f;
    public float distance;

    private void Awake()
    {
        cameraRoot = this.transform;

    }

    // Start is called before the first frame update
    void Start()
    {
        
        distance = Vector3.Distance(cameraRoot.transform.position, transform.position);

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
        pos.y = 0;
        cameraRoot.position = pos;
    }
    void MouseUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            //if (IsPointerOverUIObject()) return;
            float rotationX = Input.GetAxisRaw("Mouse X") * 4f;
            float rotationY = Input.GetAxisRaw("Mouse Y") * -4f;
            //rotationY = Mathf.Clamp(rotationY, -15, 60);

            cameraRoot.transform.localEulerAngles += new Vector3(rotationY, rotationX, 0);
            float AngleX = cameraRoot.transform.localEulerAngles.x;

            if (AngleX > 90) AngleX = 0;
            else if (AngleX >= 60 && AngleX <= 90) AngleX = 60;

            //AngleX= Mathf.Clamp(AngleX,-15,60);  
            cameraRoot.transform.localEulerAngles = new Vector3(AngleX, cameraRoot.transform.localEulerAngles.y, cameraRoot.transform.localEulerAngles.z);

        }
        if (Input.GetMouseButton(0))
        {
            float posX = Input.GetAxis("Mouse Horizontal") * MoveSpeed * -4;
            float posZ = Input.GetAxis("Mouse Vertical") * MoveSpeed * -4;
            cameraRoot.transform.Translate(new Vector3(posX, 0, posZ), Space.Self); ;
            cameraRoot.transform.localPosition = (new Vector3(cameraRoot.transform.localPosition.x, 0, cameraRoot.transform.localPosition.z)); ;
            //cameraRoot.transform.localPosition+=(new Vector3(posX, 0, posZ));

        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            float rotationY = Input.GetAxisRaw("Mouse ScrollWheel") * ZoomSpeed;
            float distance = Vector3.Distance(cameraRoot.transform.position, mainCamera.transform.position);
            if (distance < minDis && rotationY > 0) { Debug.Log("Min:"+ distance); return; }
            else if (distance > maxDis && rotationY < 0) { Debug.Log("Max:"+ distance); return; }
            else
            {
                mainCamera.transform.Translate(new Vector3(0, 0, rotationY));//localPosition-= (new Vector3(0, 0,rotationY));
            }                                                                //Vector3(25.6000061,771.200012,-1411)Vector3(25.6000061,746.98999,-1369.06006)
        }
    }
}
