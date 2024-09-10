using System;
using Microsoft.Win32;
using UnityEngine;

public static class RegistryUtil
{
    const string path_str = "path";
    const string registryTest = "SOFTWARE\\TEST";
    const string notExe = "";
    const string exe_str = ".exe";

    public static void CheckRegistry()
    {
        string path = Application.dataPath;
        string name = Application.productName;
        path = path.Substring(0, path.LastIndexOf('/'));
        path += "/" + name + exe_str;



#if UNITY_STANDALONE
        //string programPath = Application.
        Debug.Log("path : " + path);

        //RegistryKey regKey = Registry.LocalMachine.OpenSubKey(registryTest, true);

        //if (regKey == null)
        //{
        //    regKey = Registry.LocalMachine.CreateSubKey(registryTest, RegistryKeyPermissionCheck.ReadWriteSubTree);
        //    regKey.SetValue(path_str, programName, RegistryValueKind.String);
        //}
        //else
        //{
        //    var pathVal = regKey.GetValue(path_str);
        //    if (pathVal != null)
        //    {
        //        regKey.SetValue(path_str, programName, RegistryValueKind.String);
        //    }

        //}
#endif

#if UNITY_EDITOR
        Debug.Log("path3 : " + notExe);
#endif




    }
}
