using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class CustomUIButton : Button
{

    public bool IsPointDown { get; private set; }

    ButtonPointerDownEvect _onPointerButtonDown = new ButtonPointerDownEvect();
    public ButtonPointerDownEvect onPointerButtonDown  { get { return _onPointerButtonDown; } set { _onPointerButtonDown = value; } }

    ButtonDeselectedEvent _onDeselcted = new ButtonDeselectedEvent();
    public ButtonDeselectedEvent onDeselcted { get { return _onDeselcted; } set { _onDeselcted = value; } }

    //public SelectionState MyCurrentSelectionState { get { return base.currentSelectionState; } }

    //public SelectionState GetState()
    //{
    //    return base.currentSelectionState;
    //}

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        IsPointDown = true;
        onPointerButtonDown.Invoke();
        print("OnPointerDown~~~~~");
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        IsPointDown = false;
        print("OnPointerUp~~~~~");
    }

    protected override void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        print("Deselect~~~~~");
        onDeselcted.Invoke();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        print("Select~~~~~");
    }

    public class ButtonPointerDownEvect : UnityEvent
    {
        //public ButtonPointerDownEvect() { }
    }

    public class ButtonDeselectedEvent : UnityEvent
    {
        public ButtonDeselectedEvent() { }
    }
}
