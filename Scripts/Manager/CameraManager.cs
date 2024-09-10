using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class CameraManager : SingletonMonoBehaviour<CameraManager>
{

    [SerializeField]
    CineMachineManager cmManager;

    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    OutlineAnimation mainOutlineAni;

    [Header("CameraTarget")]
    [SerializeField]
    RaycastController mainRayController;

    [SerializeField]
    CameraController cameraController;

    [SerializeField]
    CameraRootController cameraRootController;

    [SerializeField]
    GameObject splitScreenCameraRoot;
    [SerializeField]
    List<Camera> splitCameras;      //0:T1, 1:CA, 2:T2

    [SerializeField]
    CameraRootController2 devicePopupCameraRoot;

    // Start is called before the first frame update
    void Start()
    {
        SetOutlineAniOnOff(false);

        SetRootMoveOnOff(false);

        cameraController.SetOutlineAniNum(1);

        devicePopupCameraRoot.SetMoveOnOff(false);

    }

    public void SetOutlineAniOnOff(bool isOn)
    {
        mainOutlineAni.enabled = isOn;
    }

    public void SetRaycastOnOff(bool isOn)
    {
        mainRayController.RaycastOnOff(isOn);
    }
    
    
    public void SetVirCam(AIRPORTAREA area)
    {
        VircamController controller = cmManager.SetVirCam(area);
        SetCamRoot(controller);
    }
    public void SetVirCam(AIRPORTAREA area, AIRPORTAREA2 menu)
    {
        VircamController controller = cmManager.SetVirCam(area, menu);
        SetCamRoot(controller);
    }
    public void SetVirCam(AIRPORTAREA area, SUBAREA subArea)
    {
        VircamController controller = cmManager.SetVirCam(area, subArea);
        SetCamRoot(controller);
    }
    public void SetVirCam(string targetId)
    {
        VircamController controller = cmManager.SetVirCam(targetId);
        SetCamRoot(controller);
    }

    void SetCamRoot(VircamController controller)
    {
        if(controller != null)
        {
            cameraRootController.SetPos(controller.GetPlaneVector());
            cameraRootController.RotateTarget(controller.transform);
            cameraRootController.SetMainCamera(controller.transform);
            
            controller.SetParent(cameraRootController.transform);
        }
    }
    public void SetCineAllOff()
    {
        cmManager.AllOff();
    }

    public void SetRootMoveOnOff(bool isOn)
    {
        cameraRootController.SetMoveOnOff(isOn);

    }

    public void SetCameraOutlineAniOnOff(bool isOn)
    {
        cameraController.SetOutlineAniOnOff(isOn);
        
    }

    public void SetSplitScreenOnOff(bool isOn)
    {
        splitScreenCameraRoot.SetActive(isOn);
        cameraController.SetCameraSplitOnOff(isOn);
    }

    public Camera GetSplitCamera(int idx)
    {
        return splitCameras[idx];
    }

    public Camera GetMainCamera()
    {
        return mainCamera;
    }


    public void SetDevicePopupCameraOnOff(bool isOn)
    {
        if (isOn)
        {
            devicePopupCameraRoot.SetBasicRot();
        }
        devicePopupCameraRoot.SetActiveOnOff(isOn);
        devicePopupCameraRoot.SetMoveOnOff(isOn);
    }
}
