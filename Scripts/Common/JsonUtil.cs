using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonUtil 
{
    public static string ObjectToJson(object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    // Json To Object (Json String을 Object로 Deserialization)
    public static T JsonToObject<T>(string jsonData)
    {
        return JsonConvert.DeserializeObject<T>(jsonData);
    }

    public static string ObjectToJsonEx(object obj)
    {
        //Vector3의 프로퍼티인 normalized에서 다시 normalized를 호출할 수 있기 때문에 발생하는 문제
        JsonSerializerSettings setting = new JsonSerializerSettings(); ;
        setting.Formatting = Formatting.Indented;
        setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        return JsonConvert.SerializeObject(obj, setting);
    }

    /// <summary>
    /// path는 반드시 확장자를 제거한 파일 이름만 넣어 줘야함
    /// </summary>
    public static void SaveJsonData(object obj, string path)
    {
#if !UNITY_WEBGL
        // 
        string s_assetPath = Application.streamingAssetsPath + "/";
#else
        //string s_assetPath = Application.dataPath + "/Resources/";
        string s_assetPath = Application.persistentDataPath + "/";
#endif

        string directory = Path.GetDirectoryName(s_assetPath + path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        //Vector3의 프로퍼티인 normalized에서 다시 normalized를 호출할 수 있기 때문에 발생하는 문제
        JsonSerializerSettings setting = new JsonSerializerSettings(); ;
        setting.Formatting = Formatting.Indented;
        setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

        string jsonStr = JsonConvert.SerializeObject(obj, setting);
        System.IO.File.WriteAllText(s_assetPath + path + ".json", jsonStr);
        //Debug.Log("Saved " + s_assetPath + path + ".json");
        DebugScrollView.Instance.Print("Saved " + s_assetPath + path + ".json");
    }
    /// <summary>
    /// path는 반드시 확장자를 제거한 파일 이름만 넣어 줘야함
    /// </summary>
    public static T LoadJsonData<T>(string path)
    {
        //TextAsset ta = Resources.Load<TextAsset>(Application.dataPath + "/Resources/" + path);
        
#if !UNITY_WEBGL
        string myPath = Application.streamingAssetsPath + "/" + path + ".json";
        string jsonStr = LoadFile(myPath);
        //Debug.Log("This is not WebGL!");
#else
        //https://docs.unity3d.com/ScriptReference/Application-streamingAssetsPath.html
        //It is not possible to access the StreamingAssets folder on WebGL and Android platforms. 
        //No file access is available on WebGL. Android uses a compressed .apk file. 
        //These platforms return a URL. Use the UnityWebRequest class to access the Assets.
        string myPath = Application.persistentDataPath + "/" + path + ".json";
        DebugScrollView.Instance.Print(myPath);
        string jsonStr = LoadFile(myPath);
        Debug.Log("This is WebGL!  " + jsonStr);
#endif
        // 없으면 Resources의 것으로 저장하여 사용
        if (jsonStr == null)
        {
            TextAsset ta = Resources.Load<TextAsset>(path);
            if (ta != null)
            {
                jsonStr = ta.text;
#if UNITY_ANDROID
                string currentPath = JistUtil.GetPath("/json/" + path + ".json");
                string dirPath = Path.GetDirectoryName(currentPath);
                if (dirPath != "" && !Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                DebugScrollView.Instance.Print("saving path " + currentPath);
                System.IO.File.WriteAllText(currentPath, jsonStr);
#elif !UNITY_WEBGL
                System.IO.File.WriteAllText(myPath, jsonStr);
#else
                // No file access is available on WebGL.
                Debug.Log("WebGL Not work -> No Load Json....");
#endif
            }
            else
            {
                return default(T);
            }
        }

        DebugScrollView.Instance.Print("Loaded");
        return JsonConvert.DeserializeObject<T>(jsonStr);
    }

    public static T LoadJsonDataFromResource<T>(string path)
    {
        string jsonStr = null;

        TextAsset ta = Resources.Load<TextAsset>(path);
        if (ta != null)
        {
            jsonStr = ta.text;
        }
        else
        {
            return default(T);
        }

        DebugScrollView.Instance.Print("Loaded");
        return JsonConvert.DeserializeObject<T>(jsonStr);
    }

    public static string LoadFile(string path)
    {
        //JistUtil.CheckLine(path);
        if (System.IO.File.Exists(path))
        {
            try
            {
                string s = System.IO.File.ReadAllText(path);
                DebugScrollView.Instance.Print("File Loaded...");
                return s;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("Exception when calling LoadFile: " + e.Message);
                UnityEngine.Debug.LogError(e);
                UnityEngine.Debug.LogError(e.GetType());
                return e.Message;
            }
        }
        else
        {
            DebugScrollView.Instance.Print("File Not Found...");
        }
        return null;
    }
}
