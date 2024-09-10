using System.Collections.Generic;


public static class MenuTree
{

    static Dictionary<AIRPORTAREA, List<SUBAREA>> CKITree;
    static Dictionary<AIRPORTAREA, List<SUBAREA>> DEPTree;
    public static Dictionary<AIRPORTAREA, Dictionary<SUBAREA, List<SUBAREA>>> GTETree;
    static List<SUBAREA> CAGTETree;
    static Dictionary<AIRPORTAREA, List<SUBAREA>> subInteractList;

    static Dictionary<AIRPORTAREA, Dictionary<AIRPORTAREA2, List<DeviceType>>> menuDevice;   //�޴��� POI����̽� Ÿ��

    static string[] deviceTypeKor = { "���ε�ϴܸ���", "������Ű����ũ", "���������İ���Ʈ", "����Ȯ��Ű����ũ",
        "������������Ʈ", "���ι������԰���Ʈ",  "�����������԰���Ʈ",  "ȯ�°���Ʈ", "ȯ�½º��Ȱ���Ʈ", };

    static Dictionary<AIRPORTAREA, List<AIRPORTAREA2>> areaMenu;



    //������ �޴� ����
    static MenuTree()
    {
        CKITree = new Dictionary<AIRPORTAREA, List<SUBAREA>>();
        DEPTree = new Dictionary<AIRPORTAREA, List<SUBAREA>>();
        GTETree = new Dictionary<AIRPORTAREA, Dictionary<SUBAREA, List<SUBAREA>>>();
        CAGTETree = new List<SUBAREA>();

        CKITree.Add(AIRPORTAREA.T1, new List<SUBAREA>());
        CKITree[AIRPORTAREA.T1].Add(SUBAREA.���Ϸ���A);
        CKITree[AIRPORTAREA.T1].Add(SUBAREA.���Ϸ���B);
        CKITree[AIRPORTAREA.T1].Add(SUBAREA.���Ϸ���C);
        CKITree[AIRPORTAREA.T1].Add(SUBAREA.���Ϸ���E);
        CKITree[AIRPORTAREA.T1].Add(SUBAREA.���Ϸ���F);
        CKITree[AIRPORTAREA.T1].Add(SUBAREA.���Ϸ���G);
        CKITree[AIRPORTAREA.T1].Add(SUBAREA.���Ϸ���H);
        CKITree[AIRPORTAREA.T1].Add(SUBAREA.���Ϸ���J);
        CKITree[AIRPORTAREA.T1].Add(SUBAREA.���Ϸ���K);
        CKITree[AIRPORTAREA.T1].Add(SUBAREA.���Ϸ���L);
        CKITree[AIRPORTAREA.T1].Add(SUBAREA.���Ϸ���M);
        CKITree[AIRPORTAREA.T1].Add(SUBAREA.���Ϸ���N);

        DEPTree.Add(AIRPORTAREA.T1, new List<SUBAREA>());
        DEPTree[AIRPORTAREA.T1].Add(SUBAREA.�ⱹ��2��);
        DEPTree[AIRPORTAREA.T1].Add(SUBAREA.�ⱹ��3��);
        DEPTree[AIRPORTAREA.T1].Add(SUBAREA.�ⱹ��4��);
        DEPTree[AIRPORTAREA.T1].Add(SUBAREA.�ⱹ��5��);

        GTETree.Add(AIRPORTAREA.T1, new Dictionary<SUBAREA, List<SUBAREA>>());
        GTETree[AIRPORTAREA.T1].Add(SUBAREA.ž�±��߾�, new List<SUBAREA>());
        GTETree[AIRPORTAREA.T1].Add(SUBAREA.ž�±�����, new List<SUBAREA>());
        GTETree[AIRPORTAREA.T1][SUBAREA.ž�±�����].Add(SUBAREA.ž�±�10��);
        GTETree[AIRPORTAREA.T1][SUBAREA.ž�±�����].Add(SUBAREA.ž�±�15��);
        GTETree[AIRPORTAREA.T1][SUBAREA.ž�±�����].Add(SUBAREA.ž�±�17��);
        GTETree[AIRPORTAREA.T1][SUBAREA.ž�±�����].Add(SUBAREA.ž�±�21��);
        GTETree[AIRPORTAREA.T1][SUBAREA.ž�±�����].Add(SUBAREA.ž�±�23��);

        GTETree[AIRPORTAREA.T1].Add(SUBAREA.ž�±�����, new List<SUBAREA>());
        GTETree[AIRPORTAREA.T1][SUBAREA.ž�±�����].Add(SUBAREA.ž�±�36��);
        GTETree[AIRPORTAREA.T1][SUBAREA.ž�±�����].Add(SUBAREA.ž�±�45��);


        CKITree.Add(AIRPORTAREA.T2, new List<SUBAREA>());
        CKITree[AIRPORTAREA.T2].Add(SUBAREA.���Ϸ���A);
        CKITree[AIRPORTAREA.T2].Add(SUBAREA.���Ϸ���B);
        CKITree[AIRPORTAREA.T2].Add(SUBAREA.���Ϸ���C);
        CKITree[AIRPORTAREA.T2].Add(SUBAREA.���Ϸ���E);
        CKITree[AIRPORTAREA.T2].Add(SUBAREA.���Ϸ���F);
        CKITree[AIRPORTAREA.T2].Add(SUBAREA.���Ϸ���G);
        CKITree[AIRPORTAREA.T2].Add(SUBAREA.���Ϸ���H);

        DEPTree.Add(AIRPORTAREA.T2, new List<SUBAREA>());
        DEPTree[AIRPORTAREA.T2].Add(SUBAREA.�ⱹ��1��);
        DEPTree[AIRPORTAREA.T2].Add(SUBAREA.�ⱹ��2��);

        GTETree.Add(AIRPORTAREA.T2, new Dictionary<SUBAREA, List<SUBAREA>>());
        GTETree[AIRPORTAREA.T2].Add(SUBAREA.ž�±��߾�, new List<SUBAREA>());
        GTETree[AIRPORTAREA.T2][SUBAREA.ž�±��߾�].Add(SUBAREA.ž�±�248��);

        GTETree[AIRPORTAREA.T2].Add(SUBAREA.ž�±�����, new List<SUBAREA>());
        GTETree[AIRPORTAREA.T2][SUBAREA.ž�±�����].Add(SUBAREA.ž�±�232��);
        GTETree[AIRPORTAREA.T2][SUBAREA.ž�±�����].Add(SUBAREA.ž�±�247��);

        GTETree[AIRPORTAREA.T2].Add(SUBAREA.ž�±�����, new List<SUBAREA>());
        GTETree[AIRPORTAREA.T2][SUBAREA.ž�±�����].Add(SUBAREA.ž�±�254��);
        GTETree[AIRPORTAREA.T2][SUBAREA.ž�±�����].Add(SUBAREA.ž�±�255��);
        GTETree[AIRPORTAREA.T2][SUBAREA.ž�±�����].Add(SUBAREA.ž�±�265��);
        GTETree[AIRPORTAREA.T2][SUBAREA.ž�±�����].Add(SUBAREA.ž�±�266��);
        GTETree[AIRPORTAREA.T2][SUBAREA.ž�±�����].Add(SUBAREA.ž�±�267��);


        CAGTETree.Add(SUBAREA.ž�±�114��);
        CAGTETree.Add(SUBAREA.ž�±�122��);

        areaMenu = new Dictionary<AIRPORTAREA, List<AIRPORTAREA2>>();
        areaMenu.Add(AIRPORTAREA.ALL, new List<AIRPORTAREA2>());
        areaMenu.Add(AIRPORTAREA.T1, new List<AIRPORTAREA2>());
        areaMenu[AIRPORTAREA.T1].Add(AIRPORTAREA2.CKI);
        areaMenu[AIRPORTAREA.T1].Add(AIRPORTAREA2.DEP);
        areaMenu[AIRPORTAREA.T1].Add(AIRPORTAREA2.TRS);
        areaMenu[AIRPORTAREA.T1].Add(AIRPORTAREA2.GTE);

        areaMenu.Add(AIRPORTAREA.CA, new List<AIRPORTAREA2>());
        areaMenu[AIRPORTAREA.CA].Add(AIRPORTAREA2.TRS);
        areaMenu[AIRPORTAREA.CA].Add(AIRPORTAREA2.GTE);

        areaMenu.Add(AIRPORTAREA.T2, new List<AIRPORTAREA2>());
        areaMenu[AIRPORTAREA.T2].Add(AIRPORTAREA2.CKI);
        areaMenu[AIRPORTAREA.T2].Add(AIRPORTAREA2.DEP);
        areaMenu[AIRPORTAREA.T2].Add(AIRPORTAREA2.TRS);
        areaMenu[AIRPORTAREA.T2].Add(AIRPORTAREA2.GTE);



        //���� - �޴� - ���� ���Ÿ��
        menuDevice = new Dictionary<AIRPORTAREA, Dictionary<AIRPORTAREA2, List<DeviceType>>>();
        menuDevice.Add(AIRPORTAREA.ALL, new Dictionary<AIRPORTAREA2, List<DeviceType>>());
        menuDevice[AIRPORTAREA.ALL].Add(AIRPORTAREA2.CKI, new List<DeviceType>());
        menuDevice[AIRPORTAREA.ALL][AIRPORTAREA2.CKI].Add(DeviceType.BRT);
        menuDevice[AIRPORTAREA.ALL][AIRPORTAREA2.CKI].Add(DeviceType.BEG);
        menuDevice[AIRPORTAREA.ALL][AIRPORTAREA2.CKI].Add(DeviceType.BDG);

        menuDevice.Add(AIRPORTAREA.T1, new Dictionary<AIRPORTAREA2, List<DeviceType>>());
        menuDevice[AIRPORTAREA.T1].Add(AIRPORTAREA2.CKI, new List<DeviceType>());
        menuDevice[AIRPORTAREA.T1][AIRPORTAREA2.CKI].Add(DeviceType.BRT);
        menuDevice[AIRPORTAREA.T1][AIRPORTAREA2.CKI].Add(DeviceType.BEG);
        menuDevice[AIRPORTAREA.T1][AIRPORTAREA2.CKI].Add(DeviceType.BDG);

        menuDevice[AIRPORTAREA.T1].Add(AIRPORTAREA2.DEP, new List<DeviceType>());
        menuDevice[AIRPORTAREA.T1][AIRPORTAREA2.DEP].Add(DeviceType.DPK);
        menuDevice[AIRPORTAREA.T1][AIRPORTAREA2.DEP].Add(DeviceType.DPG);

        menuDevice[AIRPORTAREA.T1].Add(AIRPORTAREA2.TRS, new List<DeviceType>());
        menuDevice[AIRPORTAREA.T1].Add(AIRPORTAREA2.GTE, new List<DeviceType>());
        menuDevice[AIRPORTAREA.T1][AIRPORTAREA2.GTE].Add(DeviceType.PVK);
        menuDevice[AIRPORTAREA.T1][AIRPORTAREA2.GTE].Add(DeviceType.SBG);

        menuDevice[AIRPORTAREA.T1].Add(AIRPORTAREA2.NONE, new List<DeviceType>());
        menuDevice[AIRPORTAREA.T1][AIRPORTAREA2.NONE].Add(DeviceType.BRT);
        menuDevice[AIRPORTAREA.T1][AIRPORTAREA2.NONE].Add(DeviceType.BEG);
        menuDevice[AIRPORTAREA.T1][AIRPORTAREA2.NONE].Add(DeviceType.BDG);
        menuDevice[AIRPORTAREA.T1][AIRPORTAREA2.NONE].Add(DeviceType.DPK);
        menuDevice[AIRPORTAREA.T1][AIRPORTAREA2.NONE].Add(DeviceType.DPG);
        menuDevice[AIRPORTAREA.T1][AIRPORTAREA2.NONE].Add(DeviceType.PVK);
        menuDevice[AIRPORTAREA.T1][AIRPORTAREA2.NONE].Add(DeviceType.SBG);

        menuDevice.Add(AIRPORTAREA.T2, new Dictionary<AIRPORTAREA2, List<DeviceType>>());
        menuDevice[AIRPORTAREA.T2].Add(AIRPORTAREA2.CKI, new List<DeviceType>());
        menuDevice[AIRPORTAREA.T2].Add(AIRPORTAREA2.DEP, new List<DeviceType>());
        menuDevice[AIRPORTAREA.T2][AIRPORTAREA2.DEP].Add(DeviceType.DPG);

        menuDevice[AIRPORTAREA.T2].Add(AIRPORTAREA2.TRS, new List<DeviceType>());
        menuDevice[AIRPORTAREA.T2].Add(AIRPORTAREA2.GTE, new List<DeviceType>());
        menuDevice[AIRPORTAREA.T2][AIRPORTAREA2.GTE].Add(DeviceType.PVK);
        menuDevice[AIRPORTAREA.T2][AIRPORTAREA2.GTE].Add(DeviceType.SBG);

        menuDevice[AIRPORTAREA.T2].Add(AIRPORTAREA2.NONE, new List<DeviceType>());
        menuDevice[AIRPORTAREA.T2][AIRPORTAREA2.NONE].Add(DeviceType.DPG);
        menuDevice[AIRPORTAREA.T2][AIRPORTAREA2.NONE].Add(DeviceType.PVK);
        menuDevice[AIRPORTAREA.T2][AIRPORTAREA2.NONE].Add(DeviceType.SBG);

        menuDevice.Add(AIRPORTAREA.CA, new Dictionary<AIRPORTAREA2, List<DeviceType>>());
        
        
        menuDevice[AIRPORTAREA.CA].Add(AIRPORTAREA2.TRS, new List<DeviceType>());
        menuDevice[AIRPORTAREA.CA].Add(AIRPORTAREA2.GTE, new List<DeviceType>());
        menuDevice[AIRPORTAREA.CA][AIRPORTAREA2.GTE].Add(DeviceType.PVK);
        menuDevice[AIRPORTAREA.CA][AIRPORTAREA2.GTE].Add(DeviceType.SBG);

        menuDevice[AIRPORTAREA.CA].Add(AIRPORTAREA2.NONE, new List<DeviceType>());
        menuDevice[AIRPORTAREA.CA][AIRPORTAREA2.NONE].Add(DeviceType.PVK);
        menuDevice[AIRPORTAREA.CA][AIRPORTAREA2.NONE].Add(DeviceType.SBG);


        //������ Ȱ��ȭ �޴�
        subInteractList = new Dictionary<AIRPORTAREA, List<SUBAREA>>();

        subInteractList.Add(AIRPORTAREA.T1, new List<SUBAREA>());
        subInteractList[AIRPORTAREA.T1].Add(SUBAREA.���Ϸ���A);
        subInteractList[AIRPORTAREA.T1].Add(SUBAREA.���Ϸ���B);
        subInteractList[AIRPORTAREA.T1].Add(SUBAREA.���Ϸ���C);
        subInteractList[AIRPORTAREA.T1].Add(SUBAREA.���Ϸ���L);

        subInteractList[AIRPORTAREA.T1].Add(SUBAREA.�ⱹ��2��);
        subInteractList[AIRPORTAREA.T1].Add(SUBAREA.�ⱹ��3��);
        subInteractList[AIRPORTAREA.T1].Add(SUBAREA.�ⱹ��4��);
        subInteractList[AIRPORTAREA.T1].Add(SUBAREA.�ⱹ��5��);

        subInteractList[AIRPORTAREA.T1].Add(SUBAREA.ž�±��߾�);
        subInteractList[AIRPORTAREA.T1].Add(SUBAREA.ž�±�����);
        subInteractList[AIRPORTAREA.T1].Add(SUBAREA.ž�±�����);

        subInteractList.Add(AIRPORTAREA.CA, new List<SUBAREA>());
        subInteractList[AIRPORTAREA.CA].Add(SUBAREA.ž�±�114��);
        subInteractList[AIRPORTAREA.CA].Add(SUBAREA.ž�±�122��);

        subInteractList.Add(AIRPORTAREA.T2, new List<SUBAREA>());
        subInteractList[AIRPORTAREA.T2].Add(SUBAREA.�ⱹ��1��);
        subInteractList[AIRPORTAREA.T2].Add(SUBAREA.�ⱹ��2��);

        subInteractList[AIRPORTAREA.T2].Add(SUBAREA.ž�±��߾�);
        subInteractList[AIRPORTAREA.T2].Add(SUBAREA.ž�±�����);
        subInteractList[AIRPORTAREA.T2].Add(SUBAREA.ž�±�����);

    }

