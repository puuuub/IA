using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SingletonClass<T> where T : SingletonClass<T>, new()
{
    protected SingletonClass() { }

    private static T _instance;

    public virtual void Init()
    {
    }

    public void DoNothing() { }

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
                _instance.Init();
            }
            return _instance;
        }
    }
}
