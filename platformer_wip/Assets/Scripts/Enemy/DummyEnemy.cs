using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : EnemyTemplate
{
    // Start is called before the first frame update
    override public void Start()
    {
        hp_max = 10;
        hp_marked_max = 3;
        hp_reduction_timer_max = 5;
        xp = 10;
        base.Start();
    }

    // Update is called once per frame
    override public void LateUpdate()
    {
        base.LateUpdate();

        print("Enemy HP: " + hp.ToString() + "\n"
            + "HP Marked: " + hp_marked.ToString() + "\n"
            + "Timer: " + hp_reduction_timer.ToString() + "\n"
            + "Player HP: " + PlayerState.data.hp.ToString());
    }
}
