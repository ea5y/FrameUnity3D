//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-24 14:01
//================================

using UnityEngine;
using System;

namespace Easy.FrameUnity.ScriptableObj
{
    [SerializableAttribute]
    public class InjectionTexture2D
    {
        public string Name;
        public UnityEngine.Object Texture2D;
    }

    public class SObjTexture2D: ScriptableObject
    {
        public InjectionTexture2D[] Injections;
    }
}
