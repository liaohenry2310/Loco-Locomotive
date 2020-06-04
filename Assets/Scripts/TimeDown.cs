using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TimeDown : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField]
    private float timeRemaining = 300.0f;
    [SerializeField]
    private bool timerIsRunning = false;

    private Text textCountDown;
    private GameManager gameManager;

    //Public functions
    public void PauseTimer()
    {
        timerIsRunning = false;
    }

    public void ResumeTimer()
    {
        timerIsRunning = true;
    }

    //Private functions
    private void Awake()
    {
        textCountDown = GetComponent<Text>();
        gameManager = FindObjectOfType<GameManager>();
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
                gameManager.YouWin();
            }

        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60f);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60f);
        textCountDown.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}