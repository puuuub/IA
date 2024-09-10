using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonDataManager : MonoBehaviour
{
    private static CommonDataManager _ins = null;

    public static CommonDataManager ins
    {
        get
        {
            if (_ins == null)
            {
                GameObject go = new GameObject("CommonDataManager", typeof(CommonDataManager));
                go.transform.localPosition = new Vector3(9999999, 9999999, 9999999);
                _ins = go.GetComponent<CommonDataManager>();

                if (Application.isPlaying)
                {
                    DontDestroyOnLoad(_ins.gameObject);
                }
                else
                {
                    Debug.LogWarning("[CommonDataManager] You'd better ignore CommonDataManager in Editor mode");
                }
            }

            return _ins;
        }
    }
}
