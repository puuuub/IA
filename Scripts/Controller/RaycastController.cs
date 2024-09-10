using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    public Camera mainCamera;

    [SerializeField]
    bool raycastOn = true;

    Ray ray;
    RaycastHit hit;
    LayerMask lm;

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    void Start()
    {
        lm = (1 << 6); 

        RaycastOnOff(false);
    }

    private void Update()
    {
        if (raycastOn && Input.GetMouseButtonDown(0))
        {
            RaycastShot();
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(raycastOn)
            Raycast();
    }
    
    void Raycast()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, lm))
        {
            //hit.transform.gameObject
            MainManager.Instance.HitRaycast(hit.transform.gameObject);
        }
        else
        {
            MainManager.Instance.NoHitRaycast();
        }
    }
    void RaycastShot()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, lm))
        {
            //hit.transform.gameObject
            MainManager.Instance.ObjectClick(hit.transform.gameObject);
        }
    }
    public void RaycastOnOff(bool x)
    {
        raycastOn = x;

    }

}
