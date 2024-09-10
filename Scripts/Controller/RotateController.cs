using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Lean.Common;
using Lean.Touch;
using DG.Tweening;

/// <summary>
/// 1. 부모오브젝트의 position을 오브젝트의 중심으로 이동시킨다
/// 2. 메인카메라의 position을 조정하여 오브젝트의 중심(부모오브젝트)과 거리를 조정한다.
/// 3. 부모오브젝트의 rotation을 조정하여 실행후 초기 각도를 조정한다.
/// </summary>

public class RotateController : SingletonMonoBehaviour<RotateController>
{
    public GameObject RootObject; // 부모오브젝트
    public LeanDragTranslate LeanObject;
    public Vector3 Axis;

    Vector3 BasePosition;

    Vector3 PreLeanObjectPos;

    float baseDistance;
    public float distance;
    public float baseLeanSensitivity;
    LeanDragTranslate myLean;

    Vector3 BaseLeanObjectPos;

    /// <summary>
    /// 즉시 초기화
    /// </summary>
    public void Init()
    {
        // 회전값을 LeanDragTranslate오브젝트의 localposition으로 변환하기
        Quaternion rot = RootObject.transform.localRotation;
        
        Vector3 resultRotate = new Vector3(-rot.eulerAngles.y / Axis.x, rot.eulerAngles.x / Axis.y, rot.eulerAngles.z / Axis.z);
        // 조정된 위치로 재배치
        LeanObject.transform.localPosition = resultRotate;

        PreLeanObjectPos = LeanObject.transform.localPosition;
    }

    public Tween InitLerp(float duration = 1.0f, float delay = 0.0f)
    {
        Tween tw = LeanObject.transform.DOLocalMove(Vector3.zero, duration).SetDelay(delay);
        return tw;
    }

    public void SetEnable(bool enable)
    {
        if (!enable)
        {
            LeanObject.enabled = enable;
        }
        else
        {
            LeanObject.enabled = enable;
        }

    }

    public void Rotate(Quaternion rot, float duration)
    {
        Vector3 resultRotate = new Vector3(-rot.eulerAngles.y / Axis.x, rot.eulerAngles.x / Axis.y, rot.eulerAngles.z / Axis.z);
        // 조정된 위치로 재배치
        //LeanObject.transform.localPosition = resultRotate;

        Tween tw = LeanObject.transform.DOLocalMove(resultRotate, duration);
    }

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        LeanObject = GetComponent<LeanDragTranslate>();
        //baseLeanSensitivity = LeanObject.Sensitivity;

        RootObject = transform.parent.gameObject;

        // 초기 카메라 세팅 대응
        Vector3 baseRotate = RootObject.transform.localRotation.eulerAngles;
        // 카메라 X축 각도에 따라 다르게 대응해야함( 측면뷰, 탑뷰(층별) )
        float xDegree = Camera.main.transform.localRotation.eulerAngles.x;
        if (xDegree < 45.0f)
        {
            //// LeanObject가 카메라 영역에 있게해야 Lean Drag Translatte 컴포넌트가 제대로 작동함 
            //if (tempLeanPos.y < 0 && tempLeanPos.z < 0)
            //    tempLeanPos.z = -tempLeanPos.z;

            LeanObject.transform.localPosition = new Vector3(-baseRotate.y / Axis.y, baseRotate.x / Axis.x, 0.0f);
        }
        // 탑뷰
        else
        {
            LeanObject.transform.localPosition = new Vector3(-baseRotate.z / Axis.z, baseRotate.x / Axis.x, 0.0f);
        }
        BaseLeanObjectPos = LeanObject.transform.localPosition;
        baseDistance = distance = Vector3.Distance(LeanObject.transform.position, Camera.main.transform.position);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 중심에 가까울수록 회전감도 상승하게함
        LeanObject.Sensitivity = baseLeanSensitivity * baseDistance / distance;
        
        Vector3 tempLeanPos = LeanObject.transform.localPosition;

        // 선택한 축만 회전하기
        if (Input.GetKey(KeyCode.X) || Camera.main.orthographic)
        {
            tempLeanPos.y = PreLeanObjectPos.y;
        }
        if (Input.GetKey(KeyCode.Y))
        {
            tempLeanPos.x = PreLeanObjectPos.x;
        }


        // 회전 시킬 값 만들기
        Vector3 resultRotate = new Vector3(tempLeanPos.x * Axis.x, tempLeanPos.y * Axis.y, tempLeanPos.z * Axis.z);

        // 범위는 한바퀴 회전만 
        Vector3 mod = new Vector3(resultRotate.x % 360.0f, resultRotate.y % 360.0f, resultRotate.z % 360.0f);

        // 회전값을 조정할 오브젝트의 위치값의 범위도 조정 
        if (resultRotate.x < -360 || resultRotate.x > 360)
        {
            tempLeanPos.x = 0;
        }
        if (resultRotate.y < -360 || resultRotate.y > 360)
        {
            tempLeanPos.y = 0;
        }
        if (resultRotate.z < -360 || resultRotate.z > 360)
        {
            tempLeanPos.z = 0;
        }

        // 카메라 X축 각도에 따라 다르게 대응해야함( 측면뷰, 탑뷰(층별) )
        float xDegree = Camera.main.transform.localRotation.eulerAngles.x;

        // 측면뷰
        if (xDegree < 45.0f)
        {
            // LeanObject가 카메라 영역에 있게해야 Lean Drag Translatte 컴포넌트가 제대로 작동함 
            if (tempLeanPos.y < 0 && tempLeanPos.z < 0)
                tempLeanPos.z = -tempLeanPos.z;

            RootObject.transform.localRotation = Quaternion.Euler(mod.y, -mod.x, 0.0f);
        }
        // 탑뷰
        else
        {
            RootObject.transform.localRotation = Quaternion.Euler(mod.z, -mod.x, 0.0f);
        }
        // 조정된 위치로 재배치
        LeanObject.transform.localPosition = tempLeanPos;

        PreLeanObjectPos = LeanObject.transform.localPosition;
    }

    private void LateUpdate()
    {

        distance = Vector3.Distance(LeanObject.transform.position, Camera.main.transform.position);
        //TargetObject.transform.localRotation = Quaternion.AngleAxis(LeanObject.transform.localPosition.z, Axis);

        //if (Input.GetMouseButtonUp(1))
        //{
        //    UIManager.Instance.CamBasicPos = Camera.main.transform.position;
        //    UIManager.Instance.ZoomPrevalueSync();
        //    print("UIManager.Instance.CamBasicPos 갱신 ");
        //}
    }
}
