using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    private bool playersSpawned = false;

    public bool IsGameOver { get; private set; } = false;

    //Properties
    static public GameManager Instance { get; private set; }

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
        IsGameOver = true;
        Debug.Log("Game Over");
        Time.timeScale = 0.0f;
        GameOverPanel.SetActive(true);
        GameOverPanel.GetComponentInChildren<Button>().Select();
    }

    public void YouWin()
    {
        IsGameOver = !IsGameOver;
        Debug.Log("You Win");
        Time.timeScale = 0.0f;
        YouWinPanel.SetActive(true);
        YouWinPanel.GetComponentInChildren<Button>().Select();
    }

    //Private functions
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        SceneManager.sceneLoaded += (scene, mode) => SceneLoaded();

        mPlayerControllers = new List<PlayerController>();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Debug.Log("Quit the game!");
        Application.Quit();
    }

    public void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex - 1 < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadTitleScreen()
    {
        SceneManager.LoadScene(0);
    }

    void SceneLoaded()
    {
        //Resets level parameters and retrieve inital player spawn points.
        Time.timeScale = 1.0f;
        mTrain = GameObject.Find("Train");
        playersSpawned = false;
        if (SceneManager.GetActiveScene().buildIndex > 0)
            Invoke("SpawnPlayers", 1.0f);

        GameObject spawnpoints = GameObject.Find("InitialSpawn");
        if (spawnpoints)
        {
            mInitialSpawnPoints = new List<Transform>(spawnpoints.GetComponentsInChildren<Transform>());
            mInitialSpawnPoints.RemoveAt(0);
        }

        YouWinPanel.SetActive(false);
        GameOverPanel.SetActive(false);
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        //When PlayerInputManager detects that a player has joined by pressing a button on their controller, they will be added to the PlayerControllers list.
        if (playerInput.TryGetComponent(out PlayerController controller))
        {
            mPlayerControllers.Add(controller);
            FindObjectOfType<TitleMenuControl>()?.UpdatePlayerStatus(mPlayerControllers.Count - 1, playerInput.currentControlScheme);

            //Spawns player avatar if player joins late into the level. (SHOULD ONLY BE USED FOR TESTING)
            if (playersSpawned)
            {
                var avatar = Instantiate(playerPrefab, mInitialSpawnPoints[mPlayerControllers.Count - 1].position, Quaternion.identity, mTrain.transform).GetComponent<Player>();
                mPlayerControllers[mPlayerControllers.Count - 1].SetPlayer(avatar);

                //Remove this line of code if changing color of player avatar sprite is unwanted
                avatar.GetComponentInChildren<SpriteRenderer>().color = GetPlayerColor(mPlayerControllers.Count - 1);
            }
        }
    }

    void OnPlayerLeft(PlayerInput playerInput)
    {
        int index = mPlayerControllers.IndexOf(playerInput.GetComponent<PlayerController>());
        mPlayerControllers.RemoveAt(index);
        FindObjectOfType<TitleMenuControl>()?.UpdatePlayerStatus(index, null);
    }

    void SpawnPlayers()
    {
        if (playersSpawned)
        {
            return;
        }

        for (int i = 0; i < mPlayerControllers.Count; ++i)
        {
            var avatar = Instantiate(playerPrefab, mInitialSpawnPoints[i].position, Quaternion.identity, mTrain.transform).GetComponent<Player>();
            mPlayerControllers[i].SetPlayer(avatar);

            //Remove this line of code if changing color of player avatar sprite is unwanted
            avatar.GetComponentInChildren<SpriteRenderer>().color = GetPlayerColor(i);
        }

        playersSpawned = true;
    }

    public Color GetPlayerColor(int playerNum)
    {
        Color color = Color.black;
        switch (playerNum)
        {
            case 0:
                color = Color.cyan;
                break;
            case 1:
                color = Color.magenta;
                break;
            case 2:
                color = Color.yellow;
                break;
            case 3:
                color = Color.grey;
                break;
        }
        return color;
    }
}
