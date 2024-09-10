using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

public static class EditorUtil
{
    public enum Severity
    {
        Info,
        Warning,
        Error
    }

    public static void InfoBox(string message, Severity severity)
    {
        MessageType messageType = MessageType.None;
        switch (severity)
        {
        case Severity.Info: messageType = MessageType.Info; break;
        case Severity.Warning: messageType = MessageType.Warning; break;
        case Severity.Error: messageType = MessageType.Error; break;
        }
        EditorGUILayout.HelpBox(message, messageType);
    }

    public static int InfoBoxWithButtons(string message, Severity severity, params string[] buttons)
    {
        Color oldBackgroundColor = GUI.backgroundColor;
        switch (severity)
        {
        case Severity.Info: GUI.backgroundColor = new Color32(154, 176, 203, 255); break;
        case Severity.Warning: GUI.backgroundColor = new Color32(255, 255, 0, 255); break;
        case Severity.Error: GUI.backgroundColor = new Color32(255, 0, 0, 255); break;
        }

        GUILayout.BeginVertical("textarea");
        GUI.backgroundColor = oldBackgroundColor;

        GUIStyle labelStyle = new GUIStyle("label");
        labelStyle.wordWrap = true;

        GUILayout.Label(message, labelStyle, GUILayout.ExpandWidth(true));

        int buttonPressed = -1;
        if (buttons != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            for (int i = 0; i < buttons.Length; ++i)
            {
                if (GUILayout.Button(buttons[i], EditorStyles.miniButton))
                {
                    buttonPressed = i;
                }
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();

        return buttonPressed;
    }

    private static bool backupGuiChangedValue = false;
    public static void BeginChangeCheck()
    {
        backupGuiChangedValue = GUI.changed;
        GUI.changed = false;
    }

    public static bool EndChangeCheck()
    {
        bool hasChanged = GUI.changed;
        GUI.changed |= backupGuiChangedValue;
        return hasChanged;
    }


    public static string SaveFileInProject(string title, string directory, string filename, string ext)
    {
        string path = EditorUtility.SaveFilePanel(title, directory, filename, ext);
        if (path.Length == 0) // cancelled
        {
            return "";
        }

        string cwd = System.IO.Directory.GetCurrentDirectory().Replace("\\","/") + "/assets/";
        if (path.ToLower().IndexOf(cwd.ToLower()) != 0)
        {
            path = "";
            EditorUtility.DisplayDialog(title, "Assets must be saved inside the Assets folder", "Ok");
        }
        else
        {
            path = path.Substring(cwd.Length - "/assets".Length);
        }
        return path;
    }

    public static int InlineToolbar(string name, int val, string[] names)
    {
        int selectedIndex = val;
        GUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));
        GUILayout.Label(name, EditorStyles.toolbarButton);
        GUILayout.FlexibleSpace();
        for (int i = 0; i < names.Length; ++i)
        {
            bool selected = (i == selectedIndex);
            bool toggled = GUILayout.Toggle(selected, names[i], EditorStyles.toolbarButton);
            if (toggled == true)
            {
                selectedIndex = i;
            }
        }

        GUILayout.EndHorizontal();
        return selectedIndex;
    }

    private static int pushDepth = 0;
    private static Color oldGUIBackgroundColor = Color.black;
    public static void PushGUIBackgroundColor(Color newColor)
    {
        oldGUIBackgroundColor = GUI.backgroundColor;
        GUI.backgroundColor = newColor;
        pushDepth++;
    }

    public static void PopGUIBackgroundColor()
    {
        if (pushDepth > 0)
        {
            pushDepth--;
            if (pushDepth == 0)
            {
                GUI.backgroundColor = oldGUIBackgroundColor;
            }
        }
    }


    // CSV

    //sizeY = grid.GetUpperBound(1) + 1;
    //sizeX = grid.GetUpperBound(0) + 1;

    // splits a CSV file into a 2D string array
    static public string[,] SplitCSVGrid(string csvText, out int sizeX, out int sizeY)
    {
        string[] lines = csvText.Split("\n"[0]);

        // finds the max width of row
        int width = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string[] row = SplitCSVLine(lines[i]);
            width = Mathf.Max(width, row.Length);
        }

