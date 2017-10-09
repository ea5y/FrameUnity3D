//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-09-29 10:45
//================================

using System;
using UnityEngine;
using XLua;
using Easy.FrameUnity;

namespace Easy.FrameUnity.Panel
{
    [HotfixAttribute]
    public class PanelMain : LuaBehaviour
    {
        public UILabel LBLTest;
        private void Awake()
        {
            base.Awake();
            /*
            LuaEnv luaEnv = new LuaEnv();
            luaEnv.DoString(@"
            xlua.hotfix(CS.Easy.FrameUnity.Panel.PanelMain, 'Start',
                function(self)
                    self.LBLTest.text = 'After Hotfix'
                end)
        ");
        */
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
}
