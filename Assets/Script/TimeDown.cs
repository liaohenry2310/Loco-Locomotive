using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TimeDown : MonoBehaviour
{
    // Cyro review
    [Header("Properties")]
    [SerializeField]
    private float TimeRemaining = 300.0f; // 5 minutes in seconds

    private Text TextCountDown;
    private bool TimerIsRunning = false;
    bool isPause = true;

    void Start()
    {

        //GameTime = CountDownTime;
        TextCountDown = GetComponent<Text>();
        TimerIsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerIsRunning)
        {
            if (TimeRemaining > 0f)
            {
                TimeRemaining -= Time.deltaTime;
                DisplayTime(TimeRemaining);

            }
            else
            {
                Debug.Log("Time has run out!");
                TimeRemaining = 0f;
                TimerIsRunning = false;
            }

        }
        if (isPause)
        {
            if ((Input.GetKeyDown(KeyCode.Escape)))
            {
                Time.timeScale = 0;
                isPause = false;
            }
        }
        else
        {
            if ((Input.GetKeyDown(KeyCode.Escape)))
            {
                Time.timeScale = 1f;
                isPause = true;
            }
        }


    }

    private void DisplayTime(float timeToDisplay)
    {
        timeToDisplay++;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60f);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60f);
        TextCountDown.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }



}
// Start is called before the first frame update
//public float CountDownTime;
//private float GameTime;
//private float timer = 0;
//public Text GameCountTimeText;
//if ()//gamestart
#region Crhisty's code
//int M = (int)(GameTime / 60);
//float S = GameTime % 60;
////if ()//gamestart
////{
//timer += Time.deltaTime;
//if (timer >= 1f)
//{
//    timer = 0;
//    GameTime--;
//    GameCountTimeText.text = M + ":" + string.Format("{0:00}", S);

//    if (S <= 0)
//    {
//        GameTime = 0;
//        //Gameover
//    }
//}
#endregion