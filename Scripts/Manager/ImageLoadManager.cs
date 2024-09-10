//#define TEST_IMAGEDICT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.IO;
using UnityEngine.Experimental.Rendering;

public class ImageLoadManager :  SingletonMonoBehaviour<ImageLoadManager>
{
    public static Dictionary<string, Texture2D> TempImageDict = new Dictionary<string, Texture2D>();

    [SerializeField]
    Image MyTestImage;

    // Start is called before the first frame update
    void Start()
    {
        StartEnqueueUpdate();
    }

    /// <summary>
    /// action : 다운로드완료 후 할 액션
    /// </summary>
    public Image LoadImage(Image myImg, string Url, UnityAction action = null)
    {
        ImageLoader loader = new ImageLoader();
        //loader.EnableBusyIcon = false;
        loader.LoadFromWeb(this, myImg, Url, action);

        return myImg;
    }

    public Image LoadImage(Image myImg, string Url, string fileName)
    {
        ImageLoader loader = new ImageLoader();
        loader.LoadFromWeb(this, myImg, Url + fileName);
        return myImg;
    }

    //    // StreamingAssets 다운로드후 폴더에 저장하기
    //    public Image LoadImage(string ImgeId, string downloadURL, Image myImg, UnityAction action = null)
    //    {
    //        try
    //        {
    //            JistUtil.CheckLine(downloadURL);
    //            //UriBuilder ub = new UriBuilder(downloadURL);

    //            Uri uri = new Uri(downloadURL);// ub.Uri;
    //            JistUtil.CheckLine();
    //            string localPath = uri.LocalPath;
    //            string path = JistUtil.GetStreamingAssetsPath(localPath);
    //            JistUtil.CheckLine(path);
    //            string dirPath = Path.GetDirectoryName(path);
    //            string fileName = Path.GetFileName(path);

    //            string curImagePath = PlayerPrefs.GetString(ImgeId);


    //#if (UNITY_EDITOR || !UNITY_WEBGL) && !TEST_IMAGEDICT
    //            // 저장된 이미지가 없으면
    //            if (curImagePath == "" || !File.Exists(path))
    //            {
    //                // 다운로드
    //                ImageLoader loader = new ImageLoader();
    //                loader.EnableBusyIcon = false;
    //                loader.LoadFromWeb(this, myImg, downloadURL, delegate { SaveCopyImage(path, myImg.sprite.texture); if (action != null) action.Invoke(); });
    //                DebugScrollView.Instance.Print("ImageLoader : " + downloadURL);
    //                PlayerPrefs.SetString(ImgeId, downloadURL);

    //            }
    //            else if (curImagePath != downloadURL)  // 다른 이미지로 교체시
    //            {
    //                ImageLoader loader = new ImageLoader();
    //                loader.EnableBusyIcon = false;
    //                loader.LoadFromWeb(this, myImg, downloadURL, delegate { SaveCopyImage(path, myImg.sprite.texture); if (action != null) action.Invoke(); });
    //                DebugScrollView.Instance.Print("ImageLoader : " + downloadURL);
    //                PlayerPrefs.SetString(ImgeId, downloadURL);
    //                Uri curUri = new Uri(curImagePath);
    //                string curLocalPath = curUri.LocalPath;
    //                string existedFilePath = JistUtil.GetStreamingAssetsPath(curLocalPath);
    //                // 이전 버전 이미지 제거
    //                DeleteFile(existedFilePath);
    //            }
    //            else
    //            {
    //                // 로컬 저장소의 이미지를 로드
    //                ImageLoader loader = new ImageLoader();
    //                //loader.EnableBusyIcon = false;
    //                // 메소드 이름에 web이 들어가지만 로컬위치의 경로임
    //                loader.LoadFromWeb(this, myImg, path);
    //                myImg.SetNativeSize();
    //                DebugScrollView.Instance.Print("ImageLoader : " + path);
    //            }
    //#else 
    //            // WebGL이면 딕션너리에서 찾아보고
    //            Texture2D tex = null;
    //            if (TempImageDict.TryGetValue(downloadURL, out tex))
    //            {
    //                ImageLoader.ChangeImage(myImg, tex);
    //                print("Loaded Dictionary " + downloadURL);
    //            }
    //            else // 없으면 다운로드
    //            {
    //                // 다운로드
    //                ImageLoader loader = new ImageLoader();
    //                loader.LoadFromWeb(this, myImg, downloadURL);
    //                DebugScrollView.Instance.Print("ImageLoader : " + downloadURL);
    //            }
    //#endif
    //        }
    //        catch (UriFormatException e)
    //        {
    //            print("downloadURL : " + downloadURL);
    //            JistUtil.CheckLine(downloadURL);
    //            print(e.Message);
    //            print(e.StackTrace );
    //            print(e.InnerException.StackTrace);
    //        }
    //        return myImg;
    //    }


