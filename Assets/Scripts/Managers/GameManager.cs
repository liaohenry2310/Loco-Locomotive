using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Editor Fields
    [SerializeField] private SceneTransition sceneTransition = null;

    //Public Members
    static public GameManager Instance { get; private set; }

    public static Vector3 GetScreenBounds
    {
        get
        {
            Camera MainCamera = FindObjectOfType<Camera>();
            return MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
        }
    }

    public void Restart()//Restart the game 
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadLevel(int level)
    {
        StartCoroutine(LoadScene(level));
    }

    public void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings)
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void ReturnToMainMenu()//Main menu
    {
        StartCoroutine(LoadScene(1));
    }

    public void QuitGame()//Quit the game 
    {
        Debug.Log("Quit the game!");
        Application.Quit();
    }

    public string GetLevelNames()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void SaveLevelCompleted()
    {
        if (level < SceneManager.GetActiveScene().buildIndex)
        {
            level = SceneManager.GetActiveScene().buildIndex;
            PlayerPrefs.SetInt("Level", level);
        }
    }

    //Private Members
    private int level;
    private bool _loadingScene = false;
    private TitleScreen titleScreen = null;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // nao precisa disso
        }

        if (PlayerPrefs.GetInt("Level") < 2)
            PlayerPrefs.SetInt("Level", 2);

        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => { if (scene.buildIndex == 1) LoadTitleScreen(); };
    }

    private IEnumerator LoadScene(int sceneBuildIndex)
    {
        if (!_loadingScene)
        {
            _loadingScene = true;
            sceneTransition.StartTransition();
            yield return new WaitForSecondsRealtime(sceneTransition.Duration);
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
            yield return new WaitForSecondsRealtime(0.5f);
            sceneTransition.StartTransition();
            _loadingScene = false;
        }
    }

    private void LoadTitleScreen()
    {
        titleScreen = FindObjectOfType<TitleScreen>();
        if (titleScreen)
        {
            titleScreen.Initialize();
        }
    }
}