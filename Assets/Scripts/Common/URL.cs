using UnityEngine;
using System.Collections;

public class URL
{
	public static readonly string ASSETBUNDLE_HOST_URL = 
#if UNITY_ANDROID && !UNITY_EDITOR 
        "http://127.0.0.1/resource/Android/";
#elif UNITY_IPHONE
        "http://127.0.0.1/resource/IOS/";
#else
        "http://127.0.0.1/resource/Win/";
#endif

    //Maybe can use Application.streamingAssetsPath + "/";
    public static readonly string ASSETBUNDLE_LOCAL_URL =
#if UNITY_ANDROID && !UNITY_EDITOR
        "jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_ANDROID && UNITY_EDITOR || UNITY_STANDALONE_WIN && UNITY_EDITOR
        "file://" + Application.dataPath + "/StreamingAssets/";

#elif UNITY_IPHONE && !UNITY_EDITOR
        "file://" + Application.dataPath + "/Raw/";
#elif UNITY_IPHONE && UNITY_EDITOR || UNITY_STANDALONE_OSX
        "file://" + Application.dataPath + "/StreamingAssets/Mac/";
#else
        string.Empty;
#endif

	public static readonly string ASSETBUNDLE_INPUT_URL = Application.dataPath + "/ResForBundle/";
	public static readonly string ASSETBUNDLE_OUTPUT_URL = "Assets/Bundle/";

	public static readonly string BUNDLE_FILES_URL = "";
}
