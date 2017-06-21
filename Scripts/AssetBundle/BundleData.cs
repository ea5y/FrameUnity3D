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
        //FiberManager.AddFiber(Dispos());
        this.bundle.Unload(false);
    }

    private IEnumerator Dispos()
    {
        this.bundle.Unload(false);
        yield return new WaitSecondsForFiber(0.01f);
    }

    protected void LoadAsset(Action<UnityEngine.Object> callback)
    {
        var url = URL.ASSETBUNDLE_URL + assetName + suffix;
		Debug.Log("AssetBundleURL: " + url);
		Debug.Log("AssetBundle: " + this.bundle);

		Debug.Log("AssetBundleName: " + this.assetName + endWith);
        var asset = this.bundle.LoadAsset("Cube" + endWith);
		Debug.Log("Asset: " + asset);

        callback(asset);
    }

    public IEnumerator LoadAssetAsync(Action<UnityEngine.Object> callback)
    {
		var url = URL.ASSETBUNDLE_URL + assetName + suffix;
		Debug.Log("AssetBundleURL: " + url);
		Debug.Log("AssetBundle: " + this.bundle);
		
        var asyncReq = this.bundle.LoadAssetAsync("Cube" + endWith);
		Debug.Log("Req: " + asyncReq);
		Debug.Log("AssetBundleName: " + this.assetName + endWith);
        yield return asyncReq;

		var asset = asyncReq.asset as GameObject;
		Debug.Log("Asset: " + asset);
        callback(asset);
    }

    private void LoadNoCache()
    {

    }

    protected IEnumerator LoadAndCache(string url)
    {
            Debug.Log("Loading...");
            while (!Caching.ready)
                yield return null;

            /*
            using (WWW wwwa = WWW.LoadFromCacheOrDownload(URL.ASSETBUNDLE_URL + "AssetBundles",0))
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

                using (WWW www = WWW.LoadFromCacheOrDownload(url, this.version))
                {
                    yield return www;
                    if (www.error != null)
                        throw new System.Exception("WWW download had an error:" + www.error);
                    this.bundle = www.assetBundle;

                }
            }
            */

            using (WWW www = WWW.LoadFromCacheOrDownload(url, this.version))
            {
                yield return www;
                if (www.error != null)
                    throw new System.Exception("WWW download had an error:" + www.error);
                this.bundle = www.assetBundle;
                this.isLoaded = true;
            }
    }
}

public class BundleIcon : BundleData
{
	public void GetIcon(string assetPath, string assetName, Action<UIAtlas, string> callback)
	{
		this.assetPath = assetPath;
		this.assetName = assetName;
		this.suffix = ".unity3d";
		FiberManager.AddFiber(this.LoadAssetAsync((obj)=>{
					//var atlas = obj as UIAtlas;
					//callback(atlas, assetName);
					}));
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
    /*
    public IEnumerator GetPrefab(string assetPath, string assetName, Action<UnityEngine.Object> callback)
    {
		this.assetPath = assetPath;
		this.assetName = assetName;
		this.endWith = ".prefab";

		var url = URL.ASSETBUNDLE_URL + assetName + suffix;
        //yield return this.LoadAndCache(url);
        //while (this.bundle == null)
        //    yield return null;
        this.LoadAsset(callback);
    }
    */

    public IEnumerator GetPrefabAsync(string assetPath, string assetName, Action<UnityEngine.Object> callback)
    {
		this.assetPath = assetPath;
		this.assetName = assetName;
		this.endWith = ".prefab";
        yield return this.LoadAssetAsync(callback);
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
		this.assetPath = assetPath;
		this.assetName = assetName;
		this.endWith = ".prefab";
		var url = URL.ASSETBUNDLE_URL + assetName + suffix;
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
		this.assetPath = assetPath;
		this.assetName = assetName;
		this.endWith = ".prefab";
		var url = URL.ASSETBUNDLE_URL + assetName + suffix;
        yield return this.LoadAndCache(url);
        yield return this.LoadAssetAsync(callback);
    }

        /*
	public void GetPrefab(string assetPath, string assetName, Action<GameObject> callback)
	{
		this.assetPath = assetPath;
		this.assetName = assetName;
		this.endWith = ".prefab";
		FiberManager.AddFiber(this.LoadAssetAsync((obj)=>{
					//var atlas = obj as UIAtlas;
					callback(obj);
					}));
	}
    */
}
