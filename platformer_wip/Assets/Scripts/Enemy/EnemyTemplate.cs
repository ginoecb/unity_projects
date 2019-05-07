using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnemyTemplate : MonoBehaviour
{
    public BoxCollider collider;

    public float hp;
    public float hp_max;
    public float hp_marked;
    public float hp_marked_max;
    public float hp_reduction_timer;
    public float hp_reduction_timer_max;
    public float xp;

    // Start is called before the first frame update
    public virtual void Start()
    {
        hp = hp_max;
        hp_marked = 0;
        hp_reduction_timer = 0;
    }

    // Update is called once per frame
    public virtual void LateUpdate()
    {
        UpdateHP();
        OnKill();
    }

    // Ranged attacks mark hp
    public virtual void OnHitRanged(float scratch_damage)
    {
        if (hp_marked < hp_marked_max)
        {
            hp_marked = (hp_marked + scratch_damage < hp_marked_max) ? hp_marked + scratch_damage : hp_marked_max;
            hp_reduction_timer = hp_reduction_timer_max;
        }
    }

    // Melee attacks remove all marked hp
    public virtual void OnHitMelee()
    {
        if (hp_marked > 0)
        {
            hp -= hp_marked;
            hp_marked = 0;
            hp_reduction_timer = 0;
            // TODO: Update formula for recovering hp
            PlayerState.data.hp += hp_marked * hp_marked / hp_marked_max;
        }
    }

    // On ranged hit, enemies will receive scratch damage, marking a portion of their health
    // which will regenerate unless hit by a melee attack
    private void UpdateHP()
    {
        if (hp_marked > 0)
        {
            // After ranged hit, hp regeneration is delayed
            if (hp_reduction_timer > 0)
            {
                hp_reduction_timer -= Time.deltaTime;
            }
            // Regenerate hp after time elapsed
            else
            {
                float hp_regain = (hp_marked > 1) ? 1 : hp_marked;
                hp = (hp + hp_regain < hp_max) ? hp + hp_regain : hp_max;
                hp_marked -= hp_regain;
            }
        }
    }

    public virtual void OnKill()
    {
        if (hp <= 0)
        {
            PlayerState.data.xp += xp;
            Destroy(this.gameObject);
        }
    }
}
