using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomToggle : MonoBehaviour
{
    [SerializeField]
    Toggle MyToggle;

    [SerializeField]
    GameObject ChildGraphicRoot;


    Graphic[] MyGraphics;

    private void Awake()
    {
        MyGraphics = ChildGraphicRoot.GetComponentsInChildren<Graphic>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Graphic g in MyGraphics)
        {
            g.canvasRenderer.SetAlpha(MyToggle.graphic.canvasRenderer.GetAlpha());
        }
    }
}
