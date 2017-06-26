using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class BundleData
{
    public string assetPath;
    public string assetName;
    public string endWith;
    public string suffix = "";

    public int version = 0;

    public AssetBundle bundle;
    public bool isLoaded = false;
    public bool isSuccess = false;
    public string error = "";

    protected void LoadAllAssets()
    {

    }

    ~BundleData()
    {
        Debug.Log("Releas");
    }

    public IEnumerator HangUPAndLoadAsset(string assetPath, string assetName, Action<UnityEngine.Object> callback)
    {
        Debug.Log("HangUp");
        while (this.isLoaded == false || this.bundle == null)
            yield return null;
        this.LoadAsset(callback);
    }

    public IEnumerator LoadBundleAndLoadAsset(string assetPath, string assetName, Action<UnityEngine.Object> callback)
    {
        Debug.Log("LoadBundle");
		var url = URL.ASSETBUNDLE_LOCAL_URL + this.assetPath + suffix;
        Debug.Log("URL: " + url);
        yield return this.LoadAndCache(url);
        this.LoadAsset(callback);
    }

    public IEnumerator HangUPAndLoadAssetAsync(string assetPath, string assetName, Action<UnityEngine.Object> callback)
    {
        Debug.Log("HangUp");
        while (this.isLoaded == false || this.bundle == null)
            yield return null;
        yield return this.LoadAssetAsync(callback);
    }

    public IEnumerator LoadBundleAndLoadAssetAsync(string assetPath, string assetName, Action<UnityEngine.Object> callback)
    {
        Debug.Log("LoadBundle");
		var url = URL.ASSETBUNDLE_LOCAL_URL + this.assetPath + suffix;
        yield return this.LoadAndCache(url);
        yield return this.LoadAssetAsync(callback);
    }

    protected void LoadAsset(Action<UnityEngine.Object> callback)
    {
		Debug.Log("Bundle: " + this.bundle);
        Debug.Log("LoadAsset...");

        var strArray = this.bundle.AllAssetNames();
        for(int i = 0; i < strArray.Length; i++)
        {

            Debug.Log("\nAssetName: " + strArray[i]);
        }
        var asset = this.bundle.LoadAsset(this.assetName + endWith);
        if(asset == null)
        {
            Debug.LogError("Load asset error: " + "\nbundle===>" + URL.ASSETBUNDLE_LOCAL_URL + this.assetPath + " not has asset:" + this.assetName + this.endWith);
            return;
        }
            
		Debug.Log("Asset: " + asset);

        callback(asset);
    }

    public IEnumerator LoadAssetAsync(Action<UnityEngine.Object> callback)
    {
		Debug.Log("Bundle: " + this.bundle);
		Debug.Log("BundleName: " + this.assetName + endWith);
        Debug.Log("LoadAssetAsync...");
		
        var asyncReq = this.bundle.LoadAssetAsync(this.assetName + endWith);
        yield return asyncReq;

		var asset = asyncReq.asset;
        if(asset == null)
        {
            Debug.LogError("Load asset error: " + "\nbundle===>" + URL.ASSETBUNDLE_LOCAL_URL + this.assetPath + " not has asset:" + this.assetName + this.endWith);
            yield break;
        }
        
		Debug.Log("Asset: " + asset);
        callback(asset);
    }

    private void LoadNoCache()
    {
        

    }

    protected IEnumerator LoadAndCache(string url)
    {
            Debug.Log("Loading...");

            //will cache to unity's cache folder in the local stroage device
            //so, must clean cache for debug
#if UNITY_EDITOR
            Caching.CleanCache();
#endif
            while (!Caching.ready)
                yield return null;

            using (WWW wwwa = WWW.LoadFromCacheOrDownload(URL.ASSETBUNDLE_LOCAL_URL + "Android",0))
            {
                yield return wwwa;
                if(wwwa.error != null)
                {
                    throw new Exception("WWW download had an error:" + wwwa.error);
                }

                AssetBundle bundle = wwwa.assetBundle;
                AssetBundleManifest abMainfest = bundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
                bundle.Unload(false);
                if(abMainfest == null)
                {
                    Debug.Log("abManifest is null");
                    yield return null;
                }
                else
                {
                    string[] depNames = abMainfest.GetAllDependencies(assetName);
                    Debug.Log("depNames length:" + depNames.Length);
                }
            }

            using (WWW www = WWW.LoadFromCacheOrDownload(url, this.version))
            {
                yield return www;
                if (www.error != null)
                    throw new System.Exception("WWW download had an error:" + www.error);
                this.bundle = www.assetBundle;
                if(this.bundle != null)
                {
                    Debug.Log("LoadBundle success!");
                }
                this.isLoaded = true;
            }
    }
}

