using GamePlay;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    //Public members
    static public LevelManager Instance { get; private set; }

    public GameObject GameOverPanel;
    public GameObject GameWinPanel;

    public float TimeLimit = 300.0f;
    public float TimeRemaining = 0.0f;

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
            _gameManager.SaveLevelCompleted();
        }
    }

    public void LoadNextLevel()
    {
        _gameManager.LoadNextLevel();
        Time.timeScale = 1.0f;
    }
    public void RestartLevel()
    {
        _gameManager.Restart();
        Time.timeScale = 1.0f;
    }

    public void ReturnToMainMenu()
    {
        _gameManager.ReturnToMainMenu();
        Time.timeScale = 1.0f;
    }

    public void QuitGame()
    {
        _gameManager.QuitGame();
    }

    //Private members
    private GameManager _gameManager;
    private bool timerIsRunning = false;

    private void Awake()
    {
        Instance = this;
        _gameManager = GameManager.Instance;

        TimeRemaining = TimeLimit;
        timerIsRunning = true;

        Train train = FindObjectOfType<Train>();
        if (train)
            train.OnGameOver += GameOver;
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            if (TimeRemaining > 0f)
            {
                TimeRemaining -= Time.deltaTime;
            }
            else
            {
                timerIsRunning = false;
                GameWin();
            }
        }
    }
}