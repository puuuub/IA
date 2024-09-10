using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour 
{
    private static ResourceManager _ins = null;
    public static ResourceManager ins
    {
        get
        {
            if (_ins == null)
            {
                _ins = FindObjectOfType<ResourceManager>();
                if (_ins == null)
                {
                    GameObject container = new GameObject("ResourceManager");
                    _ins = container.AddComponent<ResourceManager>();
                }
            }
            return _ins;
        }
    }

    private Dictionary<string, Object> resource = new Dictionary<string, Object>();

    void Awake()
    {
        DontDestroyOnLoad(this);
        resource.Clear();
    }
        
    public T Load<T>(string name) where T : Object
    {
        string resourceName = name.Trim();

        T t = null;

        if (resource.ContainsKey(resourceName))
            t = (T)resource[resourceName];
        else
        {
            t = Resources.Load<T>(resourceName);
            resource.Add(resourceName, t);
        }

        if (t == null)
            Debug.LogErrorFormat("ResourceManager ----- resource load fail.........resourceName : {0}", resourceName);
            
        return t;
    }

    public GameObject InstantiateGameObject(Object obj)
    {
        GameObject go = Instantiate(obj) as GameObject;

        return go;
    }

    public GameObject InstantiateGameObject(Object obj, Transform parent)
    {
        GameObject go = Instantiate(obj, parent) as GameObject;

        return go;
    }

    public GameObject InstantiateGameObject(Object obj, Vector3 position, Quaternion rotation)
    {
        GameObject go = Instantiate(obj, position, rotation) as GameObject;

        return go;
    }
}
