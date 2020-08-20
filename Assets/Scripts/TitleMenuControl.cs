using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleMenuControl : MonoBehaviour
{
    public GameObject LevelSelectPanel;
    public GameObject[] PlayerStatuses;
    private List<Button> buttons;
    private void Start()
    {
        buttons = new List<Button>(LevelSelectPanel.GetComponentsInChildren<Button>());
        buttons[0].onClick.AddListener(() => { SceneManager.LoadScene(1); });
        buttons[1].onClick.AddListener(() => { SceneManager.LoadScene(2); });
        buttons[2].onClick.AddListener(() => { SceneManager.LoadScene(3); });
        buttons[3].onClick.AddListener(() => { SceneManager.LoadScene(4); });
        buttons[4].onClick.AddListener(() => { SceneManager.LoadScene(5); });
        buttons[5].onClick.AddListener(() => { SceneManager.LoadScene(6); });
        buttons[6].onClick.AddListener(() => { SceneManager.LoadScene(7); });
        buttons[7].onClick.AddListener(() => { SceneManager.LoadScene(8); });
        buttons[8].onClick.AddListener(() => { SceneManager.LoadScene(9); });
        buttons[9].onClick.AddListener(() => { SceneManager.LoadScene(10); });

        int levelReached = PlayerPrefs.GetInt("levelReached", 1);
        for (int i = 0; i < 10; i++)
        {
            if (i + 1 > levelReached)
            {
                buttons[i].interactable = false;
            }
        }


    }

    public void UpdatePlayerStatus(int playerIndex, string controlScheme)
    {
        string statusText = null;
        Color color = Color.white;

        if (!string.IsNullOrEmpty(controlScheme))
        {
            color = GameManager.Instance.GetPlayerColor(playerIndex);
        }

        switch(controlScheme)
        {
            case "WASD":
                statusText = "Ready!\nWASD Keyboard";
                break;
            case "Arrows":
                statusText = "Ready!\nArrows Keyboard";
                break;
            case "Gamepad":
                statusText = "Ready!\nGamepad";
                break;
            case null:
                statusText = "Not\nConnected";
                break;
        }

        PlayerStatuses[playerIndex].GetComponentInChildren<Text>().text = statusText;
        PlayerStatuses[playerIndex].GetComponentInChildren<Image>().color = color;
    }
}
