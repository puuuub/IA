using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    public Color[] colors = new Color[2];
    GameObject ScrollIconRoot;
    ScrollRect MyScrollRect;
    GameObject MyScrollView;
    Scrollbar Scrollbar;
    Button LeftButton, RightButton;
    public float Speed = 1.0f;
    public float deltaSpeed = 1.0f;

    public int VisibleItemCnt = 3;
    private float scroll_pos = 0;
    List<float> PositionArr;
    //float[] PositionArr;
    private bool runIt = false;
    private float time;
    private Button takeTheBtn;
    public int btnNumber;

    bool IsNoItem;

    public class SwipeMenuItem
    {
        public GameObject Item;
        public GameObject ScrollIcon;
        public float Position;
    }

    Vector2 normalItemScale = new Vector2(0.9f, 0.9f);
    public Vector2 NormalItemScale { get { return normalItemScale; } set { normalItemScale = value; } }

    // 시작시 보여줄 센터 위치
    int startItemIndex = 1; // 두번째가 센터
    public int StartItemIndex { get { return startItemIndex; } set { startItemIndex = value; } }

    float DistanceInteval;

    public void AddItem(GameObject go)
    {
        go.transform.SetParent(transform);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Scrollbar = CommonUtility.GetChildScript<Scrollbar>("Scrollbar Horizontal", MyScrollView);

        HorizontalLayoutGroup hl = GetComponent<HorizontalLayoutGroup>();

        if(!IsNoItem)
        {
            RectTransform rt = MyScrollView.GetComponent<RectTransform>();
            float w = rt.rect.width;
            float itemW = transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x;
            float itemAreaW = w / VisibleItemCnt;
            float itemW_Gap = (itemAreaW - itemW) / 2;
            if (hl != null)
            {
                hl.padding.left = hl.padding.right = (int)((itemAreaW) * ((VisibleItemCnt - 1) / 2) + itemW_Gap);
            }
        }
        // 하단 아이콘 사용하면 스크롤바 비표시
        Image[] images = Scrollbar.GetComponentsInChildren<Image>();
        foreach(Image img in images)
        {
            img.enabled = false;
      }
    }

    public void Init()
    {
        MyScrollView = transform.parent.parent.gameObject;
        MyScrollRect = MyScrollView.GetComponent<ScrollRect>();

        ScrollIconRoot = CommonUtility.FindChildObject("ScrollIconRoot", MyScrollView);
        LeftButton = CommonUtility.GetChildScript<Button>("LeftButton", MyScrollView.transform);
        LeftButton.onClick.RemoveAllListeners();
        LeftButton.onClick.AddListener(delegate { OnClickLeftRight(-1); });
        RightButton = CommonUtility.GetChildScript<Button>("RightButton", MyScrollView.transform);
        RightButton.onClick.RemoveAllListeners();
        RightButton.onClick.AddListener(delegate { OnClickLeftRight(1); });

        RefreshItem();
    }

    public void EnableDragScroll(bool enable)
    {
        MyScrollRect.horizontal = enable;
    }

    public void RefreshItem()
    {

        GameObject noItemText = CommonUtility.FindChildObject("NoItemText", MyScrollView, false);
        if (noItemText != null)
        {
            noItemText.SetActive(false);
        }
        IsNoItem = false;

        if (transform.childCount <= 1)
        {
            LeftButton.interactable = false;
            RightButton.interactable = false;
        }

        if (transform.childCount == 0) // 아이템이 없는 경우 대응문구 표시
        {
            if (noItemText != null)
            {
                noItemText.SetActive(true);
            }
            IsNoItem = true;
            return;
        }
        PositionArr = new List<float>();

        // 아이템별 위치계산
        PositionArr.Clear();
        //PositionArr = new float[transform.childCount];
        DistanceInteval = transform.childCount != 1 ?  1.0f / (transform.childCount - 1.0f) : 1.0f;

        for (int i = 0; i < transform.childCount; i++)
        {
            //PositionArr[i] = DistanceInteval * i;
            PositionArr.Add(DistanceInteval * i);
        }

        InitScrollIcon();
        // 시작시 보여줄 센터 위치
        ScrollIconRoot.transform.GetChild(startItemIndex).GetComponent<Button>().onClick.Invoke();
    }

    void InitScrollIcon()
    {
        JistUtil.DestroyWithChildren(ScrollIconRoot);
        // 스크롤 아이콘 추가
        for (int k = 0; k < transform.childCount; k++)
        {
            GameObject icon = CommonUtility.LoadPrefab("ScrollIcon", ScrollIconRoot.transform);
            Button iconButton = icon.GetComponent<Button>();
            icon.GetComponent<Button>().onClick.AddListener(delegate { WhichBtnClicked(iconButton); });
            //// 시작시 보여줄 센터 위치
            //if (k == startItemIndex)
            //    iconButton.onClick.Invoke();
        }
    }

    // 아이템 지우기
    public void RemoveItem(int index)
    {
        // 아이템 제거
        DestroyImmediate(transform.GetChild(index).gameObject);
        // 스크롤인덱스 아이콘 제거
        DestroyImmediate(ScrollIconRoot.transform.GetChild(index).gameObject);
        // 위치 계산 리스트 요소 제거
        PositionArr.RemoveAt(index);
        RefreshItem();
    }

    public void RemoveItem(SwipeMenuItem item)
    {
        // 아이템 제거
        DestroyImmediate(item.Item);
        // 스크롤인덱스 아이콘 제거
        DestroyImmediate(item.ScrollIcon);
        // 위치 계산 리스트 요소 제거
        PositionArr.Remove(item.Position);
        RefreshItem();
    }

    public void RemoveAll()
    {
        JistUtil.DestroyWithChildren(gameObject);
        JistUtil.DestroyWithChildren(ScrollIconRoot);
        if (PositionArr != null)
            PositionArr.Clear();
        RefreshItem();
    }

    public GameObject GetScrollIcon(int idx)
    {
        return ScrollIconRoot.transform.GetChild(idx).gameObject;
    }

    public float GetPosition(int idx)
    {
        return PositionArr[idx];
    }

    public void SelectItem(int index)
    {
        Button[] btns = ScrollIconRoot.GetComponentsInChildren<Button>();
        btns[index].onClick.Invoke();
    }

    public void SetActiveLRButton(bool active)
    {
        RightButton.gameObject.SetActive(active);
        LeftButton.gameObject.SetActive(active);
    }

    // Update is called once per frame
    void Update()
    {
        if(IsNoItem || PositionArr == null)
        {
            return;
        }
        //pos = new float[transform.childCount];
        DistanceInteval = 1f / (PositionArr.Count - 1f);

        if (runIt)
        {
            Refresh(DistanceInteval, PositionArr, takeTheBtn);
            time += Time.deltaTime * Speed;

            if (time > 1f)
            {
                time = 0;
                runIt = false;
            }
        }

        //for (int i = 0; i < pos.Length; i++)
        //{
        //    pos[i] = distance * i;
        //}

        if (Input.GetMouseButton(0) && !runIt)
        {
            scroll_pos = Scrollbar.value;
        }
        else
        {
            for (int i = 0; i < PositionArr.Count; i++)
            {
                if (scroll_pos < PositionArr[i] + (DistanceInteval / 2) && scroll_pos > PositionArr[i] - (DistanceInteval / 2))
                {
                    Scrollbar.value = Mathf.Lerp(Scrollbar.value, PositionArr[i], 0.1f * Speed);
                }
            }
        }


        for (int i = 0; i < PositionArr.Count; i++)
        {
            if (scroll_pos < PositionArr[i] + (DistanceInteval / 2) && scroll_pos > PositionArr[i] - (DistanceInteval / 2))
            {
                //매 프레임 호출됨....
                //Debug.LogWarning("Current Selected Level" + i);
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, Vector2.one, 0.1f);
                ScrollIconRoot.transform.GetChild(i).localScale = Vector2.Lerp(ScrollIconRoot.transform.GetChild(i).localScale, new Vector2(1.2f, 1.2f), 0.1f);
                ScrollIconRoot.transform.GetChild(i).GetComponent<Image>().color = colors[1];
                for (int j = 0; j < PositionArr.Count; j++)
                {
                    if (j != i)
                    {
                        transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, normalItemScale, 0.1f);
                        ScrollIconRoot.transform.GetChild(j).localScale = Vector2.Lerp(ScrollIconRoot.transform.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                        ScrollIconRoot.transform.GetChild(j).GetComponent<Image>().color = colors[0];
                    }
                    else
                    {
                        btnNumber = j;
                        RightButton.interactable = j < PositionArr.Count - 1;
                        LeftButton.interactable = j > 0;
                    }
                }
            }
        }


    }

    private void Refresh(float distance, float[] pos, Button btn)
    {
        // btnSayi = System.Int32.Parse(btn.transform.name);

        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                Scrollbar.value = Mathf.Lerp(Scrollbar.value, pos[btnNumber], 1f * Time.deltaTime * Speed);
            }
        }

        //for (int i = 0; i < btn.transform.parent.transform.childCount; i++)
        {
            btn.transform.name = ".";
        }
    }

    private void Refresh(float distance, List<float> pos, Button btn)
    {
        // btnSayi = System.Int32.Parse(btn.transform.name);

        for (int i = 0; i < pos.Count; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                Scrollbar.value = Mathf.Lerp(Scrollbar.value, pos[btnNumber], 1f * Time.deltaTime * Speed);
            }
        }

        for (int i = 0; i < btn.transform.parent.transform.childCount; i++)
        {
            btn.transform.name = ".";
        }
    }

    public void OnClickLeftRight(int direction)
    {
        //if (runIt)
        //{
        //    Speed += deltaSpeed;
        //    return;
        //}
        //Speed = 1.0f;
        Button[] btns = ScrollIconRoot.GetComponentsInChildren<Button>();
        if(btnNumber + direction < btns.Length)
            btns[btnNumber + direction].onClick.Invoke();
    }

    public void WhichBtnClicked(Button btn)
    {
        btn.transform.name = "clicked";
        int cnt = btn.transform.parent.transform.childCount;
        for (int i = 0; i < cnt; i++)
        {
            if (btn.transform.parent.transform.GetChild(i).transform.name == "clicked")
            {
                btnNumber = i;
                takeTheBtn = btn;
                time = 0;
                scroll_pos = (PositionArr[btnNumber]);
                runIt = true;
            }
        }
    }

    //public void ShowPage(int index)
    //{
    //    ScrollIconRoot.transform.GetChild(index).GetComponent<Button>().onClick.Invoke();
    //}
}