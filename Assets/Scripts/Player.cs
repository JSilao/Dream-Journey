using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Health System")]
    public int maxHealth = 3;
    public int health = 3;

   public float gravity;
   public Vector2 velocity;
   public float maxXVelocity = 100;
   public float maxAcceleration = 10;
   public float acceleration = 10;
   public float distance = 0;

   public float jumpVelocity= 50;
   public float groundHeight = 10;
   public bool isGrounded = false;
   

   public bool isHoldingJump = false;
   public float maxHoldJumpTime = 0.4f;
   public float maxMaxHoldJumpTime = 0.4f;
   public float holdJumpTime = 0.0f;

   public float jumpGroundThreshold = 1;
   public bool isDead = false;
   [HideInInspector] public bool levelCompleted = false;
   public bool debugNoDeath = true;

   public Animator animator;
   

   public LayerMask groundLayerMask;
   public LayerMask obstacleLayerMask;
   
   GroundFall fall;
   CameraShake cameraShake;
    void Start()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }
    

//    void Update()
// {
//     Vector2 pos = transform.position;
//     float groundDistance = Mathf.Abs(pos.y - groundHeight);

//     // Jump start
//     if (isGrounded || groundDistance <= jumpGroundThreshold)
//     {
//         if (Input.touchCount > 0)
//         {
//             Touch touch = Input.GetTouch(0);
//             if (touch.phase == TouchPhase.Began)
//             {
//                 isGrounded = false;
//                 velocity.y = jumpVelocity;
//                 isHoldingJump = true;
//                 holdJumpTime = 0;

//                 if (fall != null)
//                 {
//                     fall.player = null;
//                     fall = null;
//                     cameraShake.StopShaking();
//                 }
//             }
//         }

//         // Optional editor support
// #if UNITY_EDITOR
//         if (Input.GetKeyDown(KeyCode.Space))
//         {
//             isGrounded = false;
//             velocity.y = jumpVelocity;
//             isHoldingJump = true;
//             holdJumpTime = 0;

//             if (fall != null)
//             {
//                 fall.player = null;
//                 fall = null;
//                 cameraShake.StopShaking();
//             }
//         }
// #endif
//     }

//     // Jump release
//     if (Input.touchCount > 0)
//     {
//         Touch touch = Input.GetTouch(0);
//         if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
//         {
//             isHoldingJump = false;
//         }
//     }

// #if UNITY_EDITOR
//     if (Input.GetKeyUp(KeyCode.Space))
//     {
//         isHoldingJump = false;
//     }
// #endif
// }

void Update()
{
    Vector2 pos = transform.position;
    float groundDistance = Mathf.Abs(pos.y - groundHeight);

    // Jump start
    if (isGrounded || groundDistance <= jumpGroundThreshold)
    {
        // Touch input (mobile)
        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;
            if (touch.press.wasPressedThisFrame)
            {
                isGrounded = false;
                velocity.y = jumpVelocity;
                isHoldingJump = true;
                holdJumpTime = 0;

                if (fall != null)
                {
                    fall.player = null;
                    fall = null;
                    cameraShake.StopShaking();
                }
            }
        }

        // Keyboard input (editor)
#if UNITY_EDITOR
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            isGrounded = false;
            velocity.y = jumpVelocity;
            isHoldingJump = true;
            holdJumpTime = 0;

            if (fall != null)
            {
                fall.player = null;
                fall = null;
                cameraShake.StopShaking();
            }
        }
