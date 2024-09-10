using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraZoomController : SingletonMonoBehaviour<CameraZoomController>
{
    public Scrollbar ZoomScrollbar;
    public float ZoomStep = 0.05f;
    Lean.Touch.LeanPinchScale FakeScaleObject;

    GameObject MyCanvas;

    Vector3 _CamBasicPos;

    public Vector3 CamBasicPos { get { return _CamBasicPos; } set { _CamBasicPos = value; } }
    public void Init()
    {
        ZoomScrollbar.value = 0.5f;
    }

    public void SetEnable(bool enable)
    {
        gameObject.SetActive(enable);
    }

    // Start is called before the first frame update
    void Start()
    {
#if ROTATE_MODEL_OBJECT
        CamBasicPos = Camera.main.transform.position;
#else
        CamBasicPos = Camera.main.transform.localPosition;
#endif

        //ZoomScrollbar = CommonUtility.FindChildObject("ZoomScrollbar", MyCanvas.transform).GetComponent<Scrollbar>();

        ZoomScrollbar.onValueChanged.AddListener(ZoomScaleValueChange);
        ZoomScrollbar.gameObject.SetActive(false);
        ZoomScaleValueChange(ZoomScrollbar.value);
    }

    private void LateUpdate()
    {
        // RotateControlCube ��ũ�� ��ǥ�� ����
#if USE_3D_OBJECT_UI_ROTATE
        Vector3 SWpos = Camera.main.ScreenToWorldPoint(WSpos);
        RotateControlObj.transform.position = SWpos;  //3D UI 
#endif


        ScaleUpdate();
    }


    void ScaleUpdate()
    {
        List<RaycastResult> rayResult = Lean.Touch.LeanTouch.RaycastGui(Input.mousePosition);
        if (rayResult.Count != 0)
            return;
        // ���콺 ��ũ�ѷ� �ʸ𵨹������� ī�޶� Ȯ�뿬�� , �˾� ����� �۵�����
        if (Input.mouseScrollDelta.y != 0.0f /*&& CommonPopup.ins.GetCurrentPopup() == null*/)
        {
            //print(Input.mouseScrollDelta);
           
            //ZoomScrollbar.value = (CamBasicPos.z - Camera.main.transform.localPosition.z) / (CamBasicPos.z - MAXZOOM_Z);
            if ((Input.mouseScrollDelta.y > 0 && ZoomScrollbar.value <= 1.0f) || (Input.mouseScrollDelta.y < 0 && ZoomScrollbar.value >= 0.0f))
            {
                ZoomScrollbar.value += Input.mouseScrollDelta.y * ZoomStep;
                if (FakeScaleObject != null)
                    FakeScaleObject.transform.localScale = Vector3.one * ZoomScrollbar.value * 1.0f;
            }
        }
        else if (FakeScaleObject != null && FakeScaleObject.enabled)
        {
            if ((ZoomScrollbar.value <= 1.0f) && (ZoomScrollbar.value >= 0.0f))
            {
                // �հ��� 2�� �̻� ��ġ ������ localScale.x ���� 0�̸�
                if (FakeScaleObject.Use.UpdateAndGetFingers().Count > 1 && FakeScaleObject.transform.localScale.x == 0.0f)
                    FakeScaleObject.transform.localScale = Vector3.one * 0.01f;

                ZoomScrollbar.value = FakeScaleObject.transform.localScale.x * 1.0f;
            }
        }
        if (ZoomScrollbar.value > 1.0f)
        {
            ZoomScrollbar.value = 1.0f;
            if (FakeScaleObject != null)
                FakeScaleObject.transform.localScale = Vector3.one * 1.0f;
        }
        else if (ZoomScrollbar.value < 0.0f)
        {
            ZoomScrollbar.value = 0.0f;
            if (FakeScaleObject != null)
                FakeScaleObject.transform.localScale = Vector3.one * 0.0f;
        }
        // ������Ʈ�� LeanPinchScal�� localScale�� �����ϰ� �� ���� ī�޶��� FOV�� �����Ͽ� Ȯ����� ����
        // �ν����� â���� ��Ȱ���� (���� 10:27 2021-02-23)
        //if (ScalelRotateControlCube.transform.localScale.x > 90)
        //    ScalelRotateControlCube.transform.localScale = Vector3.one * 90;
        //else if(ScalelRotateControlCube.transform.localScale.x < 1)
        //    ScalelRotateControlCube.transform.localScale = Vector3.one;

        //Camera.main.fieldOfView = BasicFieldOfView - ScalelRotateControlCube.transform.localScale.x / 1 + 20;
    }

    public int modScaleDefault = 0;
    public int modScaleRange = 0;
    public float distance;
    public float PreValue;
    public float PreValueOrtho;

    /// <summary>
    /// ī�޶��� ���� ��ǥ(z��)�� �����Ͽ� �� ��Ʈ��
    /// </summary>
    /// <param name="value"></param>
    public void ZoomScaleValueChange(float value)
    {
        if (!Camera.main.orthographic)
        {
            if ((value > PreValue && Camera.main.transform.localPosition.magnitude > 18.0f) || value < PreValue)
            {
                ZoomScrollbar.enabled = true;
                float max = float.MaxValue;
                int maxfloat = (int)max;
#if USE_ASTAR
        if (CommonMyTool.Instance == null) // ������������ �ƴ϶�� ī�޶� ������ ��������� ���� �ƴϸ� ��������� ī�޶� ���� �����Ѵ�.
#endif
                {
                    Camera.main.transform.localPosition = _CamBasicPos;
                    Camera.main.transform.Translate(new Vector3(0.0f, 0.0f, (value - 0.5f) * modScaleRange + modScaleDefault), Space.Self);
                }
                PreValue = value;
                ZoomScrollbar.value = PreValue;
            }
            else
            {
                ZoomScrollbar.value = PreValue;
            }
        }
        else
        {
            if ((value > PreValueOrtho) || value < PreValueOrtho)
            {
                ZoomScrollbar.enabled = true;
                float max = float.MaxValue;
                int maxfloat = (int)max;
#if USE_ASTAR
        if (CommonMyTool.Instance == null) // ������������ �ƴ϶�� ī�޶� ������ ��������� ���� �ƴϸ� ��������� ī�޶� ���� �����Ѵ�.
#endif
                {
                    //Camera.main.transform.localPosition = _CamBasicPos;
                    //Camera.main.transform.Translate(new Vector3(0.0f, 0.0f, (value - 0.5f) * modScaleRange + modScaleDefault), Space.Self);
                    
                    Camera.main.orthographicSize = (1.0f - value) * 90 + 5;
                }
                PreValueOrtho = value;
                ZoomScrollbar.value = PreValueOrtho;
            }
            else
            {
                ZoomScrollbar.value = PreValueOrtho;
            }
        }

        distance = Vector3.Distance(Camera.main.transform.position, transform.parent.position);
    }
}
