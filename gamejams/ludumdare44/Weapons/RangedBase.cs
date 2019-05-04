using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RangedBase : MonoBehaviour
{
    public BoxCollider collider;
    private float velocity = 15.5f;
    private bool rotation_set;
    public Vector3 rotation;
    public float force;
    public float damage;

    // Do not move shot until rotation direction established
    private void Awake()
    {
        rotation_set = false;
    }

    // Move shot in direction of Target
    private void Update()
    {
        transform.Translate(rotation * velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<EnemyBase>().SendMessage("OnHit", new EnemyBase.OnHitData(rotation * force * velocity * PlayerState.data.knockback_mod, damage));
    }

    // Destroy shot when offscreen
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    // Get rotation vector from player
    public void TranslateFromVector3(Vector3 vec)
    {
        rotation = vec;
    }
}
