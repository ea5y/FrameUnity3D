//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-09 19:37
//================================

using UnityEngine;
using System;
using System.IO;
using UnityEditor;

namespace Easy.FrameUnity.ScriptableObj
{
    public class ScriptableObjectHelp
    {
        public static bool Create(string type, string path, string name)
        {
            if(new DirectoryInfo(path).Exists == false)
            {
                Debug.LogError("Can't create asset, path not found");
                return false;
            }
            if(string.IsNullOrEmpty(name))
            {
                Debug.LogError("Can't create asset, the name is empty");
                return false;
            }

            string assetPath = Path.Combine(path, name + ".asset");

            var t = ScriptableObject.CreateInstance(type);
            AssetDatabase.CreateAsset(t, assetPath);
            Selection.activeObject = t;
            return true;
        }
    }
}
