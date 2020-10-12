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

    static bool isPlay = false;
    static bool isPlayAgain = false;
    static bool isBack = false;

    public float transitionTime = 1f;
    private void Start()
    {
        ScoreP1.text = DeathAnimation.P1Wins.ToString();
        ScoreP2.text = DeathAnimation.P2Wins.ToString();
        Winner.text = DeathAnimation.winner;
        isPlay = false;
        isPlayAgain = false;
        isBack = false;
    }

    private void Update()
    {
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
    public void PlayGame()
    {
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
        isPlayAgain = true;
    }
    public void BackToMainMenu()
    {
        isBack = true;
    }
    public void ResetScore()
    {
        DeathAnimation.P1Wins = 0;
        DeathAnimation.P2Wins = 0;
        ScoreP1.text = DeathAnimation.P1Wins.ToString();
        ScoreP2.text = DeathAnimation.P2Wins.ToString();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
