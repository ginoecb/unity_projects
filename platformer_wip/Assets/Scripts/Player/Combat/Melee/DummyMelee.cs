using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMelee : MeleeAttack
{
    // Set weapon damage
    override public void Awake()
    {
        base.Awake();
        damage = 1;
    }
}
