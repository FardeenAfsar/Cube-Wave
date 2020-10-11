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

    public GameObject Round1;
    Material matround;


    Controller2D controller;
    void Start()
    {
        
        controller = GetComponent<Controller2D>();
        material = otherPlayer.GetComponent<SpriteRenderer>().material;
        matround = Round1.GetComponent<SpriteRenderer>().material;

        Round1.SetActive(true);
        GetComponent<Player_Movement>().enabled = false;
        StartCoroutine("PlayerTransitionIn");
        StartCoroutine("RoundFadeOut");
    }

    IEnumerator PlayerTransitionIn()
    {
        for (float f = 0.01f; f <= 1; f += 0.02f)
        {
            material.SetFloat("_Fade", f);
            yield return new WaitForSeconds(0.02f);
        }
        GetComponent<Player_Movement>().enabled = true;
    }

    IEnumerator RoundFadeOut()
    {
        for (float f = 1.5f; f >= -0.05f; f -=0.02f)
        {
            matround.SetFloat("_Step",f);
            yield return new WaitForSeconds(0.02f);
        }
        Round1.SetActive(false);
    }

    void Update()
    {
        if (controller.collisions.isDying)
        {
            controller.collisions.otherCollider.GetComponent<Player_Movement>().enabled = false;
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
