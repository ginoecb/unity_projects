using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyRanged : RangedAttack
{
    // Set weapon damage
    override public void Awake()
    {
        base.Awake();
        damage = 1;
        has_punchthrough = false;
    }
}
