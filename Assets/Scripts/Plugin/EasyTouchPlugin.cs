using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyTouchPlugin : Singleton<EasyTouchPlugin>
{
    private void Awake()
    {
        base.GetInstance();
        this.Enable(false);
    }

    public void Enable(bool enable)
    {
        this.gameObject.SetActive(enable);
    }
}
