//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-23 17:08
//================================

using UnityEditor;
namespace Easy.FrameUnity.Editor
{
    public static class MenuEditor
    {
        [MenuItem("Hotfix/Build AssetBundle/For android")]
        public static void BuildABForAndroid()
        {
            AssetBundle.ExportForAndroid();
        }

        [MenuItem("Hotfix/Build AssetBundle/For ios")]
        public static void BuildABForIOS()
        {
            AssetBundle.ExportForIOS();
        }

        [MenuItem("Hotfix/Build AssetBundle/For win")]
        public static void BuildABForWIN()
        {
            AssetBundle.ExportForWin();
        }
        
        [MenuItem("Hotfix/Create LuaFileList")]
        public static void CreateLuaFileList()
        {
            AssetBundle.CreateResourceFileList<LuaFile>(URL.ASSETBUNDLE_OUTPUT_URL + "Lua/");
        }

        [MenuItem("Hotfix/Create ScriptableObject")]
        public static void CreateScriptableObject()
        {
            ScriptableObject.CreateFromSelected();            
        }
    }
}
