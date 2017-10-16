//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-16 13:23
//================================

using System;
using System.Collections.Generic;
using UnityEngine;
namespace Easy.FrameUnity.Manager
{
    public interface IManager
    {
        void Init();
    }

    public abstract class ManagerBase<T> : Singleton<T>, IManager where T : ManagerBase<T>
    {
        public abstract void Init();
    }

    public class ManagerCollections
    {
        //There are some managers that need to be inited,sort by priority
        public static List<IManager> ManagerList = new List<IManager>(){
            AssetPoolManager.Inst,
            HotfixManager.Inst,
        };
    }
}
