using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using CasualBase;
using System.Runtime.InteropServices;
using System.Reflection;
using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


//============================================================================================
// 공용으로 사용하는 함수들 임돠
//============================================================================================
public static class CommonUtility
{
    #region InitPrefab
    public static GameObject LoadPrefab(string prefabPath, Transform targetTrans)
    {
        GameObject loadPrefabObj = ResourceManager.ins.Load<GameObject>(prefabPath);

        return InitPrefab(loadPrefabObj, targetTrans);
    }

    public static GameObject InitPrefab(GameObject initObj, Transform targetTrans)
    {
        GameObject initPrefabObj = GameObject.Instantiate(initObj) as GameObject;
        initPrefabObj.transform.SetParent(targetTrans);
        initPrefabObj.transform.localPosition = Vector3.zero;
        initPrefabObj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        initPrefabObj.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        initPrefabObj.name = initObj.name;

        return initPrefabObj;
    }
    #endregion

    /// <summary>
    /// isSelfTransForm : 자신의 TransForm 유지
    /// </summary>
    /// <param name="prefabPath"></param>
    /// <param name="targetTrans"></param>
    /// <param name="isSelfTransForm">자신의 TransForm 유지</param>
    /// <returns></returns>    
    public static GameObject CreatePrefab(string prefabPath, Transform targetTrans, bool isSelfTransForm = false)
    {
        GameObject loadPrefabObj = ResourceManager.ins.Load<GameObject>(prefabPath);
        Transform selfTrans = loadPrefabObj.transform;
        GameObject initPrefabObj = GameObject.Instantiate(loadPrefabObj) as GameObject;
        initPrefabObj.transform.SetParent(targetTrans);
        if (isSelfTransForm)
        {
            initPrefabObj.transform.localPosition = selfTrans.localPosition;
            initPrefabObj.transform.localRotation = selfTrans.localRotation;
            initPrefabObj.transform.localScale = selfTrans.localScale;
        }

        return initPrefabObj;
    }

    #region AddButtonEvent
    public static Button AddButtonEvent(Button button, Action callBack)
    {
        Button.ButtonClickedEvent btnEnvnt = new Button.ButtonClickedEvent();
        button.onClick = btnEnvnt;
        btnEnvnt.AddListener(delegate() { callBack(); });

        return button;
    }


    public static GameObject AddClickEvent(GameObject clickObj, Action onCallBack, Action offCallBack = null)
    {
        Button btn = clickObj.GetComponent<Button>();
        if(btn != null)
        {
            Button.ButtonClickedEvent btnEnvnt = new Button.ButtonClickedEvent();
            btn.onClick = btnEnvnt;
            btnEnvnt.AddListener(delegate() { onCallBack(); });

            return clickObj;
        }
        Debug.LogError("Not Find Button : " + clickObj);
        return null;
    }

    public static GameObject AddClickEvent(string clickObjName, Transform targetTrans, Action onCallBack, Action offCallBack = null)
    {
        GameObject findObj = FindChildObject(clickObjName, targetTrans, true);

        return (findObj == null) ? findObj : AddClickEvent(findObj, onCallBack, offCallBack);

    }
    #endregion


    #region FindObject
    //오브젝트 이름으로 자식의 게임 오브젝트를 찾음(자식의 이름이 유니크 해야함)
    public static GameObject FindChildObject(string name, Transform targetTrans, bool useLog = true)
    {
        Transform findObj = FindObject(name, targetTrans);

        if (findObj == null)
        {
            if (useLog)
            {
                Debug.LogError("Not Find Object Name : " + name + " (Controller Name : " + targetTrans.name + ")");
            }
            return null;
        }

        return findObj.gameObject;
    }

    public static GameObject FindChildObject(string name, GameObject obj, bool useLog = true)
    {
        Transform targetTrans = obj.transform;
        Transform findObj = FindObject(name, targetTrans);

        if (findObj == null)
        {
            if (useLog)
            {
                Debug.LogError("Not Find Object Name : " + name + " (Controller Name : " + targetTrans.name + ")");
            }
            return null;
        }

        return findObj.gameObject;
    }

    private static Transform FindObject(string name, Transform parentTrans, bool useLog = true)
    {
        for (int i = 0; i < parentTrans.childCount; ++i)
        {
            Transform childTrans = parentTrans.GetChild(i);

            Transform findObj = FindObject(name, childTrans);
            if (findObj != null)
                return findObj;

            if (childTrans.name != name)
                continue;

            return childTrans;
        }

        return null;
    }

    /* 이름으로 자식 T 찾음 */
    public static T GetChildScript<T>(string name, Transform targetTrans, bool useLog = true) where T : UnityEngine.Object
    {
        T t = null;

        GameObject go = FindChildObject(name, targetTrans, useLog);
        if (go != null)
            t = go.GetComponent(typeof(T)) as T;

        return t;
    }

    public static T GetChildScript<T>(string name, GameObject obj, bool useLog = true) where T : UnityEngine.Object
    {
        T t = null;
        Transform targetTrans = obj.transform;
        GameObject go = FindChildObject(name, targetTrans, useLog);
        if (go != null)
            t = go.GetComponent(typeof(T)) as T;

        return t;
    }
    #endregion


    public static string GetDescription(Enum value)
    {
        FieldInfo field = value.GetType().GetField(value.ToString());

        DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                    as DescriptionAttribute;

        return attribute == null ? value.ToString() : attribute.Description;
    }

    public static void removeAllChild(Transform trans)
    {
        while (trans.childCount > 0)
        {
            int childcount = trans.childCount;
            Transform child = trans.GetChild(0);
            GameObject.DestroyImmediate(child.gameObject);
            //Destroy(child.gameObject);
            // 제거가 되지 않고 오류 발생.
            if (childcount == trans.childCount)
            {
                Debug.LogError("childcount == trans.childCount");
                break;
            }
        }
    }

    public static string GetDeviceInfo()
    {
        string info = SystemInfo.deviceName + " "
            + SystemInfo.processorType + " "
            + SystemInfo.processorCount + " "
            + SystemInfo.processorFrequency + " "
            + SystemInfo.deviceModel + " "
            + SystemInfo.deviceType + " "
            + SystemInfo.graphicsDeviceName + " "
            + SystemInfo.graphicsDeviceType;
        Debug.Log(info);
        return info;
    }

    public static T DeepCopy<T>(this T source) where T : new()
    {   //확장 메서드
        //복사할 클래스가 [Serializable] 이어야함

        if (!typeof(T).IsSerializable)
        {
            // fail
            return source;
        }

        try
        {
            object result = null;
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, source);
                ms.Position = 0;
                result = (T)formatter.Deserialize(ms);
                ms.Close();
            }

            return (T)result;
        }
        catch (Exception)
        {
            // fail
            return new T();
        }
    }
}
