//#define USE_FILE_OPEN_DIALOG // Windows 빌드라면 Assets\Plugins\System.Windows.Forms.dll 플러그인이 있어야 작동함

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using System.Runtime.InteropServices;
using System.IO;
using UnityEngine.Events;
using System;
using System.Net;
using System.Threading.Tasks;

public class ImageLoader : MonoBehaviour
{

	// Start is called before the first frame update
	public string CurrentURL = "D:/Download/unnamed.jpg";
    private GameObject UICanvas;
    private GameObject ImageBG;

    public Image MyUIImg;

    private Vector3 OringinBGScale;
    Texture2D TemporaryTex;
    string BGFilePath;
    bool _enableBusyIcon = true;

    public bool EnableBusyIcon { get { return _enableBusyIcon; } set { _enableBusyIcon = value; } }

    //public static string MY_BASE_FOLDER;// = Application.dataPath + "/StreamingAssets/";
    public static string MY_SAVE_FOLDER = "Save/";

    private static ImageLoader _ins = null;

    public static ImageLoader ins
    {
        get
        {
            if (_ins == null)
            {
                _ins = FindObjectOfType(typeof(ImageLoader)) as ImageLoader;
                if (_ins == null)
                {
#if !RELEASE
                    Debug.LogError("Error, Fail to get the ImageLoader instance");
#endif
                }
            }
            return _ins;
        }
    }
    public Texture2D GetTexture()
    {
        return TemporaryTex;
    }

    private void Start()
    {
        //MY_BASE_FOLDER = Application.dataPath + "/StreamingAssets/";

        //ImageBG = CommonUtility.FindChildObject("Plane_BG", transform);
        UICanvas =  GameObject.Find("Canvas_UI");
        MyUIImg = GetComponent<Image>();
        if (ImageBG != null)
        {
            OringinBGScale = ImageBG.transform.localScale;
        }
        //GameObject button_OpenFile = CommonUtility.FindChildObject("Button_OpenFile", UICanvas.transform);
        //Button button = button_OpenFile.GetComponent<Button>();
        //button.onClick.AddListener(OpenFileBrowser);
        //Debug.Log("Application.dataPath " + Application.dataPath);
        //Debug.Log("Application.streamingAssetsPath " + Application.streamingAssetsPath);
        //BGFilePath = JsonUtil.LoadJsonData<string>(ImageLoader.MY_SAVE_FOLDER + "InfoSave");

        //// 자동로드
        //if(BGFilePath != null)
        //{
        //    // 이미지로드
        //    //StartCoroutine(LoadFileNew(BGFilePath));
        //}
        //ChangeImageFromResources("png/Character_1");
        //StartCoroutine(LoadFileFromWeb("https://lh4.googleusercontent.com/ZGrh6EmhzGCyhreRmvE23f_0DozTzyI3KIYbBKNRx0evHsir1lGqvYmmMnzlNghsIbAq4Z1PciPOt8Za5TZ1KfAuWjr8H5-RhElbnMRASHMHirtgOlY=w1280"));
    }

    // https://docs.unity3d.com/ScriptReference/WWW-texture.html
    // The data must be an image in JPG or PNG format.
    IEnumerator LoadFile(string url)
    {
		// Start a download of the given URL
		UnityWebRequest www = new UnityWebRequest(url);

		// Wait for download to complete
		yield return www;

        GameObject.Destroy(TemporaryTex);

        TemporaryTex = ((DownloadHandlerTexture)www.downloadHandler).texture;

        ChangeTexture(TemporaryTex);
    }

    IEnumerator LoadFileNew(string url, bool refresh = false)
    {
        // 절대경로로 조립한다.
        string tempPath = Application.dataPath + url;
        DebugScrollView.Instance.Print("Application.dataPath + url  " + tempPath);
        if (_enableBusyIcon)
        {
            BusyWating.ins.Show();
        }

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(tempPath);
        yield return www.SendWebRequest();

        //if (www.isNetworkError || www.isHttpError)
        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            GameObject.Destroy(TemporaryTex);
            TemporaryTex = ((DownloadHandlerTexture)www.downloadHandler).texture;
            TemporaryTex.name = Path.GetFileName(tempPath);
            ChangeTexture(TemporaryTex);
            if (refresh)
            {
                // 바꾸면 로컬에서 읽어들이기 위해 복사본을 만든다. 
                // StreamingAssets 
                SaveCopyImage(MY_SAVE_FOLDER, TemporaryTex);
            }
        }
        www.Dispose();

