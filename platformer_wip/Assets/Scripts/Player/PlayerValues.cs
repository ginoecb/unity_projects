/// <summary>
/// Constant values used by PlayerController.cs
/// </summary>

// Running values
public class RunBehaviour
{
    public float speed = 6.9f;
    public float acceleration_time_x = 0.09f;
    public float smooth_velocity_x;
}

// Jumping values
public class JumpBehaviour
{
    public float height_min = 0.05f;
    public float height_max = 0.13f;
    public float time_limit = 0.4f;
    public float speed_x = 7.3f;
    public float speed_y_min;
    public float speed_y_max;
    public float acceleration_time_x = 0.15f;
    public int count;
    public int count_limit = 2;
    // Modifier reduces jump height for multi jumps
    public float multi_mod = 0.9f;

    // Constructor initializes jump count
    public JumpBehaviour(int jump_count)
    {
        count = jump_count;
    }
}

// Wall interaction values
public class WallBehaviour
{
    // Modifier reduces gravity when sliding down a wall
    public float slide_gravity_mod = 0.008f;
    public float jump_speed_x = 0.7f;
    public float jump_speed_y_mod = 0.8f;
    public float climb_speed = 0.06f;
    public float climb_time;
    public float climb_time_limit = 5;
    public bool is_sliding;
}

// Evasion values
public class EvadeBehaviour
{
    public float speed = 1.3f;
    public float time;
    public float time_limit = 0.6f;
    public int count;
    public int count_limit = 1;
}
