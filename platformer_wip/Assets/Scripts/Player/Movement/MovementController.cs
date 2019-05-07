using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(BoxCollider))]
public class MovementController : MonoBehaviour
{
    // Player input
    public PlayerInput inputs;
    // Movement values
    RunBehaviour run = new RunBehaviour();
    JumpBehaviour jump = new JumpBehaviour(2);
    WallBehaviour wall = new WallBehaviour();
    EvadeBehaviour evade = new EvadeBehaviour();
    public float gravity;
    public float gravity_mod = 1.3f;
    public Vector3 velocity;
    public bool is_right_facing;
    // Collision detection
    public float slope;
    public float slope_limit = 45;
    public const float edge = 0.03f;
    public int ray_count_x = 4;
    public int ray_count_y = 4;
    public float ray_spacing_x;
    public float ray_spacing_y;
    RayCastOrigins corners;
    Collisions collisions;
    public BoxCollider collider;
    public LayerMask collision_mask;

    float damage_velocity;
    float damage_timer;
    float damage_timer_max = 0.3f;
    bool can_take_damage;

    // Start
    void Start()
    {
        velocity = new Vector3();
        // Set gravity
        gravity = -2 * jump.height_max / Mathf.Pow(jump.time_limit, 2) * gravity_mod;
        // Set speeds for y component of movement
        jump.speed_y_min = Mathf.Sqrt(2 * Mathf.Abs(gravity) * jump.height_min);
        jump.speed_y_max = Mathf.Abs(gravity) * jump.time_limit;
        // Initialize other stats;
        wall.is_sliding = false;
        is_right_facing = true;
        corners = new RayCastOrigins();
        collisions = new Collisions();
        damage_timer = 0;
        can_take_damage = true;
    }

    // Update
    void Update()
    {
        // Reset variables
        CheckResets();
        // Apply movement to Vector3 velocity
        GetMovement();
        // Check collisions & translate player
        Move();
        // Update damage velocity
        if (damage_timer > 0)
        {
            damage_velocity /= 3;
            damage_timer -= Time.deltaTime;
        }
    }

    /// DAMAGE

    void OnDamageToPlayer(DamageTemplate.DamageInfo damage)
    {
        damage_velocity = damage.force * damage.direction;
        damage_timer = damage_timer_max;
        // Invulnerability works, but does not flow well with this combat system
        //StartCoroutine(DamageToPlayerInvulnerability(0.5f));
        
        // On taking damage hp is marked, not immediately removed
        // It will decay over time (relative to the amount of hp marked)
        PlayerState.data.hp_marked = damage.amount;
    }

    // Buffer of invulnerability after taking damage
    // This will last longer than damage_timer_max
    private IEnumerator DamageToPlayerInvulnerability(float seconds)
    {
        can_take_damage = false;
        yield return new WaitForSeconds(seconds);
        can_take_damage = true;
    }
    
    /// MOVEMENT

    // Translate player using Vector2 velocity as input
    void Move()
    {
        // Reset RayCastOrigins positions
        corners.GetBounds(collider);
        // Check for collisions
        GetCollisions();
        // Apply calculated movement
        transform.Translate(velocity);
        // Ensure player is not colliding with ceiling or floor
        if (collisions.up || collisions.down)
        {
            velocity.y = 0;
        }
    }

    // Handle movement conditions
    void GetMovement()
    {
        // Forced knockback if damaged
        if (damage_timer > 0)
        {
            velocity.x = damage_velocity;
            velocity.y += gravity * Time.deltaTime;
        }
        // Controller movement otherwise
        else
        {
            GetMovementX();
            GetMovementY();
        }
    }

    // Handle movement conditions along x axis
    void GetMovementX()
    {
        // Get x component from raw Vector2 input
        velocity.x = GetVelocity(inputs.input * Time.deltaTime);
        HandleEvade();
    }

    // Handle movement conditions along y axis
    void GetMovementY()
    {
        if (inputs.is_dashing && collisions.down)
        {
            velocity.y = 0;
        }
        else
        {
            HandleJump();
            HandleWallAction();
        }
    }

    // Handle evasion
    void HandleEvade()
    {
        ///TODO: Change this to run with a Coroutine, as it is currently spammable
        // When evade key is pressed
        if (inputs.dash_start && !inputs.is_dashing && collisions.down && evade.count > 0)
        {
            evade.time -= Time.deltaTime;
            evade.count -= 1;
            if (evade.time <= 0)
            {
                inputs.is_dashing = false;
                evade.time = evade.time_limit;
            }
        }
        // If currently evading
        else if (inputs.is_dashing)
        {
            evade.time -= Time.deltaTime;
            velocity.x = evade.speed * (is_right_facing ? 1 : -1);
        }
    }

    // Handle jumping
    void HandleJump()
    {
        // Start of a jump
        if (inputs.jump_start)
        {
            // Standard jump occurs when grounded, when jump counter > 0, or when climbing a wall
            if ((collisions.down || jump.count > 0) && !wall.is_sliding)
            {
                // Reduction in jump height for multi jump
                velocity.y = collisions.down ? jump.speed_y_max : jump.speed_y_max * jump.multi_mod;
                // Reduce jump count when necessary
                if (!wall.is_sliding)
                {
                    jump.count -= 1;
                }
            }
            // Wall jumping
            else if (collisions.left || collisions.right)
            {
                // Kicks player out horizontally from wall
                velocity.x = wall.jump_speed_x * (collisions.left ? 1 : -1);
                // Wall jump with modifier
                velocity.y = jump.speed_y_max * wall.jump_speed_y_mod;
            }
        }
        else if (inputs.jump_end && velocity.y > jump.speed_y_min)
        {
            velocity.y = jump.speed_y_min;
        }
    }

