//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-09-14 19:36
//================================

using System;
using UnityEngine;

namespace Easy.FrameUnity.Net
{
    public class ThreadDebug
    {
        public static void Log(string msg)
        {
            Net.InvokeAsync(()=>{
                Debug.Log(msg);
            });
        }

        public static void LogError(string msg)
        {
            Net.InvokeAsync(()=>{
                Debug.LogError(msg);
            });
        }
    }
}
