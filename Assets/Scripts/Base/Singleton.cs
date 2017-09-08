//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-06-25 20:52
//================================

using UnityEngine;
using System.Collections;
using System;
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Inst;
    private object _objSync = new object();

    protected void GetInstance()
    {
        lock (_objSync)
        {
            if (Inst == null)
            {
                Inst = this as T;
            }
            else
            {
                Debug.LogError("===>Singleton Clash!!!");
            }
        }
    }

    public void Destroy()
    {
        Debug.Log("Singleton Destroy");
        Inst = null;
    }
}