    // Handle wall climbing and sliding
    void HandleWallAction()
    {
        // If climbing up a wall, do not apply gravity
        if (inputs.input.y > 0 && wall.is_sliding && wall.climb_time > 0)
        {
            velocity.y = wall.climb_speed;
            wall.climb_time -= Time.deltaTime;
        }
        // If sliding down a wall, force of gravity is reduced
        else if (wall.is_sliding && velocity.y < 0)
        {
            velocity.y += gravity * Time.deltaTime * wall.slide_gravity_mod;
        }
        // Otherwise, apply gravity normally
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
    }
    
    // Get x component of velocity from raw Vector2 input
    float GetVelocity(Vector3 input)
    {
        // Smooth x component of movement
        var speed = collisions.down ? run.speed : jump.speed_x;
        var acceleration_time = collisions.down ? run.acceleration_time_x : jump.acceleration_time_x;
        return Mathf.SmoothDamp(velocity.x, input.x * speed, ref run.smooth_velocity_x, acceleration_time);
    }

    /// COLLISIONS
    
    //Snaps player to edge of platform if collision is imminent
    void GetCollisions()
    {
        // Reset collision data befeore detecting
        collisions.Reset();
        // Check for collisions along the x axis
        if (velocity.x != 0)
        {
            is_right_facing = velocity.x > 0;
            GetCollisionsX();
        }
        // Check for collisions along the y axis
        GetCollisionsY();
    }

    // Detect collisions along x axis
    void GetCollisionsX()
    {
        // Check if moving left or right
        float direction = (is_right_facing ? 1 : -1);
        // Raycast for handling collisions
        Vector3 ray_origin_anchor = (direction < 0) ? corners.bottom_left : corners.bottom_right;
        float ray_length = Mathf.Abs(velocity.x) + edge;
        for (int i = 0; i < ray_count_x; i++)
        {
            float ray_origin_mod = ray_spacing_x * i;
            Debug.DrawRay(
                ray_origin_anchor + Vector3.up * ray_origin_mod,
                Vector3.right * direction * ray_length,
                Color.yellow
                );
            RaycastHit hit;
            if (Physics.Raycast(
                ray_origin_anchor + Vector3.up * ray_origin_mod,
                Vector3.right * direction,
                out hit,
                ray_length,
                collision_mask
                ))
            {
                velocity.x = (hit.distance - edge) * direction;
                ray_length = hit.distance;
                collisions.left = direction < 0;
                collisions.right = direction > 0;
            }
        }

    }

    // Detect collisions along y axis
    void GetCollisionsY()
    {
        // Check if moving down or up
        float direction = (velocity.y <= 0) ? -1 : 1;
        // Raycast for handling collisions
        Vector3 ray_origin_anchor = (direction < 0) ? corners.bottom_left : corners.top_left;
        float ray_length = Mathf.Abs(velocity.y) + edge;
        for (int i = 0; i < ray_count_y; i++)
        {
            // Offset using velocity.x since detecting collision in next frame
            float ray_origin_mod = ray_spacing_y * i + velocity.x;
            Debug.DrawRay(
                ray_origin_anchor + Vector3.right * ray_origin_mod,
                Vector3.up * direction * ray_length,
                Color.yellow
                );
            RaycastHit hit;
            if (Physics.Raycast(
                ray_origin_anchor + Vector3.right * ray_origin_mod,
                Vector3.up * direction,
                out hit,
                ray_length,
                collision_mask
                ))
            {
                velocity.y = (hit.distance - edge) * direction;
                ray_length = hit.distance;
                collisions.down = direction < 0;
                collisions.up = direction > 0;
            }
        }
    }

    // Check which variables need to reset on Update
    void CheckResets()
    {
        CheckWallSliding();
        OnGroundReset();
        OnAnyReset();
    }
    
    // Check if player is sliding down a wall
    void CheckWallSliding()
    {
        wall.is_sliding = ((collisions.left && inputs.input.x < 0) || (collisions.right && inputs.input.x > 0) ? true : false);
    }

    // Reset variables when grounded
    void OnGroundReset()
    {
        if (collisions.down){
            jump.count = jump.count_limit;
            wall.climb_time = wall.climb_time_limit;
        }
    }

    // Reset variables wehn grounded or sliding on wall
    void OnAnyReset()
    {
        if (collisions.down || wall.is_sliding)
        {
            evade.count = evade.count_limit;
        }
    }

    // Bounds of the player's BoxCollider
    class RayCastOrigins
    {
        public Vector3 top_left;
        public Vector3 top_right;
        public Vector3 bottom_left;
        public Vector3 bottom_right;
        // Update all Vector3s for player's BoxCollider bounds
        public void GetBounds(BoxCollider box_collider)
        {
            Bounds bounds = box_collider.bounds;
            // Adjust bounds for edge
            bounds.Expand(edge * -2);
            top_left = new Vector3(bounds.min.x, bounds.max.y, 0);
            top_right = new Vector3(bounds.max.x, bounds.max.y, 0);
            bottom_left = new Vector3(bounds.min.x, bounds.min.y, 0);
            bottom_right = new Vector3(bounds.max.x, bounds.min.y, 0);
        }
    }
    
    // Collision detection
    class Collisions
    {
        public bool up;
        public bool down;
        public bool left;
        public bool right;
        // Reset collisions
        public void Reset()
        {
            up = false;
            down = false;
            left = false;
            right = false;
        }
    }
}
