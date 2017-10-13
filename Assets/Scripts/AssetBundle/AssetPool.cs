//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-13 11:09
//================================

using System;
using System.Collections;
using System.Collections.Generic;
namespace Easy.FrameUnity.EsAssetBundle
{
    public interface IDynamicObject
    {
        bool IsValidate{get;set;}
        string Identifier{get;set;}
        IEnumerator Create(object param);
        void Release();
        object GetInnerObject();
    }

    public struct CreateAssetPoolItemParam<T>
    {
        public string BundleName;
        public string PrePath;
        public string AssetName;
        public Action<T> Callback;
    }

    public class PoolItem<T> where T : IDynamicObject , new()
    {
        private IDynamicObject _object;
        private object _createParam;

        public bool HasInnerObject{get;private set;}
        public object InnerObject
        {
            get{return _object.GetInnerObject();}
        }

        public string Identifier
        {
            get{return _object.Identifier;}
        }

        public bool IsValidate
        {
            get{return _object.IsValidate;}
        }

        private bool _isUsing;
        public bool IsUsing
        {
            get{return _isUsing;}
            set{_isUsing = value;}
        }

        public PoolItem()
        {
            this.HasInnerObject = false;
        }

        /*
        public PoolItem(object param)
        {
            _createParam = param;
            this.Create();
        }
        */

        public void Create(object param)
        {
            _createParam = param;
            _object = new T();
            _object.Create(_createParam);
            this.HasInnerObject = true;
        }

        public IEnumerator CreateInnerObject<K>(object param) where K : IDynamicObject, new()
        {
            _createParam = param;
            _object = new K();
            yield return _object.Create(_createParam);
            this.HasInnerObject = true;
        }

        public void Recreate()
        {
            this.Release();
            this.Create(_createParam);
        }

        public void Release()
        {
            _object.Release();
            _object = null;
            this.HasInnerObject = false;
        }

    }

    public sealed class ObjectPool<T> where T : IDynamicObject, new ()
    {
        private int _capacity;

        private int _currentSize;
        public int CurrentSize
        {
            get{return _currentSize;}
        }

        public int ActiveCount
        {
            get{return _hashSetUsingIndex.Count;}
        }

        private Hashtable _hashTableObject;

        private HashSet<string> _hashSetFreeIndex;
        private HashSet<string> _hashSetUsingIndex;

        public ObjectPool(int initSize, int capacity)
        {
            if(initSize < 0 || capacity < 1 || initSize > capacity)
            {
                throw(new System.Exception("Create ObjectPool with invalid parameter!"));
            }

            _capacity = capacity;
            _hashTableObject = new Hashtable(capacity);
            _hashSetFreeIndex = new HashSet<string>();
            _hashSetUsingIndex = new HashSet<string>();

            for(int i = 0; i < initSize; i++)
            {
                PoolItem<T> pItem = new PoolItem<T>();
                _hashTableObject.Add(pItem.Identifier, pItem);
                _hashSetFreeIndex.Add(pItem.Identifier);
            }
            
            _currentSize = _hashTableObject.Count;
        }

        public void Release()
        {
            lock(this)
            {
                foreach(DictionaryEntry de in _hashTableObject)
                {
                    ((PoolItem<T>)de.Value).Release();
                }

                _hashTableObject.Clear();
                _hashSetFreeIndex.Clear();
                _hashSetUsingIndex.Clear();
            }
        }

        public PoolItem<T> FindPoolItem(string identifier)
        {
            lock(this)
            {
                /*
                T innerObject;
                if(this.Find(identifier, out innerObject))
                {
                    return innerObject;
                }
                else
                {
                    //check if has free obj
                    //  No:Create a new PoolItem obj
                    //  yes:Get free obj
                    //PoolItem obj create inner obj
                }
                return innerObject;
                */

                PoolItem<T> pItem = null;
                if(!this.Find(identifier, out pItem))
                {
                    pItem = this.GetOneFreeItem();
                }
                return pItem;
            }
        }

