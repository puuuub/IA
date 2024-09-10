using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 0.5f;

    [SerializeField]
    bool isUp;

    [SerializeField]
    bool isMiddle;
    
    float[] posZ = { -195f, 430f };
    float targetPos;
    float middlePos = 130f;

    // Start is called before the first frame update
    void Start()
    {
        if (isUp)
            targetPos = posZ[0];
        else
            targetPos = posZ[1];

    }

    // Update is called once per frame
    void Update()
    {
        float zz;
        if (isMiddle)
        {
            zz = middlePos;
        }
        else
        {
            zz = targetPos;
        }



        Vector3 pos = (transform.localPosition.x * Vector3.right) + (transform.localPosition.y * Vector3.up) 
            + (Mathf.Lerp(transform.localPosition.z, zz, moveSpeed) * Vector3.forward);
        
        transform.localPosition = pos;


        if (transform.localPosition.z > zz - 1 && transform.localPosition.z < zz + 1)
        {
            isMiddle = !isMiddle;
        }
    }
}
