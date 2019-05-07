using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public float damage;
    public bool direction_is_set;
    public LayerMask collision_mask;

    public virtual void Awake()
    {
        direction_is_set = false;
        // REQUIRED: Set weapon damage
    }

    // Run coroutine to destroy hitbox
    public virtual void Update()
    {
        StartCoroutine(DestroyAfterSeconds(Time.deltaTime + 0.03f));
    }

    // Detect collision with enemy
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Boss")
        {
            other.gameObject.GetComponent<EnemyTemplate>().SendMessage("OnHitMelee", damage);
        }
    }

    // Destroy hitbox after time elapses
    public virtual IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }

    // Determine direction of melee attack
    public void SetDirection(float direction)
    {
        direction_is_set = true;
    }
}
