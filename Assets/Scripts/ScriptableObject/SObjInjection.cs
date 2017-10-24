//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-24 14:43
//================================

using UnityEngine;
using System;

namespace Easy.FrameUnity.ScriptableObj
{
    [SerializableAttribute]
    public class Injection
    {
        public string Name;
        public UnityEngine.Object Object;
    }

    public class SObjInjection: ScriptableObject
    {
        public Injection[] Injections;
    }
}
