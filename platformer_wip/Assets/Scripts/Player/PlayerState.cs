using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState data;
    public float hp;
    public float hp_max = 100;
    public float hp_marked;
    public float xp;

    // Ensures only one instance of PlayerState exists
    private void Awake()
    {
        if (data == null)
        {
            DontDestroyOnLoad(gameObject);
            data = this;
        }
        else if (data != this)
        {
            Destroy(gameObject);
        }
    }

        // Start is called before the first frame update
        void Start()
    {
        hp = hp_max;
        hp_marked = 0;
    }

    // Update is called once per frame
    void Update()
    {
        StopCoroutine(ReduceHP());
    }

    // Removes amount of markded hp from total hp
    private IEnumerator ReduceHP()
    {
        float hp_marked_on_hit = hp_marked;
        // Seconds of buffer after receiving damage
        yield return new WaitForSeconds(1);
        // If no more damage is taken
        if (hp_marked == hp_marked_on_hit)
        {

        }

    }
}
