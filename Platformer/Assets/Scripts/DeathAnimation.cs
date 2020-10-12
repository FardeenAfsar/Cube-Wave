using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Controller2D))]
public class DeathAnimation : MonoBehaviour
{
    Material material;
    public GameObject otherPlayer;
    float fade = 1f;
    public float dissolveTime = 1f;

    private GameObject Round;
    public GameObject Round1;
    public GameObject Round2;
    public GameObject Final;
    
    Material materialRound;


    static int P2Points = 0;
    static int P1Points = 0;
    public static int P1Wins = 0;
    public static int P2Wins = 0;

    public static string winner;



    Controller2D controller;
    void Start()
    {
        
        controller = GetComponent<Controller2D>();
        material = otherPlayer.GetComponent<SpriteRenderer>().material;
        RoundTransitions();

    }

    void RoundTransitions()
    {
        if (P1Points == 0 && P2Points == 0)
        {
            materialRound = Round1.GetComponent<SpriteRenderer>().material;
            Round1.SetActive(true);
            Round = Round1;
        }
        else if ((P1Points == 1 && P2Points == 0) || (P2Points == 1 && P1Points == 0))
        {
            materialRound = Round2.GetComponent<SpriteRenderer>().material;
            Round2.SetActive(true);
            Round = Round2;
        }
        else
        {
            materialRound = Final.GetComponent<SpriteRenderer>().material;
            Final.SetActive(true);
            Round = Final;
        }

        GetComponent<Player_Movement>().enabled = false;
        StartCoroutine(PlayerTransitionIn());
        StartCoroutine(RoundFadeOut(Round));
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

    IEnumerator RoundFadeOut(GameObject Round)
    {
        for (float f = 1.5f; f >= 0.05; f -=0.02f)
        {
            materialRound.SetFloat("_Step",f);
            yield return new WaitForSeconds(0.02f);
        }
        Round.SetActive(false);
    }

    void PlayerScore()
    {
        if (controller.collisions.otherCollider.tag == "Player1") { P2Points++; }
        else { P1Points++; }

        if (P1Points > 1)
        {
            Debug.Log("Pink won");
            winner = "Player1 Wins";
            P1Wins++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            P1Points = 0;
            P2Points = 0;
        }
        else if (P2Points > 1)
        {
            Debug.Log("Cyan Won");
            winner = "Player2 Wins";
            P2Wins++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            P1Points = 0;
            P2Points = 0;
        }
        else
        {
            Debug.Log("Pink: " + P1Points + "Cyan: " + P2Points);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
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
                PlayerScore();

            }

            material.SetFloat("_Fade", fade);

        }
    }
}
