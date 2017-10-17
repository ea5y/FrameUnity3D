using Easy.FrameUnity.EsAssetBundle;
using System;
using System.Collections;
using Easy.FrameUnity.Util;

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
            StartCoroutine(_FindAsset<AssetType, CallbackParamType>(assetPath, assetName, callback));
        }

        private IEnumerator _FindAsset<AssetType, CallbackParamType>(string assetPath, string assetName,
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
            if(_assetPool != null)
                _assetPool.Release();
        }
    }
}