using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject playerControllerPrefab;
    public GameObject GameOverPanel;
    public GameObject YouWinPanel;

    private List<PlayerController> mPlayerControllers;
    private List<Transform> mInitialSpawnPoints;
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

    public static Vector3 GetScreenBounds
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
        Time.timeScale = 1.0f;
        mPlayerControllers = new List<PlayerController>();
        mInitialSpawnPoints = new List<Transform>(GetComponentsInChildren<Transform>());
        mInitialSpawnPoints.Remove(transform);
        mTrain = GameObject.Find("Train");
    }

    private void Start()
    {
        //Create player controllers.
        var player1 = PlayerInput.Instantiate(playerControllerPrefab, 0, "KeyboardRight", -1, Keyboard.current);
        var player2 = PlayerInput.Instantiate(playerControllerPrefab, 1, "KeyboardLeft", -1, Keyboard.current);

        //Spawn player avatars in scene.
        var avatar1 = Instantiate(playerPrefab, mInitialSpawnPoints[0].position, Quaternion.identity, mTrain.transform).GetComponent<Player>();
        var avatar2 = Instantiate(playerPrefab, mInitialSpawnPoints[1].position, Quaternion.identity, mTrain.transform).GetComponent<Player>();

        //Set player avatar colors.
        avatar1.GetComponentInChildren<SpriteRenderer>().color = Color.cyan;
        avatar2.GetComponentInChildren<SpriteRenderer>().color = Color.magenta;

        //Hook up player avatars with their respective player Controllers.
        player1.GetComponent<PlayerController>().SetPlayer(ref avatar1);
        player2.GetComponent<PlayerController>().SetPlayer(ref avatar2);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
