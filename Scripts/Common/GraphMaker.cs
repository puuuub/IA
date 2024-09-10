using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GraphMaker : MonoBehaviour
{
    List<float> values;

    public BusyWating busyWaiting;


    public GameObject DotGroup;
    public GameObject LineGroup;
    public GameObject BaseLineGroup;
    public GameObject LineOri;
    public GameObject DotOri;
    GameObject filledGraph;
    public GameObject bg;
    

    float GraphArea_width;
    float GraphArea_height;

    float padding = 20f;

    float MaxValue = 100f;
    float MinValue = -10f;

    int GraphLevel = 5;

    List<Vector3> DotsList;
    public Font textfont;

    public Material MyMat;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        //DotGroup = CommonUtility.FindChildObject("DotGroup", gameObject);
        //LineGroup = CommonUtility.FindChildObject("LineGroup", gameObject);
        //BaseLineGroup = CommonUtility.FindChildObject("BaseLineGroup", gameObject);
        //LineOri = CommonUtility.FindChildObject("Line_Ori", gameObject);
        //DotOri = CommonUtility.FindChildObject("Dot_Ori", gameObject);
        //filledGraph = CommonUtility.FindChildObject("FilledGraph", gameObject);

       
    }

    public void DrawLineGraph(List<float> values_, float Max = 10f, int level = 3)
    {
        values = values_;

        JistUtil.DestroyWithChildren(DotGroup);
        JistUtil.DestroyWithChildren(LineGroup);
        JistUtil.DestroyWithChildren(BaseLineGroup);

        GraphArea_width = bg.GetComponent<RectTransform>().rect.width - (20f * 2);
        GraphArea_height = bg.GetComponent<RectTransform>().rect.height;

        MaxValue = Max;
        MinValue = 0f;
        GraphLevel = level;

        float Xspace = GraphArea_width / (values.Count - 1);
        float Yspace = GraphArea_height / (MaxValue - MinValue);

        GameObject PreDot = null;

        DotsList = new List<Vector3>();

        for (int i = 0; i < values.Count; i++)
        {
            GameObject dot = Instantiate(DotOri, DotGroup.transform);
            //dot.SetActive(true);
            dot.name = "dot_" + i;
            //dot.GetComponent<Image>().color = GraphDotColor;
            dot.transform.GetChild(0).GetComponent<Text>().text = values[i].ToString();
            dot.transform.localPosition = new Vector3(-(GraphArea_width / 2) + (i * Xspace), -(GraphArea_height / 2) + (values[i] * Yspace), 0);

            if (PreDot != null)
            {
                //Line
                GameObject line = Instantiate(LineOri, LineGroup.transform);
                //line.SetActive(true);
                line.name = "line_" + i;
                //line.GetComponent<Image>().color = GraphLineColor;

                float lineX = (dot.transform.localPosition.x + PreDot.transform.localPosition.x) / 2;
                float lineY = (dot.transform.localPosition.y + PreDot.transform.localPosition.y) / 2;
                float lineWidth = Vector2.Distance(dot.transform.localPosition, PreDot.transform.localPosition);
                Vector2 dir = (dot.transform.localPosition - PreDot.transform.localPosition).normalized;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                line.transform.localPosition = new Vector3(lineX, lineY, 0);
                line.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lineWidth);
                line.transform.localRotation = Quaternion.Euler(0f, 0f, angle);


                //렌더링 정점 넣기
                DotsList.Add(new Vector3(PreDot.transform.localPosition.x, -(GraphArea_height / 2), 0));
                DotsList.Add(new Vector3(PreDot.transform.localPosition.x, PreDot.transform.localPosition.y, 0));
                DotsList.Add(new Vector3(dot.transform.localPosition.x, dot.transform.localPosition.y, 0));
                DotsList.Add(new Vector3(dot.transform.localPosition.x, -(GraphArea_height / 2), 0));

            }

            PreDot = dot;

        }


        for (int i = 0; i < GraphLevel; i++)
        {
            //기준선
            //GameObject BaseLine = Instantiate(LineOri, BaseLineGroup.transform);
            //BaseLine.SetActive(true);
            //BaseLine.name = "BaseLine_" + i;
            //BaseLine.GetComponent<Image>().color = BaseLineColor;
            //BaseLine.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, GraphArea_width);
            //BaseLine.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1);   //줄 두께
            //BaseLine.transform.localPosition = new Vector3(0, -(GraphArea_height / 2) + (i * (GraphArea_height / (GraphLevel - 1))), 0);

            //Y축 기준값
            //위치 조정 필요
            GameObject BaseText = new GameObject();
            BaseText.transform.SetParent(BaseLineGroup.transform);
            BaseText.name = "BaseText_" + i;
            Text comptext = BaseText.AddComponent<Text>();
            float tmpText = MinValue + (i * (MaxValue - MinValue) / (GraphLevel - 1));
            comptext.font = textfont;
            comptext.fontSize = 20;
            comptext.alignment = TextAnchor.MiddleCenter;
            comptext.color = Color.white;
            comptext.text = tmpText.ToString();
            BaseText.transform.localPosition = new Vector3(-((GraphArea_width + 40) / 2), -(GraphArea_height / 2) + (i * (GraphArea_height / (GraphLevel - 1))), 0);

            BaseText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 30);
            BaseText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);



        }

        //MakeRenderer(DotsList.ToArray(), values.Count);
    }
    public void DrawDotGraph(List<float> values_, float Max = 5f, int level = 3)
    {
        values = values_;
        

        GraphArea_width = bg.GetComponent<RectTransform>().rect.width - (20f * 2);
        GraphArea_height = bg.GetComponent<RectTransform>().rect.height;

        MaxValue = Max;
        MinValue = 0f;
        GraphLevel = level;

        if (values.Count > 100)
        {
            StartCoroutine(MakeDOt());
        }
        else
        {
            int child = DotGroup.transform.childCount;
            //100개 이하일때
            float Xspace = GraphArea_width / (values.Count - 1);
            float Yspace = GraphArea_height / (MaxValue - MinValue);

            GameObject dot;
            if (child > values.Count)
            {
                for (int i = 0; i < child; i++)
                {
                    if (i < values.Count)
                    {
                        dot = DotGroup.transform.GetChild(i).gameObject;
                        dot.transform.localPosition = new Vector3(-(GraphArea_width / 2) + (i * Xspace), -(GraphArea_height / 2) + (values[i] * Yspace), 0);

                        dot.SetActive(true);
                    }
                    else
                    {
                        DotGroup.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                for (int i = 0; i < values.Count; i++)
                {
                    if (i < child)
                    {
                        dot = DotGroup.transform.GetChild(i).gameObject;
                        dot.transform.localPosition = new Vector3(-(GraphArea_width / 2) + (i * Xspace), -(GraphArea_height / 2) + (values[i] * Yspace), 0);

                        dot.SetActive(true);
                    }
                    else
                    {

                        dot = Instantiate(DotOri, DotGroup.transform);
                        dot.transform.localPosition = new Vector3(-(GraphArea_width / 2) + (i * Xspace), -(GraphArea_height / 2) + (values[i] * Yspace), 0);

                        dot.SetActive(true);
                    }
                }
            }


        }
    }

    public void DotGroupOnOff(bool isOn)
    {
        DotGroup.SetActive(isOn);
    }
    
    IEnumerator MakeDOt()
    {
        int child = DotGroup.transform.childCount;
        float Xspace = GraphArea_width / (values.Count - 1);
        float Yspace = GraphArea_height / (MaxValue - MinValue);

        DotGroup.SetActive(false);
        busyWaiting.Show();

        GameObject dot;
        if (child > values.Count)
        {
            for (int i = 0; i < child; i++)
            {
                if (i < values.Count)
                {
                    dot = DotGroup.transform.GetChild(i).gameObject;
                    dot.transform.localPosition = new Vector3(-(GraphArea_width / 2) + (i * Xspace), -(GraphArea_height / 2) + (values[i] * Yspace), 0);

                    dot.SetActive(true);
                }
                else
                {
                    DotGroup.transform.GetChild(i).gameObject.SetActive(false);
                }
                yield return null;
            }
        }
        else
        {
            for (int i = 0; i < values.Count; i++)
            {
                if (i < child)
                {
                    dot = DotGroup.transform.GetChild(i).gameObject;
                    dot.transform.localPosition = new Vector3(-(GraphArea_width / 2) + (i * Xspace), -(GraphArea_height / 2) + (values[i] * Yspace), 0);

                    dot.SetActive(true);
                }
                else
                {

                    dot = Instantiate(DotOri, DotGroup.transform);
                    dot.transform.localPosition = new Vector3(-(GraphArea_width / 2) + (i * Xspace), -(GraphArea_height / 2) + (values[i] * Yspace), 0);

                    dot.SetActive(true);
                }

                yield return null;
            }
        }

        busyWaiting.Hide();
        DotGroup.SetActive(true);
    }
    public void DrawStickGraph(List<float> values_, float Max = 10f, int level = 3)
    {
        values = values_;

        JistUtil.DestroyWithChildren(DotGroup);
        JistUtil.DestroyWithChildren(LineGroup);
        JistUtil.DestroyWithChildren(BaseLineGroup);

        GraphArea_width = bg.GetComponent<RectTransform>().rect.width - (20f * 2);
        GraphArea_height = bg.GetComponent<RectTransform>().rect.height;

        MaxValue = Max;
        MinValue = 0f;
        GraphLevel = level;

        float Xspace = GraphArea_width / (values.Count - 1);
        float Yspace = GraphArea_height / (MaxValue - MinValue);

        Vector3 dotpos;
        Vector3 dotpos_zero;

        for (int i = 0; i < values.Count; i++)
        {
            GameObject dot = Instantiate(DotOri, DotGroup.transform);
            //dot.SetActive(true);
            dot.name = "dot_" + i;
            //dot.GetComponent<Image>().color = GraphDotColor;
            dot.transform.GetChild(0).GetComponent<Text>().text = values[i].ToString();
            
            dotpos_zero = new Vector3(-(GraphArea_width / 2) + (i * Xspace), -(GraphArea_height / 2), 0);

            
            dotpos = new Vector3(-(GraphArea_width / 2) + (i * Xspace), -(GraphArea_height / 2) + (values[i] * Yspace), 0);
            
            
            
            float yyy = -(GraphArea_height / 2) + (values[i] * Yspace);
            dot.transform.localPosition = dotpos_zero;
            DOTween.To(() => -(GraphArea_height / 2), y => dot.transform.localPosition = new Vector3(dot.transform.localPosition.x, y, 0), dotpos.y, 1.0f);

            //dot.transform.localPosition = dotpos;


            //Line
            GameObject line = Instantiate(LineOri, LineGroup.transform);
            //line.SetActive(true);
            line.name = "line_" + i;
            //line.GetComponent<Image>().color = GraphLineColor;

            //float lineX = (dot.transform.localPosition.x + PreDot.transform.localPosition.x) / 2;
            float lineY = -(GraphArea_height / 2) + ((values[i] * Yspace) / 2);
            float lineWidth = (values[i] * Yspace);
            //float lineWidth = Vector2.Distance(dot.transform.localPosition, PreDot.transform.localPosition);
            //Vector2 dir = (dot.transform.localPosition - PreDot.transform.localPosition).normalized;

            line.transform.localPosition = dotpos_zero;
            //line.transform.localPosition = new Vector3(dotpos.x, -(GraphArea_height / 2), 0);
            DOTween.To(() => -(GraphArea_height / 2), y => line.transform.localPosition = new Vector3(line.transform.localPosition.x, y, 0), lineY, 1.0f);
            //line.transform.localPosition = new Vector3(dotpos.x, lineY, 0);

            line.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            DOTween.To(() => 0, x => line.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x), lineWidth, 1.0f);
            //line.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lineWidth);
            

            //렌더링 정점 넣기
            //DotsList.Add(new Vector3(PreDot.transform.localPosition.x, -(GraphArea_height / 2), 0));
            //DotsList.Add(new Vector3(PreDot.transform.localPosition.x, PreDot.transform.localPosition.y, 0));
            //DotsList.Add(new Vector3(dot.transform.localPosition.x, dot.transform.localPosition.y, 0));
            //DotsList.Add(new Vector3(dot.transform.localPosition.x, -(GraphArea_height / 2), 0));

            

            //PreDot = dot;

        }


        for (int i = 0; i < GraphLevel; i++)
        {
            //기준선
            //GameObject BaseLine = Instantiate(LineOri, BaseLineGroup.transform);
            //BaseLine.SetActive(true);
            //BaseLine.name = "BaseLine_" + i;
            //BaseLine.GetComponent<Image>().color = BaseLineColor;
            //BaseLine.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, GraphArea_width);
            //BaseLine.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1);   //줄 두께
            //BaseLine.transform.localPosition = new Vector3(0, -(GraphArea_height / 2) + (i * (GraphArea_height / (GraphLevel - 1))), 0);

            //Y축 기준값
            //위치 조정 필요
            GameObject BaseText = new GameObject();
            BaseText.transform.SetParent(BaseLineGroup.transform);
            BaseText.name = "BaseText_" + i;
            Text comptext = BaseText.AddComponent<Text>();
            float tmpText = MinValue + (i * (MaxValue - MinValue) / (GraphLevel - 1));
            comptext.font = textfont;
            comptext.fontSize = 20;
            comptext.alignment = TextAnchor.MiddleCenter;
            comptext.color = Color.white;
            comptext.text = tmpText.ToString();
            BaseText.transform.localPosition = new Vector3(-((GraphArea_width + 40) / 2), -(GraphArea_height / 2) + (i * (GraphArea_height / (GraphLevel - 1))), 0);

            BaseText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 30);
            BaseText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);



        }

        //MakeRenderer(DotsList.ToArray(), values.Count);
    }

    
    void MakeRenderer(Vector3[] graphPoints, int valcnt)
    {
        int trianleCnt = (valcnt - 1) * 3;  //삼각형 수
        int[] triangles = new int[trianleCnt * 2];  //정점 수

        int idx = 0;
        for(int i = 1; i < valcnt; i++)
        { 
            triangles[idx++] = (i - 1) * 4;             
            triangles[idx++] = (i - 1) * 4 + 1;         
            triangles[idx++] = (i - 1) * 4 + 2;         

            triangles[idx++] = (i - 1) * 4 ;            
            triangles[idx++] = (i - 1) * 4 + 2;         
            triangles[idx++] = (i - 1) * 4 + 3;         
        }

        Mesh filledGraphMesh = new Mesh();
        filledGraphMesh.vertices = graphPoints;
        filledGraphMesh.triangles = triangles;


        CanvasRenderer renderer = filledGraph.GetComponentInChildren<CanvasRenderer>(true);
        renderer.SetMesh(filledGraphMesh);

        //RawImage rawImg = filledGraph.GetComponent<RawImage>();
        //Material rawMat = rawImg.GetComponent<Material>();

        //Material mat = new Material(Shader.Find("UI/Default"));
        //Color tmpColor = BaseLineColor;
        //tmpColor.a = 0.5f;
        //mat.color = tmpColor;
        //renderer.SetMaterial(mat, null);
        //renderer.SetMaterial(MyMat, null);


        //filledGraph.GetComponent<RectTransform>().anchoredPosition = new Vector3(GraphArea_width / 2 + 2, GraphArea_height / 2 + 1, 0);
        filledGraph.GetComponent<RectTransform>().anchoredPosition = new Vector3(-4f,-1f,0);

    }


   
}
