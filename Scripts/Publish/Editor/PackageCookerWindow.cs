//#define UPDATE_DEFINE_SYMBOL

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class PackageCookerWindow : EditorWindow
{
	public static string assembliesFingerprint = "";

	public Publisher publisher = Publisher.None;
    //public BuildTargetGroup buildTargetGroup = BuildTargetGroup.Standalone;
    public BuildTarget buildTarget = BuildTarget.WebGL;
    public BuildConfiguration buildConfig = BuildConfiguration.DEV; //@fixme 시리얼라이징이 안되네...??
    public string extraDefineSymbols = "";
#if UNITY_ANDROID
    //public string bundleVersion = PlayerSettings.bundleVersion;
    //public int bundleCode = PlayerSettings.Android.bundleVersionCode;
    public string bundleVersion = "";
    public int bundleCode = 1;
#else
    public string bundleVersion = "";
    public int bundleCode = 1;
#endif

    public bool development = false;
    public bool allowScriptDebugging = false;
    public bool connectToProfiler = false;
    public bool showBuiltResult = false;
    public bool updateBuiltDate = true;
	public bool withLogging = false;
    public bool FPSShow = false;
    public bool onButton = false;
    public bool maintenance_live = false;
    public bool liveLabel = false;
    DateTime lastBuiltDate = new DateTime(2014,1,1);

	Vector2 sceneListScrollPos = Vector2.zero;

	private string GetOutputPackageName(string suffix)
    {
        //DateTime Now = DateTime.Now;
        // 빌드 패키지의 폴더이름과 빌드데이트를 매칭하기 위함
        DateTime Now = lastBuiltDate;
        
        // 프로젝트이름 대신 scene 이름 쓰기
        string name = PlayerSettings.productName;
        Scene currentScene = SceneManager.GetActiveScene();
        name = currentScene.name;

        switch (buildTarget)
		{
            case BuildTarget.Android:
                return string.Format("BuiltPackages/Android/{0}_{1:yy.MM.dd_HH.mm.ss}_v{2}({3}){4}.apk", name, Now, bundleVersion.Replace(".", "_"), bundleCode, suffix);
            case BuildTarget.iOS:
                return "/Users/hanbitsoft/Documents/UnityWorkspace/BuiltPackages/iOS/" + name;//"BuiltPackages/iOS/AM";
            case BuildTarget.WebGL:
                return string.Format("BuiltPackages/Web/{0}_{1:yy.MM.dd_HH.mm.ss}_v{2}{3}", name, Now, bundleVersion.Replace(".", "_"), suffix);
            case BuildTarget.StandaloneWindows:
                return string.Format("BuiltPackages/Win32/{0}_{1:yy.MM.dd_HH.mm.ss}_v{2}{3}/{0}.exe", name, Now, bundleVersion.Replace(".", "_"), suffix);
            case BuildTarget.StandaloneWindows64:
                return string.Format("BuiltPackages/Win64/{0}_{1:yy.MM.dd_HH.mm.ss}_v{2}{3}/{0}.exe", name, Now, bundleVersion.Replace(".", "_"), suffix);
            default:
                return string.Format("BuiltPackages/Win32/{0}_{1:yy.MM.dd_HH.mm.ss}_v{2}{3}/{0}.exe", name, Now, bundleVersion.Replace(".", "_"), suffix);
        }
    }

	private void UpdateBuiltInfo()
	{
        if (updateBuiltDate)
        {
            PlayerSettings.bundleVersion = bundleVersion;
#if UNITY_ANDROID
            PlayerSettings.Android.bundleVersionCode = bundleCode;
#endif
        }

        lastBuiltDate = DateTime.Now;

        var fs = new FileStream("Assets/Resources/BuiltInfo.txt", FileMode.Create);
        if (fs.CanWrite)
        {
            TextWriter w = new StreamWriter(fs);
            w.WriteLine(lastBuiltDate);
            w.WriteLine(bundleVersion);
			w.WriteLine(buildTarget.ToString());
            w.Close();
        }
        fs.Close();
        fs = null;

        AssetDatabase.Refresh();
	}

	private string GetBuildSummary()
	{
        string summary = "Please check below summary before building.\n\n";

		summary += string.Format("- Publisher: {0}\n", publisher);
		summary += string.Format("- BuildTarget: {0}\n", buildTarget);

        summary += string.Format("- Configuration: {0}\n", buildConfig);
        summary += string.Format("- BundleVersion: {0}\n", bundleVersion);
		if (buildTarget == BuildTarget.Android)
		{
			summary += string.Format("- BundleCode: {0}\n", bundleCode);
		}
        summary += string.Format("- DefineSymbols: {0}\n", GetFullDefineSymbols());
        summary += string.Format("- Development: {0}\n", development ? "Yes" : "No");
        summary += string.Format("- AllowScriptDebugging: {0}\n", allowScriptDebugging ? "Yes" : "No");
		summary += string.Format("- WithLogging: {0}\n", withLogging ? "Yes" : "No");
        summary += string.Format("- ConnectToProfiler: {0}\n", connectToProfiler ? "Yes" : "No");
        //summary += string.Format("- ShowBuiltResult: {0}\n", showBuiltResult ? "Yes" : "No");
        summary += string.Format("- UpdateBuiltDate: {0}\n", updateBuiltDate ? "Yes" : "No");
                
        if (updateBuiltDate)
        {
            summary += string.Format("- BuiltDate: {0:yy/MM/dd_HH:mm:ss} {1}\n", DateTime.Now, buildTarget.ToString());
        }
        //summary += string.Format("- TargetFolder: {0}\n", GetOutputPackageName(GetBuildConfigurationSuffix()));
        return summary;
	}

    private string GetBuildConfigurationDesc()
    {
        switch (buildConfig)
        {
        case BuildConfiguration.DEV:        return "내부 개발용";//return "Development";
        case BuildConfiguration.DEV_DEBUG:  return "내부 개발용(디버그)";//return "Development(Debug)";
        case BuildConfiguration.LIVE:       return "라이브 빌드";//return "Live";
        case BuildConfiguration.LIVE_DEBUG: return "라이브 빌드(디버그)";//return "Live(Debug)";
        default: throw new Exception();
        }
        return "";
    }

   private  string GetBuildConfigurationSuffix()
    {
        switch (buildConfig)
        {
        case BuildConfiguration.DEV:        return "_DEV";
        case BuildConfiguration.DEV_DEBUG:  return "_DEV_DEBUG";
        case BuildConfiguration.LIVE:       return "_LIVE";
        case BuildConfiguration.LIVE_DEBUG: return "_LIVE_DEBUG";
        default: throw new Exception();
        }
    }

    private string GetBaseDefineSymbols()
    {
        switch (buildConfig)
        {
        case BuildConfiguration.DEV:        return "BUILD_DEV;";
        case BuildConfiguration.DEV_DEBUG:  return "BUILD_DEV;DEBUG;";
        case BuildConfiguration.LIVE:       return "BUILD_LIVE;";
        case BuildConfiguration.LIVE_DEBUG: return "BUILD_LIVE;DEBUG;";
        default: throw new Exception();
        }
    }

	private string GetPublisherDefineSymbols()
    {
        switch (publisher)
        {
            case Publisher.None: return "PUBLISHER_NONE;";
            case Publisher.Hanbit: return "PUBLISHER_HANBIT;";
            case Publisher.Line: return "PUBLISHER_LINE;";
            case Publisher.Kakao: return "PUBLISHER_KAKAO;";
            case Publisher.Google: return "PUBLISHER_GOOGLE;";
            case Publisher.Rokwon: return "PUBLISHER_ROKWON;";
            default: throw new Exception();
        }
	}

    private string GetFullDefineSymbols()
    {
#if !UPDATE_DEFINE_SYMBOL
        return PlayerSettings.GetScriptingDefineSymbolsForGroup(UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup);
#endif
        string defineSymbols = GetPublisherDefineSymbols();

		defineSymbols += GetBaseDefineSymbols();

        if (!string.IsNullOrEmpty(extraDefineSymbols))
        {
            defineSymbols += ";";
            defineSymbols += extraDefineSymbols;
        }

		if (withLogging)
		{
			defineSymbols += "WITH_LOGGING;";
		}
        if (onButton)
        {
            defineSymbols += "ON_BUTTONS;";
        }
        if (maintenance_live)
        {
            defineSymbols += "MAINTENANCE_LIVE;";
        }
        if(liveLabel)
        {
            defineSymbols += "LIVE_LABEL;";
        }
        defineSymbols = defineSymbols.Replace(";;", ";");

        if (!defineSymbols.EndsWith(";"))
        {
            defineSymbols += ";";
        }

        return defineSymbols;
    }

	private BuildTargetGroup BuildTargetGroupFromBuildTarget(BuildTarget value)
	{
		switch (value)
		{
        case BuildTarget.StandaloneOSX: return BuildTargetGroup.Standalone;
        case BuildTarget.StandaloneOSXIntel: return BuildTargetGroup.Standalone;
        case BuildTarget.StandaloneWindows: return BuildTargetGroup.Standalone;
        case BuildTarget.WebGL: return BuildTargetGroup.WebGL;
 //       case BuildTarget.WebPlayerStreamed: return BuildTargetGroup.WebPlayer;
        case BuildTarget.iOS: return BuildTargetGroup.iOS;
        case BuildTarget.PS3: return BuildTargetGroup.PS3;
        case BuildTarget.XBOX360: return BuildTargetGroup.XBOX360;
        case BuildTarget.Android: return BuildTargetGroup.Android;
 //       case BuildTarget.StandaloneGLESEmu: return BuildTargetGroup.GLESEmu;
 //       case BuildTarget.NaCl: return BuildTargetGroup.NaCl;
        case BuildTarget.StandaloneLinux: return BuildTargetGroup.Standalone;
 //       case BuildTarget.FlashPlayer: return BuildTargetGroup.FlashPlayer;
        case BuildTarget.StandaloneWindows64: return BuildTargetGroup.Standalone;
        case BuildTarget.WSAPlayer: return BuildTargetGroup.Metro;
        case BuildTarget.StandaloneLinux64: return BuildTargetGroup.Standalone;
        case BuildTarget.StandaloneLinuxUniversal: return BuildTargetGroup.Standalone;
        case BuildTarget.WP8Player: return BuildTargetGroup.Metro;
        case BuildTarget.StandaloneOSXIntel64: return BuildTargetGroup.Standalone;
        case BuildTarget.BlackBerry: return BuildTargetGroup.BlackBerry;
        //case BuildTarget.[Obsolete("Use BlackBerry instead")]
        //case BuildTarget.BB10:
        case BuildTarget.Tizen: return BuildTargetGroup.Tizen;
        case BuildTarget.PSP2: return BuildTargetGroup.PSP2;
        case BuildTarget.PS4: return BuildTargetGroup.PS4;
        case BuildTarget.PSM: return BuildTargetGroup.PSM;
        case BuildTarget.XboxOne: return BuildTargetGroup.XboxOne;
        case BuildTarget.SamsungTV: return BuildTargetGroup.SamsungTV;
		default: throw new Exception();
		}
	}

    private void BuildPackage(string apkSuffix, bool buildAndRun)
    {
        Util.EnsureTargetPathMustExists("BuiltPackages/Android/");
        Util.EnsureTargetPathMustExists("BuiltPackages/iOS/");
        Util.EnsureTargetPathMustExists("BuiltPackages/Web/");
        Util.EnsureTargetPathMustExists("BuiltPackages/Win32/");

        UpdateBuiltInfo();

		BuildTargetGroup buildTargetGroup = BuildTargetGroupFromBuildTarget(buildTarget);

        //주의:
        // 안드로이드의 경우, keyalias/keystore 비밀번호를 지정해야함.
        // 현재는 키를 발급 받은 상황이 아니므로, 차후에는 키를 발급 받고 비밀번호를
        // 정상적으로 부여해야함.
        // 
        if (buildTarget == BuildTarget.Android)
        {
            PlayerSettings.Android.keyaliasPass = "1234567895";
            PlayerSettings.Android.keystorePass = "1234567895";
        }

#if UPDATE_DEFINE_SYMBOL
        string defineSymbols = GetFullDefineSymbols();
        if (!string.IsNullOrEmpty(defineSymbols))
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defineSymbols);
        }
