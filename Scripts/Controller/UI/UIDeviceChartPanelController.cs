using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIDeviceChartPanelController : MonoBehaviour
{
    [SerializeField]
    GameObject contentsMask;

    [SerializeField]
    GameObject contents;

    [SerializeField]
    Toggle hide_tg;

    [SerializeField]
    Image[] graph_imgs;
    [SerializeField]
    TMP_Text[] value_txts;

    float[] hideY = { -137f, 0f };

    const float FILLDURATION = 2f;
    // Start is called before the first frame update
    void Start()
    {
        hide_tg.onValueChanged.AddListener(HideAction);


        SetContentsOnOff(false);
    }
    
    void HideAction(bool x)
    {
        
        if (x)
        {
            hide_tg.image.transform.localRotation = Quaternion.Euler(180f * Vector3.forward);
            contents.transform.DOLocalMoveY(hideY[0], MainManager.HIDEDURATION);
        }
        else
        {
            hide_tg.image.transform.localRotation = Quaternion.Euler(Vector3.zero);
            contents.transform.DOLocalMoveY(hideY[1], MainManager.HIDEDURATION);
        }
    }
    public void SetContentsOn(int idx, float percent)
    {
        contentsMask.SetActive(true);

        graph_imgs[idx].DOFillAmount(percent, FILLDURATION);
        value_txts[idx].text = ((int)(percent * 100)).ToString() + StaticText.SetSize(12, StaticText.EMPTY + StaticText.PERCENT);


    }

    public void SetContentsOnOff(bool isOn)
    {
        //true는 단순히 사용
        //데이터 셋은 SetContentsOn에서

        contentsMask.SetActive(isOn);
    }

}