    // Application.persistentDataPath 다운로드후 폴더에 저장하기
    public Image LoadImageEx(string ImgeId, string downloadURL, Image myImg, UnityAction action = null)
    {
        string preImageURL = PlayerPrefs.GetString(ImgeId);
        try
        {
            //JistUtil.CheckLine(downloadURL);
            DebugScrollView.Instance.Print(downloadURL);
            //UriBuilder ub = new UriBuilder(downloadURL);

            Uri uri = new Uri(downloadURL);// ub.Uri;
            //JistUtil.CheckLine();
            string localPath = uri.LocalPath;
            // 최신 파일경로
            string currentPath = JistUtil.GetPath(localPath);
            //JistUtil.CheckLine(path);
            string dirPath = Path.GetDirectoryName(currentPath);
            string fileName = Path.GetFileName(currentPath);
            
            bool isFirstImage = preImageURL == "";

            Uri preUri = null;
            string preLocalPath = null;
            if (!isFirstImage)
            {
                preUri = new Uri(preImageURL);
                preLocalPath = JistUtil.GetPath(preUri.LocalPath);
            }

#if (UNITY_EDITOR || !UNITY_WEBGL) && !TEST_IMAGEDICT
            // 저장된 적이 없던가 아니면 저장된 이미지파일이 없으면
            if (isFirstImage || !File.Exists(preLocalPath))
            {
                // 다운로드
                ImageLoader loader = new ImageLoader();
                loader.EnableBusyIcon = false;
                // 다운로드 후 로컬에 저장 + 이미지 네이티브사이즈로 설정
                loader.LoadFromWeb(this, myImg, downloadURL, delegate { SaveCopyImage(currentPath, myImg.sprite); if (action != null) action.Invoke(); });
                DebugScrollView.Instance.Print("ImageLoader : " + downloadURL);
                // 다운로드 받았던 경로 저장
                PlayerPrefs.SetString(ImgeId, downloadURL);

            }
            // 기존에 있는지부터 비교해서 삭제하기
            else if (preImageURL != downloadURL)  // 다른 이미지로 교체시
            {
                ImageLoader loader = new ImageLoader();
                loader.EnableBusyIcon = false;
                // 다운로드 후 로컬에 저장 + 이미지 네이티브사이즈로 설정
                loader.LoadFromWeb(this, myImg, downloadURL, delegate { SaveCopyImage(currentPath, myImg.sprite); if (action != null) action.Invoke(); });
                DebugScrollView.Instance.Print("ImageLoader : " + downloadURL);
                PlayerPrefs.SetString(ImgeId, downloadURL);
                 // 이전 버전 이미지 제거
                DeleteFile(preLocalPath);
            }
            else
            {
                // 로컬 저장소의 이미지를 로드
                ImageLoader loader = new ImageLoader();
                //loader.EnableBusyIcon = false;
                // 메소드 이름에 web이 들어가지만 로컬위치의 경로임
                loader.LoadLocalFile(myImg, currentPath);
                myImg.SetNativeSize();
                DebugScrollView.Instance.Print("ImageLoader : " + currentPath);
            }
#else 
            // WebGL이면 딕션너리에서 찾아보고
            Texture2D tex = null;
            if (TempImageDict.TryGetValue(downloadURL, out tex))
            {
                ImageLoader.ChangeImage(myImg, tex);
                DebugScrollView.Instance.Print("Loaded Dictionary " + downloadURL);
                myImg.SetNativeSize();
            }
            else // 없으면 다운로드
            {
                // 다운로드
                ImageLoader loader = new ImageLoader();
                loader.LoadFromWeb(this, myImg, downloadURL, delegate {
                    // WebGL이면 딕셔너리에 존재하는지 검사후
                    if (!TempImageDict.ContainsKey(downloadURL))
                    {
                        //이미지 딕셔너리에 등록
                        ImageLoadManager.TempImageDict.Add(downloadURL, loader.GetTexture());
                        DebugScrollView.Instance.Print("added to Dictionary " + downloadURL);
                    }
                    //foreach(KeyValuePair<string, Texture2D> element in ImageLoadManager.TempImageDict)
                    //{
                    //    print(element);
                    //}
                    
                    myImg.SetNativeSize();
                });
                
                DebugScrollView.Instance.Print("ImageLoader : " + downloadURL);
            }
#endif
        }
        catch (UriFormatException e)
        {
            print("preImageURL : " + downloadURL);
            print("preImageURL : " + preImageURL);
            //JistUtil.CheckLine(downloadURL);
            print(e.Message);
            print(e.StackTrace);
            // catch문에서 null 참조 에러발생해도 유니티에서 에러로그 안찍힘
            // 그래서 에러없는데 이후 코드가 실행이 안되고 로그에는 아무런 문제 없는 것처럼 표시됨
            if(e.InnerException != null)
                print(e.InnerException.StackTrace);
            return myImg;
        }
        return myImg;
    }

