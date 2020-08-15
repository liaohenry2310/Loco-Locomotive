using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMenuControl : MonoBehaviour
{
    public GameObject LevelSelectPanel;
    public GameObject[] PlayerStatuses;

    private List<Button> buttons;

    private void Start()
    {
        buttons = new List<Button>(LevelSelectPanel.GetComponentsInChildren<Button>());
        buttons[0].onClick.AddListener(() => { SceneManager.LoadScene(2); });
    }

    public void UpdatePlayerStatus(int playerIndex, string controlScheme)
    {
        string statusText = null;
        Color color = Color.white;

        if (!string.IsNullOrEmpty(controlScheme))
        {
            color = GameManager.Instance.GetPlayerColor(playerIndex);
        }

        switch (controlScheme)
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
