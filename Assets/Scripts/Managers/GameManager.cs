using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{


    //Properties
    static public GameManager Instance { get; private set; }

    private int level;

    public static Vector3 GetScreenBounds
    {
        get
        {
            Camera MainCamera = FindObjectOfType<Camera>();
            return MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
        }
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
           // DontDestroyOnLoad(gameObject); // nao precisa disso
        }
    }

    public void Update()
    {
        level = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("Level", level);
    }

    public void Restart()//Restart the game 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex - 1 < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ReturnToMainMenu()//Main menu
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()//Quit the game 
    {
        Debug.Log("Quit the game!");
        Application.Quit();
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public string GetLevelNames()
    {
        return SceneManager.GetActiveScene().name;
    }

    public int LevelCompleted()
    {
        if (SceneManager.GetActiveScene().buildIndex >2)
            return SceneManager.GetActiveScene().buildIndex - 1;
        else
            return 2;
    }


}
