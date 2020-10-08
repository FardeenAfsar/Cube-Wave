using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PowerUps : MonoBehaviour
{
    Material material;
    public GameObject powerUp;
    float fade = 1f;
    public float dissolveTime = 1f;
    
    Controller2D controller;
    void Start()
    {
        controller = GetComponent<Controller2D>();
    }
    void Update()
    {
        if (controller.collisions.isPowerUp)
        {
            material = controller.collisions.powerUpCollider.GetComponent<SpriteRenderer>().material;
            fade -= Time.deltaTime * dissolveTime;
            if (fade <= 0f)
            {
                fade = 0f;
                controller.collisions.isPowerUp = false;
                Destroy(controller.collisions.powerUpCollider);
                fade = 1f;
            }
            material.SetFloat("_Fade", fade);
        }
    }
}
