//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-11 18:21
//================================

using System;
using UnityEngine;
using System.Collections.Generic;
using XLua;
public static class LuaConfig
{
    [LuaCallCSharpAttribute]
    public static List<Type> LuaCallCsWhiteList = new List<Type>()
    {
        typeof(EventDelegate),
        typeof(Easy.FrameUnity.Panel.PanelMain)
    };
}

