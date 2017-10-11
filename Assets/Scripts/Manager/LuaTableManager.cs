//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-10 19:50
//================================

using UnityEngine;
using XLua;
using System;
using Easy.FrameUnity.Base;
namespace Easy.FrameUnity.Manager
{
    /*
    public abstract class ManagerBase<T> : Singleton<ManagerBase>
    {
        public abstract void Init();
    }
    */

    [CSharpCallLuaAttribute]
    public interface ILuaTable : IPanel
    {
        void Awake();
        void Start();
        void OnEnable();
        void OnDisable();
        void Update();
        void OnDestroy();
    }

    public class LuaTableManager : Singleton<LuaTableManager>
    {
        private LuaEnv _luaEnv;

        private void Awake()
        {
            base.GetInstance();
        }

        private void Start()
        {
        }

        public ILuaTable PanelMain;
        public ILuaTable PanelOther;
        private void Init()
        {
            _luaEnv = ApplicationManager.Inst.LuaEnv;
            PanelMain = _luaEnv.Global.Get<ILuaTable>("PanelMain");
            PanelOther = _luaEnv.Global.Get<ILuaTable>("PanelOther");
        }
    }
}
