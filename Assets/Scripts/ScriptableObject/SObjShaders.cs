//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-23 12:26
//================================

using UnityEngine;
using System;

namespace Easy.FrameUnity.ScriptableObj
{
    [SerializableAttribute]
    public class InjectionShader
    {
        public string Name;
        public Shader Shader;
    }

    public class SObjShaders : ScriptableObject
    {
        public InjectionShader[] Injections;
    }
}
