using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{

    public GameObject TargetImg;

    public bool IsMove;
    public bool IsMouseOver;

    bool IsDragging;
    Vector3 DiffPosition;

    

    
    
    //���콺����
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (IsMouseOver)
        {
            //TargetImg.sprite = MouseOverTargetsp;
            //TargetImg.SetNativeSize();
            TargetImg.SetActive(true);
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (IsMouseOver)
        {
            TargetImg.SetActive(false);
        }
    }


    //�巡���̵�
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {


        if (IsMove && eventData.pointerId == -1)//���콺 ����Ŭ���� ����
        {
            DiffPosition = this.transform.position - new Vector3(eventData.position.x, eventData.position.y, 0);
            IsDragging = true;
        }
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (IsMove && IsDragging)
        {
            this.transform.position = new Vector3(eventData.position.x, eventData.position.y, 0) + DiffPosition;
        }
    }
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        IsDragging = false;
    }


  
}
