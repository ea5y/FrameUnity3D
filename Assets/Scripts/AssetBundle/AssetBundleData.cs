using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Easy.FrameUnity.Util;

namespace Easy.FrameUnity.EsAssetBundle
{
    public struct CreateAssetPoolItemParam
    {
        public string BundleName;
        public string PrePath;
        public string AssetName;
    }

    public abstract class BundleData
    {
        public AssetBundle bundle;
        protected string bundlePath;
        protected string bundleName;
        protected string bundleSuffix = "";

        private bool _isBundleLoaded;
        public bool IsBundleLoaded
        {
            get{ return _isBundleLoaded;}
            protected set{_isBundleLoaded = value;}
        }

        protected IEnumerator LoadBundle(string url)
        {
            Debug.Log("Load Bundle...");
            if(this.bundle != null)
                yield break;

            using(WWW www = new WWW(url))
            {
                yield return www;
                if(www.error != null)
                    throw new System.Exception("WWW download had an error:" + www.error);
                this.bundle = www.assetBundle;
                if(this.bundle != null)
                {
                    var msg = string.Format("Bundle: {0} load success!", this.bundle);
                    Debug.Log(msg);
                    this.IsBundleLoaded = true;
                }
                else
                {
                    this.IsBundleLoaded = false;
                }
            }
        }
    }

    public class AssetData : BundleData, IDynamicObject 
    {
        protected string assetPath;
        protected string assetName;
        protected string assetSuffix = "";

        private object _asset;

        protected static Dictionary<string, BundleData> bundleDic = new Dictionary<string, BundleData>();

        public bool IsValidate
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Identifier { get; set; }

        public DateTime Timestamp { get; set; }

        public AssetData()
        {
            this.Timestamp = DateTime.Now;
        }

        protected bool FindBundle(string bundleName, out BundleData bundle)
        {
            var msg = string.Format("===>Find bundle from bundleDic");
            Debug.Log(msg);
            if(bundleDic.TryGetValue(bundleName, out bundle))
            {
                return true;
            }
            bundleDic.Add(bundleName, this);
            return false;
        }

        protected void LoadAsset()
        {
            var msg = "Load Asset...";
            Debug.Log(msg);

            var strArray = this.bundle.GetAllAssetNames();
            for(int i = 0; i < strArray.Length; i++)
            {
                msg = string.Format("\nAssetName: {0}", strArray[i]);
                Debug.Log(msg);
            }

            //Assign param to attribute
            var asset = this.bundle.LoadAsset(this.assetName + this.assetSuffix);

            if(asset == null)
            {
                msg = string.Format("Load asset error: " + "\nbundle===>" + URL.ASSETBUNDLE_LOCAL_URL + this.bundleName+ " not has asset:" + this.assetName + this.assetSuffix);
                Debug.LogError(msg);
                return;
            }

            msg = string.Format("Asset: {0}", asset);
            Debug.Log(msg);
            _asset = asset;
        }

        protected IEnumerator HangUpForWaitLoadingBundle(BundleData bundle)
        {
            while (!bundle.IsBundleLoaded)
                yield return null;
        }

        public virtual IEnumerator Create(object param)
        {
            CreateAssetPoolItemParam _param = (CreateAssetPoolItemParam)param;
            this.bundleName = _param.BundleName;
            this.assetName = _param.AssetName;
            this.Identifier = this.bundleName + this.assetName;
            BundleData bundle;
            if(!this.FindBundle(_param.BundleName, out bundle))
            {
                //Load Bundle
                yield return this.LoadBundle(URL.ASSETBUNDLE_LOCAL_URL + _param.BundleName + this.bundleSuffix);
            }
            else
            {
                if (!bundle.IsBundleLoaded)
                {
                    yield return this.HangUpForWaitLoadingBundle(bundle);
                }
                this.bundle = bundle.bundle;
                this.IsBundleLoaded = true;
            }

            //Load Asset
            this.LoadAsset();
        }

        public void Release()
        {
            _asset = null;
        }

        public object GetInnerObject()
        {
            return _asset;
        }
    }

    public class AssetPrefab : AssetData
    {
        public override IEnumerator Create(object param)
        {
            this.assetSuffix = ".prefab";
            yield return base.Create(param);
        }
    }

    public class AssetScriptableObject : AssetData
    {
        public override IEnumerator Create(object param)
        {
            this.assetSuffix = ".asset";
            yield return base.Create(param);
        }
    }

}
