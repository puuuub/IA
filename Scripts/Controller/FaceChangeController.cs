using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceChangeController : MonoBehaviour
{
    [SerializeField]
    Renderer ren;

    [SerializeField]
    Material[] mats;

    public float changeTime = 5f;
    int curIdx;
    // Start is called before the first frame update
    void Start()
    {
        curIdx = 0;

        Invoke("ChangeMats", changeTime);
    }

    void ChangeMats()
    {
        ren.material = mats[curIdx++];
        
        if(curIdx > 1)
        {
            curIdx = 0;
        }

        Invoke("ChangeMats", changeTime);
    }
}
