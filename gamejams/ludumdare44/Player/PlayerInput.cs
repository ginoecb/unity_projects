using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 input;
    public bool ranged;
    public bool melee;
    public bool shop_action;

    private void Update()
    {
        input = new Vector2(0, 0);
        ranged = false;
        melee = false;
        shop_action = false;
        GetMovement();
        GetAttacks();
        GetSpecials();
    }

    // Handles translational movement input with WASD keys
    private void GetMovement()
    {
        // Up key press
        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            input.y = 1;
        }
        // Down key press
        else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
        {
            input.y = -1;
        }
        // Left key press
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            input.x = -1;
        }
        // Right key press
        else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            input.x = 1;
        }
    }

    // Handles attack input with mouse buttons
    private void GetAttacks()
    {
        // Left click for ranged attack
        if (Input.GetMouseButtonDown(0))
        {
            ranged = true;
        }
        // Right click for melee attack
        if (Input.GetMouseButtonDown(1))
        {
            melee = true;
        }
    }

    // Handles special action input with QE & Space keys
    private void GetSpecials()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shop_action = true;
        }
    }
}
