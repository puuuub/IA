using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UIPOIController : POIBasic
{
    [SerializeField]
    string objId;

    public DeviceType deviceType { get; set; }
    public AIRPORTAREA area { get; set; }

    [SerializeField]
    GameObject contents;

    [SerializeField]
    Image poiBg_img;

    [SerializeField]
    GameObject textBg;

    [SerializeField]
    TMP_Text type_txt;


    [SerializeField]
    TMP_Text lane_txt;

    [SerializeField]
    Image smallPoi_img;

    [SerializeField]
    Button poi_btn;

    RectTransform canvasRectTransForm;
    public string targetId { get; set; }

    bool isNormalSize = true;

    bool isActive;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasRectTransForm = UIManager.Instance.UICanvas.GetComponent<RectTransform>();
    }
    private void LateUpdate()
    {
        SetTransformPos();
    }
    public override void SetTransformPos()
    {
        if (targetCamera == null || targetTr == null) return;

        if(targetCamera.WorldToScreenPoint(targetTr.position).z > 0)
        {
            if(!isActive)
                SetContentsOnOff(true);

            //canvas render mode : screen space - overlay
            //transform.position = targetCamera.WorldToScreenPoint(targetTr.position) + subTargetPos;

            //canvas render mode : screen space - camera
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(targetCamera, targetTr.position);
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransForm, screenPoint, targetCamera, out Vector2 localPoint))
            {
                rectTransform.localPosition = localPoint;
            }



            //거리에 따라 작게 크게
            if (Vector3.Distance(targetCamera.transform.position, targetTr.position) > UIPOIManager.Instance.POISMALLDIS && isNormalSize)
            {
                smallPoi_img.gameObject.SetActive(true);

                poiBg_img.gameObject.SetActive(false);
                textBg.SetActive(false);

                isNormalSize = false;
            }
            else if (Vector3.Distance(targetCamera.transform.position, targetTr.position) < UIPOIManager.Instance.POISMALLDIS && !isNormalSize)
            {
                smallPoi_img.gameObject.SetActive(false);

                poiBg_img.gameObject.SetActive(true);
                textBg.SetActive(true);

                isNormalSize = true;

            }


        }
        else
        {
            SetContentsOnOff(false);
        }
    }

    public void AddButtonAction(UnityAction act)
    {
        poi_btn.onClick.AddListener(act);
    }
    public void SetContentsOnOff(bool isOn)
    {
        contents.SetActive(isOn);
        isActive = isOn;
    }

    public void SetPOIImage(Sprite poiBg, Sprite small = null)
    {
        poiBg_img.sprite = poiBg;
        smallPoi_img.sprite = small;
    }
    public void SetDeviceType(DeviceType type, string typeText)
    {
        deviceType = type;
        type_txt.text = typeText;
    }
    public void SetLaneText(string laneText)
    {
        lane_txt.text = laneText;
    }
    public void SetButtonInteraction(bool isOn)
    {
        poi_btn.interactable = isOn;
    }
}
