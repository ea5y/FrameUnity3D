//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-06-03 17:50
//===Update: 2017-06-03 20:41
//================================

using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetBundles
{
	public static string SOURCE_PATH = Application.dataPath + "/ResForBundle/";
	public static string OUTPUT_PATH = "Assets/Bundle/";

    public static string ASSET_PREFAB = ".prefab";
	public static string ASSET_MODEL = "models";
	public static string ASSET_TEXTURES = "texture";
	public static string ASSET_AUDIO = "audio";
	public static string ASSET_SCENE = "scene";

	[MenuItem("Tools/Build AssetBundle #%b")]
	public static void Export()
	{
		ClearAssetBundlesName();
		//FindFileAndSetAssetBundleName(SOURCE_PATH);
        SetPathBundleName(SOURCE_PATH);

		var outputpath = OUTPUT_PATH + "/Android";
		if(!Directory.Exists(outputpath))
		{
			Directory.CreateDirectory(outputpath);
		}

		BuildPipeline.BuildAssetBundles(outputpath, 0, EditorUserBuildSettings.activeBuildTarget);
		AssetDatabase.Refresh();
		Debug.Log("Build AssetBundles Completed!");
		ClearAssetBundlesName();
	}

	private static void ClearAssetBundlesName()
	{
		int length = AssetDatabase.GetAllAssetBundleNames().Length;
		Debug.Log("AssetBundleNames.Length: " + length);

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
		Debug.Log("AssetBundleNames.Length: " + length);
	}

    //just set file
	private static void FindFileAndSetAssetBundleName(string path)
	{
		var folder = new DirectoryInfo(path);
		FileSystemInfo[] fileInfos = folder.GetFileSystemInfos();
		foreach(var fileInfo in fileInfos)
		{
			if(fileInfo is DirectoryInfo)
				FindFileAndSetAssetBundleName(fileInfo.FullName);
			else
				if(!fileInfo.Name.EndsWith(".meta"))
					SetAssetBundleName(fileInfo.FullName);
		}
	}

    private static void SetPathBundleName(string path)
    {
        var folder = new DirectoryInfo(path);
        FileSystemInfo[] fileInfos = folder.GetFileSystemInfos();
        foreach(var fileInfo in fileInfos)
        {
            if(!fileInfo.Name.EndsWith(".meta"))
            {
                //SetAssetBundleName(fileInfo.FullName);
                SetBundleNameTest(fileInfo.FullName);
            }
        }
    }

    private static void SetBundleNameTest(string bundleFullName)
    {
        Debug.Log("BundleFullName: " + bundleFullName);
        var bundlePJPath = "Assets" + bundleFullName.Substring(Application.dataPath.Length);

        var strArray = bundlePJPath.Split('/');
        var bundleName = strArray[strArray.Length -1];
        var extension = Path.GetExtension(bundleName);
        if(string.IsNullOrEmpty(extension))
            extension = "";
        else
            extension = ".unity3d";
        bundleName += extension;
        Debug.Log("BundleName: " + bundleName);

        AssetImporter assetImporter = AssetImporter.GetAtPath(bundlePJPath);
        assetImporter.assetBundleName = bundleName;
    }

	private static void SetAssetBundleName(string fileFullName)
	{
		Debug.Log("FileFullName: " + fileFullName);
		var filePJPath = "Assets" + fileFullName.Substring(Application.dataPath.Length);

		var strArray = filePJPath.Split('/');		
		var fileName = strArray[strArray.Length - 1];
		var assetName = fileName.Replace(Path.GetExtension(fileName), ".unity3d");
		Debug.Log("AssetName: " + assetName);

		AssetImporter assetImporter = AssetImporter.GetAtPath(filePJPath);
		assetImporter.assetBundleName = assetName;
	}
}

