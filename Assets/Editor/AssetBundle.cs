//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-23 17:08
//================================

using UnityEditor;
using UnityEngine;
using Easy.FrameUnity.ScriptableObj;
namespace Easy.FrameUnity.Editor
{
    public class AssetBundle 
    {
        public static void ExportForAndroid()
        {
        }

        public static void ExportForIOS()
        {
        }

        public static void ExportForWin()
        {
        }
        
        [MenuItem("Tools/Create AssetBundleMain")]
        public static void CreateAssetBundleMain()
        {
            Object[] selectedObj = Selection.GetFiltered(typeof(object), SelectionMode.DeepAssets);

            foreach(Object obj in selectedObj)
            {
                string targetPath = Application.dataPath + "/Hotfix/BundleNew/";
                if(BuildPipeline.BuildAssetBundles(targetPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget))
                    Debug.Log(obj.name + "build success!");
                else
                    Debug.Log(obj.name + "build fail!");
            }
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Create AssetBundleAll")]
        public static void CreateAssetBundleAll()
        {
            var url = "Assets/Hotfix/Bundle/Materials/Materials.asset";
            var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<SObjMaterials>(url);
            foreach(var v in asset.Injections)
            {
                var t = v.Material.mainTexture;
                var _t = AssetDatabase.GetAssetPath(t);
                var s = AssetDatabase.GetAssetPath(v.Material);
                Debug.Log("Material: " + s);
                Debug.Log("Texture: " + _t);
            }
        }

        private static void ClearAssetBundlesName()
        {
        }

        public static void SetAssetName()
        {

        }

        private static void SetShareAssetName()
        {
        }

        private static void SetPersonalAssetName()
        {
        }

        public static void SetMaterialsAssetName()
        {

        }

        public static void SetTexturesAssetName()
        {
        }

        public static void SetShadersAssetName()
        {
        }
    }
}
