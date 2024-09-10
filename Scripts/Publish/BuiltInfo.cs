/*
Samsung Galaxy S4 LTE-A:

    SystemInfo.deviceModel         : samsung SHV-E330S
    SystemInfo.deviceName          : <unknown>
    SystemInfo.deviceType          : Handheld
    SystemInfo.graphicsDeviceName  : Adreno <TM> 330
    SystemInfo.graphicsDeviceVendor: Qualcomm
    SystemInfo.graphicsMemorySize  : 188
    SystemInfo.graphicsSahderLevel : 30
    SystemInfo.npotSupport         : Full
    SystemInfo.operatingSystem     : Android OS 4.2.2 / API-17 <JDQ39/E330SKSUAMI1>
    SystemInfo.processorCount      : 4
    SystemInfo.processorType       : ARMv7 VFPv3 NEON
    SystemInfo.systemMemorySize    : 1860
*/
using UnityEngine;
using UnityEditor;
using System;
using System.Text;
using System.IO;

public static class BuiltInfo
{
#if STANDALONE
    public static bool hasExpireDate = false; //@note 기간제한 해제!
    public static int validHours = 24 * 7 * 4;
#else
    public static bool hasExpireDate = false;
    public static int validHours = 0;
#endif

    private static DateTime cachedBuiltDate = new DateTime(0);
    private static string cachedAppVersion = "";
	private static string cachedPlatform = "";

    public static DateTime BuiltDate { get { Init(); return cachedBuiltDate; } }
    public static string AppVersion { get { Init(); return cachedAppVersion; } }
	public static string Platform { get { Init(); return cachedPlatform; } }

    public static DateTime ExpiredDate { get { return BuiltDate.AddHours(validHours); } }
    public static TimeSpan TimeToExpire { get { return ExpiredDate - DateTime.Now; } }
    public static TimeSpan TimeSinceBuilt { get { return DateTime.Now - BuiltDate; } }
    public static bool IsExpired { get { return hasExpireDate && (TimeToExpire.Ticks < 0 || TimeToExpire.Hours > validHours); } }

    static void Init()
    {
#if UNITY_EDITOR
		if (cachedPlatform == "")
		{
			cachedBuiltDate = DateTime.Now;
            cachedAppVersion = "InEditor";
			cachedPlatform = "standalonewindows" + " for " + EditorUserBuildSettings.selectedBuildTargetGroup;
		}
#else
        if (cachedBuiltDate.Ticks == 0)
        {
			// "Resources/BuiltInfo.txt" 파일로 있어야함.
			// 패키지 빌드시에, 임의로 넣어줌.
            var ta = Resources.Load("BuiltInfo", typeof(TextAsset)) as TextAsset;

            string buildDateStr = "";
            string versionStr = "";
			string platformStr = "";

            try
            {
                TextReader reader = new StringReader(ta.text);
                buildDateStr = reader.ReadLine().Trim();
                versionStr = reader.ReadLine().Trim();
				platformStr = reader.ReadLine().Trim();
                reader.Close();

                cachedBuiltDate = DateTime.Parse(buildDateStr);
                cachedAppVersion = versionStr;
				cachedPlatform = platformStr;
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("BuiltInfo initializing was failed : {0}", ex));
                Application.Quit();
            }
        }
#endif
    }

    public static string BuildTypeName
    {
        get
        {
#if BUILD_LIVE
            return "live"; //라이브
#else
            return "dev";  //내부 개발용(기본)
#endif
        }
    }

    public static void PrintBuildType()
    {
        string log = "";

        //log += "DeviceModel : " + SystemInfo.deviceModel;

#if !BUILD_LIVE
        log += string.Format("[{0}]", BuildTypeName.ToUpper());
#else
        log += string.Format("\n----------- BUILD_DEV -----------\n\nVERSION : {0}\nDATE    : {1}", BuiltInfo.AppVersion, BuiltInfo.BuiltDate);
#endif

#if DEBUG
        log += "\n[DEBUG] Build";
#endif

        if (log.Length > 0)
        {
            Debug.Log(log);
        }
    }
}
