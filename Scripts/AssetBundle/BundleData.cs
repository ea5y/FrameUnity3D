using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class BundleData
{
    public string assetPath;
    public string assetName;
	public string endWith;
	public string suffix = ".unity3d";

    public int version = 0;

    public AssetBundle bundle;

    protected void LoadAllAssets()
    {

    }

    protected void LoadAsset()
    {
        
    }

    protected IEnumerator LoadAssetAsync(Action<GameObject> callback)
    {
		var url = URL.ASSETBUNDLE_URL + assetName + suffix;
        yield return LoadAndCache(url);
		Debug.Log("AssetBundleURL: " + url);
		Debug.Log("AssetBundle: " + this.bundle);
		
        var asyncReq = this.bundle.LoadAsset(this.assetName + endWith);
		Debug.Log("Req: " + asyncReq);
		Debug.Log("AssetBundleName: " + this.assetName + endWith);
		//while(!asyncReq.isDone)
		//	yield return null;
			//yield return new WaitSecondsForFiber(0.5f);
        //yield return asyncReq;

		//var asset = asyncReq.asset as GameObject;
		//Debug.Log("Asset: " + asset);
		callback((GameObject)asyncReq);

		this.bundle.Unload(false);
    }

    private void LoadNoCache()
    {

    }

    private IEnumerator LoadAndCache(string url)
    {
		Debug.Log("Loading...");
        while (!Caching.ready)
            yield return null;

		using (WWW wwwa = WWW.LoadFromCacheOrDownload(URL.ASSETBUNDLE_URL + "Android",0))
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
}
