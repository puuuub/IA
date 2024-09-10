using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIPositionIn3DWorld : MonoBehaviour
{
    public GameObject Target;
    public Vector3 AddingPos;
    GameObject MyContent;


    GameObject VisibleController;

    public void Init(GameObject target, GameObject visibleCntrlr)
    {
        Target = target;
        VisibleController = visibleCntrlr;
        MyContent = CommonUtility.FindChildObject("Content", transform);
    }

    private void Awake()
    {
        //MyContent = CommonUtility.FindChildObject("MyContent", transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        //SmallBoard = GetComponentInChildren<SmallBoard>().gameObject;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (Target != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(Target.transform.position + AddingPos);
            screenPos.z = 0.0f;
            transform.position = screenPos;

            bool onCamera = false;

            {

                Vector3 vp = Camera.main.WorldToViewportPoint(Target.transform.position);
                //print(Name.text + " " + vp);
                bool xTest = 0 <= vp.x && vp.x <= 1;
                bool yTest = 0 <= vp.y && vp.y <= 1;
                bool zTest = vp.z >= Camera.main.nearClipPlane;
                onCamera = xTest && yTest && zTest;
            }

            MyContent.SetActive(VisibleController.activeSelf && onCamera);
        }
    }
}
