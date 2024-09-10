using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public GameObject Content;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // Returns the first active loaded object of Type type.
                // 활성화상태인 오브젝트만 찾을 수 있음
                instance = FindObjectOfType<T>() as T;
                if (instance == null)
                {
                    GameObject obj;
                    // 오브젝트 찾기
                    // This function only returns active GameObjects.
                    // 활성화상태인 오브젝트만 찾을 수 있음
                    obj = GameObject.Find(typeof(T).Name);
                    if (obj != null)
                    {
                        instance = obj.GetComponent<T>();
                    }
#if !RELEASE
                    if (instance == null)
                        Debug.LogWarning(string.Format("Warning, Fail to get the {0} instance", typeof(T).Name));
#endif
                }
                else
                {
                    DebugScrollView.Instance.Print(string.Format("Success get the {0} instance", typeof(T).Name));
                }
            }
            return instance;
        }


    }
}
