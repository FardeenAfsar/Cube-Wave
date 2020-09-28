using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Controller2D))]
public class Player_Movement : MonoBehaviour
{
    public float jumpHeight = 4;
    public float timeToJumpApex = 0.4f;
    float accelerationTimeAir = .125f;
    float accelerationTimeGrounded = 0.045f;
    float moveSpeed = 6;
    float gravity;

    float jumpVelocity;
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

    void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }
    
    void Jump()
    {
        jumpTimer = 0;
        maxHeightReached = Mathf.NegativeInfinity;
        velocity.y = jumpVelocity;
        startHeight = transform.position.y;
        reachedApex = false;
    }

    void Update()
    {

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if(Input.GetKeyDown(KeyCode.Z) && controller.collisions.below)
        {
            Jump();
        }
        if (!controller.collisions.below && !reachedApex)
        {
            jumpTimer += Time.fixedDeltaTime;
        }

        float targetVelocity = input.x * moveSpeed;
        oldVelocity = velocity;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAir);
        velocity.y += gravity * Time.fixedDeltaTime;
        Vector3 deltaPos = (oldVelocity + velocity) * 0.5f * Time.fixedDeltaTime; 
        controller.Move(deltaPos);

        if (!reachedApex && maxHeightReached > transform.position.y)
        {
            float delta = maxHeightReached - startHeight;
            float error = jumpHeight - delta;
            Debug.Log($"error: {error:F4}, delta: {delta:F4}, time: {jumpTimer:F4}, gravity: {gravity:F4}, jumpforce: {jumpVelocity:F4}");
            reachedApex = true;
        }

        maxHeightReached = Mathf.Max(transform.position.y, maxHeightReached);
    }   

}
