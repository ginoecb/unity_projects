using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponState : MonoBehaviour
{
    public static WeaponState data;

    // Master lists of weapons for use with RNG
    public List<Weapon> Ranged_Weapons = new List<Weapon>();
    public List<Weapon> Melee_Weapons = new List<Weapon>();

    // Ensures only one instance of WeaponBase exists
    // Also initializes master lists of Weapons
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
        // Initializing master lists of Weapons
        Ranged_Weapons.Add(Pistol);  
        Ranged_Weapons.Add(Sniper);  
        Melee_Weapons.Add(Baseball_Bat);  
        Melee_Weapons.Add(Katana);  
    }

    // Base class for weapon enumerations
    public class Weapon
    {
        public string name;
        public bool is_default;
        public bool is_ranged;
        public float damage;
        public float knockback;
        public float cooldown;
        public float hp;
        public float cost;
    }

    // Ranged
    public Weapon Pistol = new Weapon()
    {
        name = "Pistol",
        is_default = false,
        is_ranged = true,
        damage = 30,
        knockback = 0.2f,
        cooldown = 0.3f,
        hp = 50,
        cost = 20
    };

    public Weapon Sniper = new Weapon()
    {
        name = "Sniper",
        is_default = false,
        is_ranged = true,
        damage = 50,
        knockback = 0.6f,
        cooldown = 1,
        hp = 30,
        cost = 40
    };

    // Melee
    public Weapon Baseball_Bat = new Weapon()
    {
        name = "Baseball Bat",
        is_default = false,
        is_ranged = false,
        damage = 20,
        knockback = 0.45f,
        cooldown = 0.3f,
        hp = 100,
        cost = 10
    };

    public Weapon Katana = new Weapon()
    {
        name = "Sword",
        is_default = false,
        is_ranged = false,
        damage = 50,
        knockback = 0.1f,
        cooldown = 0.1f,
        hp = 100,
        cost = 30
    };

    // Default (fists)
    public Weapon Fists = new Weapon()
    {
        name = "Unarmed",
        is_default = true,
        is_ranged = false,
        damage = 10,
        knockback = 0.25f,
        cooldown = 0.1f,
        hp = 10,
        cost = 0
    };
}
