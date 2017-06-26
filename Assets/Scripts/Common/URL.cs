using UnityEngine;
using System.Collections;

public class URL
{
	public static readonly string ASSETBUNDLE_HOST_URL = "http://127.0.0.1/resource/Android/";
    public static readonly string ASSETBUNDLE_LOCAL_URL =
#if UNITY_ANDROID && !UNITY_EDITOR
        //"jar:file://" + Application.dataPath + "!/assets/";
        Application.streamingAssetsPath + "/";
#elif UNITY_IPHONE
        Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
        "file://" + Application.dataPath + "/StreamingAssets/";
#else
        string.Empty;
#endif

	public static readonly string ASSETBUNDLE_INPUT_URL = Application.dataPath + "/ResForBundle/";
	public static readonly string ASSETBUNDLE_OUTPUT_URL = "Assets/Bundle/";
}
