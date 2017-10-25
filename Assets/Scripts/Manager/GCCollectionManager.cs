//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-20 09:52
//================================
using System;
using System.Threading;

namespace Easy.FrameUnity.Manager
{

    public enum CollectionType
    {
        Bundle,
        Asset
    }

    public class GCCollectionManager : Singleton<GCCollectionManager>
    {
        private object _syncObj = new object();
        private bool _isStop = false;
        public bool IsStop
        {
            get
            {
                return _isStop;
            }
            set
            {
                lock(_syncObj)
                {
                    _isStop = value;
                }
            }
        }

        private Action _collectionBundle;
        private Action _collectionAsset;

        private void Awake()
        {
            base.GetInstance();
        }

        public void AddCollection(Action collection, CollectionType type)
        {
            WaitCallback task = null;
            switch(type)
            {
                case CollectionType.Bundle:
                    _collectionBundle = collection;
                    task = new WaitCallback(CollectionBundle);
                    break;
                case CollectionType.Asset:
                    _collectionAsset = collection;
                    task = new WaitCallback(CollectionAsset);
                    break;
            }

            ThreadPool.QueueUserWorkItem(task);
        }

        private void CollectionBundle(object state)
        {
            while(!_isStop)
            {
                Thread.Sleep(20000);
                Net.Net.InvokeAsync(() => _collectionBundle.Invoke());
            }
        }

        private void CollectionAsset(object state)
        {
            while(!_isStop)
            {
                Thread.Sleep(10000);
                Net.Net.InvokeAsync(() => _collectionAsset.Invoke());
            }
        }

        private void OnDestroy()
        {
            this.IsStop = true;
        }
    }
}
