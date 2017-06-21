//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-06-18 11:13
//================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

    private bool GetBundleData<T>(string assetName, out T data) where T : BundleData,new()
    {
        BundleData d;
        var result = this.bundleDataDic.TryGetValue(assetName, out d);
        if(!result)
        {
            d = new T();
            this.bundleDataDic.Add(assetName, d);
            this.bundleDataList.Add(assetName);
        }
        data = d as T;
        return result;
    }

    public void GetPrefab(string assetPath, string assetName, Action<GameObject> callback)
    {
        BundlePrefab prefab;
        if(this.GetBundleData<BundlePrefab>(assetName, out prefab))
        {
            //hang up then load asset
            var co = prefab.HangUPAndLoadAsset(assetPath, assetName, (obj) => { callback(obj as GameObject); });
            StartCoroutine(co);
        }
        else
        {
            //load bundle and load asset
            var co = prefab.LoadBundleAndLoadAsset(assetPath, assetName, (obj) => { callback(obj as GameObject); });
            StartCoroutine(co);
        }
    }

    public void GetPrefabAsync(string assetPath, string assetName, Action<GameObject> callback)
    {
        BundlePrefab prefab;
        if(this.GetBundleData<BundlePrefab>(assetName, out prefab))
        {
            //hang up then load asset
            var co = prefab.HangUPAndLoadAssetAsync(assetPath, assetName, (obj) => { callback(obj as GameObject); });
            StartCoroutine(co);
        }
        else
        {
            //load bundle and load asset
            var co = prefab.LoadBundleAndLoadAssetAsync(assetPath, assetName, (obj) => { callback(obj as GameObject); });
            StartCoroutine(co);
        }
    }
}
