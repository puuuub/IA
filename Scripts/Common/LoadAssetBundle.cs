using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadAssetBundle : SingletonMonoBehaviour<LoadAssetBundle>
{
    GameObject BuilingRoot;
    public GameObject BuildingObject { get; private set; }

    private void Awake()
    {
        BuilingRoot = GameObject.Find("BuildingRoot");
        LoadBuilding();
    }

    void Start() {
        //StartCoroutine(GetBuilding(Path.Combine(Application.streamingAssetsPath, "gameobject.unity3d")));
    }

    void LoadBuilding()
    {
        AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "gameobject.unity3d"));
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }
        var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("Bongwan_01");
        BuildingObject = Instantiate<GameObject>(prefab);
        BuildingObject.transform.SetParent(BuilingRoot.transform);
        //myLoadedAssetBundle.Unload(false);
        //AssetBundle.UnloadAllAssetBundles(true);
    }

    IEnumerator GetBuilding(string url)
    {
        using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                // Get downloaded asset bundle
                AssetBundle myLoadedAssetBundle = DownloadHandlerAssetBundle.GetContent(uwr);

                if (myLoadedAssetBundle == null)
                {
                    Debug.LogError("Failed to load AssetBundle!");
                }
                else
                {
                    var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("Bongwan_01");
                    BuildingObject = Instantiate<GameObject>(prefab);
                    BuildingObject.transform.SetParent(BuilingRoot.transform);
                    myLoadedAssetBundle.Unload(false);
                    Debug.Log("<color=#12bae9ff>loaded AssetBundle!</color>");
                }
            }
        }
    }
}
