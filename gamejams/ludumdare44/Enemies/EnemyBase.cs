using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBase : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 velocity;

    // The following must be initialized in children
    public float hp;
    public float xp;

    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(velocity * Time.deltaTime);
        float x = velocity.x;
        if (velocity.x != 0)
        {
            x = velocity.x + 5 * ((velocity.x < 0) ? Time.deltaTime : -1 * Time.deltaTime);
        }
        float y = velocity.y;
        if (velocity.y != 0){
            y = velocity.y + 5 * ((velocity.y < 0) ? Time.deltaTime : -1 * Time.deltaTime);
        }
        velocity = new Vector3(x, y, 0);
        ConditionalSuicide();
    }

    private void ConditionalSuicide()
    {
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnHit(OnHitData datum)
    {
        velocity = datum.vec;
        hp -= datum.damage;
    }

    public class OnHitData
    {
        public Vector3 vec;
        public float damage;
        public OnHitData(Vector3 new_vec, float new_damage)
        {
            vec = new_vec;
            damage = new_damage;
        }
    }
}
