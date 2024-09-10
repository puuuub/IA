using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MouseEvent2 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IScrollHandler
{

    //움직임
    public bool IsMove;
    bool IsDragging;
    Vector3 DiffPosition;


    //마우스 오버
    public bool IsMouseOver;
    UnityEvent OnOverEnterEvent;
    UnityEvent OnOverExitEvent;

    //스크롤
    public bool IsScroll;
    List<Action<PointerEventData>> Scrollactions;



    public void AddScrollAction(Action<PointerEventData> act)
    {
        if (Scrollactions == null)
        {
            Scrollactions = new List<Action<PointerEventData>>();
        }
        Scrollactions.Add(act);
    }

    public void AddEnterAction(UnityAction act)
    {
        if(OnOverEnterEvent == null)
        {
            OnOverEnterEvent = new UnityEvent();
        }
        OnOverEnterEvent.AddListener(act);
    }

    public void AddExitAction(UnityAction act)
    {
        if (OnOverExitEvent == null)
        {
            OnOverExitEvent = new UnityEvent();
        }
        OnOverExitEvent.AddListener(act);

    }

    //마우스오버
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (IsMouseOver)
        {
            OnOverEnterEvent?.Invoke();
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (IsMouseOver)
        {
            OnOverExitEvent?.Invoke();
        }
    }


    //드래그이동
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (IsMove && eventData.pointerId == -1)//마우스 왼쪽클릭만 가능
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


    public void OnScroll(PointerEventData eventData)
    {
        if (IsScroll && Scrollactions != null)
        {
            foreach (Action<PointerEventData> act in Scrollactions)
            {
                act.Invoke(eventData);
            }
        }
    }
}