    public static List<DeviceType> GetDeviceTypes(AIRPORTAREA area, AIRPORTAREA2 menu)
    {
        if (area.Equals(AIRPORTAREA.ALL))
        {
            return null;
        }
        else
        {
            return menuDevice[area][menu];
        }
    }
    public static List<AIRPORTAREA2> GetAIRPORTAREA2ByArea(AIRPORTAREA area)
    {
        return areaMenu[area];
    }
    public static List<SUBAREA> GetCKISubArea(AIRPORTAREA area)
    {
        return CKITree[area];
    }
    public static List<SUBAREA> GetSubInteractList(AIRPORTAREA area)
    {
        return subInteractList[area];
    }
    public static List<SUBAREA> GetDEPSubArea(AIRPORTAREA area)
    {
        return DEPTree[area];
    }

    public static List<SUBAREA> GetGTESubArea(AIRPORTAREA area)
    {
        if (area.Equals(AIRPORTAREA.CA))
        {
            return CAGTETree;
        }
        else
        {
            List<SUBAREA> tmp = new List<SUBAREA>();
            foreach (SUBAREA key in GTETree[area].Keys)
            {
                tmp.Add(key);
            }
            return tmp;
        }
    }

    public static string GetDeviceTypeKor(DeviceType type)
    {
        return deviceTypeKor[(int)type];
    }
}

