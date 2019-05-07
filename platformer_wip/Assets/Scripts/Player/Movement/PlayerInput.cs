using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Directional input vector
    public Vector3 input;
    // Jump vars
    public bool jump_start;
    public bool jump_end;
    public bool jump_mid;
    // Dash vars
    public bool dash_start;
    public bool is_dashing;
    // Combat vars
    public bool is_attacking_ranged;
    public bool is_attacking_melee;

    // Start is called before the first frame update
    void Start()
    {
        ResetVars();
        input = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector2(0, 0);
        // Up key press
        if (Input.GetKey(InputManager.key.Up)
            && !Input.GetKey(InputManager.key.Down))
        {
            input.y = 1;
        }
        // Down key press
        else if (Input.GetKey(InputManager.key.Down)
            && !Input.GetKey(InputManager.key.Up))
        {
            input.y = 1;
        }
        // Left key press
        if (Input.GetKey(InputManager.key.Left)
            && !Input.GetKey(InputManager.key.Right))
        {
            input.x = -1;
        }
        // Right key press
        else if (Input.GetKey(InputManager.key.Right)
            && !Input.GetKey(InputManager.key.Left))
        {
            input.x = 1;
        }
        // Jump key press
        ResetVars();
        if (Input.GetKeyDown(InputManager.key.Jump))
        {
            jump_start = true;
        }
        else if (Input.GetKeyUp(InputManager.key.Jump))
        {
            jump_end = true;
        }
        else if (Input.GetKey(InputManager.key.Jump))
        {
            jump_mid = true;
        }
        // Dash key press
        // Dashing will require holding a directional input
        if (Input.GetKeyDown(InputManager.key.Dash))
        {
            dash_start = true;
            if (!is_dashing && input.x != 0)
            {
                is_dashing = true;
            }
        }
        // Ranged attack key press
        if (Input.GetKeyDown(InputManager.key.Ranged))
        {
            is_attacking_ranged = true;
        }
        // Melee attack key press
        if (Input.GetKeyDown(InputManager.key.Melee))
        {
            is_attacking_melee= true;
        }
    }

    // Reset control vars for Jump
    void ResetVars()
    {
        jump_start = false;
        jump_end = false;
        jump_mid = false;
        dash_start = false;
        is_dashing = false;
        is_attacking_ranged = false;
        is_attacking_melee = false;
    }
}
