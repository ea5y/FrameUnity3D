using Easy.FrameUnity.EsAssetBundle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Easy.FrameUnity.Util;

namespace Easy.FrameUnity.Manager
{
    public class AssetPoolManager : Singleton<AssetPoolManager>
    {
        private ObjectPool<AssetData> _assetPool;
        public void CreateAssetPool(int initSize, int capacity)
        {
            _assetPool = new ObjectPool<AssetData>(initSize, capacity);
        }

        private void Awake()
        {
            base.GetInstance();
            this.CreateAssetPool(10, 20);
        }

        public void FindAsset<AssetType, CallbackParamType>(string assetPath, string assetName,
                Action<CallbackParamType> callback) where AssetType : AssetData, new()
        {
            //Get poolitem
            //  has innerobj
            //      callback(innerobj)
            //  has't innerobj
            //      create(param)
            //      callback(innerobj)
            //Free poolitem

            /*
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
                StartCoroutine(coroutine);
            }

            _assetPool.FreeUsingItem(pItem);
            */
            StartCoroutine(_Find<AssetType, CallbackParamType>(assetPath, assetName, callback));
        }

        private IEnumerator _Find<AssetType, CallbackParamType>(string assetPath, string assetName,
                Action<CallbackParamType> callback) where AssetType : AssetData, new()
        {
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
                Action<CallbackParamType> callback) where AssetType : IDynamicObject, new()
        {
            yield return poolItem.CreateInnerObject<AssetType>(param);

            _assetPool.UpdatePoolItemIdentifier(poolItem);

            callback((CallbackParamType)poolItem.InnerObject);
        }

        private void OnDestroy()
        {
            _assetPool.Release();
        }
    }
}