public class BundleIcon : BundleData
{
    private UIAtlas atlas;

    public IEnumerator Load(string assetPath, string assetName, bool isHangUp, Action<UIAtlas, string> callback)
    {
        if(this.atlas == null)
        {
            this.assetPath = assetPath;
            this.assetName = assetName;
            this.endWith = ".atlas";
            if(isHangUp)
            {
                yield return this.HangUPAndLoadAsset(assetPath, assetName, 
                        (obj)=>
                        {
                        this.atlas = obj as UIAtlas;
                        callback(this.atlas, this.assetName);
                        });
            }
            else
            {
                yield return this.LoadBundleAndLoadAsset(assetPath, assetName, 
                        (obj)=>
                        {
                        this.atlas = obj as UIAtlas;
                        callback(this.atlas, this.assetName);
                        });
            }
        }
        else
        {
            callback(this.atlas, this.assetName);
        }
    }

    public IEnumerator LoadAsync(string assetPath, string assetName, bool isHangUp, Action<UIAtlas, string> callback)
    {
        if(this.atlas == null)
        {
            this.assetPath = assetPath;
            this.assetName = assetName;
            this.endWith = ".atlas";
            if(isHangUp)
            {
                yield return this.HangUPAndLoadAssetAsync(assetPath, assetName, 
                        (obj)=>
                        {
                        this.atlas = obj as UIAtlas;
                        callback(this.atlas, this.assetName);
                        });
            }
            else
            {
                yield return this.LoadBundleAndLoadAssetAsync(assetPath, assetName,
                        (obj)=>
                        {
                        this.atlas = obj as UIAtlas;
                        callback(this.atlas, this.assetName);
                        });
            }
        }
        else
        {
            callback(this.atlas, this.assetName);
        }
    }
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
    private GameObject prefab;

    public IEnumerator Load(string assetPath, string assetName, bool isHangUp, Action<GameObject> callback)
    {
        if(this.prefab == null)
        {
            this.assetPath = assetPath;
            this.assetName = assetName;
            this.endWith = ".prefab";
            if(isHangUp)
            {
                yield return this.HangUPAndLoadAsset(assetPath, assetName, 
                        (obj)=>
                        {
                        this.prefab = obj as GameObject;
                        callback(this.prefab);
                        });
            }
            else
            {
                yield return this.LoadBundleAndLoadAsset(assetPath, assetName, 
                        (obj)=>
                        {
                        this.prefab = obj as GameObject;
                        callback(this.prefab);
                        });
            }
        }
        else
        {
            callback(this.prefab);
        }
    }

    public IEnumerator LoadAsync(string assetPath, string assetName, bool isHangUp, Action<GameObject> callback)
    {
        if(this.prefab == null)
        {
            this.assetPath = assetPath;
            this.assetName = assetName;
            this.endWith = ".prefab";
            if(isHangUp)
            {
                yield return this.HangUPAndLoadAssetAsync(assetPath, assetName, 
                        (obj)=>
                        {
                        this.prefab = obj as GameObject;
                        callback(this.prefab);
                        });
            }
            else
            {
                yield return this.LoadBundleAndLoadAssetAsync(assetPath, assetName, 
                        (obj)=>
                        {
                        this.prefab = obj as GameObject;
                        callback(this.prefab);
                        });
            }
        }
        else
        {
            callback(this.prefab);
        }
    }
}
