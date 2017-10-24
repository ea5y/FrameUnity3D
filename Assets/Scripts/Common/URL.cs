using UnityEngine;
using System.Collections;

public class URL
{
    public static readonly string GAME_SERVER_HOST = "192.168.1.208";
    public static readonly int GAME_SERVER_PORT = 9001;

    public static readonly string ASSETBUNDLE_INPUT_URL = Application.dataPath + "/Hotfix/Bundle/";
    public static readonly string ASSETBUNDLE_OUTPUT_URL = "HotfixExport/";
    public static readonly string ASSETBUNDLE_PERSONAL_URL = Application.dataPath + "/Hotfix/Bundle/Personal/";
    public static readonly string ASSETBUNDLE_SHARE_URL = Application.dataPath + "/Hotfix/Bundle/Share/";

    public static readonly string PERSISTENTDATA_URL = Application.persistentDataPath + "/";

    public static readonly string RESOURCE_FILE_LIST_FILENAME = "ResourceFileList.json";

    public static readonly string ASSETBUNDLE_HOST_URL = 
#if UNITY_ANDROID && !UNITY_EDITOR
        "http://120.26.119.202/luwanzhong/resource/android/";
#elif UNITY_IPHONE
        "http://120.26.119.202/luwanzhong/resource/iOS/";
#else
        "http://120.26.119.202/luwanzhong/resource/win/";
#endif

    public static readonly string ASSETBUNDLE_LOCAL_URL =
#if UNITY_EDITOR && EDITOR_MODE
        "Assets/Hotfix/Bundle/";
#else
        PERSISTENTDATA_URL;
#endif

    public static readonly string FILE_ASSETBUNDLE_LOCAL_URL = "file:///" + ASSETBUNDLE_LOCAL_URL;

    public static readonly string LUA_HOST_URL = "http://120.26.119.202/luwanzhong/resource/lua/";
    public static readonly string LUA_LOCAL_URL =
#if UNITY_EDITOR && EDITOR_MODE
        Application.dataPath + "/Hotfix/Lua/";
#else
        PERSISTENTDATA_URL;
#endif

    public static readonly string HOTFIX_MAIN_URL = LUA_LOCAL_URL + "Hotfix.lua";
}
