using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTemplate : MonoBehaviour
{
    public DamageInfo damage;

    public virtual void Start()
    {
        damage = new DamageInfo();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ///TODO: Change the direction facing logic
            GameObject player_obj = other.transform.parent.gameObject;
            damage.direction = player_obj.GetComponent<MovementController>().is_right_facing ? -1 : 1;
            // Send damage to player
            player_obj.GetComponent<MovementController>().SendMessage("OnDamageToPlayer", damage);
        }
    }

    public class DamageInfo
    {
        public float amount;
        public float force;
        public int direction;
    }
}
