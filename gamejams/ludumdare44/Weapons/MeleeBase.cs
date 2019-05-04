using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class MeleeBase : MonoBehaviour
{
    public SphereCollider collider;
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
        StartCoroutine(DestroyAfterSeconds());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (rotation_set)
        {
            other.gameObject.GetComponent<EnemyBase>().SendMessage("OnHit", new EnemyBase.OnHitData(rotation * force * velocity * PlayerState.data.knockback_mod, damage));
        }
    }

    // Destroy shot after time elapses
    private IEnumerator DestroyAfterSeconds()
    {
        new WaitForSeconds(Time.deltaTime + 0.03f);
        Destroy(this.gameObject);
        yield break;
    }

    // Get rotation vector from player
    public void TranslateFromVector3(Vector3 vec)
    {
        rotation = vec;
        rotation_set = true;
    }
}
