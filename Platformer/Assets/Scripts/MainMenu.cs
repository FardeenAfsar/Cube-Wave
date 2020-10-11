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

    private void Start()
    {
        ScoreP1.text = DeathAnimation.P1Wins.ToString();
        ScoreP2.text = DeathAnimation.P2Wins.ToString();
        Winner.text = DeathAnimation.winner;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
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
