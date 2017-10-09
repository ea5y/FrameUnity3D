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
        public static T Create<T>(string path, string name) where T : ScriptableObjectBase
        {
            if(new DirectoryInfo(path).Exists == false)
            {
                Debug.LogError("Can't create asset, path not found");
                return null;
            }
            if(string.IsNullOrEmpty(name))
            {
                Debug.LogError("Can't create asset, the name is empty");
                return null;
            }

            string assetPath = Path.Combine(path, name + ".asset");

            T t = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(t, assetPath);
            Selection.activeObject = t;
            return t;
        }
    }
}
