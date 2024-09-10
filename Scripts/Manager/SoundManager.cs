using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class SoundManager : SingletonMonoBehaviour <SoundManager>
{
    public enum EFFECT
    {
        SOS,
        HEART_DAGER,
        OVER_AREA,
        SPO2_DANGER
    }

    public enum BGM
    {
        START,
    }

    AudioSource MyAudioSource;
    AudioSource MyBGMAudioSource;

   
    public List<AudioSource> MyAudioSourceList;

    int soundIndex = 0;

    [SerializeField]
    List<AudioClip> Effects;
    [SerializeField]
    List<AudioClip> BGMs;


    public System.Action<string> OnPrintDebugMsg { get; set; }

    private void Awake()
    {
        MyAudioSource = GetComponent<AudioSource>();
        GameObject audioSrcRoot = CommonUtility.FindChildObject("AudioScrRoot", transform);
        audioSrcRoot.GetComponentsInChildren<AudioSource>(MyAudioSourceList);

        LoadAudioSource();
    }


    void LoadAudioSource()
    {
        Effects = new List<AudioClip>();

        StartCoroutine(LoadAudioClip(0, Application.streamingAssetsPath + "/" + "Sound/SOS.MP3", AudioType.MPEG));
        //StartCoroutine(LoadAudioClip(1, Application.streamingAssetsPath + "/" + "Sound/watch_out.wav", AudioType.WAV));
        //StartCoroutine(LoadAudioClip(2, Application.streamingAssetsPath + "/" + "Sound/be_careful.wav", AudioType.WAV));

        //foreach(var ac in Effects.Select((value, index)=> new { value, index }))
        //{
        //    MyAudioSourceList[ac.index].clip = ac.value;
        //}
    }

    public void PlaySE(EFFECT index)
    {
        MyAudioSource.clip = Effects[(int)index];
        MyAudioSource.Play();
    }

    public void Stop()
    {
        MyAudioSource.Stop();
    }

    public void PlaySEInList(EFFECT index)
    {
        MyAudioSourceList[((int)index)].Play();
    }

    public void Stop(EFFECT index)
    {
        MyAudioSourceList[((int)index)].Stop();
    }

    public void PlayBGM(BGM index)
    {
        MyAudioSource.clip = BGMs[(int)index];
        MyAudioSource.Play();
    }

    IEnumerator LoadAudioClip(int index, string url, AudioType type)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, type))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // 파일크기에 따라 로딩시간이 달라져 인덱스순서 꼬인는 상황 대비
                while (index > Effects.Count)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                myClip.name = url;
                Effects.Insert(index, myClip);

                MyAudioSourceList[index].clip = myClip;
                DebugScrollView.PrintEx(index +" " + myClip.name + " loaded");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //PlayBGM(BGM.START);
    }


    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    PlaySE((EFFECT)(soundIndex++ % 3));
        //}

        //if (Input.GetMouseButtonDown(1))
        //{
        //    PlaySE(EFFECT.NO_RUN);
        //}
    }
}
