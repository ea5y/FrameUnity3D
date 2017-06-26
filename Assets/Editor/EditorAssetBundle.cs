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
    /*
    public static string ASSET_PREFAB = ".prefab";
	public static string ASSET_MODEL = "models";
	public static string ASSET_TEXTURES = "texture";
	public static string ASSET_AUDIO = "audio";
	public static string ASSET_SCENE = "scene";
    */

	[MenuItem("AssetBundle/Build For Android")]
    public static void ExportForAndroid()
    {
        Export(BuildTarget.Android, URL.ASSETBUNDLE_OUTPUT_URL + "Android");
    }

	[MenuItem("AssetBundle/Build For IOS")]
    public static void ExportForIOS()
    {
        Export(BuildTarget.iOS, URL.ASSETBUNDLE_OUTPUT_URL + "Ios");
    }

	[MenuItem("AssetBundle/Build For Win")]
    public static void ExportForWin()
    {
        Export(BuildTarget.StandaloneWindows, URL.ASSETBUNDLE_OUTPUT_URL + "Win");
    }

	public static void Export(BuildTarget target, string url)
	{
		ClearAssetBundlesName();
		//FindFileAndSetAssetBundleName(SOURCE_PATH);
        SetPathBundleName(URL.ASSETBUNDLE_INPUT_URL);

		var outputpath = url;
        Debug.Log("OutPath: " + url);
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

    private static void SetPathBundleName(string path)
    {
        var folder = new DirectoryInfo(path);
        FileSystemInfo[] fileInfos = folder.GetFileSystemInfos();
        foreach(var fileInfo in fileInfos)
        {
            if(!fileInfo.Name.EndsWith(".meta"))
            {
                Path(fileInfo.FullName);
            }
        }
    }

    private static void Path(string bundleFullName)
    {
        Debug.Log("BundleFullName: " + bundleFullName);
        var bundlePJPath = "Assets" + bundleFullName.Substring(Application.dataPath.Length);

#if LINUX_EDITOR
        var strArray = bundlePJPath.Split('/');
#elif WIN_EDITOR
        var strArray = bundlePJPath.Split('\\');
#endif
        var bundleName = strArray[strArray.Length -1];
        var extension = System.IO.Path.GetExtension(bundleName);
        if(string.IsNullOrEmpty(extension))
            extension = "";
        else
            extension = ".unity3d";
        bundleName += extension;
        Debug.Log("BundleName: " + bundleName);

        AssetImporter assetImporter = AssetImporter.GetAtPath(bundlePJPath);
        assetImporter.assetBundleName = bundleName;
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

	private static void SetAssetBundleName(string fileFullName)
	{
		Debug.Log("FileFullName: " + fileFullName);
		var filePJPath = "Assets" + fileFullName.Substring(Application.dataPath.Length);

		var strArray = filePJPath.Split('/');		
		var fileName = strArray[strArray.Length - 1];
		var assetName = fileName.Replace(System.IO.Path.GetExtension(fileName), ".unity3d");
		Debug.Log("AssetName: " + assetName);

		AssetImporter assetImporter = AssetImporter.GetAtPath(filePJPath);
		assetImporter.assetBundleName = assetName;
	}
}