        if (_enableBusyIcon)
        {
            BusyWating.ins.Hide();
        }
    }
    /// <summary>
    /// action : 다운로드완료 후 할 액션
    /// </summary>
    /// <param name="mbh"></param>
    /// <param name="myImg"></param>
    /// <param name="url"></param>
    /// <param name="action"></param>
    public void LoadFromWeb(MonoBehaviour mbh, Image myImg, string url, UnityAction action = null)
    {
        mbh.StartCoroutine(LoadFileFromWeb(myImg, url, action));
    }


    IEnumerator LoadFileFromWeb(Image myImg, string url, UnityAction action)
    {
        string tempPath = CurrentURL = url;
        DebugScrollView.Instance.Print("Image  url : " + tempPath);
        if (url == null || url == "")
        {
            // 없으면 디폴트 이미지를 표시 안함
            myImg.gameObject.SetActive(false);
            if (action != null)
                action.Invoke();

            yield return null;
        }
        else
        {

            if (_enableBusyIcon)
            {
                BusyWating.ins.ShowWithCount();
            }
            // 투명상태로 시작(로딩전 흰색사각형이 보여주는 걸 방지)
            myImg.color = new Color(1, 1, 1, 0);
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(tempPath);
            www.certificateHandler = new AcceptCeritificates();
            
            Guid uuid = Guid.NewGuid();

            www.SetRequestHeader("tranId", uuid.ToString());
            
            yield return www.SendWebRequest();

            //if (www.isNetworkError || www.isHttpError)
            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                // 없으면 디폴트 이미지를 표시 안함
                myImg.gameObject.SetActive(false);

                if (action != null)
                    action.Invoke();

                Debug.Log(www.error + " : " + url);
            }
            else
            {
                GameObject.Destroy(TemporaryTex);

                TemporaryTex = DownloadHandlerTexture.GetContent(www);

                //TemporaryTex = ((DownloadHandlerTexture)www.downloadHandler).texture;
                TemporaryTex.name = Path.GetFileName(tempPath);


                //ChangeTexture(TemporaryTex);
                ChangeImage(myImg, TemporaryTex);
                // 알파값을 불투명으로 적용
                myImg.color = new Color(1, 1, 1, 1);

                if (action != null)
                    action.Invoke();

                
                //if (refresh)
                //{
                //    // 바꾸면 로컬에서 읽어들이기 위해 복사본을 만든다. 
                //    // StreamingAssets 
                //    SaveCopyImage(MY_SAVE_FOLDER, TemporaryTex);
                //}
            }
            www.Dispose();
            if (_enableBusyIcon)
            {
                BusyWating.ins.HideWithCount();
            }
        }

    }

    public void LoadFromWebOnce(MonoBehaviour mbh, Image myImg, string url, UnityAction action = null)
    {
        mbh.StartCoroutine(LoadFileFromWebOnce(myImg, url, action));
    }

    IEnumerator LoadFileFromWebOnce(Image myImg, string url, UnityAction action)
    {
        string tempPath = CurrentURL = url;
        ImageLoadManager.IsQueueActioning = true;
        DebugScrollView.Instance.Print("Image  url : " + tempPath);
        if (url == null || url == "")
        {
            // 없으면 디폴트 이미지를 표시 안함
            myImg.gameObject.SetActive(false);
            if (action != null)
                action.Invoke();

            yield return null;
        }
        else
        {

            if (_enableBusyIcon)
            {
                BusyWating.ins.ShowWithCount();
            }
            //yield return null;
            // 투명상태로 시작(로딩전 흰색사각형이 보여주는 걸 방지)
            myImg.color = new Color(1, 1, 1, 0);
            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(tempPath))
            {//UnityWebRequest www = UnityWebRequestTexture.GetTexture(tempPath);
                yield return www.SendWebRequest();

                //if (www.isNetworkError || www.isHttpError)
                if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    // 없으면 디폴트 이미지를 표시 안함
                    myImg.gameObject.SetActive(false);

                    if (action != null)
                        action.Invoke();

                    Debug.Log(www.error + " : " + url);
                }
                else
                {
                    GameObject.DestroyImmediate(TemporaryTex);

                    TemporaryTex = DownloadHandlerTexture.GetContent(www);

                    //TemporaryTex = ((DownloadHandlerTexture)www.downloadHandler).texture;
                    TemporaryTex.name = Path.GetFileName(tempPath);


                    //ChangeTexture(TemporaryTex);
                    ChangeImage(myImg, TemporaryTex);
                    // 알파값을 불투명으로 적용
                    myImg.color = new Color(1, 1, 1, 1);

                    if (action != null)
                        action.Invoke();


                    //if (refresh)
                    //{
                    //    // 바꾸면 로컬에서 읽어들이기 위해 복사본을 만든다. 
                    //    // StreamingAssets 
                    //    SaveCopyImage(MY_SAVE_FOLDER, TemporaryTex);
                    //}
                }

                if (_enableBusyIcon)
                {
                    BusyWating.ins.HideWithCount();
                }
            }
        }
        DebugScrollView.Instance.Print("download end time " + (Time.time - ImageLoadManager.downloadstart) + "  " + url);
        ImageLoadManager.IsQueueActioning = false;
        BusyWating.ins.HideWithCount();
    }

    public void LoadLocalFile(Image myImg, string path, bool isNativeSize = true)
    {
        //JistUtil.CheckLine(path);
        if (File.Exists(path))
        {
            try
            {
                if (TemporaryTex == null)
                    TemporaryTex = new Texture2D(2, 2);

                byte[] arr = File.ReadAllBytes(path);

                TemporaryTex.LoadImage(arr);
                TemporaryTex.name = Path.GetFileName(path);
                if (isNativeSize)
                {
                    ChangeImageNativeSize(myImg, TemporaryTex);
                }
                else
                {
                    ChangeImage(myImg, TemporaryTex);
                }
                Debug.Log("File Loaded...");
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("Exception when calling LoadFile: " + e.Message);
                UnityEngine.Debug.LogError(e);
                UnityEngine.Debug.LogError(e.GetType());
            }
        }
        else
        {
            Debug.Log("File Not Found...");
        }
    }

    public static void LoadLocalFileEx(Image myImg, string path, bool isNativeSize = true)
    {
        //JistUtil.CheckLine(path);
        if (File.Exists(path))
        {
            try
            {
                Texture2D tempTex = new Texture2D(2, 2);

                byte[] arr = File.ReadAllBytes(path);

                tempTex.LoadImage(arr);
                tempTex.name = Path.GetFileName(path);
                if (isNativeSize)
                {
                    ChangeImageNativeSize(myImg, tempTex);
                }
                else
                {
                    ChangeImage(myImg, tempTex);
                }
                Debug.Log("File Loaded...");
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("Exception when calling LoadFile: " + e.Message);
                UnityEngine.Debug.LogError(e);
                UnityEngine.Debug.LogError(e.GetType());
            }
        }
        else
        {
            Debug.Log("File Not Found...");
        }
    }


    //private void ButtonDownload_Click(object sender, EventArgs e)
    //{
    //    Task.Run(() => MapDownload());
    //}

    string ClientKey = "uufzppdgb9";
    string ClientSecret = "c30zoYZcAppSam8alHyzmdp7Nj5IDlLtHflcrbGt";

    //public void MapDownload()
    //{
    //    string tempAddr = "https://naveropenapi.apigw.ntruss.com/map-static/v2/raster?w=600&h=600&center=127.2054221,37.3591614&level=16&format=png";

    //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(tempAddr);

    //    request.Headers.Add("X-NCP-APIGW-API-KEY-ID", ClientKey);
    //    request.Headers.Add("X-NCP-APIGW-API-KEY", ClientSecret);

    //    UnityWebRequest unityReq = new UnityWebRequest(tempAddr);
    //    unityReq.SetRequestHeader("X-NCP-APIGW-API-KEY-ID", ClientKey);
    //    unityReq.SetRequestHeader("X-NCP-APIGW-API-KEY", ClientSecret);

    //    unityReq.SendWebRequest();
    //    try
    //    {
    //        var response = (HttpWebResponse)request.GetResponse();
    //        Console.WriteLine($"StatusCode: {response.StatusCode}");
    //        Stream ReceiveStream = response.GetResponseStream();
    //        Image img = System.Drawing.Image.FromStream(ReceiveStream);
    //        img.Save(@"\map.png", System.Drawing.Imaging.ImageFormat.Png);
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine(ex.Message);
    //        Console.WriteLine(ex.StackTrace);
    //    }
    //}

    public void GetNaverMapDownload(MonoBehaviour mbh, Image img, string url, UnityAction action)
    {
        mbh.StartCoroutine(MapDownload(img, url, action));
    }

    public IEnumerator MapDownload(Image myImg, string url, UnityAction action = null)
    {
        //string url = "https://naveropenapi.apigw.ntruss.com/map-static/v2/raster?w=600&h=600&center=126.8403392,33.31422242&level=10&format=png";

        if (_enableBusyIcon)
        {
            BusyWating.ins.ShowWithCount();
        }
        //yield return null;
        // 투명상태로 시작(로딩전 흰색사각형이 보여주는 걸 방지)
        myImg.color = new Color(1, 1, 1, 0);
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            www.SetRequestHeader("X-NCP-APIGW-API-KEY-ID", ClientKey);
            www.SetRequestHeader("X-NCP-APIGW-API-KEY", ClientSecret);
            yield return www.SendWebRequest();

            //if (www.isNetworkError || www.isHttpError)
            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                // 없으면 디폴트 이미지를 표시 안함
                myImg.gameObject.SetActive(false);

                if (action != null)
                    action.Invoke();

                Debug.Log(www.error + " : " + url);
            }
            else
            {
                GameObject.DestroyImmediate(TemporaryTex);

                TemporaryTex = DownloadHandlerTexture.GetContent(www);

                //TemporaryTex = ((DownloadHandlerTexture)www.downloadHandler).texture;
                //raster?w=600&h=600&center=127.2054221,37.3591614&level=16&format=png
                // 로컬저장소에 저장할 이름 정하기
                TemporaryTex.name = Path.GetFileName(url);
                TemporaryTex.name = TemporaryTex.name.Replace("format=", ".");
                TemporaryTex.name = TemporaryTex.name.Replace("=", "_");
                TemporaryTex.name = TemporaryTex.name.Replace("&", "");
                TemporaryTex.name = TemporaryTex.name.Replace("?", "_"); 
               

                //ChangeTexture(TemporaryTex);
                ChangeImage(myImg, TemporaryTex);
                // 알파값을 불투명으로 적용
                myImg.color = new Color(1, 1, 1, 1);

                if (action != null)
                    action.Invoke(); // 여기에서 로컬저장소에 저장


                //if (refresh)
                //{
                    // 바꾸면 로컬에서 읽어들이기 위해 복사본을 만든다. 
                    // StreamingAssets 
                    // SaveCopyImage(MY_SAVE_FOLDER, TemporaryTex);
                //}
            }

            if (_enableBusyIcon)
            {
                BusyWating.ins.HideWithCount();
            }
        }
        DebugScrollView.Instance.Print("download end time " + (Time.time - ImageLoadManager.downloadstart) + "  " + url);
        ImageLoadManager.IsQueueActioning = false;
        BusyWating.ins.HideWithCount();
    }




    public static void ChangeImageNativeSize(Image myImg, Texture2D myTexture)
    {
        if (myTexture != null)
        {
            myImg.sprite = Sprite.Create(myTexture, new Rect(Vector2.zero, new Vector2(myTexture.width, myTexture.height)), new Vector2(0.5f, 0.5f));
            myImg.SetNativeSize();
        }
    }

    public static void ChangeImage(Image myImg, Texture2D myTexture)
    {
        if(myTexture != null)
            myImg.sprite = Sprite.Create(myTexture, new Rect(Vector2.zero, new Vector2(myTexture.width, myTexture.height)), new Vector2(0.5f, 0.5f));
    }

    void ChangeImage(Texture2D myTexture)
    {
        MyUIImg.enabled = true;
        Image myImg = MyUIImg; // MyImg.GetComponent<Image>();
        myImg.sprite = Sprite.Create(myTexture, new Rect(Vector2.zero, new Vector2(myTexture.width, myTexture.height)), new Vector2(0.5f, 0.5f));
    }

    public void ChangeImageFromResources(string path)
    {
        MyUIImg.sprite = Resources.Load<Sprite>(path);
        MyUIImg.SetNativeSize();
    }

    public static bool ChangeImageFromResources(string path, Image target)
    {
        Sprite newSprite = Resources.Load<Sprite>(path);
        if (newSprite != null)
        {
            target.sprite = newSprite;// Resources.Load<Sprite>(path);
            target.SetNativeSize();
            return true;
        }
        return false;
    }

    void ChangeTexture(Texture2D texture)
    {
        
        // Anti Aliasing
        //texture.filterMode = FilterMode.Bilinear;
        // Aliasing
        texture.filterMode = FilterMode.Point; 
        
        //SpriteRenderer renderer = ImageBG.GetComponent<SpriteRenderer>();
        //Sprite _sprite = Sprite.Create(texture, 
        //    new Rect(Vector2.zero, new Vector3(texture.width, texture.height)), 
        //    new Vector2(0.5f, 0.5f), 100.0f);
        //renderer.sprite = _sprite;// Sprite.Create(texture, renderer.sprite.rect, renderer.sprite.pivot);
        MeshRenderer renderer = ImageBG.GetComponent<MeshRenderer>();
        // assign texture
        renderer.materials[0].mainTexture = texture;

        ImageBG.transform.localScale = OringinBGScale;

        float ratioW = 1.0f;
        float ratioH = 1.0f;
        //// 큰 변을 기준으로 줄이기 맞추기
        //if (texture.width > texture.height)
        //{
        //    ratioH = texture.height * 1.0f / texture.width;
        //}
        //else
        //{
        //    ratioW = texture.width * 1.0f / texture.height;
        //}

        //// 작은변을 기준으로 늘이기
        //if (texture.width > texture.height)
        //{
        //    ratioW = texture.width * 1.0f / texture.height;
        //}
        //else
        //{
        //    ratioH = texture.height * 1.0f / texture.width;
        //}

        if (texture.width / texture.height <= Camera.main.scaledPixelWidth / Camera.main.scaledPixelHeight)
        {
            // 세로길이를 기준으로 맞추기
            ratioW = ImageBG.transform.localScale.z * texture.width / texture.height;
            ratioH = ImageBG.transform.localScale.z;
        }
        else
        {
            // 가로길이를 기준으로 맞추기
            ratioW = ImageBG.transform.localScale.x;
            ratioH = ImageBG.transform.localScale.x * texture.height * 1.0f / texture.width;
        }

        ImageBG.transform.localScale = 
        new Vector3(ratioW, ImageBG.transform.localScale.y, ratioH);
    }


    // 로컬저장소(StreamingAssets)에 이미지를 복사한다.
    void SaveCopyImage(string pathDirectory, Texture2D texture)
    {
        string currentPath = Application.dataPath + "/StreamingAssets/" + pathDirectory;
        if (!Directory.Exists(currentPath))
        {
            Directory.CreateDirectory(currentPath);
        }
        byte[] data = null;
        if(texture.name.Contains(".png"))
        {
            data = texture.EncodeToPNG();
        }
        else
        {
            data = texture.EncodeToJPG();
        }
            
        System.IO.File.WriteAllBytes(currentPath + texture.name, data);

        // 복사한 이미지의 절대경로를 저장한다.
        JsonUtil.SaveJsonData(currentPath + texture.name, MY_SAVE_FOLDER + "CopiedImagePath");

        // 다음 실행시 자동으로 로드할 상대경로를 저장한다.
        currentPath = currentPath.Replace("\\", "/");
        currentPath = currentPath.Replace(Application.dataPath, "");
        JsonUtil.SaveJsonData(currentPath + texture.name, MY_SAVE_FOLDER + "InfoSave");
    }
    public void LoadImageByBase64(string base64)
    {
        Texture2D tex = new Texture2D(200, 200);
        byte[] b = Convert.FromBase64String(base64);
        tex.LoadImage(b);


        MyUIImg.material.mainTexture = tex;
    }

