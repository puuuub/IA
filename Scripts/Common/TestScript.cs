using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

//#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {
     
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Return))
        {
            DataManager.Instance.EventTest();
            MainManager.Instance.ReceiveEvent();
        }
        
    }
//#endif
}
