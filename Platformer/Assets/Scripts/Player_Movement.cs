using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Controller2D))]
public class Player_Movement : MonoBehaviour
{
    public string PlayerH;
    public string PlayerV;
    public KeyCode jump;
    public KeyCode dash;


    public float maxJumpHeight = 2.5f;
    public float minJumpHeight = 1.25f;
    public float timeToJumpApex = 0.35f;
    public float gravityScale = 1.75f;
    public float dashTime;
    public float dashSpeed;
    public float dashCoolDown;

    float accelerationTimeAir = .125f;
    float accelerationTimeGrounded = 0.045f;
    float moveSpeed = 6;
    float gravity;
    float gravity_fall;
    float maxJumpVelocity;
    float minJumpVelocity;
    float velocityXSmoothing;
    float dashTimeLeft;
    float lastDash = Mathf.NegativeInfinity;
    //For Debug
    float startHeight = Mathf.NegativeInfinity;
    float maxHeightReached = Mathf.NegativeInfinity;
    bool reachedApex = true;
    float jumpTimer = 0;
    //

    bool isDashing;
    bool canMove = true;

    int facingDirection;

    Vector3 velocity;
    Vector3 oldVelocity;

    private Controller2D controller;
    public PowerUpInfo powerup;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        gravity_fall = gravity * gravityScale;
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        
    }

    void Jump()
    {
        jumpTimer = 0;
        maxHeightReached = Mathf.NegativeInfinity;
        velocity.y = maxJumpVelocity;
        startHeight = transform.position.y;
        reachedApex = false;
    }

    void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;
    }

    void CheckDash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                canMove = false;
                velocity = new Vector2(dashSpeed * facingDirection, 0);
                dashTimeLeft -= Time.deltaTime;
            }
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                canMove = true;
            }
        }
    }

    void InputSys()
    {
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
        if (Input.GetKeyDown(jump) && controller.collisions.below && canMove)
        {
            Jump();
        }
        if (Input.GetKeyUp(jump) && canMove)
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
            }
        }
        if (!controller.collisions.below && Input.GetKeyDown(jump) && powerup.secondJump && canMove)
        {
            Jump();
            powerup.secondJump = false;
        }

        if (Input.GetKeyDown(dash))
        {
            if (Time.time >= (lastDash + dashCoolDown))
            {
                AttemptToDash();
            }

        }

    }

    void Physics()
    {
        float inputX = Input.GetAxisRaw(PlayerH);

        if (!controller.collisions.below && !reachedApex)
        {
            jumpTimer += Time.deltaTime;
        }

        float targetVelocity = inputX * moveSpeed;

        oldVelocity = velocity;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAir);
        velocity.y += (!controller.collisions.below && reachedApex ? gravity_fall : gravity) * Time.deltaTime;
        velocity.y = Mathf.Min(velocity.y, maxJumpVelocity);
        Vector3 deltaPos = (oldVelocity + velocity) * 0.5f * Time.deltaTime;
        controller.Move(deltaPos);

        if (!reachedApex && maxHeightReached > transform.position.y)
        {
            float delta = maxHeightReached - startHeight;
            float error = maxJumpHeight - delta;
            Debug.Log($"error: {error:F4}, delta: {delta:F4}, time: {jumpTimer:F4}, gravity: {gravity:F4}, jumpforce: {maxJumpVelocity:F4}");
            reachedApex = true;
        }
        maxHeightReached = Mathf.Max(transform.position.y, maxHeightReached);

        if (canMove)
        {
            if (inputX < 0)
            {
                facingDirection = -1;
            }

            if (inputX > 0)
            {
                facingDirection = 1;
            }
        }
    }
    void Update()
    {
        InputSys();
        Physics();
        CheckDash();
    }

    public struct PowerUpInfo
    {
        public bool secondJump;
    }
}
