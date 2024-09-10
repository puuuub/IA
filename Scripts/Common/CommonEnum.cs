//오브젝트 타입
public enum OBJCET_TYPE
{
    TAXI = 0,           //택시
    DeliveryService,    //택배
}


//창고 안쪽 오브젝트 타입
public enum IIWH_OBJECT_TYPE
{
    ALL_FLOOR = 0,
    FRIST_FLOOR,
    SECOND_FLOOR
}

//창고 위치 타입
public enum WAREHOUSE_POSITION_TYPE
{
    NONE = 0,
    OUTSIDE_WAREHOUSE,
    INSIDE_COMMON_WAREHOUSE,
    INSIDE_VIRTURAL_WAREHOUSE,
    INSIDE_KUMKANGTECH_WAREHOUSE,
}

//회사 종류
public enum WAREHOUSE_COMPANY_TYPE
{
    NONE = 0,
    HANLIMTECH,     //한림테크놀러지
    SKANAKOREA,     //스카나코리아
    KUMKANGTECH,    //금강테크
    TURBOLINK,      //터보링크
}