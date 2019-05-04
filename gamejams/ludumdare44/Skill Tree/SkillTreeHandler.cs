using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeHandler : MonoBehaviour
{
    // Value for next stat modifier
    public float next_SPD_mod;
    public float next_ATK_mod;
    public float next_DEF_mod;
    public float next_cooldown_mod;
    public float next_knockback_mod;
    public float next_armor_mod;

    private void Start()
    {
        //Time.timeScale = 0;
        next_SPD_mod = 1.2f;
        next_ATK_mod = 1.2f;
        next_DEF_mod = 1.2f;
        next_cooldown_mod = 0.98f;
        next_knockback_mod = 1.2f;
        next_armor_mod = 0.98f;
    }

    // Incrementally modifies specified stat
    private void UpdateStat(float stat)
    {
        stat += (stat < 1) ? -2 : 2;
    }

    // Primary bonuses cost 20 HP
    public void OnClickSPD()
    {
        PlayerState.data.SPD = next_SPD_mod;
        PlayerState.data.HP -= 20;
        UpdateStat(next_SPD_mod);
        if (PlayerState.data.cooldown_max == 0)
        {
            PlayerState.data.cooldown_max += 1;
        }
        OnClickSomething();
    }

    public void OnClickATK()
    {
        PlayerState.data.ATK = next_ATK_mod;
        PlayerState.data.HP -= 20;
        UpdateStat(next_ATK_mod);
        if (PlayerState.data.knockback_max == 0)
        {
            PlayerState.data.knockback_max += 1;
        }
        OnClickSomething();
    }

    public void OnClickDEF()
    {
        PlayerState.data.DEF = next_DEF_mod;
        PlayerState.data.HP -= 20;
        UpdateStat(next_DEF_mod);
        if (PlayerState.data.armor_max == 0)
        {
            PlayerState.data.armor_max += 1;
        }
        OnClickSomething();
    }

    // Secondary bonuses cost 10 HP
    public void OnClickCooldown()
    {
        PlayerState.data.cooldown_mod = next_cooldown_mod;
        PlayerState.data.HP -= 10;
        UpdateStat(next_cooldown_mod);
        OnClickSomething();
    }

    public void OnClickKnockback()
    {
        PlayerState.data.knockback_mod = next_knockback_mod;
        PlayerState.data.HP -= 10;
        UpdateStat(next_knockback_mod);
        OnClickSomething();
    }

    public void OnClickArmor()
    {
        PlayerState.data.armor_mod = next_armor_mod;
        PlayerState.data.HP -= 10;
        UpdateStat(next_armor_mod);
        OnClickSomething();
    }

    // Big primary bonuses cost 40 HP
    public void OnClickBigSPD()
    {
        // Boosts stat by 10 points
        for (int i = 0; i < 4; i++)
        {
            UpdateStat(next_SPD_mod);
        }
        PlayerState.data.HP -= 20;
        OnClickSPD();
    }

    public void OnClickBigATK()
    {
        // Boosts stat by 10 points
        for (int i = 0; i < 4; i++)
        {
            UpdateStat(next_SPD_mod);
        }
        PlayerState.data.HP -= 20;
        OnClickATK();
    }

    public void OnClickBigDEF()
    {
        // Boosts stat by 10 points
        for (int i = 0; i < 4; i++)
        {
            UpdateStat(next_SPD_mod);
        }
        PlayerState.data.HP -= 20;
        OnClickDEF();
    }

    public void OnClickSomething()
    {
        Time.timeScale = 1;
        Destroy(this.gameObject);
    }
}
