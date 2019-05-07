using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager key;

    public KeyCode Up;
    public KeyCode Down;
    public KeyCode Left;
    public KeyCode Right;
    public KeyCode Jump;
    public KeyCode Dash;
    public KeyCode Ranged;
    public KeyCode Melee;

    // Ensure only one instance of InputManager exists
    private void Awake()
    {
        if (key == null)
        {
            DontDestroyOnLoad(gameObject);
            key = this;
        }
        else if (key != this) {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        /// FOR TESTING
        Up = KeyCode.W;
        Down = KeyCode.S;
        Left = KeyCode.A;
        Right = KeyCode.D;
        Jump = KeyCode.Space;
        Dash = KeyCode.P;
        Ranged = KeyCode.K;
        Melee = KeyCode.O;
    }
}
