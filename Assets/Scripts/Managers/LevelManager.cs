using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    static public LevelManager Instance { get; private set; }

    public GameObject GameOverPanel;
    public GameObject GameWinPanel;
    private GameManager gameManager;

    public bool IsGameOver { get; private set; } = false;

    #region Timer
    [Header("Timer")]

    public float time = 300.0f;
    private bool timerIsRunning = false;
    public float timeRemaining = 0.0f;

    public Text textCountDown;
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        timeRemaining = time;
        timerIsRunning = true;
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0f)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);

            }
            else
            {
                timeRemaining = 0f;
                timerIsRunning = false;
                GameWin();
            }

        }
    }
    #region Timer

    private void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60f);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60f);
        //textCountDown.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void PauseTimer()
    {
        timerIsRunning = false;
    }

    public void ResumeTimer()
    {
        timerIsRunning = true;
    }


    #endregion

    public void GameOver()
    {
        IsGameOver = true;
        Debug.Log("Game Over");
        Time.timeScale = 0.0f;
        GameOverPanel.SetActive(true);
        //GameOverPanel.GetComponentInChildren<Button>().Select();
    }

    public void GameWin()
    {
        IsGameOver = !IsGameOver;
        Debug.Log("You Win");
        Time.timeScale = 0.0f;
        GameWinPanel.SetActive(true);
        //GameWinPanel.GetComponentInChildren<Button>().Select();
    }

    public void LoadNextLevel()
    {
        gameManager.LoadNextLevel();
    }

    public void ReturnToMainMenu()
    {
        gameManager.ReturnToMainMenu();
    }
    public void LoadScene(int sceneIndex)
    {
        gameManager.LoadScene(sceneIndex);
    }
}
