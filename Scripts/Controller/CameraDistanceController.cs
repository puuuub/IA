using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCreator
{

}

/// <summary>
/// 카메라에 고정된 Quad 배경경처리
/// </summary>
public class CameraDistanceController : SingletonMonoBehaviour<CameraDistanceController>
{
    Camera MyCamera;
    public float BaseScale;
    GameObject Root;
    public float val = 1.0f;
    public Vector3 Elevation;
    public float ObjHeight;
    public float ObjScale_H;


    // Start is called before the first frame update
    void Start()
    {
        MyCamera = Camera.main;//transform.parent.GetComponent<Camera>();
        BaseScale = transform.localScale.x;

        //float mod = 0.85f;

        //Vector3[] vertices = new Vector3[] { new Vector3(-1.0f * mod, 1.0f * mod, 0.0f), new Vector3(1.0f * mod, 1.0f * mod, 0.0f),
        //                                     new Vector3(0.5f, -0.5f, 0.0f), new Vector3(-0.5f, -0.5f, 0.0f)};

        //int[] triangles = new int[] { 0, 1, 2, 0, 2, 3 };

        //Vector2[] uvs = new Vector2[]
        //{
        //    new Vector2(0.0f, 1.0f), new Vector2(1.0f, 1.0f),
        //    new Vector2(1.0f, 0.0f),new Vector2(0.0f, 0.0f)
        //};

        //Mesh mesh = new Mesh();
        //mesh.vertices = vertices;
        //mesh.triangles = triangles;
        //mesh.uv = uvs;
        //mesh.RecalculateBounds();
        //mesh.RecalculateNormals();

        //MeshFilter mf = MyCamera.GetComponentInChildren<MeshFilter>();


        //mf.mesh = mesh;
        
        //mf.mesh.vertices[0] = new Vector3(-1.5f, 1.5f, 0.0f);
        //mf.mesh.vertices[1] = new Vector3(1.5f, 1.5f, 0.0f);
        //mf.mesh.RecalculateBounds();
        //mf.mesh.RecalculateNormals();
    }

    private void FixedUpdate()
    {

    }


    void Update()
    {

    }

    private void LateUpdate()
    {
        //Vector3 gap = transform.position - AreaObjectController.Instance.SelectDivisionCenterPos + Elevation;

        //// 상황에 맞게 적절한 거리계산
        //float cameraToObjDist = 0.0f;


        //if (CameraController.Instance.ZoomState == CameraController.ZOOM_STATE.ZOOM_IN)
        //{
        //    cameraToObjDist = Vector3.Distance(MyCamera.transform.position, AreaObjectController.Instance.SelectDivisionCenterPos);
        //}
        //else if (CameraController.Instance.ZoomState == CameraController.ZOOM_STATE.ZOOM_OUT)
        //{
        //    cameraToObjDist = Vector3.Distance(MyCamera.transform.position, AreaObjectController.Instance.SelectDivisionCenterPos);
        //}
        //else
        //{
        //    cameraToObjDist = CameraController.Instance.CameraToObjDist;
        //}
        //if (CameraController.Instance.TargetObject == null)
        //    return;


        //transform.position = CameraController.Instance.TargetObject.transform.position + new Vector3(0.0f, 0.0f, 0.0f);

        // 거리를 계산하여 스케일 조정하기
        float dist = Vector3.Distance(MyCamera.transform.position, transform.position);
        float scale = dist * 2.0f * BaseScale * val;
        transform.localScale = new Vector3(scale, scale, 1.0f);
        ObjScale_H = scale / 2.0f;
    }
}
