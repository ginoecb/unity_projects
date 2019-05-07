using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AttackTemplate : MonoBehaviour
{
    public BoxCollider collider;
    public float velocity;
    private bool rotation_set;
    public Vector3 rotation;
    public float force;
    public float damage;

    private void Awake()
    {
        rotation_set = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
