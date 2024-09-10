using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CineMachineManager : SingletonMonoBehaviour<CineMachineManager>
{
    
    [SerializeField]
    List<VircamController> areaVirCam;

    [SerializeField]
    List<VircamController> t1Menu;
    
    [SerializeField]
    List<VircamController> t1Boarding;

    [SerializeField]
    List<VircamController> t1CheckIn;

    [SerializeField]
    List<VircamController> t1Departure;


    [SerializeField]
    List<VircamController> caBoarding;


    [SerializeField]
    List<VircamController> t2Menu;

    [SerializeField]
    List<VircamController> t2Boarding;

    [SerializeField]
    List<VircamController> t2Departure;
    


    [SerializeField]
    List<VircamController> idVirCam;


    [SerializeField]
    VircamController exTargetCam; //전체 포문 도는거 방지

    //Coroutine checkCMMoveRoutine;

    public LayerMask target_lm; //zeroPlane

    WaitForSeconds wfs_2f;
    // Start is called before the first frame update
    void Start()
    {
        wfs_2f = new WaitForSeconds(2f);

     
    }

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="area"></param>
    public VircamController SetVirCam(AIRPORTAREA area)
    {
        VircamController result = null; 
        result = areaVirCam[(int)area];

        if (result != null)
        {
            result.SetObjectOnOff(true);
            
            if (exTargetCam != null && exTargetCam != result)
                exTargetCam.SetObjectOnOff(false);

            exTargetCam = result;
        }

        return result;
    }


    public VircamController SetVirCam(AIRPORTAREA area, AIRPORTAREA2 menu)
    {
        VircamController result = null;
        
        
        if (area.Equals(AIRPORTAREA.T1))
        {
         
            switch (menu)
            {
                case AIRPORTAREA2.CKI:
                    result = t1Menu[0];
                    
                    break;
                case AIRPORTAREA2.DEP:
                    result = t1Menu[1];
                    
                    break;
                case AIRPORTAREA2.TRS:
                    break;
                case AIRPORTAREA2.GTE:
                    result = t1Menu[2];
                    break;
            }
        }
        else if (area.Equals(AIRPORTAREA.CA))
        {
            switch (menu)
            {
                case AIRPORTAREA2.GTE:
                    result = caBoarding[0];
                    
                    break;

            }
        }
        else if (area.Equals(AIRPORTAREA.T2))
        {
            switch (menu)
            {
                case AIRPORTAREA2.CKI:
                    result = t2Menu[0];

                    break;
                case AIRPORTAREA2.DEP:
                    result = t2Menu[1];

                    break;
                case AIRPORTAREA2.TRS:
                case AIRPORTAREA2.GTE:
                    result = t2Menu[2];
                    break;

            }
        }

        if (result != null)
        {
            result.SetObjectOnOff(true);

            if (exTargetCam != null && exTargetCam != result)
                exTargetCam.SetObjectOnOff(false);

            exTargetCam = result;

        }

        return result;
    }

    public VircamController SetVirCam(AIRPORTAREA area, SUBAREA subArea)
    {
        VircamController result = null;

        if (area.Equals(AIRPORTAREA.T1))
        {
            switch (subArea)
            {
                case SUBAREA.탑승구중앙:
                    result = t1Boarding[0];
                    
                    break;
                case SUBAREA.탑승구서쪽:
                    result = t1Boarding[1];
                    
                    break;
                case SUBAREA.탑승구동쪽:
                    result = t1Boarding[2];

                    break;
                case SUBAREA.아일랜드A:
                    result = t1CheckIn[0];

                    break;
                case SUBAREA.아일랜드B:
                    result = t1CheckIn[1];

                    break;
                case SUBAREA.아일랜드C:
                    result = t1CheckIn[2];

                    break;
                case SUBAREA.아일랜드L:
                    result = t1CheckIn[3];

                    break;
                case SUBAREA.출국장2번:
                    result = t1Departure[0];

                    break;
                case SUBAREA.출국장3번:
                    result = t1Departure[1];

                    break;
                case SUBAREA.출국장4번:
                    result = t1Departure[2];

                    break;
                case SUBAREA.출국장5번:
                    result = t1Departure[3];

                    break;
            }
        }
        else if (area.Equals(AIRPORTAREA.CA))
        {
            switch (subArea)
            {
                case SUBAREA.탑승구114번:
                    result = caBoarding[1];

                    break;
                case SUBAREA.탑승구122번:
                    result = caBoarding[2];

                    break;

            }
        }
        else if (area.Equals(AIRPORTAREA.T2))
        {
            switch (subArea)
            {
                case SUBAREA.탑승구중앙:
                    result = t2Boarding[0];

                    break;
                case SUBAREA.탑승구서쪽:
                    result = t2Boarding[1];

                    break;
                case SUBAREA.탑승구동쪽:
                    result = t2Boarding[2];

                    break;
                case SUBAREA.출국장1번:
                    result = t2Departure[0];

                    break;
                case SUBAREA.출국장2번:
                    result = t2Departure[1];

                    break;

            }

        }

        if(result!= null)
        {
            result.SetObjectOnOff(true);

            if (exTargetCam != null && exTargetCam != result)
                exTargetCam.SetObjectOnOff(false);

            exTargetCam = result;
        }

        return result;
    }

    public VircamController SetVirCam(string targetId)
    {
        
        VircamController result = idVirCam.Find(x => x.gameObject.name.Equals(targetId));

        if (result != null)
        {
            result.SetObjectOnOff(true);

            if (exTargetCam != null && exTargetCam != result)
                exTargetCam.SetObjectOnOff(false);

            exTargetCam = result;
        }

        return result;
    }

    
    public void AllOff()
    {
        foreach (VircamController controller in areaVirCam)
        {
            controller.SetObjectOnOff(false);
        }
        foreach (VircamController controller in t1Menu)
        {
            controller.SetObjectOnOff(false);
        }
        foreach (VircamController controller in t1Boarding)
        {
            controller.SetObjectOnOff(false);
        }
        foreach (VircamController controller in t1CheckIn)
        {
            controller.SetObjectOnOff(false);
        }
        foreach (VircamController controller in t1Departure)
        {
            controller.SetObjectOnOff(false);
        }
        foreach (VircamController controller in caBoarding)
        {
            controller.SetObjectOnOff(false);
        }

        foreach (VircamController controller in t2Menu)
        {
            controller.SetObjectOnOff(false);
        }
        foreach (VircamController controller in t2Boarding)
        {
            controller.SetObjectOnOff(false);
        }
        foreach (VircamController controller in t2Departure)
        {
            controller.SetObjectOnOff(false);
        }

        foreach (VircamController controller in idVirCam)
        {
            controller.SetObjectOnOff(false);
        }


    }

    public LayerMask GetZeroPlaneMask()
    {
        return target_lm;
    }
}
