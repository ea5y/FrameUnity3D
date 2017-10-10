//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-06-18 11:13
//================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Easy.FrameUnity.ScriptableObj;
public class BundleManager : MonoBehaviour
{
    [SerializeField]
    private Dictionary<string, BundleData> bundleDataDic = new Dictionary<string, BundleData>();
    private Dictionary<string, BundleData> bundleDataRefDic = new Dictionary<string, BundleData>();

    public List<string> bundleDataList = new List<string>();
    public static BundleManager Instance { get; set; }

    public BundleIcon Icon{ get { return new BundleIcon(); } }
	public BundleModel Model{ get { return new BundleModel(); } }
	public BundlePrefab Prefab{ get { return new BundlePrefab(); } }
	public BundleTexture Texture{ get { return new BundleTexture(); } } 
	public BundleScene Scene{ get { return new BundleScene(); } }
	public BundleAudio Audio{ get { return new BundleAudio(); } }

    private void Awake()
    {
        Instance = this;
        StartCoroutine(this.ReleaseMemoryInterval());
    }

    private IEnumerator ReleaseMemoryInterval()
    {
        while(true)
        {
            yield return new WaitForSeconds(10);
            this.BundleUnload();
            this.AssetUnload();
        }
    }

    private void BundleUnload()
    {
        Debug.Log("Bundle Unload");
        foreach(var data in bundleDataDic)
        {
            if(data.Value.bundle != null)
            {
                data.Value.bundle.Unload(false);
                data.Value.bundle = null;
                data.Value.isLoaded = false;
                data.Value.isSuccess = false;
                data.Value.error = "";
            }
        }
        this.bundleDataDic.Clear();
    }

    private void AssetUnload()
    {
        Debug.Log("Asset Unload");
        Resources.UnloadUnusedAssets();
    }

    private bool GetBundleData<T>(string bundleName, out T data) where T : BundleData,new()
    {
        BundleData d;
        var result = this.bundleDataDic.TryGetValue(bundleName, out d);
        if(!result)
        {
            d = new T();
            this.bundleDataDic.Add(bundleName, d);
            this.bundleDataList.Add(bundleName);
        }
        data = d as T;
        return result;
    }

    public void GetAll()
    {
    }

    public IEnumerator LoadAndCache(string bundlePath)
    {
        //
        Debug.Log("Loading...");
#if UNITY_EDITOR
        Caching.CleanCache();
#endif
        while(!Caching.ready)
            yield return null;

        using(WWW www = WWW.LoadFromCacheOrDownload(URL.ASSETBUNDLE_HOST_URL + bundlePath, 0))
        {
            yield return www;
            if(www.error != null)
                throw new Exception("WWW download had an error:" + www.error);

            AssetBundle bundle = www.assetBundle;
            AssetBundleManifest manifest = bundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
        }

    }

    public void GetPrefab(string assetPath, string assetName, Action<GameObject> callback)
    {
        BundlePrefab prefab;
        var isHangUp = this.GetBundleData<BundlePrefab>(assetPath, out prefab);
        var coroutine = prefab.Load(assetPath, assetName, isHangUp, callback);
        StartCoroutine(coroutine);
    }

    public void GetPrefabAsync(string assetPath, string assetName, Action<GameObject> callback)
    {
        BundlePrefab prefab;
        var isHangUp = this.GetBundleData<BundlePrefab>(assetPath, out prefab);
        var coroutine = prefab.LoadAsync(assetPath, assetName, isHangUp, callback);
        StartCoroutine(coroutine);
    }

    public void GetAsset(string assetPath, string assetName, Action<PanelInfo> callback)
    {
        BundleAsset asset;
        var isHangUp = this.GetBundleData<BundleAsset>(assetPath, out asset);
        var coroutine = asset.Load(assetPath, assetName, isHangUp, callback);
        StartCoroutine(coroutine);
    }

    public void GetAssetAsync(string assetPath, string assetName, Action<PanelInfo> callback)
    {
        BundleAsset asset;
        var isHangUp = this.GetBundleData<BundleAsset>(assetPath, out asset);
        var coroutine = asset.LoadAsync(assetPath, assetName, isHangUp, callback);
        StartCoroutine(coroutine);
    }
}
