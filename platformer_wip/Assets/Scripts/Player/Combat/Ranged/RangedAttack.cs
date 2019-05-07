using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    public float damage;
    public bool direction_is_set;
    public bool has_punchthrough;
    private float velocity = 0.5f;
    public LayerMask collision_mask;

    public virtual void Awake()
    {
        direction_is_set = false;
        has_punchthrough = false;
    }

    // Translate projectile
    public virtual void Update()
    {
        if (direction_is_set)
        {
            transform.Translate(transform.right * velocity);
        }
    }

    // Detect collision with enemy
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Boss")
        {
            // TODO: Debug this
            other.gameObject.GetComponent<EnemyTemplate>().SendMessage("OnHitRanged", damage);
            if (!has_punchthrough)
            {
                Destroy(this.gameObject);
            }
        }
    }

    // Destroy self when off screen
    public void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    // Determine direction of attack
    // Collider moves to ensure hit registers
    public void SetDirection(float direction)
    {
        velocity *= direction;
        direction_is_set = true;
    }
}
