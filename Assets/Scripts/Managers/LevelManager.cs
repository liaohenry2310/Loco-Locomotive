using GamePlay;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    static public LevelManager Instance { get; private set; }

    public Train train;
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
        Instance = this;
        train.OnGameOver += GameOver;
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
        if (!GameWinPanel.activeInHierarchy)
        {
            Debug.Log("Game Over");
            Time.timeScale = 0.0f;
            GameOverPanel.SetActive(true);
            GameOverPanel.GetComponentInChildren<Button>().Select();
        }
    }

    public void GameWin()
    {
        if (!GameOverPanel.activeInHierarchy)
        {
            Debug.Log("You Win");
            Time.timeScale = 0.0f;
            GameWinPanel.SetActive(true);
            GameWinPanel.GetComponentInChildren<Button>().Select();
            gameManager.SaveLevelCompleted();
        }
    }

    public void LoadNextLevel()
    {
        gameManager.LoadNextLevel();
        Time.timeScale = 1.0f;
    }
    public void RestartLevel()
    {
        gameManager.Restart();
        Time.timeScale = 1.0f;
    }

    public void ReturnToMainMenu()
    {
        gameManager.ReturnToMainMenu();
        Time.timeScale = 1.0f;
    }

    public void QuitGame()
    {
        gameManager.QuitGame();
    }
}