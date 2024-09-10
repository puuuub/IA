using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : SingletonMonoBehaviour<MaterialChanger>
{
    public static List<MaterialChanger> MaterialChangerList = new List<MaterialChanger>();

    public List<Material> MaterialList;
    public Material[] MaterialArr = new Material[2];
    // Start is called before the first frame update
    void Start()
    {
        MaterialChangerList.Add(this);
        MaterialList.Clear();
        MaterialList.Add(GetComponent<Renderer>().material);
        MaterialList.Add(null);

        MaterialArr[0] = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Change(int index)
    {
        if (MaterialList[index] != null)
            GetComponent<MeshRenderer>().material = MaterialList[index];
        else
            print(string.Format("MaterialList[{0}] is NULL", index));

        if (MaterialArr[index] != null)
            GetComponent<MeshRenderer>().material = MaterialArr[index];
        else
            print(string.Format("MaterialArr[{0}] is NULL", index));
    }

    public static void ChangeAll(int index)
    {
        for(int k = 0; k < MaterialChangerList.Count; k++)
        {
            MaterialChangerList[k].Change(index);
        }
    }

}
