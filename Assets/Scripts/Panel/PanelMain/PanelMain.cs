//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-09-29 10:45
//================================

using System;
using UnityEngine;
using XLua;
using Easy.FrameUnity;
using Easy.FrameUnity.Base;
using Easy.FrameUnity.Manager;
using System.Collections.Generic;
using System.Collections;

namespace Easy.FrameUnity.Panel
{
    /*
    [HotfixAttribute]
    public class PanelMain : LuaBehaviour
    {
        public UILabel LBLTest;
        private void Awake()
        {
            base.Awake();
            LuaEnv luaEnv = new LuaEnv();
            luaEnv.DoString(@"
            xlua.hotfix(CS.Easy.FrameUnity.Panel.PanelMain, 'Start',
                function(self)
                    self.LBLTest.text = 'After Hotfix'
                end)
        ");
        }

        private void Start()
        {
            this.LBLTest.text = "Before Hotfix";
            base.Start();
        }

        private void Update()
        {
            base.Update();
        }

        private void OnDestroy()
        {
            base.OnDestroy();
        }
    }
    */
    [HotfixAttribute]
    public class PanelMain : PanelBase<PanelMain>
    {
        public Dictionary<string, GameObject> UIDic = new Dictionary<string, GameObject>();

        public override void Back()
        {
        }

        public override void Close()
        {
        }

        public override void Open(object data)
        {
        }

        private void Awake()
        {
            base.Awake();
            
        }

        /*
        public void RegisterBtnEvent()
        {
        }
        */

        protected override void Init()
        {
            base.Init();
            this.LuaTable = LuaTableManager.Inst.PanelMain;
            foreach(var ui in this.Injections)
            {
                Debug.Log("Name:" + ui.Name);
                this.UIDic.Add(ui.Name, ui.Value);
            }
            Debug.Log("PanelMain Init");
            //this.RegisterBtnEvent();
        }

        private void Start()
        {
            base.Start();
        }

        private void Update()
        {
            base.Update();
        }

        private void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
