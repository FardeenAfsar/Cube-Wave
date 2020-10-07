using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class DeathAnimation : MonoBehaviour
{
    Material material;
    public GameObject otherPlayer;
    float fade = 1f;
    public float dissolveTime = 1f;

    Controller2D controller;
    void Start()
    {
        
        controller = GetComponent<Controller2D>();
        material = otherPlayer.GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        if (controller.collisions.isDying)
        {
            fade -= Time.deltaTime * dissolveTime;
            if (fade <= 0f)
            {
                fade = 0f;
                controller.collisions.isDying = false;
                Destroy(controller.collisions.otherCollider);
            }

            material.SetFloat("_Fade", fade);

        }
    }
}