public static class PoiMapping
{
    static Dictionary<string, List<string>> poiDeviceGroup;

    static PoiMapping()
    {

        //��� 2Lane�̻� �� ��� �ϳ��� POI�� ����
        poiDeviceGroup = new Dictionary<string, List<string>>();
        //t1
        //BRT
        poiDeviceGroup.Add("ICN1CA03PT01", new List<string>());
        poiDeviceGroup["ICN1CA03PT01"].Add("ICN1CA03PT01");
        poiDeviceGroup.Add("ICN1CA09PT01", new List<string>());
        poiDeviceGroup["ICN1CA09PT01"].Add("ICN1CA09PT01");
        poiDeviceGroup.Add("ICN1CB07PT01", new List<string>());
        poiDeviceGroup["ICN1CB07PT01"].Add("ICN1CB07PT01");
        poiDeviceGroup.Add("ICN1CB03PT01", new List<string>());
        poiDeviceGroup["ICN1CB03PT01"].Add("ICN1CB03PT01");

        //DPK
        poiDeviceGroup.Add("ICN1D002DK01", new List<string>());
        poiDeviceGroup["ICN1D002DK01"].Add("ICN1D002DK01");
        poiDeviceGroup["ICN1D002DK01"].Add("ICN1D002DK02");
        poiDeviceGroup.Add("ICN1D002DK04", new List<string>());
        poiDeviceGroup["ICN1D002DK04"].Add("ICN1D002DK04");
        poiDeviceGroup["ICN1D002DK04"].Add("ICN1D002DK03");
        poiDeviceGroup.Add("ICN1D003DK01", new List<string>());
        poiDeviceGroup["ICN1D003DK01"].Add("ICN1D003DK01");
        poiDeviceGroup["ICN1D003DK01"].Add("ICN1D003DK02");
        poiDeviceGroup.Add("ICN1D003DK04", new List<string>());
        poiDeviceGroup["ICN1D003DK04"].Add("ICN1D003DK04");
        poiDeviceGroup["ICN1D003DK04"].Add("ICN1D003DK03");
        poiDeviceGroup.Add("ICN1D004DK01", new List<string>());
        poiDeviceGroup["ICN1D004DK01"].Add("ICN1D004DK01");
        poiDeviceGroup["ICN1D004DK01"].Add("ICN1D004DK02");
        poiDeviceGroup.Add("ICN1D004DK04", new List<string>());
        poiDeviceGroup["ICN1D004DK04"].Add("ICN1D004DK04");
        poiDeviceGroup["ICN1D004DK04"].Add("ICN1D004DK03");
        poiDeviceGroup.Add("ICN1D005DK01", new List<string>());
        poiDeviceGroup["ICN1D005DK01"].Add("ICN1D005DK01");
        poiDeviceGroup["ICN1D005DK01"].Add("ICN1D005DK02");
        poiDeviceGroup.Add("ICN1D005DK04", new List<string>());
        poiDeviceGroup["ICN1D005DK04"].Add("ICN1D005DK04");
        poiDeviceGroup["ICN1D005DK04"].Add("ICN1D005DK03");

        //SBG
        poiDeviceGroup.Add("ICN1G010BG01", new List<string>());
        poiDeviceGroup["ICN1G010BG01"].Add("ICN1G010BG01");
        poiDeviceGroup["ICN1G010BG01"].Add("ICN1G010BG02");
        poiDeviceGroup["ICN1G010BG01"].Add("ICN1G010BG03");
        poiDeviceGroup.Add("ICN1G015BG01", new List<string>());
        poiDeviceGroup["ICN1G015BG01"].Add("ICN1G015BG01");
        poiDeviceGroup["ICN1G015BG01"].Add("ICN1G015BG02");
        poiDeviceGroup["ICN1G015BG01"].Add("ICN1G015BG03");
        poiDeviceGroup.Add("ICN1G017BG01", new List<string>());
        poiDeviceGroup["ICN1G017BG01"].Add("ICN1G017BG01");
        poiDeviceGroup["ICN1G017BG01"].Add("ICN1G017BG02");
        poiDeviceGroup["ICN1G017BG01"].Add("ICN1G017BG03");
        poiDeviceGroup.Add("ICN1G021BG01", new List<string>());
        poiDeviceGroup["ICN1G021BG01"].Add("ICN1G021BG01");
        poiDeviceGroup["ICN1G021BG01"].Add("ICN1G021BG02");
        poiDeviceGroup["ICN1G021BG01"].Add("ICN1G021BG03");
        poiDeviceGroup.Add("ICN1G023BG01", new List<string>());
        poiDeviceGroup["ICN1G023BG01"].Add("ICN1G023BG01");
        poiDeviceGroup["ICN1G023BG01"].Add("ICN1G023BG02");
        poiDeviceGroup["ICN1G023BG01"].Add("ICN1G023BG03");
        poiDeviceGroup.Add("ICN1G036BG01", new List<string>());
        poiDeviceGroup["ICN1G036BG01"].Add("ICN1G036BG01");
        poiDeviceGroup["ICN1G036BG01"].Add("ICN1G036BG02");
        poiDeviceGroup.Add("ICN1G045BG01", new List<string>());
        poiDeviceGroup["ICN1G045BG01"].Add("ICN1G045BG01");
        poiDeviceGroup["ICN1G045BG01"].Add("ICN1G045BG02");
        poiDeviceGroup["ICN1G045BG01"].Add("ICN1G045BG03");

        //BDG BEG
        poiDeviceGroup.Add("ICN1CC07SG01", new List<string>());
        poiDeviceGroup["ICN1CC07SG01"].Add("ICN1CC07SG01");
        poiDeviceGroup["ICN1CC07SG01"].Add("ICN1CC07SG02");
        poiDeviceGroup.Add("ICN1CL01SG01", new List<string>());
        poiDeviceGroup["ICN1CL01SG01"].Add("ICN1CL01SG01");
        poiDeviceGroup.Add("ICN1CB19PG01", new List<string>());
        poiDeviceGroup["ICN1CB19PG01"].Add("ICN1CB19PG01");
        poiDeviceGroup.Add("ICN1CC25SG01", new List<string>());
        poiDeviceGroup["ICN1CC25SG01"].Add("ICN1CC25SG01");
        poiDeviceGroup["ICN1CC25SG01"].Add("ICN1CC25SG02");
        poiDeviceGroup.Add("ICN1CL32PG01", new List<string>());
        poiDeviceGroup["ICN1CL32PG01"].Add("ICN1CL32PG01");


        //DPG
        poiDeviceGroup.Add("ICN1D002DG01", new List<string>());
        poiDeviceGroup["ICN1D002DG01"].Add("ICN1D002DG01");
        poiDeviceGroup.Add("ICN1D002DG02", new List<string>());
        poiDeviceGroup["ICN1D002DG02"].Add("ICN1D002DG02");
        poiDeviceGroup.Add("ICN1D003DG01", new List<string>());
        poiDeviceGroup["ICN1D003DG01"].Add("ICN1D003DG01");
        poiDeviceGroup.Add("ICN1D003DG02", new List<string>());
        poiDeviceGroup["ICN1D003DG02"].Add("ICN1D003DG02");
        poiDeviceGroup.Add("ICN1D004DG01", new List<string>());
        poiDeviceGroup["ICN1D004DG01"].Add("ICN1D004DG01");
        poiDeviceGroup.Add("ICN1D004DG02", new List<string>());
        poiDeviceGroup["ICN1D004DG02"].Add("ICN1D004DG02");
        poiDeviceGroup.Add("ICN1D005DG01", new List<string>());
        poiDeviceGroup["ICN1D005DG01"].Add("ICN1D005DG01");
        poiDeviceGroup.Add("ICN1D005DG02", new List<string>());
        poiDeviceGroup["ICN1D005DG02"].Add("ICN1D005DG02");

        //PVK
        poiDeviceGroup.Add("ICN1G010BK01", new List<string>());
        poiDeviceGroup["ICN1G010BK01"].Add("ICN1G010BK01");
        poiDeviceGroup.Add("ICN1G015BK01", new List<string>());
        poiDeviceGroup["ICN1G015BK01"].Add("ICN1G015BK01");
        poiDeviceGroup.Add("ICN1G017BK01", new List<string>());
        poiDeviceGroup["ICN1G017BK01"].Add("ICN1G017BK01");
        poiDeviceGroup.Add("ICN1G021BK01", new List<string>());
        poiDeviceGroup["ICN1G021BK01"].Add("ICN1G021BK01");
        poiDeviceGroup.Add("ICN1G023BK01", new List<string>());
        poiDeviceGroup["ICN1G023BK01"].Add("ICN1G023BK01");
        poiDeviceGroup.Add("ICN1G036BK01", new List<string>());
        poiDeviceGroup["ICN1G036BK01"].Add("ICN1G036BK01");
        poiDeviceGroup.Add("ICN1G045BK01", new List<string>());
        poiDeviceGroup["ICN1G045BK01"].Add("ICN1G045BK01");



        //t2
        //pvk
        poiDeviceGroup.Add("ICN2G232BK01", new List<string>());
        poiDeviceGroup["ICN2G232BK01"].Add("ICN2G232BK01");
        poiDeviceGroup.Add("ICN2G247BK01", new List<string>());
        poiDeviceGroup["ICN2G247BK01"].Add("ICN2G247BK01");
        poiDeviceGroup.Add("ICN2G248BK01", new List<string>());
        poiDeviceGroup["ICN2G248BK01"].Add("ICN2G248BK01");
        poiDeviceGroup.Add("ICN2G254BK01", new List<string>());
        poiDeviceGroup["ICN2G254BK01"].Add("ICN2G254BK01");
        poiDeviceGroup.Add("ICN2G255BK01", new List<string>());
        poiDeviceGroup["ICN2G255BK01"].Add("ICN2G255BK01");
        poiDeviceGroup.Add("ICN2G266BK01", new List<string>());
        poiDeviceGroup["ICN2G266BK01"].Add("ICN2G266BK01");
        poiDeviceGroup.Add("ICN2G267BK01", new List<string>());
        poiDeviceGroup["ICN2G267BK01"].Add("ICN2G267BK01");


        //SBG
        poiDeviceGroup.Add("ICN2G232BG01", new List<string>());
        poiDeviceGroup["ICN2G232BG01"].Add("ICN2G232BG01");
        poiDeviceGroup["ICN2G232BG01"].Add("ICN2G232BG02");
        poiDeviceGroup["ICN2G232BG01"].Add("ICN2G232BG03");
        poiDeviceGroup.Add("ICN2G247BG01", new List<string>());
        poiDeviceGroup["ICN2G247BG01"].Add("ICN2G247BG01");
        poiDeviceGroup["ICN2G247BG01"].Add("ICN2G247BG02");
        poiDeviceGroup["ICN2G247BG01"].Add("ICN2G247BG03");
        poiDeviceGroup.Add("ICN2G248BG01", new List<string>());
        poiDeviceGroup["ICN2G248BG01"].Add("ICN2G248BG01");
        poiDeviceGroup["ICN2G248BG01"].Add("ICN2G248BG02");
        poiDeviceGroup["ICN2G248BG01"].Add("ICN2G248BG03");
        poiDeviceGroup.Add("ICN2G254BG01", new List<string>());
        poiDeviceGroup["ICN2G254BG01"].Add("ICN2G254BG01");
        poiDeviceGroup["ICN2G254BG01"].Add("ICN2G254BG02");
        poiDeviceGroup["ICN2G254BG01"].Add("ICN2G254BG03");
        poiDeviceGroup.Add("ICN2G255BG01", new List<string>());
        poiDeviceGroup["ICN2G255BG01"].Add("ICN2G255BG01");
        poiDeviceGroup["ICN2G255BG01"].Add("ICN2G255BG02");
        poiDeviceGroup["ICN2G255BG01"].Add("ICN2G255BG03");
        poiDeviceGroup.Add("ICN2G266BG01", new List<string>());
        poiDeviceGroup["ICN2G266BG01"].Add("ICN2G266BG01");
        poiDeviceGroup["ICN2G266BG01"].Add("ICN2G266BG02");
        poiDeviceGroup["ICN2G266BG01"].Add("ICN2G266BG03");
        poiDeviceGroup.Add("ICN2G267BG01", new List<string>());
        poiDeviceGroup["ICN2G267BG01"].Add("ICN2G267BG01");
        poiDeviceGroup["ICN2G267BG01"].Add("ICN2G267BG02");
        poiDeviceGroup["ICN2G267BG01"].Add("ICN2G267BG03");

        //DPG
        poiDeviceGroup.Add("ICN2D001DG01", new List<string>());
        poiDeviceGroup["ICN2D001DG01"].Add("ICN2D001DG01");
        poiDeviceGroup["ICN2D001DG01"].Add("ICN2D001DG02");
        poiDeviceGroup.Add("ICN2D001DG03", new List<string>());
        poiDeviceGroup["ICN2D001DG03"].Add("ICN2D001DG03");
        poiDeviceGroup["ICN2D001DG03"].Add("ICN2D001DG04");
        poiDeviceGroup.Add("ICN2D002DG01", new List<string>());
        poiDeviceGroup["ICN2D002DG01"].Add("ICN2D002DG01");
        poiDeviceGroup["ICN2D002DG01"].Add("ICN2D002DG02");
        poiDeviceGroup.Add("ICN2D002DG03", new List<string>());
        poiDeviceGroup["ICN2D002DG03"].Add("ICN2D002DG03");
        poiDeviceGroup["ICN2D002DG03"].Add("ICN2D002DG04");

        //CA
        //pvk
        poiDeviceGroup.Add("ICN3G114BK01", new List<string>());
        poiDeviceGroup["ICN3G114BK01"].Add("ICN3G114BK01");
        poiDeviceGroup.Add("ICN3G122BK01", new List<string>());
        poiDeviceGroup["ICN3G122BK01"].Add("ICN3G122BK01");


        //SBG
        poiDeviceGroup.Add("ICN3G114BG01", new List<string>());
        poiDeviceGroup["ICN3G114BG01"].Add("ICN3G114BG01");
        poiDeviceGroup["ICN3G114BG01"].Add("ICN3G114BG02");
        poiDeviceGroup.Add("ICN3G122BG01", new List<string>());
        poiDeviceGroup["ICN3G122BG01"].Add("ICN3G122BG01");
        poiDeviceGroup["ICN3G122BG01"].Add("ICN3G122BG02");
    }

    public static List<string> GetIdList(string id)
    {
        return poiDeviceGroup[id];
    }
}



