using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Lean.Common;
using Lean.Touch;
using DG.Tweening;

/// <summary>
/// 1. �θ������Ʈ�� position�� ������Ʈ�� �߽����� �̵���Ų��
/// 2. ����ī�޶��� position�� �����Ͽ� ������Ʈ�� �߽�(�θ������Ʈ)�� �Ÿ��� �����Ѵ�.
/// 3. �θ������Ʈ�� rotation�� �����Ͽ� ������ �ʱ� ������ �����Ѵ�.
/// </summary>

public class RotateController : SingletonMonoBehaviour<RotateController>
{
    public GameObject RootObject; // �θ������Ʈ
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
    /// ��� �ʱ�ȭ
    /// </summary>
    public void Init()
    {
        // ȸ������ LeanDragTranslate������Ʈ�� localposition���� ��ȯ�ϱ�
        Quaternion rot = RootObject.transform.localRotation;
        
        Vector3 resultRotate = new Vector3(-rot.eulerAngles.y / Axis.x, rot.eulerAngles.x / Axis.y, rot.eulerAngles.z / Axis.z);
        // ������ ��ġ�� ���ġ
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
        // ������ ��ġ�� ���ġ
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

        // �ʱ� ī�޶� ���� ����
        Vector3 baseRotate = RootObject.transform.localRotation.eulerAngles;
        // ī�޶� X�� ������ ���� �ٸ��� �����ؾ���( �����, ž��(����) )
        float xDegree = Camera.main.transform.localRotation.eulerAngles.x;
        if (xDegree < 45.0f)
        {
            //// LeanObject�� ī�޶� ������ �ְ��ؾ� Lean Drag Translatte ������Ʈ�� ����� �۵��� 
            //if (tempLeanPos.y < 0 && tempLeanPos.z < 0)
            //    tempLeanPos.z = -tempLeanPos.z;

            LeanObject.transform.localPosition = new Vector3(-baseRotate.y / Axis.y, baseRotate.x / Axis.x, 0.0f);
        }
        // ž��
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
        // �߽ɿ� �������� ȸ������ ����ϰ���
        LeanObject.Sensitivity = baseLeanSensitivity * baseDistance / distance;
        
        Vector3 tempLeanPos = LeanObject.transform.localPosition;

        // ������ �ุ ȸ���ϱ�
        if (Input.GetKey(KeyCode.X) || Camera.main.orthographic)
        {
            tempLeanPos.y = PreLeanObjectPos.y;
        }
        if (Input.GetKey(KeyCode.Y))
        {
            tempLeanPos.x = PreLeanObjectPos.x;
        }


        // ȸ�� ��ų �� �����
        Vector3 resultRotate = new Vector3(tempLeanPos.x * Axis.x, tempLeanPos.y * Axis.y, tempLeanPos.z * Axis.z);

        // ������ �ѹ��� ȸ���� 
        Vector3 mod = new Vector3(resultRotate.x % 360.0f, resultRotate.y % 360.0f, resultRotate.z % 360.0f);

        // ȸ������ ������ ������Ʈ�� ��ġ���� ������ ���� 
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

        // ī�޶� X�� ������ ���� �ٸ��� �����ؾ���( �����, ž��(����) )
        float xDegree = Camera.main.transform.localRotation.eulerAngles.x;

        // �����
        if (xDegree < 45.0f)
        {
            // LeanObject�� ī�޶� ������ �ְ��ؾ� Lean Drag Translatte ������Ʈ�� ����� �۵��� 
            if (tempLeanPos.y < 0 && tempLeanPos.z < 0)
                tempLeanPos.z = -tempLeanPos.z;

            RootObject.transform.localRotation = Quaternion.Euler(mod.y, -mod.x, 0.0f);
        }
        // ž��
        else
        {
            RootObject.transform.localRotation = Quaternion.Euler(mod.z, -mod.x, 0.0f);
        }
        // ������ ��ġ�� ���ġ
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
        //    print("UIManager.Instance.CamBasicPos ���� ");
        //}
    }
}
