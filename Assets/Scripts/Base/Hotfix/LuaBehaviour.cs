//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-09 10:38
//================================

using UnityEngine;
using System;
using System.Collections.Generic;
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
        public Dictionary<string, GameObject> HotfixUIDic = new Dictionary<string, GameObject>();

        protected void MapHotfixUI()
        {
            foreach(var ui in this.Injections)
            {
                Debug.Log("Name:" + ui.Name);
                this.HotfixUIDic.Add(ui.Name, ui.Value);
            }
        }
    }
}
