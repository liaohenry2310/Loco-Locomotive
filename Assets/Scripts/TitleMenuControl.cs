using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleMenuControl : MonoBehaviour
{
    public GameObject LevelSelectPanel;

    private List<Button> buttons;

    private void Start()
    {
        buttons = new List<Button>(LevelSelectPanel.GetComponentsInChildren<Button>());
        for (int i = 0; i < SceneManager.sceneCount && i < buttons.Count; ++i)
        {
            buttons[i].onClick.AddListener(() => { SceneManager.LoadScene(i + 1); });
        }
    }
}
