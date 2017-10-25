//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-09 19:37
//================================

using System.IO;
using UnityEditor;

namespace Easy.FrameUnity.Editor
{
    public class ScriptableObject
    {
        public static void CreateFromSelected()
        {
            string assetName = "New ScriptableObject";
            string assetPath = "Assets";
            string fileName = "";
            GetActiveObject(out fileName, out assetPath);
            Create(fileName, assetPath, assetName);
        }

        private static void GetActiveObject(out string fileName, out string assetPath)
        {
            fileName = "";
            assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (System.IO.Path.GetExtension(assetPath) != "")
            {
                fileName = System.IO.Path.GetFileName(assetPath).Split('.')[0];
                assetPath = System.IO.Path.GetDirectoryName(assetPath);
            }
        }

        private static void Create(string fileName, string assetPath, string assetName)
        {
            bool doCreate = true;
            string path = System.IO.Path.Combine(assetPath, assetName + ".asset");
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                doCreate = EditorUtility.DisplayDialog(assetName + " already exists.", "Do you want to overwrite the old", "Yes", "No");
            }
            if (doCreate)
            {
                var t = UnityEngine.ScriptableObject.CreateInstance(fileName);
                AssetDatabase.CreateAsset(t, path);
                Selection.activeObject = t;
                AssetDatabase.Refresh();
            }
        }
    }
}
