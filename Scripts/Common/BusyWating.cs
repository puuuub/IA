using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BusyWating : MonoBehaviour
{
    private static BusyWating _ins = null;

    public static BusyWating ins
    {
        get
        {
            if (_ins == null)
            {
                _ins = FindObjectOfType(typeof(BusyWating)) as BusyWating;
                if (_ins == null)
                {
#if !RELEASE
                    Debug.LogError("Error, Fail to get the BusyWating instance");
#endif
                }
            }
            return _ins;
        }
    }

    [SerializeField]
    int Count;
    GameObject Content;
    public Image Bg;
    public Image Icon;

    private void Awake()
    {

        Content = CommonUtility.FindChildObject("Content", transform);
        Bg = CommonUtility.GetChildScript<Image>("Bg", Content);
        Icon = CommonUtility.GetChildScript<Image>("Icon", Content);

    }

    // Start is called before the first frame update
    void Start()
    {
        //Icon.rectTransform.DOLocalMoveX(200, 1).SetLoops(-1, LoopType.Restart);
        //Icon.rectTransform.DOLocalRotate(new Vector3(0.0f, 00.0f, -360f), 0.6f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        Icon.rectTransform.DOLocalRotate(new Vector3(0.0f, 0.0f, -360f), 1.2f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);

        Hide();
    }

    public void Show()
    {
        Content.SetActive(true);
    }

    public void Hide()
    {
        Content.SetActive(false);
    }

    public void ShowWithCount()
    {
        Count++;
        Content.SetActive(true);
    }

    public void HideWithCount()
    {
        Count--;
        if(Count <= 0)
            Content.SetActive(false);
    }

    public bool IsBusy()
    {
        return Content.activeSelf;
    }
}
