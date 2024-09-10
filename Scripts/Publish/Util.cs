using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions; //regex
using System.IO;
using System.Text;

#if UNITY_EDITOR || DEBUG
using System.Xml.Serialization;
#endif

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class Util
{
#if UNITY_EDITOR || DEBUG
    public static string SerializeAsXMLString(object o)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(o.GetType());
        StringWriter textWriter = new StringWriter();

        xmlSerializer.Serialize(textWriter, o);
        string str = textWriter.ToString();

        // Strip out xml header parts
        str = str.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
        str = str.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
        return str;
    }
#else
    public static string SerializeAsXMLString(object o)
    {
        return "";
    }
#endif

    /**
    Returns true given two Rects are intersected each.
    */
    public static bool IntersectsRect(Rect rectA, Rect rectB)
    {
        int bl_x = (int)Mathf.Min(rectA.x + rectA.width  - 1, rectB.x + rectB.width  - 1);
        int bl_y = (int)Mathf.Min(rectA.y + rectA.height - 1, rectB.y + rectB.height - 1);

        int x = (int)Mathf.Max(rectA.x, rectB.x);
        int y = (int)Mathf.Max(rectA.y, rectB.y);
        int w = bl_x - x + 1;
        int h = bl_y - y + 1;

        return w > 0 && h > 0; // True if has intersection area
    }

    /**
    Returns true if rectB is fully contained by rectA.
    */
    public static bool ContainsRect(Rect rectA, Rect rectB)
    {
        return (rectA.x <= rectB.x && rectA.y <= rectB.y)
            && (rectB.x + rectB.width  <= rectA.x + rectA.width)
            && (rectB.y + rectB.height <= rectA.y + rectA.height);
    }

    private static System.Random random = new System.Random();

    public static void SetRandomSeed(int seed)
    {
        random = new System.Random(seed);
    }

    /**
    Shuffle generic list
    */
    public static void Shuffle<T>(ref List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /**
    Shuffle static array
    */
    public static void Shuffle<T>(ref T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
    }


    // Broadcasting

    public static void Broadcast(string methodName)
    {
        GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        for (int i = 0, n = gos.Length; i < n; ++i)
        {
            gos[i].SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
        }
    }

    public static void Broadcast(string methodName, object param)
    {
        GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        for (int i = 0, n = gos.Length; i < n; ++i)
        {
            gos[i].SendMessage(methodName, param, SendMessageOptions.DontRequireReceiver);
        }
    }


    // GameObject

    public static GameObject GetObject(string path, GameObject baseObj = null, bool ensureExists = false)
    {
        GameObject leaf = null;

        while (path != "")
        {
            string name = "";

            int psep = path.IndexOf("/");
            if (psep == -1)
            {
                psep = path.IndexOf("."); // Separated "." (dot)
            }

            if (psep != -1)
            {
                name = path.Substring(0, psep);
                path = path.Substring(psep + 1);
            }
            else
            {
                name = path;
                path = "";
            }

            if (name != "")
            {
                if (baseObj == null)
                {
                    leaf = GameObject.Find(name);
                    if (leaf == null)
                    {
                        if (ensureExists)
                        {
                            leaf = new GameObject(name);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    Transform t = baseObj.transform.Find(name);
                    if (t != null)
                    {
                        leaf = t.gameObject;
                    }
                    else
                    {
                        if (ensureExists)
                        {
                            leaf = new GameObject(name);
                            leaf.transform.parent = baseObj.transform;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }

                baseObj = leaf;
            }
        }

        return leaf;
    }

    public static string GetPathName(GameObject obj, string psep = "/")
    {
        string pathName = obj != null ? obj.name : "";

        while (obj.transform.parent != null)
        {
            GameObject parent = obj.transform.parent.gameObject;
            pathName = parent.name + psep + pathName;
            obj = parent;
        }
        return pathName;
    }

    public static T GetComponentOfChildChecked<T>(string childName, GameObject parent) where T : Component
    {
        if (parent != null)
        {
            Transform t = parent.transform.Find(childName);
            //////System.Diagnostics.Debug.Assert(t != null);

            T result = t.gameObject.GetComponent<T>();
            //////System.Diagnostics.Debug.Assert(result != null);
            return result;
        }
        else
        {
            GameObject child = GameObject.Find(childName);
            //////System.Diagnostics.Debug.Assert(child != null);

            T result = child.GetComponent<T>();
            //////System.Diagnostics.Debug.Assert(result != null);
            return result;
        }
    }

    public static void SweepGarbages()
    {
        // Collect garbages
        System.GC.Collect();
        System.GC.WaitForPendingFinalizers();

        // Unload all unreferenced assets
        Resources.UnloadUnusedAssets();

		//DLCAssets.SweepNullifieds();
    }


    static Vector2 cachedViewSize = Vector2.zero;

    public static Vector2 ViewSize
    {
        get { return GetViewSize(); }
    }

    public static Vector2 GetViewSize()
    {
        if (cachedViewSize.x == 0f)
        {
            cachedViewSize = GetViewSize(800, 480);
        }

        return cachedViewSize;
    }

    public static Vector2 GetViewSize(int nativeSizeX, int nativeSizeY)
    {
        float currentSizeX = (float)Screen.width;
        float currentSizeY = (float)Screen.height;

        float nativeAspect = (float)nativeSizeX / (float)nativeSizeY;
        float currentAspect = currentSizeX / currentSizeY;

        float s;
        if (currentAspect < nativeAspect)
        {
            s = (float)Screen.width / nativeSizeX;
        }
        else
        {
            s = (float)Screen.height / nativeSizeY;
        }

        float oneOverS = 1f / s;

        Vector3 result = new Vector2(currentSizeX * oneOverS, currentSizeY * oneOverS);
        return result;
    }


    public static void OpenURL(string url)
    {
        Application.OpenURL(url);
    }

    public static void SetChildrenEnabled<T>(Component component, bool enabled, bool includeSelf = true) where T : Component
    {
        if (component != null)
        {
            SetChildrenEnabled<T>(component.gameObject, enabled, includeSelf);
        }
    }

    public static void SetChildrenEnabled<T>(GameObject go, bool enabled, bool includeSelf = true) where T : Component
    {
        if (go == null)
        {
            return;
        }

        Component[] components = go.GetComponentsInChildren<T>(true); //include inactive
        if (components != null)
        {
            for (int i = 0, n = components.Length; i < n; i++)
            {
                components[i].gameObject.SetActive(enabled);
            }
        }

        if (includeSelf)
        {
            components = go.GetComponents<T>();
            if (components != null)
            {
                for (int i = 0, n = components.Length; i < n; i++)
                {
                    components[i].gameObject.SetActive(enabled);
                }
            }
        }
    }

#if UNITY_EDITOR
    //@note LoadAllAssetsAtPath가 작동을 안해서.. 쩝..
    public static T[] LoadAllAssetsAtPath<T>(string path) where T : Object
    {
        List<T> assetList = new List<T>();

        // Normalize path
        if (path.StartsWith("/"))
        {
            path = path.Substring(1);
        }

        if (path.EndsWith("/"))
        {
            path = path.Substring(0, path.Length - 1);
        }

        string fileSysPath = path;
        fileSysPath = fileSysPath.Replace("Assets/", "");

        string [] fileList = Directory.GetFiles(Application.dataPath + "/" + fileSysPath);
        foreach (string filename in fileList)
        {
            int psep = filename.LastIndexOf("\\"); //@warning Must be first!!
            if (psep < 0)
            {
                psep = filename.LastIndexOf("/");
            }

            string fn = filename;
            if (psep >= 0)
            {
                fn = fn.Substring(psep + 1);
            }

            string assetPath = path + "/" + fn;
            T asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)) as T;
            if(asset != null)
            {
                assetList.Add(asset);
            }
        }

        return assetList.ToArray();
    }

    public static string GetFileTimestamp(string path)
    {
        if (System.IO.File.Exists(path))
        {
#if TARGET_SAMSUNG_SMART_TV || UNITY_WEBPLAYER
            return "";
#else
            return (System.IO.File.GetLastWriteTime(path) - new System.DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds.ToString();
#endif
        }
        else
        {
            return "";
        }
    }

    public static string[] ReadFileList(string filePattern)
    {
        var result = new List<string>();

        var paths = new Stack<string>();
        paths.Push(Application.dataPath);
        while (paths.Count != 0)
        {
            var path = paths.Pop();
            var files = Directory.GetFiles(path, filePattern);
            foreach (var file in files)
            {
                result.Add(file.Substring(Application.dataPath.Length - 6));
            }

            foreach (var subdirs in Directory.GetDirectories(path))
            {
                paths.Push(subdirs);
            }
        }

        return result.ToArray();
    }
#endif

	public static bool EnsureTargetPathMustExists(string pathOrFullpath)
	{
		pathOrFullpath = pathOrFullpath.Replace("\\", "/");

		int psep = pathOrFullpath.LastIndexOf('/');
		if (psep == -1)
		{
			psep = pathOrFullpath.LastIndexOf('\\');
		}

		try
		{
			if (psep != -1)
			{
				string targetPath = pathOrFullpath.Substring(0, psep);
				System.IO.Directory.CreateDirectory(targetPath);
			}
			else
			{
				System.IO.Directory.CreateDirectory(pathOrFullpath);
			}
			return true;
		}
		catch (System.Exception ex)
		{
			Debug.LogWarning(ex);
			return false;
		}
	}

	public static bool ReadBytesFromFile(string path, out byte[] outData)
	{
		path = path.Replace("\\", "/");

		try
		{
			var fi = new FileInfo(path);
			outData = new byte[fi.Length];

			var fs = new FileStream(path, FileMode.Open);
			fs.Read(outData, 0, outData.Length);
			fs.Close();
			fs = null;

			return true;
		}
		catch (System.Exception ex)
		{
			//Debug.LogWarning(ex);

			outData = null;
			return false;
		}
	}

	public static bool WriteBytesToFile(string path, byte[] data)
	{
		path = path.Replace("\\", "/");

		try
		{
			EnsureTargetPathMustExists(path);

			var fs = new FileStream(path, FileMode.Create);
			fs.Write(data, 0, data.Length);
			fs.Close();
			fs = null;

			return true;
		}
		catch (System.Exception ex)
		{
			Debug.LogWarning(ex);
			return false;
		}
	}

    public static GameObject FindGameObject(string namePath, bool disableByDefault = true)
    {
        var result = GameObject.Find(namePath);
        //////System.Diagnostics.Debug.Assert(result != null);

        if (disableByDefault)
        {
            result.SetActive(false);
        }

        return result;
    }

    public static void SafeSetActive(GameObject go, bool active)
    {
        if (go != null)
        {
            go.SetActive(active);
        }
    }

    public static bool WildcardMatch(string s, string wildcard, bool casesensitive = false)
    {
        // Replace the * with an .* and the ? with a dot. Put ^ at the
        // beginning and a $ at the end
        string pattern = "^" + Regex.Escape(wildcard).Replace(@"\*", ".*").Replace(@"\?", ".") + "$";

        // Now, run the Regex as you already know
        Regex regex;
        if (casesensitive)
        {
            regex = new Regex(pattern);
        }
        else
        {
            regex = new Regex(pattern, RegexOptions.IgnoreCase);
        }

        return regex.IsMatch(s);
    }

    static public string ReplaceFileExt(string filename, string newFileExt)
    {
        int dot = filename.LastIndexOf(".");
        if (dot != -1)
        {
            filename = filename.Substring(0, dot);
        }

        if (!newFileExt.StartsWith("."))
        {
            filename += ".";
        }

        filename += newFileExt;

        return filename;
    }

    static public string StripPath(string path)
    {
        string filename = path;
        int psep = filename.LastIndexOf("/");
        if (psep == -1)
        {
            psep = filename.LastIndexOf("\\");
        }
        if (psep != -1)
        {
            filename = filename.Substring(psep + 1);
        }
        return filename;
    }

	static public string StripExt(string path)
	{
		int dot = path.LastIndexOf(".");
		if (dot >= 0)
		{
			return path.Substring(0, dot);
		}
		else
		{
			return path;
		}
	}

    static public string GetPureFilename(string path)
    {
        string filename = path;
        int psep = filename.LastIndexOf("/");
        if (psep == -1)
        {
            psep = filename.LastIndexOf("\\");
        }
        if (psep != -1)
        {
            filename = filename.Substring(psep + 1);
        }

        int dot = filename.LastIndexOf(".");
        if (dot != -1)
        {
            filename = filename.Substring(0, dot);
        }

        return filename;
    }

    static public string GetFolder(string path)
    {
        string folder = path;
        int psep = folder.LastIndexOf("/");
        if (psep == -1)
        {
            psep = folder.LastIndexOf("\\");
        }
        if (psep != -1)
        {
            folder = folder.Substring(0, psep + 1);
        }
        return folder;
    }

    static public string GetFileExtension(string path)
    {
        int dot = path.LastIndexOf(".");
        if (dot != -1)
        {
            return path.Substring(dot + 1); // Without "."
        }
        else
        {
            return "";
        }
    }

    static public string CombinePath(string path1, string path2)
    {
        string combinedPath;

        bool flag1 = (path1.EndsWith("/") || path1.EndsWith("\\"));
        bool flag2 = (path2.StartsWith("/") || path2.StartsWith("\\"));

        if (flag1 && flag2)
        {
            combinedPath = path1.Substring(0, path1.Length - 1);
            combinedPath += path2.Substring(1, path1.Length - 1);
        }
        else if (flag1 || flag2)
        {
            combinedPath = path1 + path2;
        }
        else
        {
            combinedPath = path1 + "/" + path2;
        }

        return combinedPath;
    }

    static public string CombinePaths(string path1, string path2, string path3)
    {
        return CombinePath(CombinePath(path1, path2), path3);
    }



// Version

    static public string MakeVersion(int major, int minor, int misc)
    {
        return string.Format("{0}.{1}.{2}", major, minor, misc);
    }

    static public void SplitVersion(string versionStr, out int major, out int minor)
    {
        int dummy = 0;
        SplitVersion(versionStr, out major, out minor, out dummy);
    }

    static public void SplitVersion(string versionStr, out int major, out int minor, out int misc)
    {
        major = 0;
        minor = 0;
        misc  = 0;

        try
        {
            int dot1 = versionStr.IndexOf('.');
            if (dot1 < 0)
            {
                major = int.Parse(versionStr);
            }
            else
            {
                major = int.Parse(versionStr.Substring(0, dot1));

                int dot2 = versionStr.LastIndexOf('.');
                if (dot2 >= 0)
                {
                    minor = int.Parse(versionStr.Substring(dot1 + 1, dot2 - dot1 - 1));
                    misc  = int.Parse(versionStr.Substring(dot2 + 1));
                }
                else
                {
                    minor = int.Parse(versionStr.Substring(dot1 + 1));
                }
            }
        }
        catch (System.Exception)
        {}
    }

    public static bool IsCompatibleVersion(int verMajor, int verMinor, int verMisc,
                                int minMajor, int minMinor, int minMisc)
    {
        if (verMajor < minMajor)
        {
            return false;
        }

        if (verMinor < minMinor)
        {
            return false;
        }

        if (verMisc < minMisc)
        {
            return false;
        }

        return true;
    }



    public static List<KeyValuePair<string,string>> KeyAndValuesFromString(string str)
    {
        List<KeyValuePair<string,string>> result = new List<KeyValuePair<string,string>>();

        TextReader tr = new StringReader(str);
        string line = null;
        while ((line = tr.ReadLine()) != null)
        {
            line = line.Trim();

            if (line.Length == 0 || line.StartsWith(";") || line.StartsWith("//"))
            {
                // Empty or comment line..
                continue;
            }

            int equalIndex = line.IndexOf('=');
            if (equalIndex != -1)
            {
                string key = line.Substring(0, equalIndex);
                string value = line.Substring(equalIndex + 1).Trim();
                if (value.Length >= 2)
                {
                    if (value.StartsWith("\"") && value.EndsWith("\""))
                    {
                        value = value.Substring(1, value.Length - 2);
                    }
                    else if (value.StartsWith("'") && value.EndsWith("'"))
                    {
                        value = value.Substring(1, value.Length - 2);
                    }
                }

                result.Add(new KeyValuePair<string,string>(key, value));
            }
        }

        return result;
    }

	

	// -180 ~ +180
	public static float GetAngleBetweenTwoPoints(Vector2 p1, Vector2 p2)
	{
		float xDiff = p2.x - p1.x;
		float yDiff = p2.y - p1.y;
		float degrees = Mathf.Atan2(yDiff, xDiff) * Mathf.Rad2Deg;
		return degrees;
	}

	public static float GetAngleBetweenTwoPointsSymmetric(Vector2 p1, Vector2 p2)
	{
		float xDiff = p1.x - p2.x;
		float yDiff = p1.y - p2.y;
		float degrees = Mathf.Atan2(yDiff, xDiff) * Mathf.Rad2Deg;
		return degrees;
	}

    public static float DifferenceAngles(float angle1, float angle2)
    {
		// -180 ~ +180 사이에 놓이게 해야, 각도차를 구하기 쉬움.

		if (angle1 >= 180f)
		{
			angle1 -= 360f;
		}

		if (angle2 >= 180f)
		{
			angle2 -= 360f;
		}

        return Mathf.Abs(angle2 - angle1);
    }

}
