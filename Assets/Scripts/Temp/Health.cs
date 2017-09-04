//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-09-04 16:17
//================================

using UnityEngine;

public class Health : MonoBehaviour
{
    public const int MaxHealth = 100;
    public int CurrentHealth = MaxHealth;

    private UISlider _healthBar;

    public void TakeDamage(int amount)
    {
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