        // creates new 2D string grid to output to
        string[,] outputGrid = new string[width + 1, lines.Length + 1];
        for (int y = 0; y < lines.Length; y++)
        {
            string[] row = SplitCSVLine(lines[y]);
            for (int x = 0; x < row.Length; x++)
            {
                outputGrid[x,y] = row[x];

                // This line was to replace "" with " in my output.
                // Include or edit it as you wish.
                outputGrid[x,y] = outputGrid[x,y].Replace("\"\"", "\"");
            }
        }

        sizeX = outputGrid.GetUpperBound(0);
        sizeY = outputGrid.GetUpperBound(1);

        return outputGrid;
    }

    // splits a CSV row
    static public string[] SplitCSVLine(string line)
    {
        return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
                        @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
                        System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
                        select m.Groups[1].Value).ToArray();
    }


    // Path util

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


    // Files

    public static bool IsFileExists(string filename)
    {
        return System.IO.File.Exists(filename);
    }


    //@fixme Following codes are broken currently!!
    // Splitter

    public static GUIStyle splitterStyle;

    static void InitSplitterStyle()
    {
        if (splitterStyle == null)
        {
            splitterStyle = new GUIStyle();
            splitterStyle.normal.background = EditorGUIUtility.whiteTexture;
            splitterStyle.stretchWidth = true;
            splitterStyle.margin = new RectOffset(0, 0, 7, 7);
        }
    }

    private static Color splitterColor = EditorGUIUtility.isProSkin ? new Color(0.157f, 0.157f, 0.157f) : new Color(0.5f, 0.5f, 0.5f);

    // GUILayout Style
    public static void Splitter(Color rgb, float thickness = 1)
    {
        InitSplitterStyle();

        Rect position = GUILayoutUtility.GetRect(GUIContent.none, splitterStyle, GUILayout.Height(thickness));

        if (Event.current.type == EventType.Repaint)
        {
            Color restoreColor = GUI.color;
            GUI.color = rgb;
            splitterStyle.Draw(position, false, false, false, false);
            GUI.color = restoreColor;
        }
    }

    public static void Splitter(float thickness, GUIStyle userSplitterStyle)
    {
        Rect position = GUILayoutUtility.GetRect(GUIContent.none, userSplitterStyle, GUILayout.Height(thickness));

        if (Event.current.type == EventType.Repaint)
        {
            Color restoreColor = GUI.color;
            GUI.color = splitterColor;
            userSplitterStyle.Draw(position, false, false, false, false);
            GUI.color = restoreColor;
        }
    }

    public static void Splitter(float thickness = 1)
    {
        InitSplitterStyle();

        Splitter(thickness, splitterStyle);
    }

    // GUI Style
    public static void Splitter(Rect position)
    {
        InitSplitterStyle();

        if (Event.current.type == EventType.Repaint)
        {
            Color restoreColor = GUI.color;
            GUI.color = splitterColor;
            splitterStyle.Draw(position, false, false, false, false);
            GUI.color = restoreColor;
        }
    }


    // LastFileWriteTime

    public static string GetLastWriteTime(string filePath)
    {
        System.DateTime d = System.IO.File.GetLastWriteTime(filePath);
        return d.ToString();
    }

    public static string GetLastWriteTimeOfAsset(Object asset)
    {
        string assetPath = AssetDatabase.GetAssetPath(asset);
        string filePath = Application.dataPath + assetPath;
        filePath = filePath.Replace("AssetsAssets", "Assets");
        System.DateTime d = System.IO.File.GetLastWriteTime(filePath);
        return d.ToString();
    }


    // Prefab

    public static bool IsPrefab(GameObject go)
    {
        /*
        PrefabType
            None                            The object is not a prefab nor an instance of a prefab.
            Prefab                            The object is a user created prefab asset.
            ModelPrefab                        The object is an imported 3D model asset.
            PrefabInstance                    The object is an instance of a user created prefab.
            ModelPrefabInstance                The object is an instance of an imported 3D model.
            MissingPrefabInstance            The object was an instance of a prefab, but the original prefab could not be found.
            DisconnectedPrefabInstance        The object is an instance of a user created prefab, but the connection is broken.
            DisconnectedModelPrefabInstance    The object is an instance of an imported 3D model, but the connection is broken.
        */
        return go != null && PrefabUtility.GetPrefabType(go) != PrefabType.None;
    }

    /*
    byte[] bytes = texture.EncodeToPNG();
    File file = new File.Open(Application.dataPath + "/" + filename, FileMode.Create);
    BinaryWriter w = new BinaryWriter(file);
    w.Write(bytes);
    w.Close();
    file.Close();
    */
    public static Texture2D GetMiniThumbnail(Object o)
    {
//#if UNITY_4_0
        return AssetPreview.GetMiniThumbnail(o);
//#else
//        return EditorUtility.GetMiniThumbnail(o);
//#endif
    }


    public static string SanitizePropertyDisplayName(string propertyName, bool isBoolProperty)
    {
        StringBuilder result = new StringBuilder();
        int outputLen = 0;

        bool isInARun = false;
        for (int i = 0, n = propertyName.Length; i < n; i++)
        {
            char ch = propertyName[i];

            bool isLowercase  = (ch >= 'a' && ch <= 'z');
            bool isUppercase  = (ch >= 'A' && ch <= 'Z');
            bool isDigit      = (ch >= '0' && ch <= '9');
            bool isUnderscore = (ch == '_');

            //bBoolProperty
            // Skip the first character if the property is a bool (they should all start with a lowercase 'b', which we don't want to keep)
            if (i == 0 && isBoolProperty && ch == 'b')
            {
                continue;
            }

            // If the current character is upper case or a digit, and the previous character wasn't, then we need to insert a space
            if ((isUppercase || isDigit) && !isInARun)
            {
                if (outputLen > 0)
                {
                    result.Append(' ');
                    outputLen++;
                }
                isInARun = true;
            }

            // A lower case character will break a run of upper case letters and/or digits
            if (isLowercase)
            {
                isInARun = false;
            }

            if (isUnderscore)
            {
                ch = ' ';
                isInARun = true;
            }

            result.Append(ch);
            outputLen++;
        }

        return result.ToString();
    }

	//[MenuItem("Audition/Editor Utilities/FX MeshCollider Cleaner")]
	//static void CleanFxMeshColliders()
	//{
 //       int removedCount = 0;

 //       var fxFiles = Directory.GetFiles("Assets/Resources/FX/", "*.prefab", SearchOption.AllDirectories);
 //       for (int i = 0; i < fxFiles.Length; i++)
 //       {
 //           var fxFile = fxFiles[i].Replace("\\", "/");
 //           if (fxFile.EndsWith(".meta"))
 //           {
 //               continue;
 //           }

	//		//Debug.Log(fxFile);

 //           var prefab = AssetDatabase.LoadAssetAtPath(fxFile, typeof(GameObject)) as GameObject;
 //           if (prefab != null)
 //           {
	//			var inst = GameObject.Instantiate(prefab) as GameObject;

 //               var meshColliders = inst.GetComponentsInChildren<MeshCollider>();
 //               if (meshColliders.Length > 0)
 //               {
 //                   Debug.Log(string.Format("Clean mesh-colliders: {0}", fxFile));

 //                   for (int meshColliderIdx = 0; meshColliderIdx < meshColliders.Length; meshColliderIdx++)
 //                   {
 //                       var meshCollider = meshColliders[meshColliderIdx];
 //                       Object.DestroyImmediate(meshCollider);
 //                   }

	//				PrefabUtility.ReplacePrefab(inst, prefab, ReplacePrefabOptions.ConnectToPrefab);

 //                   //EditorUtility.SetDirty(prefab);
 //                   removedCount++;
 //               }

	//			GameObject.DestroyImmediate(inst);
 //           }
 //       }

 //       if (removedCount > 0)
 //       {
 //           AssetDatabase.SaveAssets();
 //           AssetDatabase.Refresh();
 //       }
	//}
}
