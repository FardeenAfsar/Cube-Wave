using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI ScoreP1;
    public TextMeshProUGUI ScoreP2;
    public TextMeshProUGUI Winner;

    public Animator transition;

    public AudioSource clickAudio;

    static bool isPlay = false;
    static bool isPlayAgain = false;
    static bool isBack = false;

    public int sceneNumber;

    public float transitionTime = 1f;
    private void Start()
    {
        if (sceneNumber == 1) {
            ScoreP1.text = DeathAnimation.P1Wins.ToString();
            ScoreP2.text = DeathAnimation.P2Wins.ToString();
            Winner.text = DeathAnimation.winner;
        }
        isPlay = false;
        isPlayAgain = false;
        isBack = false;
    }

    private void Update()
    {
        if (sceneNumber == 2) {
            if (isPlay && SceneManager.GetActiveScene().buildIndex < 1)
            {
                StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
            }
            if (isPlayAgain && SceneManager.GetActiveScene().buildIndex == 2)
            {
                StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
            }
            if (isBack && SceneManager.GetActiveScene().buildIndex == 2)
            {
                StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 2));
            }
        }
    }
    public void PlayGame()
    {
        clickAudio.Play();
        isPlay = true;
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }


    public void PlayAgain()
    {
        clickAudio.Play();
        isPlayAgain = true;
    }
    public void BackToMainMenu()
    {
        clickAudio.Play();
        isBack = true;
    }
    public void ResetScore()
    {
        clickAudio.Play();
        DeathAnimation.P1Wins = 0;
        DeathAnimation.P2Wins = 0;
        ScoreP1.text = DeathAnimation.P1Wins.ToString();
        ScoreP2.text = DeathAnimation.P2Wins.ToString();
    }

    public void QuitGame()
    {
        clickAudio.Play();
        Application.Quit();
    }

    public void HowToPlayAudio()
    {
        clickAudio.Play();
    }
}
