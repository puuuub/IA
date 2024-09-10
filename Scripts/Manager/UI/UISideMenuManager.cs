using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UISideMenuManager : SingletonMonoBehaviour<UISideMenuManager>
{
    [SerializeField]
    GameObject sideBg;

    //왼쪽 메뉴
    [SerializeField]
    UISideMenuController uiSideMenuController;

    //구역 선택 Layer
    [SerializeField]
    UILayerController uiLayerController;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++) 
        {
            AIRPORTAREA area = AIRPORTAREA.ALL+i;
            uiLayerController.SetLayerToggleAction(i, delegate
            {
                MainManager.Instance.SetArea(area);
            });
            //SetLayerToggleAction(area);
        }
        uiLayerController.SetLayerAllOffAction(delegate { });

        for (int i = 0; i < 4; i++)
        {
            AIRPORTAREA2 tmp = AIRPORTAREA2.CKI + i;
            uiSideMenuController.SetSideMenuToggleAction(i, delegate
            {
                MainManager.Instance.SelectSideMenu(tmp);
            });
            
        }
        uiSideMenuController.SetSiceMenuAllOffAction(delegate {
            
            MainManager.Instance.SideMenuAllOff();
            uiSideMenuController.SetSubContentsOff(); 
        });




        uiSideMenuController.SetContentsOff();
        uiLayerController.SetContentOnOff(false);
    }

   
    public void SetSideMenu(AIRPORTAREA area)
    {
        //구역에 따라 메뉴 달라짐

        uiSideMenuController.SetSideMenuToggleOff();
        switch (area)
        {
            case AIRPORTAREA.ALL:
                uiSideMenuController.SetSideMenu();
                break;
            case AIRPORTAREA.T1:
            case AIRPORTAREA.CA:
            case AIRPORTAREA.T2:
                List<int> menuList;
                menuList = MenuTree.GetAIRPORTAREA2ByArea(area).ConvertAll(x=>(int)x);
                uiSideMenuController.SetSideMenu(menuList);

                break;
        }
    }

    public void SetLogout()
    {
        uiSideMenuController.SetContentsOff();
        uiLayerController.SetContentOnOff(false);
    }
    public void SetLogin()
    {
        SetSideMenu(AIRPORTAREA.ALL);
        uiLayerController.SetContentOnOff(true);
    }
    public void SetSubContentsList(AIRPORTAREA2 sideMenu, List<SUBAREA> subList, List<SUBAREA> subInteractList)
    {
        List<UnityAction> rowAction = new List<UnityAction>();
        
        for (int i = 0; i < subList.Count; i++)
        {
            int index = i;
            rowAction.Add(() => { MainManager.Instance.SelectSideMenuSubRow(subList[index]); }); 
        }


        uiSideMenuController.SetSubContents((int)sideMenu,
            subList.ConvertAll(x => x.ToString()),
            subInteractList.ConvertAll(x => x.ToString()),
            rowAction,
            ()=> { MainManager.Instance.SideMenuSubAllOff(); }
            );
    }

    public void SetContentsOnOff(bool isOn)
    {
        sideBg.SetActive(isOn);
        uiLayerController.SetContentOnOff(isOn);
        if (isOn)
        {
            uiSideMenuController.SetContentsOn();
        }
        else
        {
            uiSideMenuController.SetContentsOff();
        }
    }

    public bool LayerInvoke(int idx)
    {
        return uiLayerController.LayerToggleInvoke(idx);
    }
}