#endif
    }

    // **Remove jump release input** — we now control jump height via maxHoldJumpTime only
}

    private void FixedUpdate()
    {
       Vector2 pos = transform.position;
        if (isDead || levelCompleted)
        {
            return;
        }
       if(!debugNoDeath && pos.y < -20)
{
            isDead = true;
        }

        if (!isGrounded)
        {
             if (isHoldingJump)
            {
                holdJumpTime += Time.fixedDeltaTime;
                if(holdJumpTime >= maxHoldJumpTime)
                {
                    isHoldingJump = false;
                }
            }

            pos.y += velocity.y * Time.fixedDeltaTime;

            if(!isHoldingJump)
            {
                velocity.y += gravity * Time.fixedDeltaTime; 
            }

            Vector2 rayOrigin = new Vector2(pos.x + 0.7f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;

            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, groundLayerMask);
            if(hit2D.collider != null)
            {
                Ground ground = hit2D.collider.GetComponent<Ground>();
                if(ground != null)
                {
                    if(pos.y >= ground.groundHeight)
                    {
                        groundHeight = ground.groundHeight;
                        pos.y = groundHeight;
                        velocity.y = 0;
                        isGrounded = true;
                    }
                    
                    fall = ground.GetComponent<GroundFall>();
                    if(fall != null)
                    {
                        fall.player = this;
                        cameraShake.StartShaking();
                    }
                    
                }
            }
            Debug.DrawRay(rayOrigin,rayDirection * rayDistance, Color.red);

            Vector2 wallOrigin = new Vector2(pos.x, pos.y);
            RaycastHit2D wallHit = Physics2D.Raycast(wallOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime, groundLayerMask);
            if(wallHit.collider != null)
            {
                Ground ground = wallHit.collider.GetComponent<Ground>();
                if(ground != null)
                {
                    if(pos.y < ground.groundHeight)
                    {
                        velocity.x = 0;
                    }
                }
            }
        }
        

        distance += velocity.x * Time.fixedDeltaTime;

        if (isGrounded)
        {
            float velocityRatio = velocity.x / maxXVelocity;
            acceleration = maxAcceleration * (1 - velocityRatio);
            maxHoldJumpTime = maxMaxHoldJumpTime * velocityRatio;

            velocity.x += acceleration * Time.fixedDeltaTime;

            if (FindFirstObjectByType<GameManager>().gameMode == GameManager.GameMode.Endless)
            {
                velocity.x += 0.01f * Time.fixedDeltaTime;
            }
            
            if(velocity.x >= maxXVelocity)
            {
                velocity.x = maxXVelocity;
            }

             Vector2 rayOrigin = new Vector2(pos.x - 0.7f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            if(fall != null)
            {
                rayDistance = -fall.fallSpeed * Time.fixedDeltaTime;
            }

            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
            if(hit2D.collider == null)
            {
                isGrounded = false;
            }
            Debug.DrawRay(rayOrigin,rayDirection * rayDistance, Color.yellow);
        }

            // Updated raycast collision checks
            Vector2 obstOrigin = new Vector2(pos.x, pos.y);

            RaycastHit2D obstHitX = Physics2D.Raycast(obstOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime, obstacleLayerMask);
            if(obstHitX.collider != null)
            {
                Obstacle obstacle = obstHitX.collider.GetComponent<Obstacle>();
                if(obstacle != null) CheckObstacleCollision(obstacle);
            }

            RaycastHit2D obstHitY = Physics2D.Raycast(obstOrigin, Vector2.up, velocity.y * Time.fixedDeltaTime, obstacleLayerMask);
            if(obstHitY.collider != null)
            {
                Obstacle obstacle = obstHitY.collider.GetComponent<Obstacle>();
                if(obstacle != null) CheckObstacleCollision(obstacle);
            }


        transform.position = pos;

        UpdateAnimator();
    }

    void hitObstacle(Obstacle obstacle)
    {
        Destroy(obstacle.gameObject);

        velocity.x *= 0.7f;

        TakeDamage(obstacle.damage);
    }

    void CheckObstacleCollision(Obstacle obstacle)
    {
        if(obstacle.isFake)
        {
            Destroy(obstacle.gameObject);
            return;
        }

        hitObstacle(obstacle);
    }

    void UpdateAnimator()
    {
        if(animator == null) return;

        // Set Jump animation when in air
        animator.SetBool("isJumping", !isGrounded);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            health = 0;
            isDead = true;
        }
    }
}
