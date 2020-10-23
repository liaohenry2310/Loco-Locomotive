using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject LevelSelectPanel;
    private GameObject mTrain;


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

        SceneManager.sceneLoaded += (scene, mode) => SceneLoaded();
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
    public void LoadLevelScreen()//Level Select menu
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame()//Quit the game 
    {
        Debug.Log("Quit the game!");
        Application.Quit();
    }

    void SceneLoaded()
    {
        Time.timeScale = 1.0f;
        mTrain = GameObject.Find("Train");
    }
}
