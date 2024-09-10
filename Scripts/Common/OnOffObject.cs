using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffObject : MonoBehaviour
{
    public GameObject[] OffObjects;
    public GameObject[] OnObjects;

    [SerializeField]
    Collider TargetCollider;

    public float OffDelayTime;

    Collider MyCollider;


    // Start is called before the first frame update
    void Start()
    {
        MyCollider = GetComponent<Collider>();
        if (MyCollider != null)
        {
            MyCollider.isTrigger = true;
        }

        Rigidbody rig = GetComponent<Rigidbody>();
        if (rig == null)
        {
            rig = gameObject.AddComponent<Rigidbody>();
        }

        BoxCollider customCollider = GetComponent<BoxCollider>();
        if (customCollider != null)
        {
            customCollider.isTrigger = true;
        }
        else
        {
            MeshCollider coll = GetComponent<MeshCollider>();
            if (coll == null)
            {
                coll = gameObject.AddComponent<MeshCollider>();
            }
            coll.convex = true;
            coll.isTrigger = true;
        }

        rig.isKinematic = true;
        //OtherObjects = new List<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("OnTriggerEnter " + gameObject + " _ " + other);
        if (TargetCollider == null)
        {
            foreach (GameObject go in OffObjects)
            {
                go.SetActive(false);
            }

            foreach (GameObject go in OnObjects)
            {
                go.SetActive(true);
            }
            if (OffDelayTime != 0.0f)
                Invoke("DeActivateOnObjects", OffDelayTime);
        }
        else if(other == TargetCollider)
        {
            foreach (GameObject go in OffObjects)
            {
                go.SetActive(false);
            }

            foreach (GameObject go in OnObjects)
            {
                go.SetActive(true);
            }
            if (OffDelayTime != 0.0f)
                Invoke("DeActivateOnObjects", OffDelayTime);
        }


    }


    void DeActivateOnObjects()
    {
        foreach (GameObject go in OnObjects)
        {
            go.SetActive(false);
        }
    }

    //// Collider ������Ʈ�� is Trigger�� true�� ���·� �浹���� ��
    //private void OnTriggerStay(Collider other)
    //{
    //    Debug.Log(other.name + "���� ��!");
    //}

    // Collider ������Ʈ�� is Trigger�� true�� ���·� �浹�� ������ ��
    private void OnTriggerExit(Collider other)
    {

    }
}
