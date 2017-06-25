﻿using UnityEngine;
using System.Collections;

public class URL
{
    public static readonly string ASSETBUNDLE_URL =
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
}