        private bool Find(string identifier, out PoolItem<T> poolItem)
        {
            poolItem = null;
            PoolItem<T> pItem = (PoolItem<T>)_hashTableObject[identifier];
            if(pItem != null)
            {
                poolItem = pItem;
                return true;
            }
            return false;
        }

        private PoolItem<T> GetOneFreeItem()
        {
            lock(this)
            {
                if(_hashSetFreeIndex.Count == 0)
                {
                    if(_currentSize == _capacity)
                        throw new System.Exception("ObjectPool no free item and CurrentSize is Max!");

                    PoolItem<T> newItem = new PoolItem<T>();
                    _hashTableObject.Add(newItem.Identifier, newItem);
                    _hashSetFreeIndex.Add(newItem.Identifier);
                    _currentSize++;
                }
            }

            string freeKey = string.Empty;
            foreach(var key in _hashSetFreeIndex)
            {
                freeKey = key;
                break;
            }

            var pItem = (PoolItem<T>)_hashTableObject[freeKey];
            _hashSetFreeIndex.Remove(freeKey);
            _hashSetUsingIndex.Add(freeKey);

            if(!pItem.IsValidate)
                pItem.Recreate();

            pItem.IsUsing = true;

            return pItem;
        }

        public void FreeUsingItem(PoolItem<T> item)
        {
            lock(this)
            {
                if(_hashTableObject.ContainsKey(item.Identifier))
                {
                    item.IsUsing = false;
                    _hashSetUsingIndex.Remove(item.Identifier);
                    _hashSetFreeIndex.Add(item.Identifier);
                }
            }
        }

        public int DecreaseSize(int size)
        {
            int decreaseSize = size;
            lock(this)
            {
                if(decreaseSize <= 0)
                {
                    return 0;
                }
                if(decreaseSize > _hashSetFreeIndex.Count)
                {
                    decreaseSize = _hashSetFreeIndex.Count;
                }

                int counter = 1;
                var tempHashSet = new HashSet<string>();
                foreach(var key in _hashSetFreeIndex)
                {
                    if(counter > decreaseSize)
                        break;
                    tempHashSet.Add(key);
                    counter++;
                }

                foreach(var key in tempHashSet)
                {
                    _hashTableObject.Remove(key);
                }

                _hashSetFreeIndex.Clear();
                _hashSetUsingIndex.Clear();

                foreach(DictionaryEntry de in _hashTableObject)
                {
                    var pItem = (PoolItem<T>)de.Value;
                    if(pItem.IsUsing)
                    {
                        _hashSetUsingIndex.Add(pItem.Identifier);
                    }
                    else
                    {
                        _hashSetFreeIndex.Add(pItem.Identifier);
                    }
                }
            }
            _currentSize -= decreaseSize;
            return decreaseSize;
        }
    }

    public class AssetPool : Singleton<AssetPool>
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

        public T FindAsset<T>(string assetPath, string assetName) where T : AssetData
        {
            var identifier = assetPath + assetName;
            PoolItem<AssetData> pItem = _assetPool.FindPoolItem(identifier);
            if(pItem.HasInnerObject)
                return (T)pItem.InnerObject;
            else
            {
                pItem.Create(identifier);
                return (T)pItem.InnerObject;
            }
        }

        public void FindAsset<T>(string assetPath, string assetName, Action<T> callback) where T : AssetData, new()
        {
            //Get poolitem
            //  has innerobj
            //      callback(innerobj)
            //  has't innerobj
            //      create(param)
            //      callback(innerobj)
            //Free poolitem
            
            var identifier = assetPath + assetName;
            PoolItem<AssetData> pItem = _assetPool.FindPoolItem(identifier);
            if(pItem.HasInnerObject)
            {
                callback((T)pItem.InnerObject);
            }
            else
            {
                CreateAssetPoolItemParam<T> param = new CreateAssetPoolItemParam<T>();
                param.BundleName = assetPath;
                param.AssetName = assetName;
                param.Callback = callback;
                StartCoroutine(pItem.CreateInnerObject<T>(param));
            }
        }
    }
}
