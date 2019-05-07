using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyDamage : DamageTemplate
{
    public override void Start()
    {
        damage = new DamageInfo()
        {
            amount = 10,
            force = 1f
        };
    }
}