    // 앱 실행시 1회저장후 있으면 로컬파일 사용
    // 삭제는 다음 실행과 동시에 삭제 (DeleteFiles)
    public Image LoadImageOnce(Image myImg, string downloadURL, UnityAction action = null)
    {
        //string preImageURL = PlayerPrefs.GetString(ImgeId);
        try
        {
            //JistUtil.CheckLine(downloadURL);
            DebugScrollView.Instance.Print(downloadURL);
            //UriBuilder ub = new UriBuilder(downloadURL);

            Uri uri = new Uri(downloadURL);// ub.Uri;
            //JistUtil.CheckLine();
            string localPath = uri.LocalPath;
            // 최신 파일경로
            string currentPath = JistUtil.GetPath(localPath);
            //JistUtil.CheckLine(path);
            string dirPath = Path.GetDirectoryName(currentPath);
            string fileName = Path.GetFileName(currentPath);

            currentPath = dirPath + "~/" + fileName; 
            Uri preUri = null;
            string preLocalPath = currentPath;


#if (UNITY_EDITOR || !UNITY_WEBGL) && !TEST_IMAGEDICT
            // 저장된 적이 없던가 아니면 저장된 이미지파일이 없으면
            if (!File.Exists(preLocalPath))
            {
                // 다운로드
                ImageLoader loader = new ImageLoader();
                loader.EnableBusyIcon = false;
   
                // 다운로드 후 로컬에 저장 + 이미지 네이티브사이즈로 설정
                //loader.LoadFromWeb(this, myImg, downloadURL, delegate { SaveCopyImage(currentPath, myImg.sprite); if (action != null) action.Invoke(); });
                EnqueueAction(delegate { loader.LoadFromWebOnce(this, myImg, downloadURL, delegate { SaveCopyImage(currentPath, myImg.sprite); if (action != null) action.Invoke(); }); });
                DebugScrollView.Instance.Print("ImageLoader : " + downloadURL);
            }
            else
            {
                // 로컬 저장소의 이미지를 로드
                ImageLoader loader = new ImageLoader();
                //loader.EnableBusyIcon = false;
                // 메소드 이름에 web이 들어가지만 로컬위치의 경로임
                loader.LoadLocalFile(myImg, currentPath, false);

                if (action != null)
                    action.Invoke();
                DebugScrollView.Instance.Print("Local Image : " + currentPath);
            }
#else 
            // WebGL
            Texture2D tex = null;
            if (TempImageDict.TryGetValue(downloadURL, out tex))
            {
                ImageLoader.ChangeImage(myImg, tex);
                DebugScrollView.Instance.Print("Loaded Dictionary " + downloadURL);
                if (action != null)
                    action.Invoke();
            }
            else // 없으면 다운로드
            {
                // 다운로드
                ImageLoader loader = new ImageLoader();


                EnqueueAction(delegate {
                    loader.LoadFromWeb(this, myImg, downloadURL, delegate {
                        // WebGL이면 딕셔너리에 존재하는지 검사후
                        if (!TempImageDict.ContainsKey(downloadURL))
                        {
                            //이미지 딕셔너리에 등록
                            TempImageDict.Add(downloadURL, loader.GetTexture());
                            DebugScrollView.Instance.Print("added to Dictionary " + downloadURL);
                        }
                        //foreach(KeyValuePair<string, Texture2D> element in ImageLoadManager.TempImageDict)
                        //{
                        //    print(element);
                        //}

                        if (action != null)
                            action.Invoke();
                    });
                });

                DebugScrollView.Instance.Print("ImageLoader : " + downloadURL);
            }
#endif
        }
        catch (UriFormatException e)
        {
            print("preImageURL : " + downloadURL);
            //JistUtil.CheckLine(downloadURL);
            print(e.Message);
            print(e.StackTrace);
            // catch문에서 null 참조 에러발생해도 유니티에서 에러로그 안찍힘
            // 그래서 에러없는데 이후 코드가 실행이 안되고 로그에는 아무런 문제 없는 것처럼 표시됨
            if (e.InnerException != null)
                print(e.InnerException.StackTrace);
            return myImg;
        }
        return myImg;
    }

