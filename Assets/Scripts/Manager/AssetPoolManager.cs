using Easy.FrameUnity.EsAssetBundle;
using System;
using System.Collections;
using Easy.FrameUnity.Util;
using UnityEngine;
using System.Threading;

namespace Easy.FrameUnity.Manager
{
    public class AssetPoolManager : ManagerBase<AssetPoolManager>
    {
        private ObjectPoolUtil<AssetData> _assetPool;
        public void CreateAssetPool(int initSize, int capacity)
        {
            _assetPool = new ObjectPoolUtil<AssetData>(initSize, capacity);
        }

        private void Awake()
        {
            base.GetInstance();
        }

        public override void Init()
        {
            this.CreateAssetPool(10, 20);
            GCCollectionManager.Inst.AddCollection(_assetPool.GCCollection, CollectionType.Asset);
        }

        public void FindAsset<AssetType, CallbackParamType>(string assetPath, string assetName,
                Action<CallbackParamType> callback) where AssetType : AssetData, new() where CallbackParamType : ScriptableObject
        {
            //Get poolitem
            //  has innerobj
            //      callback(innerobj)
            //  has't innerobj
            //      create(param)
            //      callback(innerobj)
            //Free poolitem
            StartCoroutine(_FindAsset<AssetType, CallbackParamType>(assetPath, assetName, callback));
        }

        private IEnumerator _FindAsset<AssetType, CallbackParamType>(string assetPath, string assetName,
                Action<CallbackParamType> callback) where AssetType : AssetData, new() where CallbackParamType : ScriptableObject
        { 
            if(this.manifest == null)
                yield return LoadManifest();
        
            var identifier = assetPath + assetName;
            PoolItem<AssetData> pItem = _assetPool.FindPoolItem(identifier);
            if (pItem.HasInnerObject)
            {
                callback((CallbackParamType)pItem.InnerObject);
            }
            else
            {
                CreateAssetPoolItemParam param = new CreateAssetPoolItemParam();
                param.BundleName = assetPath;
                param.AssetName = assetName;
                var coroutine = _CreateInnerObject<AssetType, CallbackParamType>(pItem, param, callback);
                yield return coroutine;
            }

            _assetPool.FreeUsingItem(pItem);
        }

        private IEnumerator _CreateInnerObject<AssetType, CallbackParamType>(
                PoolItem<AssetData> poolItem,
                CreateAssetPoolItemParam param,
                Action<CallbackParamType> callback) where AssetType : IDynamicObject, new() where CallbackParamType : ScriptableObject
        {
            yield return poolItem.CreateInnerObject<AssetType, CallbackParamType>(param);

            _assetPool.UpdatePoolItemIdentifier(poolItem);

            callback((CallbackParamType)poolItem.InnerObject);
        }

        AssetBundleManifest manifest;
        private IEnumerator LoadManifest()
        {
            using(WWW www = new WWW(URL.FILE_ASSETBUNDLE_LOCAL_URL + "win"))
            {
                yield return www;
                if(www.error != null)
                {
                    throw new System.Exception("WWW download had an error:" + www.error);
                }
                var bundle = www.assetBundle;
                if(bundle != null)
                {
                    manifest = (AssetBundleManifest)bundle.LoadAsset("AssetBundleManifest");
                    string[] dependencies = manifest.GetAllDependencies("panelmain");
                    foreach(var d in dependencies)
                    {
                        yield return Test(d);
                    }
                }
            }
        }

        private IEnumerator Test(string bundleName)
        {
            using(WWW www = new WWW(URL.FILE_ASSETBUNDLE_LOCAL_URL + bundleName))
            {
                yield return www;
                if(www.error != null)
                {
                    throw new System.Exception("WWW download had an error:" + www.error);
                }
                var bundle = www.assetBundle;
                if(bundle != null)
                {
                    www.Dispose();
                }
            }
        }

        private void OnDestroy()
        {
            if(_assetPool != null)
                _assetPool.Release();
        }
    }
}
