//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-09-04 16:17
//================================

using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{
    public const int MaxHealth = 100;

    [SyncVarAttribute]
    public int CurrentHealth = MaxHealth;

    private UISlider _healthBar;

    public void TakeDamage(int amount)
    {
        if(!isServer)
        {
            return;
        }
        this.CurrentHealth -= amount;
        if(this.CurrentHealth <= 0 )
        {
            this.CurrentHealth = 0;
            Debug.Log("Dead!");
        }

        _healthBar.value = CurrentHealth / (float)MaxHealth;
    }

    public void SetHealthBar(UISlider healthBar)
    {
        _healthBar = healthBar;
    }
}
