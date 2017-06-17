using UnityEngine;
using System.Collections;

public class URL
{
    public static string ASSETBUNDLE_URL =
#if UNITY_ANDROID
        "jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IPHONE
        Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
        "file://" + Application.dataPath + "/StreamingAssets/";
#else
        string.Empty;
#endif
}



public abstract class BundleData
{
    public string assetPath;
    public string assetName;
    public int version = 0;

    public AssetBundle bundle;

    protected void LoadAllAssets()
    {

    }

    protected void LoadAsset()
    {
        
    }

    protected IEnumerator LoadAssetAsync()
    {
        yield return LoadAndCache("");

        var asyncReq = this.bundle.LoadAssetAsync(this.assetName);
        yield return asyncReq;


    }

    private void LoadNoCache()
    {

    }

    private IEnumerator LoadAndCache(string url)
    {
        while (!Caching.ready)
            yield return null;

        using (WWW www = WWW.LoadFromCacheOrDownload(url, this.version))
        {
            yield return www;
            if (www.error != null)
                throw new System.Exception("WWW download had an error:" + www.error);
            this.bundle = www.assetBundle;
                
        }
    }
}

public class BundleIcon : BundleData
{

}

public class BundleModel : BundleData
{

}

public class BundleTexture : BundleData
{

}

public class BundleScene : BundleData
{

}

public class BundleAudio : BundleData
{

}

public class BundlePrefab : BundleData
{

}
