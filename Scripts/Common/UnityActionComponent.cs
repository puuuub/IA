using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityActionComponent : MonoBehaviour
{
    [SerializeField] 
    public List<UnityAction> events = new List<UnityAction>();
    public delegate void Callback();
    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
