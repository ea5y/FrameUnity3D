//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-09 10:38
//================================

using UnityEngine;
using System;
using XLua;
using Easy.FrameUnity.Manager;

namespace Easy.FrameUnity
{
    [SerializableAttribute]
    public class Injection
    {
        public string Name;
        public GameObject Value;
    }

    [LuaCallCSharpAttribute]
    public class LuaBehaviour : MonoBehaviour
    {
        public Injection[] Injections;

        private LuaEnv _luaEnv;

        /*
        private LuaTable _scriptEnv;

        private Action _luaStart;
        private Action _luaUpdate;
        private Action _luaOnDestroy;
        */

        internal static float _lastGCTime = 0;
        internal const float _GCInterval = 1; //1 second

        private ILuaTable _luaTable;
        protected ILuaTable LuaTable
        {
            get
            {
                return _luaTable;
            }
            set
            {
                _luaTable = value;
            }
        }

        protected void Awake()
        {
            /*
            this.Init();

            _scriptEnv = _luaEnv.NewTable();
            LuaTable meta = _luaEnv.NewTable(); 
            meta.Set("__index", _luaEnv.Global);
            _scriptEnv.SetMetaTable(meta);
            meta.Dispose();

            _scriptEnv.Set("self", this);
            foreach(var injection in Injections)
                _scriptEnv.Set(injection.Name, injection.Value);

           //_luaEnv.DoString(luaScript.text, "LuaBehaviour", _scriptEnv); 
           Action luaAwake = _scriptEnv.Get<Action>("awake");
           _scriptEnv.Get("start", out _luaStart);
           _scriptEnv.Get("update", out _luaUpdate);
           _scriptEnv.Get("onDestroy", out _luaOnDestroy);
           */
            this.Init();

            if(this.LuaTable != null)
               this.LuaTable.Awake();
        }

        protected virtual void Init()
        {
            _luaEnv = ApplicationManager.Inst.LuaEnv;
            Debug.Log("LuaBehaviour Init");
        }

        protected void Start()
        {
            if(this.LuaTable != null)
                this.LuaTable.Start();
        }

        protected void Update()
        {
            if(this.LuaTable != null)
            {
                this.LuaTable.Update();
            }

            if(Time.time - LuaBehaviour._lastGCTime > _GCInterval)
            {
                _luaEnv.Tick();
                LuaBehaviour._lastGCTime = Time.time;
            }
        }

        protected void OnDestroy()
        {
            if(this.LuaTable != null)
            {
                this.LuaTable.OnDestroy();
            }
            this.LuaTable = null;
            Injections = null;
        }
    }
}
