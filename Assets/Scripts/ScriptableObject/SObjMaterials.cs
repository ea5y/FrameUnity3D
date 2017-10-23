//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-23 12:39
//================================

using UnityEngine;
using System;

namespace Easy.FrameUnity.ScriptableObj
{
    [SerializableAttribute]
    public class InjectionMaterial
    {
        public string Name;
        public Material Material;
    }

    public class SObjMaterials : ScriptableObject 
    {
        public InjectionMaterial[] Injections;
    }
}