#endif

        EditorUserBuildSettings.development = development;
        EditorUserBuildSettings.allowDebugging = allowScriptDebugging;

        BuildOptions buildOptions = BuildOptions.None;
        if (development)
        {
            buildOptions |= BuildOptions.Development;
        }

        if (allowScriptDebugging)
        {
            buildOptions |= BuildOptions.AllowDebugging;
        }

        if (connectToProfiler)
        {
            buildOptions |= BuildOptions.ConnectWithProfiler;
        }

        if (showBuiltResult)
        {
            buildOptions |= BuildOptions.ShowBuiltPlayer;
        }

        if (buildAndRun)
        {
            buildOptions |= BuildOptions.AutoRunPlayer;
        }


        //EditorUserBuildSettings.SwitchActiveBuildTarget(buildTarget);
        EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup, buildTarget);
        BuildPipeline.BuildPlayer(SceneList, GetOutputPackageName(apkSuffix), buildTarget, buildOptions);
#if UPDATE_DEFINE_SYMBOL
        // Clear define symbols
        if (!string.IsNullOrEmpty(defineSymbols))
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, "");
        }
#endif
		if (buildTarget == BuildTarget.Android)
		{
			WriteApkMarker(GetOutputPackageName(apkSuffix)); //@warning android only!!
		}
    }

    public void WriteApkMarker(string filename)
    {
        // Calculate checksum
        /*
        string checksum = "";
        try
        {
            FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(file);
            file.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            checksum = sb.ToString();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }


        // Save checksum and extra
        filename = filename.Replace(".apk", "_TAG.txt");

        FileStream fs = new FileStream(filename, FileMode.Create);
        if (fs.CanWrite)
        {
            TextWriter w = new StreamWriter(fs);
            w.Write(string.Format("{0}|{1}|{2}|{3}", bundleVersion, lastBuiltDate.Ticks, lastBuiltDate, checksum));
            w.Close();
        }
        fs.Close();
        fs = null;
        */

        /*
        string assemblySignatures = AHSinger.GetAssemblySignatures();

        filename = filename.Replace(".apk", "_FINGERPRINT.txt");

        FileStream fs = new FileStream(filename, FileMode.Create);
        if (fs.CanWrite)
        {
            TextWriter w = new StreamWriter(fs);
            w.Write(assemblySignatures);
            w.Close();
        }
        fs.Close();
        fs = null;
        */
    }

	private static string[] SceneList
	{
		get
		{
			List<string> sceneList = new List<string>();
			foreach (var s in EditorBuildSettings.scenes)
			 {
				 if (s.enabled)
				 {
					 //string name = s.path.Substring(S.path.LastIndexOf('/')+1);
					 //name = name.Substring(0,name.Length-6);
					 //temp.Add(name);
					sceneList.Add(s.path);
				 }
			 }
			 return sceneList.ToArray();
		}
	}

    private void OnGUI()
    {
        GUILayout.Space(5);

        EditorUtil.InfoBoxWithButtons("Configuration", EditorUtil.Severity.Warning);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Publisher", GUILayout.Width(100));
        EditorUtil.PushGUIBackgroundColor(Color.red);
        var publisherNames = System.Enum.GetNames(typeof(Publisher));
        var publisherValues = (int[])System.Enum.GetValues(typeof(Publisher));
        publisher = (Publisher)EditorGUILayout.IntPopup((int)publisher, publisherNames, publisherValues, GUILayout.Width(260));
        EditorUtil.PopGUIBackgroundColor();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("BuildTarget", GUILayout.Width(100));
        EditorUtil.PushGUIBackgroundColor(Color.green);
        var buildTargetNames = System.Enum.GetNames(typeof(BuildTarget));
        var buildTargetValues = (int[])System.Enum.GetValues(typeof(BuildTarget));
        buildTarget = (BuildTarget)EditorGUILayout.IntPopup((int)buildTarget, buildTargetNames, buildTargetValues, GUILayout.Width(260));
        EditorUtil.PopGUIBackgroundColor();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Configuration", GUILayout.Width(100));
        EditorUtil.PushGUIBackgroundColor(Color.cyan);
        var buildConfigNames = System.Enum.GetNames(typeof(BuildConfiguration));
        var buildConfigValues = (int[])System.Enum.GetValues(typeof(BuildConfiguration));
        buildConfig = (BuildConfiguration)EditorGUILayout.IntPopup((int)buildConfig, buildConfigNames, buildConfigValues, GUILayout.Width(260));
        EditorUtil.PopGUIBackgroundColor();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
		GUILayout.Label("", GUILayout.Width(100));
        GUILayout.Label(GetBuildConfigurationDesc());
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        updateBuiltDate = GUILayout.Toggle(updateBuiltDate, "Update Built Date");
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        EditorUtil.InfoBoxWithButtons("Version", EditorUtil.Severity.Warning);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Protocol", GUILayout.Width(100));
        GUILayout.Label("???", GUILayout.Width(100)); // 네트워크 프로토콜
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Version", GUILayout.Width(100));
        if (updateBuiltDate)
        {
            bundleVersion = EditorGUILayout.TextField(bundleVersion, GUILayout.Width(100));
        }
        else
        {
            GUILayout.Label(bundleVersion.ToString(), GUILayout.Width(100));
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
		if (buildTarget == BuildTarget.Android)
		{
			GUILayout.Label("VersionCode", GUILayout.Width(100));
			if (updateBuiltDate)
			{
				bundleCode = EditorGUILayout.IntField(bundleCode, GUILayout.Width(100));
			}
			else
			{
				GUILayout.Label(bundleCode.ToString(), GUILayout.Width(100));
			}
			GUILayout.Label(" * Google Play Store Only");
		}
        GUILayout.EndHorizontal();

        //GUILayout.Space(10);

        //EditorUtil.InfoBoxWithButtons("Define Symbols", EditorUtil.Severity.Warning);
        //GUILayout.BeginHorizontal();
        //GUILayout.Label("Symbols", GUILayout.Width(100));
        //GUILayout.Label(GetFullDefineSymbols());
        //GUILayout.EndHorizontal();
        //GUILayout.BeginHorizontal();
        //GUILayout.Label("Extra Symbols", GUILayout.Width(100));
        //extraDefineSymbols = GUILayout.TextField(extraDefineSymbols);
        //GUILayout.EndHorizontal();

        //GUILayout.Space(10);

        //EditorUtil.InfoBoxWithButtons("Build Settings", EditorUtil.Severity.Warning);
        //GUILayout.BeginHorizontal();        
        //development = GUILayout.Toggle(development, "Development");
        //GUILayout.Space(178f);
        //onButton = GUILayout.Toggle(onButton, "On_Buttons(emailLogin,Korean,Logout)");        
        //GUILayout.EndHorizontal();

        //GUILayout.BeginHorizontal();
        //allowScriptDebugging = GUILayout.Toggle(allowScriptDebugging, "Allow Script Debugging");
        //GUILayout.Space(178f);
        //maintenance_live = GUILayout.Toggle(maintenance_live, "Maintenance_Live(Check :Live, not Check :test)");       
        //GUILayout.EndHorizontal();

        //GUILayout.BeginHorizontal();
        //withLogging = GUILayout.Toggle(withLogging, "With Logging");
        //GUILayout.Space(250f);
        //liveLabel = GUILayout.Toggle(liveLabel, "LiveLabel(Check : show Live,not Check : show QA)");       
        //GUILayout.EndHorizontal();

        //GUILayout.BeginHorizontal();
        //connectToProfiler = GUILayout.Toggle(connectToProfiler, "Connect To Profiler");
        //GUILayout.EndHorizontal();

        //GUILayout.BeginHorizontal();
        //showBuiltResult = GUILayout.Toggle(showBuiltResult, "Show Built Package");
        //GUILayout.EndHorizontal();

        //GUILayout.BeginHorizontal();
        //FPSShow = GUILayout.Toggle(FPSShow, "Show FPS Package");
        //GUILayout.EndHorizontal();

        GUILayout.Space(10);

        // Scene list
        var scenes = SceneList;
        EditorUtil.InfoBoxWithButtons(string.Format("Scene List(include {0} scenes.)", scenes.Length), EditorUtil.Severity.Warning);
		sceneListScrollPos = GUILayout.BeginScrollView(sceneListScrollPos);
        for (int i = 0; i < scenes.Length; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(string.Format("{0:d2} : {1}", i+1, scenes[i]));
            GUILayout.EndHorizontal();
        }
		GUILayout.EndScrollView();

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        bool build = false;
        bool run = false;

        EditorUtil.PushGUIBackgroundColor(Color.red);
        if (GUILayout.Button("Build", GUILayout.Width(120)))
        {
            if (EditorUtility.DisplayDialog("Build a package?\n\nWarning: May be takes for few minutes.\n\n", GetBuildSummary(), "Build Now!", "Cancel"))
            {
                //BuildPackage(GetBuildConfigurationSuffix(), false);
                build = true;
                run = false;
            }
        }
        EditorUtil.PopGUIBackgroundColor();

        EditorUtil.PushGUIBackgroundColor(Color.magenta);
		if (buildTarget == BuildTarget.Android ||
			buildTarget == BuildTarget.StandaloneWindows || buildTarget == BuildTarget.StandaloneWindows64 ||
            buildTarget == BuildTarget.WebGL)
		{
			if (GUILayout.Button("Build and Run", GUILayout.Width(120)))
			{
				if (EditorUtility.DisplayDialog("Build and Run a package?\n\nWarning: May be takes for few minutes.\n\n", GetBuildSummary(), "Build and Run Now!", "Cancel"))
				{
					//BuildPackage(GetBuildConfigurationSuffix(), true);
					build = true;
					run = true;
				}
			}
		}
        EditorUtil.PopGUIBackgroundColor();

        if (GUILayout.Button("Close", GUILayout.Width(60)))
        {
            Close();
        }

        GUILayout.EndHorizontal();

		GUILayout.Space(10);

        if (build)
        {
            //if(FPSShow)
            //{
            //    SceneManager.Instance.HUDFPSObj.SetActive(true);
            //}
            //else
            //{
            //    SceneManager.Instance.HUDFPSObj.SetActive(false);
            //}
            BuildPackage(GetBuildConfigurationSuffix(), run);
        }
    }

    private void OnInspectorUpdate()
    {
        Repaint();
    }

	[MenuItem("Tools/Publish/Package Cooker")]
	static void OpenWindow()
	{
		var win = GetWindow<PackageCookerWindow>();
        win.title = "Package Cooker";
		win.Show();
	}

    [MenuItem("Tools/Publish/Package Cooker Close")]
    static void CloseWindow()
    {
        var win = GetWindow<PackageCookerWindow>();
        win.Close();
    }

}
