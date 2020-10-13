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
    public KeyCode smash;



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

    
    float maxHeightReached = Mathf.NegativeInfinity;
    bool reachedApex = true;

    public bool isDashing;
    bool canMove = true;
    public bool isSmash;

    int facingDirection;

    Vector3 velocity;
    Vector3 oldVelocity;

    private Controller2D controller;
    public PowerUpInfo powerup;

    public GameObject UpArrow;
    public GameObject DashArrow;
    public GameObject DownArrow;

    public AudioSource audioDash;
    public AudioSource audioJump;
    public AudioSource audioSmash;

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
        maxHeightReached = Mathf.NegativeInfinity;
        velocity.y = maxJumpVelocity;
        reachedApex = false;
        audioJump.Play();
    }

    void AttemptToSmash()
    {
        isSmash = true;
        audioSmash.Play();
    }

    void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;
        audioDash.Play();
    }

    void CheckSmash()
    {
        if (isSmash)
        {
            canMove = false;
            velocity = new Vector2(0, -maxJumpVelocity*1.5f);
        }
        if (isSmash && controller.collisions.below)
        {
            Debug.Log("SmashReset");
            canMove = true;
            velocity = new Vector2(0, 0);
            isSmash = false;
            powerup.smashAbility = false;
        }
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
                powerup.dashAbility = false;
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

        if (Input.GetKeyDown(dash)/* && powerup.dashAbility */)
        {
            if (Time.time >= (lastDash + dashCoolDown))
            {
                AttemptToDash();
            }
        }

        if (Input.GetKeyDown(smash) && !controller.collisions.below && canMove /* && powerup.smashAbility */)
        {
            AttemptToSmash();
        }

    }

    void Physics()
    {
        float inputX = Input.GetAxisRaw(PlayerH);
        float targetVelocity = inputX * moveSpeed;

        oldVelocity = velocity;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAir);
        velocity.y += (!controller.collisions.below && reachedApex ? gravity_fall : gravity) * Time.deltaTime;
        velocity.y = Mathf.Min(velocity.y, maxJumpVelocity);
        Vector3 deltaPos = (oldVelocity + velocity) * 0.5f * Time.deltaTime;
        controller.Move(deltaPos);

        if (!reachedApex && maxHeightReached > transform.position.y)
        {
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

    void CheckPowerCue()
    {
        UpArrow.SetActive(powerup.secondJump);
        DownArrow.SetActive(powerup.smashAbility);
        DashArrow.SetActive(powerup.dashAbility);
    }

    void Update()
    {
        InputSys();
        Physics();
        CheckDash();
        CheckSmash();
        CheckPowerCue();
    }

    public struct PowerUpInfo
    {
        public bool secondJump, dashAbility, smashAbility;
    }
}