#if USE_FILE_OPEN_DIALOG
    public void OpenFileBrowser()
    {
        // Open file
#if ((!UNITY_EDITOR) && (UNITY_WEBGL))
        ////Fee.Platform.WebGL_OpenFileDialog.OpenFileDialog(m_RootMono, "Open", "gpx");
        //Fee.Platform.Platform.GetInstance().OpenFileDialog("Open", "gpx");
        //m_IsShowFileDialog = true;
#else
        System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();

        if (BGFilePath != null)
            openFileDialog.InitialDirectory = Path.GetDirectoryName(Application.dataPath + BGFilePath);
        // The data must be an image in JPG or PNG format.
        openFileDialog.Filter = "image files (*.png,*.jpg,*.jpeg)|*.png;*.jpg;*.jpeg";
        //openFileDialog.FilterIndex = 2;
        openFileDialog.RestoreDirectory = true;
        string filePath = string.Empty;
        if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            //Get the path of specified file
            filePath = openFileDialog.FileName;
        }

        if (filePath != null && filePath != "")
        {
            //if(BGFilePath == null)
            BGFilePath = filePath;// Path.GetDirectoryName(filePath);
            BGFilePath = BGFilePath.Replace("\\", "/");
            // 상대경로르 만든다.
            BGFilePath = BGFilePath.Replace(Application.dataPath, "");
            JsonUtil.SaveJsonData(BGFilePath, MY_SAVE_FOLDER + "InfoSave");
            //FilePath = Path.GetFileName(filePath);
            //FilePath = Path.GetFileNameWithoutExtension(filePath);
            //FilePath = Path.GetFullPath(filePath);
            //StartCoroutine(LoadFile(filePath));
            StartCoroutine(LoadFileNew(BGFilePath, true));
        }
#endif
    } 
#endif


    //// Update is called once per frame
    //void Update()
    //{

    //}
}
