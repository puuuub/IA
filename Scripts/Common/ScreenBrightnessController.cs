using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class ScreenBrightnessController : SingletonClass<ScreenBrightnessController>
{
    public Button add;
    public Button reduce;
    public Text text;
    public float brightness;
    float BaseBrightness;


    //void Start()
    //{

    //    brightness = 0;
    //    add.onClick.AddListener(ADD);
    //    reduce.onClick.AddListener(Reduce);

    //}

    //void ADD()
    //{
    //    brightness += 0.1f;

    //    if (brightness >= 1)
    //    {
    //        brightness = 1;
    //    }
    //    SetApplicationBrightnessTo(brightness);
    //    text.text = brightness.ToString();
    //}

    //void Reduce()
    //{
    //    brightness -= 0.1f;

    //    if (brightness <= 0)
    //    {
    //        brightness = 0;
    //    }
    //    SetApplicationBrightnessTo(brightness);
    //    text.text = brightness.ToString();
    //}



    public void InitBaseBrightness()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaObject Activity = null;
        Activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        Activity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
            AndroidJavaObject Window = null, Attributes = null;
            Window = Activity.Call<AndroidJavaObject>("getWindow");
            Attributes = Window.Call<AndroidJavaObject>("getAttributes");
            BaseBrightness = Attributes.Get<float>("screenBrightness");
            Window.Call("setAttributes", Attributes);
        }));
        #endif
    }

    public void SetBaseBrightness()
    {
        SetApplicationBrightnessTo(BaseBrightness);
    }

    public void SetApplicationBrightnessTo(float Brightness)
    {
  #if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaObject Activity = null;
        Activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        Activity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
            AndroidJavaObject Window = null, Attributes = null;
            Window = Activity.Call<AndroidJavaObject>("getWindow");
            Attributes = Window.Call<AndroidJavaObject>("getAttributes");
            Attributes.Set("screenBrightness", Brightness);
            Window.Call("setAttributes", Attributes);
        }));
  #endif
    }

}
