using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using cakeslice;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    OutlineEffect outlineEffect;

    [SerializeField]
    OutlineAnimation outlineAnimation;

    [SerializeField]
    CinemachineBrain cmBrain;


    [SerializeField]
    GameObject cameraRoot;


    public CinemachineBrain CMBrain { get { return cmBrain; } }

    Rect[] splitRects;

    void Start()
    {
        splitRects = new Rect[2];
        splitRects[0] = new Rect(Vector2.zero, Vector2.one);
        splitRects[1] = new Rect(new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f));

    }

    public void SetOutlineAniOnOff(bool isOn)
    {
        outlineAnimation.enabled = isOn;
    }

    public void SetOutlineAniNum(int idx)
    {
        outlineAnimation.SetLineColor(idx);
    }

    public void SetCameraSplitOnOff(bool isOn)
    {
        int idx = 0;
        if (isOn)
        {
            idx = 1;
        }

        mainCamera.rect = splitRects[idx];
    }
}
