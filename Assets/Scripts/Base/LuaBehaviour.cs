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
        private LuaTable _scriptEnv;

        private Action _luaStart;
        private Action _luaUpdate;
        private Action _luaOnDestroy;

        internal static float _lastGCTime = 0;
        internal const float _GCInterval = 1; //1 second

        protected void Awake()
        {
            _luaEnv = ApplicationManager.Inst.LuaEnv;
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

           if(luaAwake != null)
               luaAwake();
        }

        protected void Start()
        {
            if(_luaStart != null)
                _luaStart();
        }

        protected void Update()
        {
            if(_luaUpdate != null)
                _luaUpdate();

            if(Time.time - LuaBehaviour._lastGCTime > _GCInterval)
            {
                _luaEnv.Tick();
                LuaBehaviour._lastGCTime = Time.time;
            }
        }

        protected void OnDestroy()
        {
            if(_luaOnDestroy != null)
            {
                _luaOnDestroy();
            }

            _luaOnDestroy = null;
            _luaUpdate = null;
            _luaStart = null;
            _scriptEnv.Dispose();
            Injections = null;
        }
    }
}
