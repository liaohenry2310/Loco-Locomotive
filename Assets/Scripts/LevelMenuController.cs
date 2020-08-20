using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMenuController : MonoBehaviour
{
    public Button[] levelButton;

    void Start()
    {
        //scene 0-> game Loader
        //scene 1-> main menu
        //scene 2-> level select
        //level button 1 ->load scene 3
        levelButton[1].onClick.AddListener(() => { SceneManager.LoadScene(3); });
        levelButton[2].onClick.AddListener(() => { SceneManager.LoadScene(4); });
        levelButton[3].onClick.AddListener(() => { SceneManager.LoadScene(5); });
        levelButton[4].onClick.AddListener(() => { SceneManager.LoadScene(6); });
        levelButton[5].onClick.AddListener(() => { SceneManager.LoadScene(7); });
        levelButton[6].onClick.AddListener(() => { SceneManager.LoadScene(8); });
        levelButton[7].onClick.AddListener(() => { SceneManager.LoadScene(9); });
        levelButton[8].onClick.AddListener(() => { SceneManager.LoadScene(10); });
        levelButton[9].onClick.AddListener(() => { SceneManager.LoadScene(11); });
        levelButton[10].onClick.AddListener(() => { SceneManager.LoadScene(12); });


        int levelReached = PlayerPrefs.GetInt("levelReach", 2);
        for (int i = 1; i < levelButton.Length; i++)
        {
            if (i+1 > levelReached)     
            {
                levelButton[i].interactable = false; 
            }
        }
    }
    public void LoadTitleScreen()
    {
        SceneManager.LoadScene(1);
    }
}