    public Image LoadMapImage(Image myImg, string downloadURL, UnityAction action = null)
    {
        //string preImageURL = PlayerPrefs.GetString(ImgeId);
        try
        {
            //JistUtil.CheckLine(downloadURL);
            DebugScrollView.Instance.Print(downloadURL);
            //UriBuilder ub = new UriBuilder(downloadURL);

            string  tempName = downloadURL;
            tempName = tempName.Replace("format=", ".");
            tempName = tempName.Replace("=", "_");
            tempName = tempName.Replace("&", "");
            tempName = tempName.Replace("?", "_");


            Uri uri = new Uri(tempName);// ub.Uri;

            //JistUtil.CheckLine();
            string localPath = uri.LocalPath;
            // 최신 파일경로
            string currentPath = JistUtil.GetPath(localPath);
            //JistUtil.CheckLine(path);
            string dirPath = Path.GetDirectoryName(currentPath);
            string fileName = Path.GetFileName(currentPath);

            currentPath = dirPath + "~/" + fileName;
            Uri preUri = null;
            string preLocalPath = currentPath;


#if (UNITY_EDITOR || !UNITY_WEBGL) && !TEST_IMAGEDICT
            // 저장된 적이 없던가 아니면 저장된 이미지파일이 없으면
            if (!File.Exists(preLocalPath))
            {
                // 다운로드
                ImageLoader loader = new ImageLoader();
                loader.EnableBusyIcon = false;

                // 다운로드 후 로컬에 저장 + 이미지 네이티브사이즈로 설정
                //loader.LoadFromWeb(this, myImg, downloadURL, delegate { SaveCopyImage(currentPath, myImg.sprite); if (action != null) action.Invoke(); });
                EnqueueAction(delegate { loader.GetNaverMapDownload(this, myImg, downloadURL, delegate { SaveCopyImage(currentPath, myImg.sprite); if (action != null) action.Invoke(); }); });
                DebugScrollView.Instance.Print("ImageLoader : " + downloadURL);
            }
            else
            {
                // 로컬 저장소의 이미지를 로드
                // ImageLoader loader = new ImageLoader();
                //loader.EnableBusyIcon = false;
                // 메소드 이름에 web이 들어가지만 로컬위치의 경로임
                ImageLoader.LoadLocalFileEx(myImg, currentPath, false);

                if (action != null)
                    action.Invoke();
                DebugScrollView.Instance.Print("Local Image : " + currentPath);
            }
#else 
            // WebGL이면 딕션너리에서 찾아보고
            Texture2D tex = null;
            if (TempImageDict.TryGetValue(downloadURL, out tex))
            {
                ImageLoader.ChangeImage(myImg, tex);
                DebugScrollView.Instance.Print("Loaded Dictionary " + downloadURL);
                if (action != null)
                    action.Invoke();
            }
            else // 없으면 다운로드
            {
                // 다운로드
                ImageLoader loader = new ImageLoader();
                //loader.LoadFromWeb(this, myImg, downloadURL, delegate {
                //    // WebGL이면 딕셔너리에 존재하는지 검사후
                //    if (!ImageLoadManager.TempImageDict.ContainsKey(downloadURL))
                //    {
                //        //이미지 딕셔너리에 등록
                //        ImageLoadManager.TempImageDict.Add(downloadURL, loader.GetTexture());
                //        DebugScrollView.Instance.Print("added to Dictionary " + downloadURL);
                //    }
                //    //foreach(KeyValuePair<string, Texture2D> element in ImageLoadManager.TempImageDict)
                //    //{
                //    //    print(element);
                //    //}

                //    if (action != null)
                //        action.Invoke();
                //});

                EnqueueAction(delegate {
                    loader.LoadFromWeb(this, myImg, downloadURL, delegate {
                        // WebGL이면 딕셔너리에 존재하는지 검사후
                        if (!ImageLoadManager.TempImageDict.ContainsKey(downloadURL))
                        {
                            //이미지 딕셔너리에 등록
                            ImageLoadManager.TempImageDict.Add(downloadURL, loader.GetTexture());
                            DebugScrollView.Instance.Print("added to Dictionary " + downloadURL);
                        }
                        //foreach(KeyValuePair<string, Texture2D> element in ImageLoadManager.TempImageDict)
                        //{
                        //    print(element);
                        //}

                        if (action != null)
                            action.Invoke();
                    });
                });

                DebugScrollView.Instance.Print("ImageLoader : " + downloadURL);
            }
#endif
        }
        catch (UriFormatException e)
        {
            print("preImageURL : " + downloadURL);
            //JistUtil.CheckLine(downloadURL);
            print(e.Message);
            print(e.StackTrace);
            // catch문에서 null 참조 에러발생해도 유니티에서 에러로그 안찍힘
            // 그래서 에러없는데 이후 코드가 실행이 안되고 로그에는 아무런 문제 없는 것처럼 표시됨
            if (e.InnerException != null)
                print(e.InnerException.StackTrace);
            return myImg;
        }
        return myImg;
    }

