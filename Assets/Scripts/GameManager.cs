using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float timeLeft = 25;

    bool gameOver = false;
    bool win = false;

    public GameObject winText;
    public GameObject gameOverText;
    public GameObject ball;
    public PlayerController player;
    public TextMeshProUGUI timerText;

    public GameObject timeText;
    
    public int currentScene;
    public int nextScene;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft >= 0 && !win)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = timeLeft.ToString("F1");
        }
        if (timeLeft <= 0 && !win)
        {
            GameOver();
        }
    }

    public void WinGame()
    {
        win = true;
        winText.SetActive(true);
        player.enabled = false;
        ball.SetActive(false);
    }

    public void GameOver()
    {
        gameOver = true;
        gameOverText.SetActive(true);
        player.enabled = false;
        ball.SetActive(false);
    }
    
    public void TimeText()
    {
        timeText.SetActive(false);
    }
    

    public void RestartGame()
    {
        SceneManager.LoadScene(currentScene);
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(nextScene);
    }
    public void Back()
    {
        SceneManager.LoadScene(0);
    }
    public void LevelMenu()
    {
        SceneManager.LoadScene(1);
    }
}
