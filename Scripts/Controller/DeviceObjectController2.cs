using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class DeviceObjectController2 : BasicObjectController
{
    //DevicePopupPanel용 controller

    
    [SerializeField]
    List<GameObject> partOutlineList;      //0:안면인식기, 1:탑승권리더기, 2:여권판독기, 3:게이트도어

    //<partOutlineList, List<material>
    Dictionary<GameObject, List<Material>> partTransDic;

    void Start()
    {
        partTransDic = new Dictionary<GameObject, List<Material>>();
        for(int i= 0; i < partOutlineList.Count; i++)
        {
            if (!partTransDic.ContainsKey(partOutlineList[i]))
            {
                partTransDic.Add(partOutlineList[i], new List<Material>());

                var rens = partOutlineList[i].GetComponentsInChildren<Renderer>();
                for (int j = 0; j < rens.Length; j++)
                {
                    partTransDic[partOutlineList[i]].Add(rens[j].material);
                }
            }
        }

        SetAllOutlineOff();
    }
    /// <summary>
    /// 파츠 outline 켜기
    /// outline 대체 투명 오브젝트
    /// </summary>
    /// <param name="idx">0:안면인식기, 1:탑승권리더기, 2:여권판독기, 3:게이트도어</param>
    public void SetPartOutlineOn(int idx, Color col)
    {
        
        for(int i = 0; i < partOutlineList.Count; i++)
        {
            partOutlineList[i].SetActive(idx.Equals(i));

            var mats = partTransDic[partOutlineList[i]];
            for(int j = 0; j < mats.Count; j++)
            {
                mats[j].color = col;
            }
        }

    }
    /// <summary>
    /// 전체 outline 끄기 
    /// outline 대체 투명 오브젝트
    /// </summary>
    /// <param name="isOn"></param>
    public void SetAllOutlineOff()
    {
        for (int i = 0; i < partOutlineList.Count; i++)
        {
            partOutlineList[i].SetActive(false);
        }

    }

    public void SetContentsOnOff(bool isOn)
    {
        gameObject.SetActive(isOn);
    }
}
