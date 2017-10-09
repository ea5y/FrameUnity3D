//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-09 19:25
//================================

using UnityEngine;
using System.Collections.Generic;

namespace Easy.FrameUnity.ScriptableObj
{
    public class ScriptableObjectBase : ScriptableObject
    {
    }

    public class PanelInfo : ScriptableObjectBase
    {
        public List<string> PanelNameList = new List<string>();
    }
}
