using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Controller2D))]
public class Player_Movement : MonoBehaviour
{
    public string PlayerH;
    public string PlayerV;
    public KeyCode jump;

    public float maxJumpHeight = 2.5f;
    public float minJumpHeight = 1.25f;
    public float timeToJumpApex = 0.35f;
    public float gravityScale = 1.75f;
    float accelerationTimeAir = .125f;
    float accelerationTimeGrounded = 0.045f;
    float moveSpeed = 6;
    float gravity;
    float gravity_fall;

    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;
    private Controller2D controller;
    //For Debug
    float startHeight = Mathf.NegativeInfinity;
    float maxHeightReached = Mathf.NegativeInfinity;
    bool reachedApex = true;
    float jumpTimer = 0;
    //
    Vector3 oldVelocity;

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

    void Update()
    {

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
        Vector2 input = new Vector2(Input.GetAxisRaw(PlayerH), Input.GetAxisRaw(PlayerV));

        if(Input.GetKeyDown(jump) && controller.collisions.below)
        {
            Jump();
        }
        if (Input.GetKeyUp(jump))
        {
            if (velocity.y > minJumpVelocity) {
                velocity.y = minJumpVelocity;
            }
        }
        if (!controller.collisions.below && Input.GetKeyDown(jump) && powerup.secondJump)
        {
            Jump();
            powerup.secondJump = false;
        }

        if (!controller.collisions.below && !reachedApex)
        {
            jumpTimer += Time.deltaTime;
        }

        float targetVelocity = input.x * moveSpeed;
        oldVelocity = velocity;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAir);
        velocity.y += (!controller.collisions.below && reachedApex?gravity_fall:gravity) * Time.deltaTime;
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
    }

    public struct PowerUpInfo
    {
        public bool secondJump;
    }
}
