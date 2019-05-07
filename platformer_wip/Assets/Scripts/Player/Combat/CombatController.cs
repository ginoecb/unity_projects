using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(BoxCollider))]
public class CombatController : MonoBehaviour
{
    // Player input
    public PlayerInput inputs;
    public BoxCollider collider;
    // Movement Controller
    public MovementController movement;
    // Attack objects to spawn
    public GameObject ranged_obj;
    public GameObject melee_obj;
    public PlayerWeapon ranged;
    public PlayerWeapon melee;
    public LayerMask collision_mask;

    // Set default weapons
    void Start()
    {
        ranged = new PlayerWeapon(ranged_obj, 0.3f);
        //ranged = new PlayerWeapon(ranged_obj, 1, 0.3f);
        melee = new PlayerWeapon(melee_obj, 0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        HandleAttacks();
    }

    private void HandleAttacks()
    {
        HandleRanged(inputs.is_attacking_ranged, ranged);
        HandleMelee(inputs.is_attacking_melee, melee);
    }

    private void HandleRanged(bool input, PlayerWeapon weapon)
    {
        //HandleHitscan(input, weapon);
        HandleProjectile(input, weapon);
    }

    // For ranged attacks that hit instantly
    private void HandleHitscan(bool input, PlayerWeapon weapon)
    {
        weapon.UpdateCooldown();
        // Ranged attack cannot cancel melee attack
        if (input && ranged.cooldown <= 0 && melee.cooldown <= 0)
        {
            float direction = (movement.is_right_facing ? 1 : -1);
            // Raycast fires forward
            Vector3 ray_origin_anchor = transform.position
                + transform.up
                + transform.right * direction;
            float ray_length = Screen.width;
            weapon.obj.GetComponent<LineRenderer>().SetPosition(0, ray_origin_anchor);
            // If ranged attack hits an enemy
            RaycastHit hit;
            if (Physics.Raycast(
                ray_origin_anchor,
                Vector3.right * direction,
                out hit,
                ray_length,
                collision_mask
                ))
            {
                if (hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "Boss")
                {
                    hit.collider.gameObject.SendMessage("OnHitRanged", weapon.hitscan_damage);
                }
                weapon.obj.GetComponent<LineRenderer>().SetPosition(1, hit.point);
            }
            // If ranged attack fires off screen
            else
            {
                weapon.obj.GetComponent<LineRenderer>().SetPosition(1, ray_origin_anchor + Vector3.right * direction * ray_length);
            }
            // Draw LineRenderer for shot
            GameObject this_weapon = (GameObject)Instantiate(weapon.obj);
            StartCoroutine(DestroyAfterSeconds(this_weapon, 0.13f));
            Debug.DrawRay(
                ray_origin_anchor,
                Vector3.right * direction * ray_length,
                Color.yellow
                );
        }
    }

    // Destroy GameObject after time elapses
    private IEnumerator DestroyAfterSeconds(GameObject obj, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(obj);
    }

    // For ranged attacks with travel time
    private void HandleProjectile(bool input, PlayerWeapon weapon)
    {
        weapon.UpdateCooldown();
        // Ranged attack cannot cancel melee attack
        if (input && ranged.cooldown <= 0 && melee.cooldown <= 0)
        {
            float direction = (movement.is_right_facing ? 1 : -1);
            // Ranged attack hitbox
            GameObject this_weapon = (GameObject)Instantiate(
                weapon.obj,
                transform.position + transform.up + transform.right * direction,
                Quaternion.identity
                );
            this_weapon.SendMessage("SetDirection", direction);
            // Side effects of attack
            weapon.cooldown = weapon.cooldown_max;
        }
    }

    // For melee attacks
    private void HandleMelee(bool input, PlayerWeapon weapon)
    {
        weapon.UpdateCooldown();
        if (input && melee.cooldown <= 0)
        {
            float direction = (movement.is_right_facing ? 1 : -1);
            GameObject this_weapon = (GameObject)Instantiate(
                weapon.obj,
                transform.position + transform.up + transform.right * direction,
                Quaternion.identity
                );
            // Attach melee hitbox to player
            this_weapon.transform.SetParent(transform);
            this_weapon.SendMessage("SetDirection", direction);
            // Side effects of attack
            weapon.cooldown = weapon.cooldown_max;
        }
    }
    
    // Class for handling different weapons used by player
    public class PlayerWeapon
    {
        public GameObject obj;
        public float cooldown;
        public float cooldown_max;
        public float hitscan_damage;
        // Update cooldown timer
        public void UpdateCooldown()
        {
            if (cooldown > 0)
            {
                cooldown -= Time.deltaTime;
            }
        }
        // Constructors
        // For ranged hitscan weapons
        public PlayerWeapon(GameObject new_obj, float new_hitscan_damage, float cooldown_timer_max)
        {
            obj = new_obj;
            hitscan_damage = new_hitscan_damage;
            cooldown = 0;
            cooldown_max = cooldown_timer_max;
        }
        // For ranged projectile & melee weapons
        public PlayerWeapon(GameObject new_obj, float cooldown_timer_max)
        {
            obj = new_obj;
            hitscan_damage = 0;
            cooldown = 0;
            cooldown_max = cooldown_timer_max;
        }
    }
}
