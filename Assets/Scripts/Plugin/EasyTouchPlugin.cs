using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyTouchPlugin : Singleton<EasyTouchPlugin>
{
    public ETCJoystick Joystick;
    public ETCButton BtnAttack;
    public ETCButton BtnSkill_1;

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
