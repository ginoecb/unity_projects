using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState data;
    public GameObject skill_tree;

    // Health
    public float HP;

    // Score
    public float XP;

    // Base player stats
    public float SPD;
    public float ATK;
    public float DEF;

    // Secondary player stats
    public float cooldown_mod;
    public float knockback_mod;
    public float armor_mod;

    // Weapons equipped
    // TODO: Add weapon classes here

    // Skill tree upgrades
    public int SPD_max;
    public int ATK_max;
    public int DEF_max;
    public int cooldown_max;
    public int knockback_max;
    public int armor_max;

    // Ensures only one instance of PlayerState exists
    private void Awake()
    {
        if (data == null)
        {
            DontDestroyOnLoad(gameObject);
            data = this;
        }
        else if (data != this)
        {
            Destroy(gameObject);
        }

        HP = 500;
        XP = 0;
        SPD = 1;
        ATK = 1;
        DEF = 1;
        cooldown_mod = 1;
        knockback_mod = 1;
        armor_mod = 1;
        SPD_max = 0;
        ATK_max = 0;
        DEF_max = 0;
        cooldown_max = -1;
        knockback_max = -1;
        armor_max = -1;
    }

    // Level up every 100 XP
    public void OnLevelUp()
    {
        ShopState.data.GetShopItems();
        Instantiate(skill_tree);
    }
}
