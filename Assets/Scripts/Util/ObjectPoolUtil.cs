//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-13 11:09
//================================

using Easy.FrameUnity.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Easy.FrameUnity.Util
{
    public interface IDynamicObject
    {
        bool IsValidate{get;set;}
        string Identifier{get;set;}
        DateTime Timestamp { get; }
        IEnumerator Create<T>(object param) where T : ScriptableObject;
        void Release();
        object GetInnerObject();
    }

    public class PoolItem<T> where T : IDynamicObject, new()
    {
        private object _syncObj = new object();
        public IDynamicObject PoolObject { get; private set; }
        private object _createParam;

        public bool HasInnerObject { get; private set; }
        public object InnerObject
        {
            get { return PoolObject.GetInnerObject(); }
        }

        private static int _tempIdentifier = 0;
        private int _ownerTempIdentifier;
        public string OwnerTempIdentifier
        {
            get { return _ownerTempIdentifier.ToString(); }
        }

        public string Identifier
        {
            get
            {
                lock(_syncObj)
                {
                    if (PoolObject == null)
                        return _ownerTempIdentifier.ToString();
                    return PoolObject.Identifier;
                }
            }
        }


        public string GetTempIdentifier(bool isNew)
        {
            var id = isNew == true ? ++_tempIdentifier : _tempIdentifier;
            _ownerTempIdentifier = _tempIdentifier;
            return id.ToString();
        }

        public bool IsValidate
        {
            get{return PoolObject == null ? true : PoolObject.IsValidate;}
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

        public IEnumerator CreateInnerObject<AssetType, CallbackParamType>(object param) where AssetType : IDynamicObject, new() where CallbackParamType : ScriptableObject
        {
            _createParam = param;
            PoolObject = new AssetType();
            yield return PoolObject.Create<CallbackParamType>(_createParam);
            this.HasInnerObject = true;
        }

        public void Recreate()
        {
            //Release;
            //Create;
        }

        public void Release()
        {
            if (PoolObject == null)
                return;
            PoolObject.Release();
            PoolObject = null;
            this.HasInnerObject = false;
        }

    }

    public sealed class ObjectPoolUtil<T> where T : IDynamicObject, new ()
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

        private List<string> _hashSetFreeIndex;
        private List<string> _hashSetUsingIndex;

        private System.Timers.Timer _gcTimer;
        public ObjectPoolUtil(int initSize, int capacity, int gcInterval = 5)
        {
            this.Init(initSize, capacity);
            //Because i write a collection manager, so i am not use this timer, you can delete the annotations to enable
            //this.StartGCCollection(gcInterval);
        }

        private void Init(int initSize, int capacity)
        {
            if(initSize < 0 || capacity < 1 || initSize > capacity)
            {
                throw(new System.Exception("Create ObjectPool with invalid parameter!"));
            }

            _capacity = capacity;
            _hashTableObject = new Hashtable(capacity);
            _hashSetFreeIndex = new List<string>();
            _hashSetUsingIndex = new List<string>();

            for(int i = 0; i < initSize; i++)
            {
                PoolItem<T> pItem = new PoolItem<T>();
                _hashTableObject.Add(pItem.GetTempIdentifier(true), pItem);
                _hashSetFreeIndex.Add(pItem.GetTempIdentifier(false));
            }
            
            _currentSize = _hashTableObject.Count;
        }
        
        /*
        private void StartGCCollection(int gcInterval)
        {
            _gcTimer = new System.Timers.Timer(gcInterval * 1000);
            _gcTimer.Elapsed += new System.Timers.ElapsedEventHandler(GCCollection);
            _gcTimer.AutoReset = true;
            _gcTimer.Enabled = true;
            _gcTimer.Start();
        }
        */

        public void StopGCCollection()
        {
            _gcTimer.Stop();
        }

        public PoolItem<T> FindPoolItem(string identifier)
        {
            lock(this)
            {
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

        public void UpdatePoolItemIdentifier(PoolItem<T> poolItem)
        {
            _hashTableObject.Remove(poolItem.OwnerTempIdentifier);
            _hashSetUsingIndex.Remove(poolItem.OwnerTempIdentifier);
            _hashTableObject.Add(poolItem.Identifier, poolItem);
            _hashSetUsingIndex.Add(poolItem.Identifier);
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

            if(_gcTimer != null)
            {
                _gcTimer.Stop();
                _gcTimer = null;
            }
        }

        public void GCCollection()
        {
            lock(this)
            {
                Debug.Log("ObjectPool GCColleting...");
                var tempTable = new Hashtable(_hashTableObject);
                foreach(DictionaryEntry de in tempTable)
                {
                    var pItem = (PoolItem<T>)_hashTableObject[de.Key];
                    if(pItem.HasInnerObject && !pItem.IsUsing)
                    {
                        var timeSpan = this.GetTimeSpanSecond(pItem.PoolObject.Timestamp, DateTime.Now);
                        if (timeSpan >= 10)
                        {
                            var msg = string.Format("Collection Asset: {0}", pItem.PoolObject.GetType().Name);
                            Debug.Log(msg);
                            _hashSetFreeIndex.Remove(pItem.Identifier);
                            _hashTableObject.Remove(pItem.Identifier);

                            pItem.Release();

                            _hashSetFreeIndex.Add(pItem.Identifier);
                            _hashTableObject.Add(pItem.Identifier, pItem);
                        }
                    }
                }
            }
        }

        public void GCCollection(object source, System.Timers.ElapsedEventArgs e)
        {
            lock(this)
            {
                Debug.Log("ObjectPool GCColleting...");
                foreach(DictionaryEntry de in _hashTableObject)
                {
                    var pItem = (PoolItem<T>)de.Value;
                    if(pItem.HasInnerObject && !pItem.IsUsing)
                    {
                        var timeSpan = this.GetTimeSpanSecond(pItem.PoolObject.Timestamp, DateTime.Now);
                        if(timeSpan >= 10)
                        {
                            var msg = string.Format("Collection Asset: {0}", pItem.PoolObject.GetType().Name);
                            Debug.Log(msg);
                            _hashSetFreeIndex.Remove(pItem.Identifier);
                            _hashTableObject.Remove(pItem.Identifier);

                            pItem.Release();
                            _hashSetFreeIndex.Add(pItem.Identifier);
                            _hashTableObject.Add(pItem.Identifier, pItem);

                        }
                    }
                }
            }
        }

        private int GetTimeSpanSecond(DateTime start, DateTime end)
        {
            TimeSpan ts1 = new TimeSpan(start.Ticks);
            TimeSpan ts2 = new TimeSpan(end.Ticks);
            TimeSpan ts3 = ts1.Subtract(ts2).Duration();

            return ts3.Seconds;
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
}
