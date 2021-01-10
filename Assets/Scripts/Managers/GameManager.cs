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

    public void Restart()
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

    public void ReturnToMainMenu()
    {
        StartCoroutine(LoadScene(1));
    }

    public void QuitGame()
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
        if (level < SceneManager.GetActiveScene().buildIndex + 1)
        {
            level = SceneManager.GetActiveScene().buildIndex + 1;
            PlayerPrefs.SetInt("Level", level);
        }
    }

    //Private Members
    private int level = 0;
    private bool _loadingScene = false;
    private TitleScreen titleScreen = null;
    private ObjectPoolManager _objectPoolManager = null;

    private void Awake()
    {
        
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }

        if (PlayerPrefs.GetInt("Level") < 2)
            PlayerPrefs.SetInt("Level", 2);
        else
            level = PlayerPrefs.GetInt("Level");

        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => { if (scene.buildIndex == 1) LoadTitleScreen(); };
    }

    private IEnumerator LoadScene(int sceneBuildIndex)
    {
        if (!_loadingScene)
        {
            _loadingScene = true;
            sceneTransition.StartTransition();
            yield return new WaitForSecondsRealtime(sceneTransition.Duration);
            _objectPoolManager = ServiceLocator.Get<ObjectPoolManager>();
            _objectPoolManager.RecycleEntirePool();
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
            Time.timeScale = 1.0f;
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