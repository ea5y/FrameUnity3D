﻿using UnityEngine;
using System.Collections;

public class URL
{
    //public static readonly string GAME_SERVER_HOST = "192.168.0.108";
    //public static readonly string GAME_SERVER_HOST = "127.0.0.1";
    public static readonly string GAME_SERVER_HOST = "192.168.1.208";
    public static readonly int GAME_SERVER_PORT = 9001;

	public static readonly string ASSETBUNDLE_HOST_URL = 
#if UNITY_ANDROID && !UNITY_EDITOR 
        //"http://127.0.0.1/resource/android/";
        "http://120.26.119.202/luwanzhong/resource/android/";
#elif UNITY_IPHONE
        "http://127.0.0.1/resource/iOS/";
#else
        //"http://127.0.0.1/resource/win/";
        "http://120.26.119.202/luwanzhong/resource/win/";
#endif

    public static readonly string LUA_HOST_URL = "http://120.26.119.202/luwanzhong/resource/lua/";

    public static readonly string RESOURCE_FILE_LIST_FILENAME = "ResourceFileList.json";

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

    public static readonly string STREAMINGASSETS_URL =
#if UNITY_ANDROID && !UNITY_EDITOR
        //"jar:file://" + Application.dataPath + "!/assets/";
        Application.persistentDataPath + "/";
#elif UNITY_ANDROID && UNITY_EDITOR || UNITY_STANDALONE_WIN && UNITY_EDITOR
        Application.streamingAssetsPath;
#elif UNITY_IPHONE && !UNITY_EDITOR
        "file://" + Application.dataPath + "/Raw/";
#elif UNITY_IPHONE && UNITY_EDITOR || UNITY_STANDALONE_OSX
        "file://" + Application.dataPath + "/StreamingAssets/";
#else
        string.Empty;
#endif

    public static readonly string RELATIVE_STREAMINGASSETS_URL =
#if UNITY_ANDROID && !UNITY_EDITOR
        //"jar:file://" + Application.dataPath + "!/assets/";
        Application.persistentDataPath + "/";
#elif UNITY_ANDROID && UNITY_EDITOR || UNITY_STANDALONE_WIN && UNITY_EDITOR
        "Assets/StreamingAssets/";
        //Application.persistentDataPath + "/";
#elif UNITY_IPHONE && !UNITY_EDITOR
        "file://" + Application.dataPath + "/Raw/";
#elif UNITY_IPHONE && UNITY_EDITOR || UNITY_STANDALONE_OSX
        "file://" + Application.dataPath + "/StreamingAssets/";
#else
        string.Empty;
#endif

    public static readonly string ASSETBUNDLE_INPUT_URL = Application.dataPath + "/ResForBundle/";
	public static readonly string ASSETBUNDLE_OUTPUT_URL = "Assets/HotfixRes/";

	public static readonly string BUNDLE_FILES_URL = "";

    public static readonly string HOTFIX_URL = RELATIVE_STREAMINGASSETS_URL + "Hotfix.lua";
}
