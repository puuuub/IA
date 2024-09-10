using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public interface BasePopup
{
    void SetText(string text);

    void ShowPopUp();
    void HidePopUp();

    void Init();
}

public class CommonPopup : MonoBehaviour, BasePopup
{
    public enum Mode
    {
        CONFIRM,
        YES_NO,
        LOGIN,
        LOGOUT,
        //TICKET_POPUP,
        //USERLIST_POPUP,
        //PURCHASELIST_POPUP,
        //PURCHASEDETAIL_POPUP,
        //QR_POPUP,
        //USERINFO_POPUP,
        //TERMS_POPUP,
        //NOTICE_POPUP,
        //INFOMATION_POPUP,
        //BASKET_POPUP,
        //TUTORIAL_POPUP
    }

    public Text ContentText;
    public GameObject Content;
    GameObject Confirmbutton;
    public Button YesButton;
    public Button NoButton;
    GameObject YesNoRoot;
    Mode currentMode;

    public Mode CurrentMode { get { return currentMode; } }

    protected List<UnityAction> MyConfirmActionList = new List<UnityAction>();
    protected List<UnityAction> MyYesActionList = new List<UnityAction>();
    protected List<UnityAction> MyNoActionList = new List<UnityAction>();
    private static Stack<CommonPopup> PopUpStack = new Stack<CommonPopup>();
    private static CommonPopup _ins = null;

    public static CommonPopup ins
    {
        get
        {
            if (_ins == null)
            {
                //_ins = FindObjectOfType(typeof(CommonPopup)) as CommonPopup;
                // FindObjectOfType 는 자식 클래스를 찾아서 게임오브젝트로 찾음
                _ins = GameObject.Find("CommonPopUp").GetComponent<CommonPopup>();
                if (_ins == null)
                {
#if !RELEASE
                    Debug.LogError("Error, Fail to get the CommonPopup instance");
#endif
                }
            }
            return _ins;
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        Init();
        // Unity 에디터에서 작업시 보이는 상태이므로 런타임미 숨김
        Content.SetActive(false);
    }

    public virtual void Init()
    {
        Content = CommonUtility.FindChildObject("Content", transform);
        ContentText = CommonUtility.FindChildObject("ContentText", transform).GetComponent<Text>();
        Confirmbutton = CommonUtility.FindChildObject("Confirm_Button", transform);

        AddMyAction(HidePopUp);

        YesButton = CommonUtility.GetChildScript<Button>("Yes_Button", transform);
        AddYesAction(HidePopUp);

        NoButton = CommonUtility.GetChildScript<Button>("No_Button", transform);
        AddNoAction(HidePopUp);
        YesNoRoot = YesButton.transform.parent.gameObject;
    }

    public void SetMode(Mode mode)
    {
        currentMode = mode;
        if ((int)currentMode > (int)Mode.YES_NO)
        {
            return;
        }
        YesNoRoot.SetActive(true);
        Confirmbutton.gameObject.SetActive(true);
        switch (mode)
        {
            case Mode.CONFIRM:
                // Yes No 숨기기
                YesNoRoot.SetActive(false);
                break;
            case Mode.YES_NO:
                // Confirm 숨기기
                Confirmbutton.gameObject.SetActive(false);
                break;
        }
    }


    public void SetText(string text)
    {
        ContentText.text = text;
    }
    public void SetYesButtonText(string text = "")
    {
        Text txt = CommonUtility.FindChildObject("Text", YesButton.transform).GetComponent<Text>();
        if (text != "")
            txt.text = text;
        else
            txt.text = "예";
    }
    public void SetNoButtonText(string text = "")
    {
        Text txt = CommonUtility.FindChildObject("Text", NoButton.transform).GetComponent<Text>();
        if (text != "")
            txt.text = text;
        else
            txt.text = "아니오";
    }

    public virtual void ShowPopUp()
    {
        //if ((int)CurrentMode > (int)Mode.YES_NO)
        {
            TouchDefender.ins.SetEnable(true);
        }
        Content.SetActive(true);
        
        // 이전 팝업 비표시
        if (PopUpStack.Count > 0)
        {
            CommonPopup prePopup = PopUpStack.Peek();
            //현재 팝업 닫기로 이전 팝업 보여줄 때 자신 팝업까지 사라지는 거 막음
            if (prePopup != this)
            {
                prePopup.Content.SetActive(false);
            }
        }
        if (!PopUpStack.Contains(this))
        {
            PopUpStack.Push(this);
        }
        //gameObject.SetActive(true);
    }

    public void HidePopUp()
    {
        if(PopUpStack.Count != 0)
            PopUpStack.Pop();

        if ((PopUpStack.Count == 0))
        {
            TouchDefender.ins.SetEnable(false);
        }
        Content.SetActive(false);
        
        // 이전 팝업 표시
        if (PopUpStack.Count > 0)
        {
            CommonPopup prePopup = PopUpStack.Peek();
            prePopup.ShowPopUp();
        }
        CleanUpActions(YesButton, MyYesActionList);
        //gameObject.SetActive(false);
    }

    // 이전 팝업을 보여주지 않고 패스
    public void PopPopupStack()
    {
        if(PopUpStack.Count > 0)
            PopUpStack.Pop();
    }

    public CommonPopup GetCurrentPopup()
    {
        if (PopUpStack.Count > 0)
        {
            CommonPopup popup = PopUpStack.Peek();
            return popup;
        }
        else
        {
            return null;
        }
    }

    public void AddMyAction(UnityAction action)
    {
        //MyConfirmActionList += action;
        if (!MyConfirmActionList.Contains(action))
        {
            MyConfirmActionList.Add(action);
            Confirmbutton.GetComponent<Button>().onClick.AddListener(MyConfirmActionList[MyConfirmActionList.Count - 1]);
        }
    }
    public void RemoveMyAction(UnityAction action)
    {
        //MyConfirmActionList -= action;
        if (MyConfirmActionList.Contains(action))
        {
            UnityAction retAct = MyConfirmActionList.Find(x => x.Equals(action));
            Confirmbutton.GetComponent<Button>().onClick.RemoveListener(retAct);
            MyConfirmActionList.Remove(action);
        }
    }

    public void AddYesAction(UnityAction action)
    {
        if (!MyYesActionList.Contains(action))
        {
            MyYesActionList.Add(action);
            YesButton.onClick.AddListener(MyYesActionList[MyYesActionList.Count - 1]);
        }
    }

    public void RemoveYesAction(UnityAction action)
    {
        if (MyYesActionList.Contains(action))
        {
            UnityAction retAct = MyYesActionList.Find(x => x.Equals(action));
            YesButton.onClick.RemoveListener(retAct);
            MyYesActionList.Remove(action);
        }
    }

    public void AddNoAction(UnityAction action)
    {
        if (!MyNoActionList.Contains(action))
        {
            MyNoActionList.Add(action);
            NoButton.onClick.AddListener(MyNoActionList[MyNoActionList.Count - 1]);
        }
    }

    public void RemoveNoAction(UnityAction action)
    {
        if (MyNoActionList.Contains(action))
        {
            UnityAction retAct = MyNoActionList.Find(x => x.Equals(action));
            NoButton.onClick.RemoveListener(retAct);
            MyNoActionList.Remove(action);
        }
    }

    void CleanUpActions(Button target, List<UnityAction> actList)
    {
        if(target != null)
            target.onClick.RemoveAllListeners();
        
        actList.RemoveAll(IsActive);
    }

    bool IsActive(UnityAction act)
    {
        return act.Target != null;
    }
}
