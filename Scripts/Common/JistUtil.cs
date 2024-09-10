using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security;

using DG.Tweening;


public class JistUtil// : MonoBehaviour
{
    public enum JistBlendMode
    {
        Opaque,
        Cutout,
        Fade,
        Transparent,
        FadeNormal,
        Additive,
    }

#if UNITY_EDITOR
    [MenuItem("Tools/MyTool/Do It in C#")]
    static void DoIt()
    {
        EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
    }
#endif

    private static JistUtil _instance = null;
    public static JistUtil instance
    {
        get
        {
            if (_instance == null)
                _instance = new JistUtil();
            return _instance;
        }
    }

    public JistUtil()
    {

    }

    /*
    Windows Store Apps: Application.persistentDataPath points to %userprofile%\AppData\Local\Packages\<productname>\LocalState.

    iOS: Application.persistentDataPath points to /var/mobile/Containers/Data/Application/<guid>/Documents.

    Android: Application.persistentDataPath points to /storage/emulated/0/Android/data/<packagename>/files on most devices(some older phones might point to location on SD card if present), 
    the path is resolved using android.content.Context.getExternalFilesDir.
    */
    public static string GetPath(string path)
    {
        string coolPath = "";
#if UNITY_EDITOR
        coolPath = Application.dataPath + path;
#elif UNITY_ANDROID
        coolPath = Application.persistentDataPath + path.Replace("\\", "/");
#elif UNITY_IPHONE
        coolPath = Application.persistentDataPath + "/" + path.Replace("\\", "/");
#elif UNITY_WEBGL
        coolPath = Application.persistentDataPath + path.Replace("\\", "/");
#else
        coolPath = Application.dataPath + "/" + path.Replace("\\", "/");
#endif

        return coolPath;
    }

    public static string GetStreamingAssetsPath(string path)
    {
        string assPath = "";
#if UNITY_EDITOR
        assPath = Application.dataPath + "/StreamingAssets" + path;
        //assPath = Application.dataPath + "/FakeAssets~" + path;
#elif UNITY_ANDROID
        assPath = "jar:file://" + Application.dataPath + "!/assets" + path.Replace("\\", "/");
#elif UNITY_IPHONE
        assPath = Path.Combine(Application.dataPath + " / Raw", path.Replace("\\", " / "));
#elif UNITY_STANDALONE_WIN
        assPath = Application.dataPath + "/StreamingAssets" + path;
#else
        assPath = Application.dataPath + "/" + path.Replace("\\", "/");
#endif

        return assPath;
    }

    public static string GetSavePath()
    {
        string savePath = "";
#if !UNITY_WEBGL
        // 
        savePath = Application.streamingAssetsPath + "/";
#else
        //string s_assetPath = Application.dataPath + "/Resources/";
        savePath = Application.persistentDataPath + "\\";
#endif

        return savePath;
    }

    // 디버깅 전용
    public static void CheckLine(string msg = "JIST",
            [System.Runtime.CompilerServices.CallerFilePath] string file = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int line = 0,
            [System.Runtime.CompilerServices.CallerMemberName] string Member = "")
    {
        //Debug.Log($"{file} Line : {line} {Member} {msg}");
        DebugScrollView.Instance.Print($"{file} Line : {line} {Member} {msg}");
    }

