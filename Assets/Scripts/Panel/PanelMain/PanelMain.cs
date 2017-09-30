//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-09-29 10:45
//================================

using System;
using UnityEngine;
using XLua;

namespace Easy.FrameUnity.Panel
{
    [HotfixAttribute]
    public class PanelMain : MonoBehaviour
    {
        public UILabel LBLTest;
        private void Awake()
        {
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
        }

    }
}
