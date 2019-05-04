using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(BoxCollider))]
public class PlayerController : MonoBehaviour
{
    public PlayerInput inputs;
    public BoxCollider collider;
    public GameObject target;

    // Movement
    public Vector2 velocity;
    public float run_speed = 6.9f;
    private float velocity_smoothing_x;
    private float velocity_smoothing_y;
    public float acceleration_time = 0.09f;

    // Aiming
    private Vector3 mouse_position;
    public GameObject ranged_obj;
    public GameObject melee_obj;
    public GameObject default_obj;

    // Weapons
    WeaponState.Weapon Ranged;
    WeaponState.Weapon Melee;
    private float ranged_cooldown;
    private float melee_cooldown;
    private float ranged_hp;
    private float melee_hp;

    // Other
    private bool shop_ready;
    public bool shop_active;
    public GameObject shop_ui;

    // Start is called before the first frame update
    private void Start()
    {
        velocity = new Vector2(0, 0);
        shop_ready = false;
        shop_active = false;

        ResetRanged(WeaponState.data.Fists);
        ResetMelee(WeaponState.data.Fists);
        ShopState.data.GetShopItems();
    }

    // Update is called once per frame
    private void Update()
    {
        // Handle inputs
        GetVelocity();
        HandleAttacks();
        MoveCrosshair();
        HandleShop();
        MovePlayer();
        // Update attack variables
        if (ranged_cooldown > 0)
        {
            ranged_cooldown -= Time.deltaTime;
        }
        if (melee_cooldown > 0)
        {
            melee_cooldown -= Time.deltaTime;
        }
    }

    // Allows shop interaction
    private void OnTriggerEnter(Collider other)
    {
        if (!shop_ready)
        {
            shop_ready = true;
        }
    }

    // Disallows shop interaction
    private void OnTriggerExit(Collider other)
    {
        if (shop_ready)
        {
            shop_ready = false;
        }
    }

    // Handles translational movement
    private void MovePlayer()
    {
        if (inputs.input == new Vector2(0, 0))
        {
            velocity = new Vector2(0, 0);
        }
        transform.Translate(velocity);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    // Adjust velocity vector using input
    private void GetVelocity()
    {
        velocity.x = Mathf.SmoothDamp(
            velocity.x,
            inputs.input.x * Time.deltaTime * run_speed * PlayerState.data.SPD,
            ref velocity_smoothing_x,
            acceleration_time
            );
        velocity.y = Mathf.SmoothDamp(
            velocity.y,
            inputs.input.y * Time.deltaTime * run_speed * PlayerState.data.SPD,
            ref velocity_smoothing_y,
            acceleration_time
            );
    }

    // Rotates player target to face mouse cursor
    private void MoveCrosshair()
    {
        mouse_position = Input.mousePosition;
        mouse_position.z = 5.23f;
        Vector3 object_position = Camera.main.WorldToScreenPoint(target.transform.position);
        float angle = Mathf.Atan2(
            mouse_position.y - object_position.y,
            mouse_position.x - object_position.x
            ) * Mathf.Rad2Deg;
        target.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    // Handles attacks
    private void HandleAttacks()
    {
        if (Time.timeScale != 0)
        {
            HandleRanged();
            HandleMelee();
        }
    }

    // Handles ranged attacks
    private void HandleRanged()
    {
        // Fists cannot break
        if (Ranged.is_default)
        {
            ranged_hp = 10;
            if (inputs.ranged && ranged_cooldown <= 0)
            {
                GameObject this_ranged = HandleDefault();
                ranged_cooldown = Ranged.cooldown * PlayerState.data.cooldown_mod;
            }
        }
        // Perform ranged attack
        else if (inputs.ranged && ranged_cooldown <= 0 && ranged_hp >= 0)
        {
            GameObject this_ranged = (GameObject)Instantiate(
                ranged_obj,
                transform.position + target.transform.right,
                Quaternion.identity
                );
            this_ranged.SendMessage("TranslateFromVector3", target.transform.right);
            // Side effects of attack
            ranged_cooldown = Ranged.cooldown * PlayerState.data.cooldown_mod;
            ranged_hp -= 1 * PlayerState.data.armor_mod;
            // Switch to Fists on Weapon break
            if (ranged_hp <= 0)
            {
                OnWeaponBreak(Ranged.is_ranged);
            }
        }
    }

    // Handles melee attacks
    private void HandleMelee()
    {
        // Fists cannot break
        if (Melee.is_default)
        {
            melee_hp = 10;
            if (inputs.melee && melee_cooldown <= 0)
            {
                GameObject this_melee = HandleDefault();
                melee_cooldown = Melee.cooldown * PlayerState.data.cooldown_mod;
            }
        }
        // Perform melee attack
        else if (inputs.melee && melee_cooldown <= 0 && melee_hp >= 0)
        {
            GameObject this_melee = (GameObject)Instantiate(
                melee_obj,
                transform.position + target.transform.right * 1.5f,
                Quaternion.identity
                );
            this_melee.SendMessage("TranslateFromVector3", target.transform.right);
            // Side effects of attack
            melee_cooldown = Melee.cooldown * PlayerState.data.cooldown_mod;
            melee_hp -= 1 * PlayerState.data.armor_mod;
            // Switch to Fists on Weapon break
            if (melee_hp <= 0)
            {
                OnWeaponBreak(Melee.is_ranged);
            }
        }
    }

    // Fists (default) attack
    private GameObject HandleDefault()
    {
        GameObject this_default = (GameObject)Instantiate(
                default_obj,
                transform.position + target.transform.right * 1.5f,
                Quaternion.identity
                );
        this_default.SendMessage("TranslateFromVector3", target.transform.right);
        return this_default;
    }

    // Resets Weapon stats
    public void ResetWeapon(WeaponState.Weapon new_weapon)
    {
        if (new_weapon.is_ranged)
        {
            ResetRanged(new_weapon);
        }
        else
        {
            ResetMelee(new_weapon);
        }
    }

    // Resets Weapon stats for ranged
    private void ResetRanged(WeaponState.Weapon new_weapon)
    {
        Ranged = new_weapon;
        ranged_cooldown = 0;
        ranged_hp = new_weapon.hp;
        ranged_obj.GetComponent<RangedBase>().force = Ranged.knockback;
        ranged_obj.GetComponent<RangedBase>().damage = Ranged.damage;
    }
    
    // Resets Weapon stats for melee
    private void ResetMelee(WeaponState.Weapon new_weapon)
    {
        Melee = new_weapon;
        melee_cooldown = 0;
        melee_hp = new_weapon.hp;
        melee_obj.GetComponent<MeleeBase>().force = Melee.knockback;
        melee_obj.GetComponent<MeleeBase>().damage = Melee.damage;
    }

    // Switches Weapon to fists
    private void OnWeaponBreak(bool is_ranged)
    {
        if (is_ranged)
        {
            ResetRanged(WeaponState.data.Fists);
        }
        else
        {
            ResetMelee(WeaponState.data.Fists);
        }
    }

    // Handles shop purchases
    private void HandleShop()
    {
        if (inputs.shop_action && shop_ready && !shop_active)
        {
            Instantiate(shop_ui);
            velocity = new Vector2(0, 0);
            shop_active = true;
        }
    }
}
