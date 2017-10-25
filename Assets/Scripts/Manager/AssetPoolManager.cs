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

        private int GetTimeSpanSecond(DateTime start, DateTime end)
        {
            TimeSpan ts1 = new TimeSpan(start.Ticks);
            TimeSpan ts2 = new TimeSpan(end.Ticks);
            TimeSpan ts3 = ts1.Subtract(ts2).Duration();

            return ts3.Milliseconds;
        }

        private void OnDestroy()
        {
            if(_assetPool != null)
                _assetPool.Release();
        }
    }
}
