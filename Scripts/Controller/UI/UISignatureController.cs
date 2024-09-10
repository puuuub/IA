using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UISignatureController : MonoBehaviour
{
    [SerializeField]
    UILoginBGController uiLoginController;

    [SerializeField]
    Image contents;

    [SerializeField]
    Image logo;

    
    // Start is called before the first frame update
    void Start()
    {
        contents.gameObject.SetActive(true);

        Sequence sq = DOTween.Sequence();
        Color colWhite = Color.white;
        Color colBlack = Color.black;
        colWhite.a = 0f;
        colBlack.a = 0f;


        sq.Append(logo.DOColor(Color.white, 2f))
        .Append(logo.DOColor(colWhite, 2f))
        .Join(contents.DOColor(colBlack, 2f))
        .OnComplete(delegate
        {
            contents.gameObject.SetActive(false);
            
            uiLoginController.SetInfoOn();
        });

    }

    
}
