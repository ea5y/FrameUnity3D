//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-10 19:50
//================================

using UnityEngine;
using XLua;
using System;
namespace Easy.FrameUnity.Manager
{
    /*
    public abstract class ManagerBase<T> : Singleton<ManagerBase>
    {
        public abstract void Init();
    }
    */

    public class LuaTableManager : Singleton<LuaTableManager>
    {
        private void Awake()
        {
            base.GetInstance();
        }

        private void Init()
        {

        }
    }
}
