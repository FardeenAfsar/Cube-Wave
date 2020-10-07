using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Player_Movement))]
public class Controller2D : MonoBehaviour
{
    public LayerMask collisionMask;
    public LayerMask otherPlayerMask;
    public LayerMask powerUpMask;

    public GameObject prefabPower;

    const float skinwidth = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    BoxCollider2D colliderbox;
    Player_Movement player_movement;
    RaycastOrigins raycastOrigins;

    public CollisionInfo collisions;
    void Start()
    {
        colliderbox = GetComponent<BoxCollider2D>();
        player_movement = GetComponent<Player_Movement>();
        CalculateRaySpacing();
    }

    public void Move(Vector3 velocity)
    {
        collisions.Reset();
        UpdateRaycastOrigins();
        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }
        transform.Translate(velocity);
    }


    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinwidth;


        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit2 = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, otherPlayerMask);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            RaycastHit2D hitPowerUp = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, powerUpMask);

            if (hit2)
            {
                velocity.x = (hit2.distance - skinwidth) * directionX;
                rayLength = hit2.distance;

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;

            } 
            if (hit)
            {
                velocity.x = (hit.distance - skinwidth) * directionX;
                rayLength = hit.distance;

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }
            if (hitPowerUp)
            {
                Vector3 powerPos = hitPowerUp.collider.transform.position;
                Destroy(hitPowerUp.collider.gameObject);
                player_movement.powerup.secondJump = true;
                StartCoroutine(delay(powerPos, 10f));
            }

        }
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinwidth;


        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit2 = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, otherPlayerMask);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            RaycastHit2D hitPowerUp = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, powerUpMask);

            if (hit2)
            {
                velocity.y = (hit2.distance - skinwidth) * directionY;
                rayLength = hit2.distance;

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
                if(hit2.normal.y >= 0.9f)
                {
                    collisions.isDying = true;
                    collisions.otherCollider = hit2.collider.gameObject;
                }

            }
            if (hit)
            {
                velocity.y = (hit.distance - skinwidth) * directionY;
                rayLength = hit.distance;

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }

            if (hitPowerUp)
            {
                Vector3 powerPos = hitPowerUp.collider.transform.position;
                Destroy(hitPowerUp.collider.gameObject);
                player_movement.powerup.secondJump = true;
                StartCoroutine(delay(powerPos, 10f));
            }

        }
    }
    IEnumerator delay(Vector3 pos, float delayTime)
    {
        yield return new WaitForSecondsRealtime(delayTime);
        Spawn(pos);

    }

    void Spawn(Vector3 pos)
    {
        Instantiate(prefabPower,pos,Quaternion.identity);
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = colliderbox.bounds;
        bounds.Expand(skinwidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = colliderbox.bounds;
        bounds.Expand(skinwidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;
        public bool isDying;
        public GameObject otherCollider;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }
}
