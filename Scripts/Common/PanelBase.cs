using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;


/**************************************************************************************/
/* 모든 Panel들의 기본 클래스 */
/* UI에 필요한 기본 기능 정의 */
/**************************************************************************************/
namespace CasualBase
{
    public enum ADD_SLIDER_TYPE
    {
        GAUGE = 0,
        POINT,
    }

    public class PanelBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private int panelType = 0;
        private bool isEnter = false;
        protected StringBuilder sb = new StringBuilder();
        //private List<UISoundInfoData> panelSoundInfoDataList = new List<UISoundInfoData>();

        /*cache*/
        private GameObject myGameObject = null;
        private Transform myTransform = null;
        private RectTransform myRectTransform = null;
        //private Rigidbody myRigidbody = null;

        public new GameObject gameObject
        {
            get
            {
                if (myGameObject == null)
                    myGameObject = base.gameObject;
                return myGameObject;
            }
        }

        public new Transform transform
        {
            get
            {
                if (myTransform == null)
                    myTransform = base.transform;
                return myTransform;
            }
        }

        public RectTransform rectTransform
        {
            get { return myRectTransform; }
        }

        //public new Rigidbody rigidbody
        //{
        //    get
        //    {
        //        if (myRigidbody == null)
        //            myRigidbody = GetComponent<Rigidbody>();
        //        return myRigidbody;
        //    }
        //}


        /* Move Rect */
        protected Camera mainCamera = null;
        protected Canvas canvas = null;

        // Animator component
        protected Animator _animator = null;
        public Animator animator
        {
            get { return _animator; }
        }

        public virtual void Initialize()
        {
            if (myGameObject == null)
                myGameObject = gameObject;
            if (myTransform == null)
                myTransform = transform;
            if (canvas == null)
            {
                Canvas[] canvaslist = GameObject.FindObjectsOfType(typeof(Canvas)) as Canvas[];
                foreach(var c in canvaslist)
                {
                    if (c.name.Contains(SceneManager.GetActiveScene().name))
                    {
                        canvas = c;
                        break;
                    }
                }
            }
            if (mainCamera == null)
                mainCamera = canvas.worldCamera;
            if (myRectTransform == null)
                myRectTransform = gameObject.GetComponent<RectTransform>();                
            if (_animator == null)
                _animator = myGameObject.GetComponent<Animator>();
        }

        public void SetPanelType(int panel_type)
        {
            panelType = panel_type;
        }

        public int GetPanelType()
        {
            return panelType;
        }

        public void SetAsLastSibling()
        {
            if (myTransform == null)
                return;

            // 패널을 가장 마지막 순서로 위치
            myTransform.SetAsLastSibling();
        }

        //PanelBase를 상속받은 모든 패널들은 OpenPanel을 호출해서 활성화
        public virtual void OpenPanel()
        {
            if (myGameObject == null)
                return;

            myGameObject.SetActive(true);            
        }

        public virtual void ClosePanel()
        {
            if (myGameObject == null)
                return;

            myGameObject.SetActive(false);
        }

        public bool IsEnter()
        {
            return isEnter;
        }

        public bool IsActive()
        {
            if (myGameObject == null)
                return false;

            return myGameObject.activeSelf;
        }


        //오브젝트 이름으로 자식의 게임 오브젝트를 찾음(자식의 이름이 유니크 해야함)
        protected GameObject FindChildObject(string name, bool useLog = true)
        {
            return CommonUtility.FindChildObject(name, transform, useLog);
        }

        /* 이름으로 자식 T 찾음 */
        protected T GetChildScript<T>(string name, bool useLog = true) where T : UnityEngine.Object
        {
            return CommonUtility.GetChildScript<T>(name, transform, useLog);
        }

        /* 이름으로 자식 텍스트 찾음 */
        protected Text GetChildText(string name, string init_text = null, bool useLog = true)
        {
            Text text = null;

            GameObject go = FindChildObject(name, useLog);
            if (go != null)
                text = go.GetComponent(typeof(Text)) as Text;

            if (text != null && init_text != null)
                text.text = init_text;

            return text;
        }


        /* 이름과 경로로 자식 오브젝트 찾음 */
        protected GameObject FindChildObjectPath(string name, bool active)
        {
            GameObject go = FindChildObjectPath(name);
            if (go != null)
                go.SetActive(active);
            return go;
        }

        /* 이름과 경로로 자식 오브젝트 찾음 */
        protected GameObject FindChildObjectPath(string name)
        {
            GameObject go = null;

            if (name != string.Empty)
            {
                try
                {
                    Transform tm = myTransform.Find(name);
                    if (tm != null)
                        go = tm.gameObject;
                }
                catch
                {
#if !RELEASE
                    sb.Length = 0;
                    sb.AppendFormat("{0} child not found", name);
                    Debug.Log(sb.ToString());
#endif
                }
            }
            return go;
        }

        /* 이름과 경로로 자식 스크립트 찾음*/
        protected T GetChildScriptPath<T>(string child_name) where T : Component
        {
            T t = null;

            GameObject go = FindChildObjectPath(child_name);
            if (go != null)
                t = go.GetComponent(typeof(T)) as T;

            return t;
        }


        protected Button AddButtonEvent(Button button, Action callBack)
        {

            return CommonUtility.AddButtonEvent(button, callBack);
        }       

        protected GameObject AddClickEvent(GameObject clickObj, Action onCallBack, Action offCallBack=null)
        {

            return CommonUtility.AddClickEvent(clickObj, onCallBack, offCallBack);
        }

        protected GameObject AddClickEvent(string clickObjName, Action onCallBack, Action offCallBack = null)
        {
            return CommonUtility.AddClickEvent(clickObjName, transform, onCallBack, offCallBack);
        }

        protected GameObject InitPrefab(string name)
        {
            return CommonUtility.LoadPrefab(name, transform);
        }

        protected GameObject InitPrefab(GameObject initObj)
        {
            return CommonUtility.InitPrefab(initObj, transform);
        }

        /* 기본적으로 panelbase는 들어오고 나감의 이벤트를 제공 */
        public void OnPointerEnter(PointerEventData eventData)
        {
            isEnter = true;
        }

        /* 기본적으로 panelbase는 들어오고 나감의 이벤트를 제공 */
        public void OnPointerExit(PointerEventData eventData)
        {
            isEnter = false;
        }

        /* 이벤트 달기 */
        public void AddEventTrigger(GameObject obj, EventTriggerType eventID, UnityEngine.Events.UnityAction<BaseEventData> callback)
        {
            Graphic[] graphics = obj.GetComponents<Graphic>();
            for (int i = 0; i < graphics.Length; ++i)
            {
                if (!graphics[i].raycastTarget)
                    graphics[i].raycastTarget = true;
            }

            EventTrigger eventTrigger = obj.GetComponent<EventTrigger>();
            if (eventTrigger == null)
                eventTrigger = obj.AddComponent<EventTrigger>();

            //if (eventTrigger.triggers.Find(entry => entry.eventID == eventID) == null)
            {
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = eventID;
                entry.callback.AddListener(callback);
                eventTrigger.triggers.Add(entry);
            }
        }
    }
}