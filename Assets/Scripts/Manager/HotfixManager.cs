//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-16 18:01
//================================

using System;
using System.IO;
using System.Collections.Generic;
using XLua;
using Easy.FrameUnity.EsAssetBundle;
using Easy.FrameUnity.ScriptableObj;
using Easy.FrameUnity.Base;
using UnityEngine;

namespace Easy.FrameUnity.Manager
{
    [CSharpCallLuaAttribute]
    public interface ILuaTable : IPanel
    {
        void Awake();
        void Start();
        void OnEnable();
        void OnDisable();
        void Update();
        void OnDestroy();
        void TransformGameObject(GameObject gameObject);
    }

    public class HotfixManager : ManagerBase<HotfixManager>
    {
        public LuaEnv LuaEnv;

        private Dictionary<string, ILuaTable> _luaTableDic = new Dictionary<string, ILuaTable>();

        private void Awake()
        {
            base.GetInstance();
        }

        public override void Init()
        {
            this.GetLuaTable();
        }

        public void EnableHotFix()
        {
            if(File.Exists(URL.HOTFIX_URL))
            {
                this.LuaEnv = new LuaEnv();
                this.LuaEnv.DoString(File.ReadAllText(URL.HOTFIX_URL));
            }
        }

        public void GetLuaTable()
        {
            AssetPoolManager.Inst.FindAsset<AssetScriptableObject, PanelInfo>("asset", "PanelInfo", (obj) => {
                /*
                foreach(var panelName in obj.PanelNameList)
                {
                    var table = _luaEnv.Global.Get<ILuaTable>(panelName);
                    _luaTableDic.Add(panelName, table);
                }
                */
                    var table = this.LuaEnv.Global.Get<ILuaTable>("PanelOther");
                    _luaTableDic.Add("PanelOther", table);
            });
        }

        public ILuaTable GetLuaTable(string key)
        {
            ILuaTable table;
            if(!_luaTableDic.TryGetValue(key, out table))
            {
                return null;
            }
            return table;
        }
    }
}