    public enum ImageRatio
    {
        HORIZONTAL,
        VERTICAL
    }

    public void SetImageRatio(Image img, ImageRatio ratio)
    {
        if(img.sprite == null)
        {
            return;
        }

        float ratioW = 1.0f;
        float ratioH = 1.0f;
        if (ratio == ImageRatio.VERTICAL)
        {
            // 세로길이를 기준으로 맞추기
            ratioW = img.rectTransform.sizeDelta.y * img.sprite.texture.width / img.sprite.texture.height;
            ratioH = img.rectTransform.sizeDelta.y;
        }
        else
        {
            // 가로길이를 기준으로 맞추기
            ratioW = img.rectTransform.sizeDelta.x;
            ratioH = img.rectTransform.sizeDelta.x * img.sprite.texture.height * 1.0f / img.sprite.texture.width;
        }

        img.rectTransform.sizeDelta = new Vector3(ratioW, ratioH);
    }

    void CheckVersion()
    {

    }

    void SaveCopyImage(string path, Sprite sprite)
    {
        Texture2D texture = null;
        if (sprite == null)
        {
            return;
        }
        else
        {
            texture = sprite.texture;
        }
        //string currentPath = JistUtil.instance.GetStreamingAssetsPath(path);
        string dirPath = Path.GetDirectoryName(path);
        //string fileName = Path.GetFileName(path);
        //string dirPathCustom = path.Replace(fileName, "");

        DebugScrollView.Instance.Print("save image : " + path);

        //print("dirPath : " + dirPath);
        //print("fileName : " + fileName);
        //print("dirPathCustom : " + dirPathCustom);
//#if !UNITY_ANDROID || UNITY_EDITOR
        try
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }catch(Exception e)
        {
            print(e.Message);
            print(e.StackTrace);
            print("e.InnerException.StackTrace" + e.InnerException.StackTrace);
        }
//#endif
        byte[] data = null;
        if (texture.name.Contains(".png"))
        {
            data = texture.EncodeToPNG();
        }
        else if(texture.name.Contains(".jpg"))
        {
            data = texture.EncodeToJPG();
        }
        if (data != null)
        {
            File.WriteAllBytes(path, data);
        }
    }

    /// <summary>
    /// 캡쳐
    /// </summary>
    /// <param name="path"></param>
    /// <param name="_texture"></param>
    public void SaveImageWithTexture(string path, Texture _texture)
    {
        Texture2D texture = null;
        if (_texture == null)
        {
            return;
        }
        else
        {
            string fileName = Path.GetFileName(path);
            texture = new Texture2D(_texture.width, _texture.height, TextureFormat.RGB24, false);


            var currentRT = RenderTexture.active;
            var rt = new RenderTexture(_texture.width, _texture.height, 24);

            // RawImageのTextureをRenderTextureにコピー
            Graphics.Blit(_texture, rt);
            RenderTexture.active = rt;

            // RenderTextureのピクセル情報をTexture2Dにコピー
            texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            texture.Apply();
            RenderTexture.active = currentRT;

            texture.name = fileName;

        }
        string dirPath = Path.GetDirectoryName(path);


        DebugScrollView.PrintEx("save image : " + path);

        //print("dirPath : " + dirPath);
        //print("fileName : " + fileName);
        //print("dirPathCustom : " + dirPathCustom);
        //#if !UNITY_ANDROID || UNITY_EDITOR
        try
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }
        catch (Exception e)
        {
            print(e.Message);
            print(e.StackTrace);
            print("e.InnerException.StackTrace" + e.InnerException.StackTrace);
        }
        //#endif
        byte[] data = null;
        if (texture.name.Contains(".png"))
        {
            data = texture.EncodeToPNG();
        }
        else if (texture.name.Contains(".jpg"))
        {
            data = texture.EncodeToJPG();
        }
        if (data != null)
        {
            File.WriteAllBytes(path, data);
        }
    }

    //public static Texture2D ToTexture2D(this Texture texture)
    //{
    //    return Texture2D.CreateExternalTexture(
    //        texture.width,
    //        texture.height,
    //        TextureFormat.RGB24,
    //        false, false,
    //        texture.GetNativeTexturePtr());
    //}

    void DeleteFile(string path)
    {
         File.Delete(path);
    }

    public void DeleteFiles(string directory, string ext)
    {
        string currentPath = JistUtil.GetPath(directory);
        try
        {
            string[] picList = Directory.GetFiles(currentPath, "*." + ext);
            
            foreach (string f in picList)
            {
                File.Delete(f);
            }
        }
        catch (DirectoryNotFoundException dirNotFound)
        {
            Console.WriteLine(dirNotFound.Message);
        }
    }


    public static Queue<Action> ActionQueue = new Queue<Action>();
    public static bool IsQueueActioning;
    public static float downloadstart;
    bool CoroutineTrigger = false;

    public void EnqueueAction(Action act)
    {
        ActionQueue.Enqueue(act);
        //if(ActionQueue.Count == 1 && CoroutineTrigger)
        //{
        //    StartCoroutine(UpdateQueue());
        //    CoroutineTrigger = false;
        //}
    }

    public void StartEnqueueUpdate()
    {
        StartCoroutine(UpdateQueue());
    }

    IEnumerator UpdateQueue()
    {
        while (true)
        {
            if(ActionQueue.Count > 0)
            {
                BusyWating.ins.ShowWithCount();
                if (!IsQueueActioning)
                {
                    downloadstart = Time.time;
                    //print("download start " + downloadstart);
                    Action act = ActionQueue.Dequeue();
                    act.Invoke();

                }
                
            }
            yield return null; // new WaitForSeconds(1.0f);
            //BusyWating.ins.HideWithCount();
            //CoroutineTrigger = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.M))
        //{
        //    LoadMapImage(MyTestImage, "https://naveropenapi.apigw.ntruss.com/map-static/v2/raster?w=600&h=600&center=126.8403392,33.31422242&level=10&format=png", null);
        //}
    }

    //asdzxcazcd
    public void LoadNaverMapImage(Image target, short width, short height, string lng, string lat, int level, string maptype = "basic")
    {

        string ImgURL = "https://naveropenapi.apigw.ntruss.com/map-static/v2/raster?w="+width+"&h="+height+"&center=" + lng + "," + lat + "&level=" + level + "&maptype="+maptype+"&format=png&scale=2";
        LoadMapImage(target, ImgURL, null);
    }
}
