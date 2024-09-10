using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class DeviceObjectController2 : BasicObjectController
{
    //DevicePopupPanel�� controller

    
    [SerializeField]
    List<GameObject> partOutlineList;      //0:�ȸ��νı�, 1:ž�±Ǹ�����, 2:�����ǵ���, 3:����Ʈ����

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
    /// ���� outline �ѱ�
    /// outline ��ü ���� ������Ʈ
    /// </summary>
    /// <param name="idx">0:�ȸ��νı�, 1:ž�±Ǹ�����, 2:�����ǵ���, 3:����Ʈ����</param>
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
    /// ��ü outline ���� 
    /// outline ��ü ���� ������Ʈ
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
