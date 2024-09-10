using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UIDevicePopupController : MonoBehaviour
{
    [SerializeField]
    GameObject content;

    [SerializeField]
    DeviceType curType;

    [SerializeField]
    List<GameObject> bgList;    //brt, kiosk, gate

    [SerializeField]
    TMP_Text title_txt;
    [SerializeField]
    Button close_btn;

    [Header("상단")]
    [SerializeField]
    TMP_Text deviceId_txt;

    [SerializeField]
    Button onOff_btn;
    [SerializeField]
    Image onOffIcon_img;
    [SerializeField]
    TMP_Text onOff_txt;
    UnityAction offAction;

    [SerializeField]
    Button reset_btn;
    [SerializeField]
    Image resetIcon_img;
    [SerializeField]
    TMP_Text reset_txt;
    UnityAction restartAction;

    [SerializeField]
    GameObject oper_obj;
    [SerializeField]
    Image oper_img;
    [SerializeField]
    TMP_Text oper_txt;

    [SerializeField]
    GameObject door_obj;
    [SerializeField]
    TMP_Text door_txt;

    [SerializeField]
    Image device_img;

    [Header("하단")]
    [SerializeField]
    Toggle face_tg;
    [SerializeField]
    Image face_img;
    [SerializeField]
    Toggle reader_tg;
    [SerializeField]
    Image reader_img;
    [SerializeField]
    Toggle cpu_tg;
    [SerializeField]
    TMP_Text cpu_txt;
    [SerializeField]
    Toggle passport_tg;
    [SerializeField]
    Image passport_img;
    [SerializeField]
    Toggle gate_tg;
    [SerializeField]
    Image gate_img;
    [SerializeField]
    Toggle mem_tg;
    [SerializeField]
    TMP_Text mem_txt;



    [Header("Resources")]
    [SerializeField]
    Sprite[] onOff_sps; //0:시작, 1:종료, 2:비활성화
    [SerializeField]
    Sprite[] resetIcon_sps;

    Color off_col;

    [SerializeField]
    List<Sprite> deviceList;
    [SerializeField]
    List<Sprite> statusList;    //0:정상, 1: 비정상, 2:연결X

    Color orange_col;
    Color red_col;

    const string OPERON = "운영중";
    const string OPEROFF = "운영중지";
    const string DOOROPENLIMIT = " / 800000000";
    const string DEVICEID = "ID : ";

    //0 : brt, 1 :kiosk, 2 :gate
    Vector3[] onOffPos = { new Vector3(90f, 320f,0f), new Vector3(82f, 323f, 0f), new Vector3(82f, 323f, 0f) };
    Vector3[] resetPos = { new Vector3(190f, 320f, 0f), new Vector3(184f, 323f, 0f), new Vector3(184f, 323f, 0f) };
    Vector3[] operPos = { new Vector3(287.5f, 323f, 0f), new Vector3(284f, 323f, 0f), new Vector3(284f, 323f, 0f) };
    Vector3[] devicePos = { new Vector3(0f, 24f, 0f), new Vector3(0f, 8f, 0f), Vector3.zero };
    Vector3[] facePos = { new Vector3(-222.5f, -290f, 0f), new Vector3(-222.5f, -316f, 0f), new Vector3(-222.5f, -316f, 0f) };
    Vector3[] readerPos = { new Vector3(0f, -316f, 0f), new Vector3(0f, -316f, 0f), new Vector3(0f, -316f, 0f) };
    Vector3[] cpuPos = { new Vector3(0f, -290f, 0f), new Vector3(223.7f, -316f, 0f), new Vector3(223.7f, -316f, 0f) };
    Vector3[] passportPos = { new Vector3(-222.5f, -371.5f, 0f), new Vector3(-222.5f, -371.5f, 0f), new Vector3(-222.5f, -371.5f, 0f) };
    Vector3[] gatePos = { new Vector3(0, -371.5f, 0f), new Vector3(0, -371.5f, 0f), new Vector3(0, -371.5f, 0f) };
    Vector3[] memPos = { new Vector3(223.7f, -290f, 0f), new Vector3(223.7f, -371.5f, 0f), new Vector3(223.7f, -371.5f, 0f) };

    public bool isActive { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        ColorUtility.TryParseHtmlString("#FFC000", out orange_col);
        ColorUtility.TryParseHtmlString("#FF3A3A", out red_col);
        ColorUtility.TryParseHtmlString("#A4B6CD", out off_col);

        onOff_btn.onClick.AddListener(offAction);   //원래 OnOff 엿는데 off만으로 바뀜
        reset_btn.onClick.AddListener(restartAction);


        close_btn.onClick.AddListener(UIManager.Instance.SetDevicePanelOff);
        close_btn.onClick.AddListener(delegate { CameraManager.Instance.SetDevicePopupCameraOnOff(false); });


        face_tg.onValueChanged.AddListener(x => {
            if (x)
            {
                UIManager.Instance.SetDevicePopupResClickAction(curType, 0);
            }
            else
            {
                UIManager.Instance.SetDevicePopupResOffAction(curType);

            }
        });
        reader_tg.onValueChanged.AddListener(x => {
            if (x)
            {
                UIManager.Instance.SetDevicePopupResClickAction(curType, 1);
            }
            else
            {
                UIManager.Instance.SetDevicePopupResOffAction(curType);

            }
        });
        cpu_tg.onValueChanged.AddListener(x => {
            if (x)
            {
                UIManager.Instance.SetDevicePopupResClickAction(curType, -1);
            }
            else
            {
                UIManager.Instance.SetDevicePopupResOffAction(curType);

            }
        });
        passport_tg.onValueChanged.AddListener(x => {
            if (x)
            {
                UIManager.Instance.SetDevicePopupResClickAction(curType, 2);
            }
            else
            {
                UIManager.Instance.SetDevicePopupResOffAction(curType);

            }
        });
        gate_tg.onValueChanged.AddListener(x => {
            if (x)
            {
                UIManager.Instance.SetDevicePopupResClickAction(curType, 3);
            }
            else
            {
                UIManager.Instance.SetDevicePopupResOffAction(curType);

            }
        });
        mem_tg.onValueChanged.AddListener(x => {
            if (x)
            {
                UIManager.Instance.SetDevicePopupResClickAction(curType, -1);
            }
            else
            {
                UIManager.Instance.SetDevicePopupResOffAction(curType);

            }
        });


        SetContentsOff();
    }

    //BRT,    //유인등록단말기
    //DPK,    //디파쳐키오스크
    //DPG,    //셀프디파쳐게이트
    //PVK,    //여객확인키오스크
    //SBG,    //셀프보딩게이트
    //BDG,    //유인백드랍진입게이트
    //BEG,    //셀프백드랍진입게이트
    //ATG,    //환승게이트
    //TSG     //환승승보안게이트
    public void SetContentsOn(DeviceType type, DeviceResources res)
    {
        content.SetActive(true);
        isActive = true;
        curType = type;

        int objPos = 0;
        if (type.Equals(DeviceType.BRT))
        {
            bgList[0].SetActive(true);
            bgList[1].SetActive(false);
            bgList[2].SetActive(false);

            door_obj.SetActive(false);
            
            reader_tg.gameObject.SetActive(false);
            passport_tg.gameObject.SetActive(false);
            gate_tg.gameObject.SetActive(false);


        }
        else if(type.Equals(DeviceType.DPK) || type.Equals(DeviceType.PVK))
        {
            bgList[0].SetActive(false);
            bgList[1].SetActive(true);
            bgList[2].SetActive(false);

            door_obj.SetActive(false);

            reader_tg.gameObject.SetActive(true);
            passport_tg.gameObject.SetActive(true);
            gate_tg.gameObject.SetActive(false);
            objPos = 1;
        }
        else if (type.Equals(DeviceType.DPG) || type.Equals(DeviceType.SBG) 
            || type.Equals(DeviceType.BDG) || type.Equals(DeviceType.BEG))
        {
            bgList[0].SetActive(false);
            bgList[1].SetActive(false);
            bgList[2].SetActive(true);

            door_obj.SetActive(true);

            reader_tg.gameObject.SetActive(true);
            passport_tg.gameObject.SetActive(true);
            gate_tg.gameObject.SetActive(true);

            objPos = 2;
            //게이트 일때(상태 6가지 + 도어 개폐횟수)

        }

        if (type.Equals(DeviceType.BDG) || type.Equals(DeviceType.BEG))
        {
            title_txt.text = StaticText.SetSize(20, MenuTree.GetDeviceTypeKor(type) + StaticText.OPEN_S + type.ToString() + StaticText.CLOSE_S);
        }
        else
        {
            title_txt.text = MenuTree.GetDeviceTypeKor(type) + StaticText.OPEN_S + type.ToString() + StaticText.CLOSE_S;
        }

        //장비 타입에 따라 위치가 조금씩 달라짐
        onOff_btn.transform.localPosition = onOffPos[objPos];
        reset_btn.transform.localPosition = resetPos[objPos];
        oper_obj.transform.localPosition = operPos[objPos];

        device_img.transform.localPosition = devicePos[objPos];

        face_tg.transform.localPosition = facePos[objPos];
        reader_tg.transform.localPosition = readerPos[objPos];
        cpu_tg.transform.localPosition = cpuPos[objPos];
        passport_tg.transform.localPosition = passportPos[objPos];
        gate_tg.transform.localPosition = gatePos[objPos];
        mem_tg.transform.localPosition = memPos[objPos];


        device_img.sprite = deviceList[(int)type];
        device_img.SetNativeSize();

        SetContentData(res);
    }

    void SetContentData(DeviceResources res)
    {
        deviceId_txt.text = DEVICEID  + res.deviceId;

        //단말기 상태(0=운영중지, 1=운영중)
        if (res.tsActive == -1)
        {//연결없음
            oper_img.sprite = statusList[2];
            oper_txt.text = StaticText.BAR;

            onOffIcon_img.gameObject.SetActive(true);
            onOff_txt.gameObject.SetActive(true);
            onOff_btn.image.sprite = onOff_sps[2];
            onOff_btn.interactable = false;
            
            resetIcon_img.sprite = resetIcon_sps[0];
            reset_txt.color = off_col;
            reset_btn.interactable = false;
        }
        else if (res.tsActive == 0)
        {//운영중지
            oper_img.sprite = statusList[1];
            oper_txt.text = StaticText.SetSize(14, OPEROFF);

            onOffIcon_img.gameObject.SetActive(true);
            onOff_txt.gameObject.SetActive(true);
            onOff_btn.image.sprite = onOff_sps[2];
            onOff_btn.interactable = false;

            resetIcon_img.sprite = resetIcon_sps[0];
            reset_txt.color = off_col;
            reset_btn.interactable = false;
        }
        else if(res.tsActive == 1)
        {//운영중
            oper_img.sprite = statusList[0];
            oper_txt.text = OPERON;

            onOffIcon_img.gameObject.SetActive(false);
            onOff_txt.gameObject.SetActive(false);
            onOff_btn.image.sprite = onOff_sps[1];
            onOff_btn.interactable = true;

            resetIcon_img.sprite = resetIcon_sps[1];
            reset_txt.color = Color.white;
            reset_btn.interactable = true;
        }

        //도어누적개폐횟수 text 색 확인
        if (res.ctDoorCount < 0)
        {
            door_txt.text = StaticText.BAR;// + DOOROPENLIMIT;
        }
        else
        {
            door_txt.text = res.ctDoorCount.ToString();// + DOOROPENLIMIT;
        }

        //안면인식(0=정상, 1=비정상)
        if (res.tsFace == -1)
        {
            face_img.sprite = statusList[2];
        }
        else
        {
            face_img.sprite = statusList[res.tsFace];
        }

        //탑승권리더기(0=정상, 1=비정상)
        if (res.tsBd == -1)
        {
            reader_img.sprite = statusList[2];
        }
        else
        {
            reader_img.sprite = statusList[res.tsBd];
        }

        //CPU
        if (res.tsCPU < 0 )
        {
            cpu_txt.color = Color.white;
            cpu_txt.text = StaticText.BAR + StaticText.EMPTY + StaticText.EMPTY + StaticText.SetSize(19, StaticText.PERCENT);
        }
        else 
        {
            if (res.tsCPU < 50)
            {
                cpu_txt.color = Color.white;
            }
            else if(res.tsCPU < 80)
            {
                cpu_txt.color = orange_col;
            }
            else
            {
                cpu_txt.color = red_col;
            }

            cpu_txt.text = res.tsCPU.ToString() + StaticText.SetSize(19, StaticText.PERCENT);
        }

        //여권판독기 이상 유무(0=정상, 1=비정상)
        if (res.tsPsprt == -1)
        {
            passport_img.sprite = statusList[2];
        }
        else
        {
            passport_img.sprite = statusList[res.tsPsprt];
        }

        //게이트도어 이상 유무(0=정상, 1=비정상)
        if (res.tsDoor < 0)
        {
            gate_img.sprite = statusList[2];
        }
        else
        {
            gate_img.sprite = statusList[res.tsDoor];
        }

        //Memory
        if (res.tsMem < 0)
        {
            mem_txt.color = Color.white;
            mem_txt.text = StaticText.BAR + StaticText.EMPTY + StaticText.EMPTY + StaticText.SetSize(19, StaticText.PERCENT);
        }
        else
        {
            if (res.tsMem < 50)
            {
                mem_txt.color = Color.white;
            }
            else if (res.tsMem < 80)
            {
                mem_txt.color = orange_col;
            }
            else
            {
                mem_txt.color = red_col;
            }

            mem_txt.text = res.tsMem.ToString() + StaticText.SetSize(19, StaticText.PERCENT);
        }

    }

    public void SetContentsOff()
    {
        content.SetActive(false);
        isActive = false;
    }

   
    public void SetPopupOffClickAction(UnityAction act)
    {
        offAction = act;
    }
    public void SetPopupRestartClickAction(UnityAction act)
    {
        restartAction = act;
    }

    
}
