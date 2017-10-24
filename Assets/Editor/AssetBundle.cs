//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-23 17:08
//================================

using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Easy.FrameUnity.ScriptableObj;
using Easy.FrameUnity.Util;
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
        
        [MenuItem("Tools/Build AssetBundle")]
        public static void BuildAssetBundle()
        {
            ClearAssetBundlesName();
            SetAssetName();

            var outputpath = URL.ASSETBUNDLE_OUTPUT_URL + "Bundle/win/";
            if (!Directory.Exists(outputpath))
            {
                Directory.CreateDirectory(outputpath);
            }
            BuildPipeline.BuildAssetBundles(outputpath, 0, EditorUserBuildSettings.activeBuildTarget);
            AssetDatabase.Refresh();
            Debug.Log("Build AssetBundles Completed!");

            ClearAssetBundlesName();
            CreateResourceFileList<BundleFile>(outputpath);
        }

        private static void CreateResourceFileList<T>(string inputPath) where T : ResourceFile, new()
        {
            Debug.Log("JsonFrom: " + inputPath);
            var folder = new DirectoryInfo(inputPath);
            FileSystemInfo[] fileInfos = folder.GetFileSystemInfos();

            var bundleFileList = new ResourceFileList<T>();
            foreach(var fileInfo in fileInfos) 
            {
                if (fileInfo.Name.EndsWith(".meta") || fileInfo.Name.EndsWith(".manifest"))
                    continue;
                T bundleFile = new T();
                bundleFile.name = fileInfo.Name;
                bundleFile.md5 = IOHelperUtil.GetFileMD5(fileInfo.FullName);
                bundleFile.length = ((FileInfo)fileInfo).Length.ToString();

                bundleFileList.resourceFileList.Add(bundleFile);
            }
            IOHelperUtil.SaveToJson<ResourceFileList>(bundleFileList, inputPath);
        }

        private static void ClearAssetBundlesName()
        {
            int length = AssetDatabase.GetAllAssetBundleNames().Length;
            Debug.Log("Before AssetBundleNames.Length: " + length);

            var oldAssetBundleNames = new string[length];
            for (int i = 0; i < length; i++)
            {
                oldAssetBundleNames[i] = AssetDatabase.GetAllAssetBundleNames()[i];
            }

            for (int j = 0; j < oldAssetBundleNames.Length; j++)
            {
                AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[j], true);
            }

            length = AssetDatabase.GetAllAssetBundleNames().Length;
            Debug.Log("After AssetBundleNames.Length: " + length);
        }

        public static void SetAssetName()
        {
            SetShareAssetName();
            SetPersonalAssetName();
        }

        private static void SetShareAssetName()
        {
            SetMaterialsAssetName();
            SetTexturesAssetName();
            SetShadersAssetName();
        }

        private static void SetPersonalAssetName()
        {
            string path = URL.ASSETBUNDLE_INPUT_URL;
            var folder = new DirectoryInfo(path);
            FileSystemInfo[] fileInfos = folder.GetFileSystemInfos();
            foreach(var fileInfo in fileInfos)
            {
                //if(!fileInfo.Name.EndsWith(".meta") && fileInfo.Name != "Materials" && fileInfo.Name != "Texture2Ds" && fileInfo.Name != "Shaders")
                if(!fileInfo.Name.EndsWith(".meta"))
                {
                    _SetPersonalAssetName(fileInfo.FullName);
                }
            }
        }

        private static void _SetPersonalAssetName(string bundleFullName)
        {
            var bundlePJPath = GetBundlePJPath(bundleFullName);
            var bundleName = GetBundleName(bundlePJPath);

            AssetImporter assetImporter = AssetImporter.GetAtPath(bundlePJPath);
            assetImporter.assetBundleName = bundleName;
        }

        private static string GetBundlePJPath(string bundleFullName)
        {
            var msg = string.Format("BundleFullName: {0}", bundleFullName);
            Debug.Log(msg);
            var bundlePJPath = "Assets" + bundleFullName.Substring(Application.dataPath.Length);
            return bundlePJPath;
        }

        private static string GetBundlePJPath(UnityEngine.Object obj)
        {
            var path = AssetDatabase.GetAssetPath(obj);
            return path;
        }

        private static string GetBundleName(string bundlePJPath)
        {
            bundlePJPath = bundlePJPath.Replace('/', '\\');
            var strArray = bundlePJPath.Split('/');
#if LINUX_EDITOR
            strArray = bundlePJPath.Split('/');
#elif WIN_EDITOR
            strArray = bundlePJPath.Split('\\');
#endif
            var bundleName = strArray[strArray.Length - 1];
            var extension = System.IO.Path.GetExtension(bundleName);
            if(string.IsNullOrEmpty(extension))
                extension = "";
            else
                extension = ".unity3d";
            bundleName += extension;
            var msg = string.Format("BundleName: {0}", bundleName);
            Debug.Log(msg);
            return bundleName;
        }

        public static void SetMaterialsAssetName()
        {
            var path = "Assets/Hotfix/Bundle/Materials/Materials.asset";
            SetShareAssetName(path);
        }

        public static void SetTexturesAssetName()
        {
            var path = "Assets/Hotfix/Bundle/Texture2Ds/Texture2Ds.asset";
            SetShareAssetName(path);
        }

        public static void SetShadersAssetName()
        {
            var path = "Assets/Hotfix/Bundle/Shaders/Shaders.asset";
            SetShareAssetName(path);
        }

        private static void SetShareAssetName(string path)
        {
            var asset = AssetDatabase.LoadAssetAtPath<SObjInjection>(path);
            foreach(var inject in asset.Injections)
            {
                var bundlePJPath = GetBundlePJPath(inject.Object);
                var bundleName = GetBundleName(bundlePJPath);
                AssetImporter assetImporter = AssetImporter.GetAtPath(bundlePJPath);
                assetImporter.assetBundleName = bundleName;
            }
        }

        private static void BuildAssetBundlesBuildMap(string bundleName, List<string> assetPJPathList)
        {
            AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
            buildMap[0].assetBundleName = bundleName;
            buildMap[0].assetNames = assetPJPathList.ToArray();

            BuildPipeline.BuildAssetBundles(_outPath, buildMap, BuildAssetBundleOptions.ChunkBasedCompression, _buildTarget);
        }

        private static void BuildMaterialsBuildMap()
        {
            var path = "Assets/Hotfix/Bundle/Materials/Materials.asset";
            BuildBuildMap(path, "materials_map");
        }

        private static void BuildTexture2DsBuildMap()
        {
            var path = "Assets/Hotfix/Bundle/Texture2Ds/Texture2Ds.asset";
            BuildBuildMap(path, "texture2ds_map");
        }

        private static void BuildShadersBuildMap()
        {
            var path = "Assets/Hotfix/Bundle/Shaders/Shaders.asset";
            BuildBuildMap(path, "shaders_map");
        }

        private static void BuildBuildMap(string path, string bundleName)
        {
            var asset = AssetDatabase.LoadAssetAtPath<SObjInjection>(path);
            List<string> assetPJPathList = new List<string>();
            foreach(var inject in asset.Injections)
            {
                var assetPJPath = GetBundlePJPath(inject.Object);
                assetPJPathList.Add(assetPJPath);
            }

            BuildAssetBundlesBuildMap(bundleName, assetPJPathList);
        }

        private static string _outPath = URL.ASSETBUNDLE_OUTPUT_URL + "Bundle/win/";
        private static BuildTarget _buildTarget = EditorUserBuildSettings.activeBuildTarget;
    }
}
