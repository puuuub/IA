using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPredictGaugeController : MonoBehaviour
{
    [SerializeField]
    GameObject needleRoot;

    [SerializeField]
    TMP_Text value_txt;

    [SerializeField]
    TMP_Text title_txt;


    /// <param name="rotY">0 ~ -180</param>
    public void SetContents(string val, string title, float rotY)
    {
        gameObject.SetActive(true);

        title_txt.text = title;
        value_txt.text = val;
        needleRoot.transform.localRotation = Quaternion.Euler(Vector3.forward * rotY);
    }

    public void SetContentsOff()
    {
        gameObject.SetActive(false);
    }


}
