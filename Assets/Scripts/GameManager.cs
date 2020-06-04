using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject playerControllerPrefab;
    public GameObject GameOverPanel;
    public GameObject YouWinPanel;

    private List<PlayerController> mPlayerControllers;
    private GameObject mTrain;

    //Properties
    public Vector3 ScreenBounds
    {
        get
        {
            Camera MainCamera = FindObjectOfType<Camera>();
            return MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
        }
    }

    //Public functions
    public void GameOver()
    {
        Debug.Log("Game Over");
        Time.timeScale = 0.0f;
        GameOverPanel.SetActive(true);
    }

    public void YouWin()
    {
        Debug.Log("You Win");
        Time.timeScale = 0.0f;
        YouWinPanel.SetActive(true);
    }

    //Private functions
    private void Awake()
    {
        mPlayerControllers = new List<PlayerController>();
        mTrain = GameObject.Find("Train");
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        //var player1 = PlayerInput.Instantiate(playerControllerPrefab, 0, "Gamepad", -1, Gamepad.current);
        var player2 = PlayerInput.Instantiate(playerControllerPrefab, 1, "KeyboardRight", -1, Keyboard.current);
        var player3 = PlayerInput.Instantiate(playerControllerPrefab, 2, "KeyboardLeft", -1, Keyboard.current);

        //var avatar1 = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, train.transform).GetComponent<Player>();
        var avatar2 = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, mTrain.transform).GetComponent<Player>();
        var avatar3 = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, mTrain.transform).GetComponent<Player>();

        //avatar1.GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
        avatar2.GetComponentInChildren<SpriteRenderer>().color = Color.cyan;
        avatar3.GetComponentInChildren<SpriteRenderer>().color = Color.magenta;

        //player1.GetComponent<PlayerController>().SetPlayer(ref avatar1);
        player2.GetComponent<PlayerController>().SetPlayer(ref avatar2);
        player3.GetComponent<PlayerController>().SetPlayer(ref avatar3);
    }
}