    // 인스턴스 내용 출력
    public static string PrintClassInfo<T>(T myObject)
    {
        string fieldInfoString = "";
        Type myType = myObject.GetType();
        try
        {
            // Get the fields of the specified class.
            FieldInfo[] myField = myType.GetFields(BindingFlags.Instance | BindingFlags.Public);

            Debug.Log(string.Format("{0} fields ({1}) : ", myType.Name, myField.Length));
            for (int i = 0; i < myField.Length; i++)
            {
                // Determine whether or not each field is a special name.
                //if (myField[i].IsSpecialName)
                {
                    //Debug.Log(string.Format("{0} : {1}",
                    //    myField[i].Name, myField[i].GetValue(myObject)));

                    fieldInfoString += string.Format("{0} : {1}",
                        myField[i].Name, myField[i].GetValue(myObject)) + "\n";
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(string.Format("Exception : {0} ", e.Message));
        }
        return fieldInfoString;
    }

    public static void DestroyWithChildren(GameObject go, bool exceptSelf = true)
    {
        if (go != null)
        {
            var children = new List<GameObject>();
            foreach (Transform child in go.transform) children.Add(child.gameObject);
            children.ForEach(child => GameObject.DestroyImmediate(child));
            if (!exceptSelf)
                GameObject.Destroy(go);
        }
    }

    public void DestroyWithChildren(GameObject go, string name, bool exceptSelf = true)
    {
        if (go != null)
        {
            var children = new List<GameObject>();
        foreach (Transform child in go.transform) children.Add(child.gameObject);
        foreach(GameObject child in children)
        {
            if (child.name == name)
                GameObject.DestroyImmediate(child);
        }

        if (!exceptSelf)
            GameObject.Destroy(go);
        }
    }

    public void SetAcitve(List<GameObject> list, bool active)
    {
        for(int k = 0; k < list.Count; k++)
        {
            list[k].SetActive(active);
        }
    }

    public void SetEnable(List<Behaviour> list, bool active)
    {
        for (int k = 0; k < list.Count; k++)
        {
            list[k].enabled = (active);
        }
    }

    public void AddMyAction(UnityAction action, List<UnityAction> actionList, UnityEvent uEvent = null)
    {
        if (!actionList.Contains(action))
        {
            actionList.Add(action);
            if (uEvent != null)
            {
                uEvent.AddListener(actionList[actionList.Count - 1]);
            }
        }
    }

    public void RemoveMyAction(UnityAction action, List<UnityAction> actionList, UnityEvent uEvent = null)
    {
        if (actionList.Contains(action))
        {
            actionList.Remove(action);

            if (uEvent != null)
            {
                UnityAction retAct = actionList.Find(x => x.Equals(action));
                uEvent.RemoveListener(retAct);
            }
        }
    }

    public void EventCall(List<UnityAction> actionList)
    {
        for (int index = 0; index < actionList.Count; index++)
        {
            actionList[index].Invoke();
        }
    }

    /* ================================== 메터리얼 색상 ================================== */

    /// <summary>
    /// 메터리얼 색 변경 Renderer 컴포넌트를 가지고 있어야함
    /// </summary>
    /// <param name="gameObj"></param>
    /// <param name="col"></param>
    public static void SetMaterialColor(GameObject gameObj, Color col, JistUtil.JistBlendMode blendMode)
    {
        Renderer ren = gameObj.GetComponent<Renderer>();
        // 메터리얼 색상 조정
        JistUtil.SetupMaterialWithBlendMode(ren.material, blendMode);
        ren.material.color = col;
    }

    public static void SetupMaterialsWithBlendMode(Renderer renderer, Color col, JistBlendMode blendMode)
    {
        // 메터리얼 색상 조정
        for (int k = 0; k < renderer.materials.Length; k++)
        {
            JistUtil.SetupMaterialWithBlendMode(renderer.materials[k], blendMode);
            renderer.materials[k].color = col;
        }
    }




    static Material tempMat;
    /// <summary>
    /// 메터리얼 알파값 조정
    /// </summary>
    /// <param name="root"> 부모 오브젝트 </param>
    /// <param name="alpha"> 알파 값 </param>
    public static void ChangeAlpahEx(GameObject root, float alpha)
    {
        Renderer[] renderers = root.GetComponentsInChildren<Renderer>(true);
        if (tempMat == null)
        {
            tempMat = Resources.Load<Material>("Material/Transparent");

            Debug.Log("New material " + tempMat);
        }
        //Color col = tempMat.color;
        //col.a = alpha;
        //tempMat.color = col;

        float cAlpha = tempMat.GetFloat("_CustomAlpha");
        Debug.Log("cAlpha " + cAlpha);

        tempMat.SetColor("_Color", Color.red);

        tempMat.SetFloat("_CustomAlpha", alpha);
        cAlpha = tempMat.GetFloat("_CustomAlpha");
        Debug.Log("cAlpha " + cAlpha);

        if (alpha < 1.0f)
        {
            //Shader standardShader = tempMat.shader; //Shader.Find("Legacy Shaders/Transparent/Diffuse_Ex");
            DebugScrollView.PrintEx("Transparent/Diffuse_Ex " + tempMat.shader);

            Camera.main.SetReplacementShader(tempMat.shader, "RenderType");
            SetupMaterialWithBlendMode(tempMat, JistBlendMode.FadeNormal);

            //for (int j = 0; j < renderers.Length; j++)
            //{
            //    for (int k = 0; k < renderers[j].materials.Length; k++)
            //    {
            //        // 창문은 항상 Fade로(Render mode를 유지해야하는 오브젝트는 따로 처리해야함)
            //        if (renderers[j].materials[k].name.Contains("Window"))
            //        {
            //            JistUtil.SetupMaterialWithBlendMode(renderers[j].materials[k], JistBlendMode.Opaque);
            //        }
            //    }
            //}
        }
        else
        {
            Camera.main.SetReplacementShader(null, "");

            //for (int j = 0; j < renderers.Length; j++)
            //{
            //    for (int k = 0; k < renderers[j].materials.Length; k++)
            //    {
            //        // 창문은 항상 Fade로(Render mode를 유지해야하는 오브젝트는 따로 처리해야함)
            //        if (renderers[j].materials[k].name.Contains("Window"))
            //        {
            //            JistUtil.SetupMaterialWithBlendMode(renderers[j].materials[k], JistBlendMode.FadeNormal);
            //        }
            //    }
            //}
        }
    }

    public static void SetupMaterialWithBlendMode(Renderer renderer, Color col, JistBlendMode blendMode)
    {
        JistUtil.SetupMaterialWithBlendMode(renderer.material, blendMode);
        renderer.material.color = col;
    }

    public static Tween DoTweenMaterialColor(Renderer renderer, Color col, float duration)
    {
        //renderer.material.renderQueue = 2999;
        Tween tw = renderer.material.DOColor(col, duration).SetEase(Ease.OutQuint);
        return tw;
    }

    public static Tween DoTweenMaterialColor(GameObject rootObj, Color col, float duration)
    {
        Tween tw = null;
        Renderer[] renderArr = rootObj.GetComponentsInChildren<Renderer>(true);

        for (int k = 0; k < renderArr.Length; k++)
        {
            tw = JistUtil.DoTweenMaterialColor(renderArr[k], col, duration);
        }
        //if (col.a == 0)
        //{
        //    for (int k = 0; k < renderArr.Length; k++)
        //    {
        //        JistUtil.SetupMaterialWithBlendMode(renderArr[k].material, JistUtil.JistBlendMode.Fade);
        //    }
        //}
        //tw.OnComplete(delegate
        //{
        //    if (col.a != 0)
        //    {
        //        for (int k = 0; k < renderArr.Length; k++)
        //        {
        //            JistUtil.SetupMaterialWithBlendMode(renderArr[k].material, JistUtil.JistBlendMode.Opaque);
        //        }
        //    }
        //});

        return tw;
    }

    public static void SetupMaterialWithBlendMode(Material material, JistBlendMode blendMode)
    {
        //material.SetFloat("_Mode", (float)blendMode);
        switch (blendMode)
        {
            case JistBlendMode.Opaque:
                material.SetFloat("_Mode", (float)JistBlendMode.Opaque);
                material.SetOverrideTag("RenderType", "");
                material.SetInt("_SrcBlend", (int)BlendMode.One);
                material.SetInt("_DstBlend", (int)BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case JistBlendMode.Cutout:
                material.SetInt("_SrcBlend", (int)BlendMode.One);
                material.SetInt("_DstBlend", (int)BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case JistBlendMode.Fade:
                {
                    material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    // 스트링 비교 비용은 크다.
                    //if(material.name.Contains("Top"))
                    //{
                    //    material.renderQueue = 3001;
                    //}

                    // 가끔 파라미터값을 제대로 못 가져올때가 있다
                    // _JistParam("CustomQueueNormal", Float) = 3000.0  ---> _JistParamEx("CustomQueueNormal", Float) = 3000.0
                    // 프로퍼티이름을 바꿔서 다시 적용하면 됨

                    int customQueue = ((int)material.GetFloat("_JistParamEx"));

                    material.renderQueue = customQueue;
                    //Debug.Log(material.name + "  customQueue  " + customQueue);
                }
                break;
            case JistBlendMode.FadeNormal:
                {
                    float mode = material.GetFloat("_Mode");
                    //if (mode != (float)JistBlendMode.Fade)
                    {
                        material.SetFloat("_Mode", (float)JistBlendMode.Fade);
                        material.SetOverrideTag("RenderType", "Transparent");

                        material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
                        material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
                        material.SetInt("_ZWrite", 0);
                        material.DisableKeyword("_ALPHATEST_ON");
                        material.EnableKeyword("_ALPHABLEND_ON");
                        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");

                        material.renderQueue = 3000;
                    }
                    //else
                    //{
                    //    material.SetInt("_SrcBlend", (int)BlendMode.One);
                    //    material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
                    //    material.SetInt("_ZWrite", 0);
                    //    material.DisableKeyword("_ALPHATEST_ON");
                    //    material.DisableKeyword("_ALPHABLEND_ON");
                    //    material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    //    material.renderQueue = 3000;
                    //}
                    //Debug.Log(material.name + "  customQueue  " + customQueue);
                }
                break;
            case JistBlendMode.Transparent:
                material.SetFloat("_Mode", (float)JistBlendMode.Transparent);
                material.SetInt("_SrcBlend", (int)BlendMode.One);
                material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case JistBlendMode.Additive:
                break;
        }
    }

    public static Sequence Fade(GameObject root, float start, float end, float duration = 1.0f, float delay = 0.0f)
    {
        Graphic[] GraphicItems = root.GetComponentsInChildren<Graphic>(true);
        Sequence sq = DOTween.Sequence();

        foreach (Graphic item in GraphicItems)
        {
            item.color = new Color(item.color.r, item.color.g, item.color.b, start);
        }
        int i = 0;
        foreach (Graphic item in GraphicItems)
        {
            Color col = new Color(item.color.r, item.color.g, item.color.b, end);
            if (i > 0)
            {
                sq.Join(item.DOColor(col, duration));
            }
            else
            {
                sq.Append(item.DOColor(col, duration).SetDelay(delay));
                i++;
            }
        }
        return sq;
    }

    public void SetWebParam(string param)
    {
        Debug.Log("SetWebParam ====>" + param);
    }
}