using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    //Editor Fields
    [SerializeField] RectTransform anchorPosition = null;
    [SerializeField] GameObject levelSelectMenu = null;
    [SerializeField] Button StartButton = null;
    [SerializeField] Button BackButton = null;
    [SerializeField] GameObject buttonCopy = null;

    [SerializeField] float transitionDuration = 0.0f;
    [SerializeField] float titleScreenAnchorPosX = 0.0f;
    [SerializeField] float LevelSelectAnchorPosX = 0.0f;

    //Public Members
    public void Initialize()
    {
        StartButton.Select();
        BackButton.enabled = false;
        _buttons.Add(buttonCopy);
        LoadLevelButtons();
        SetPlayableLevels();
    }

    public void GoToTitleScreen()
    {
        StartCoroutine(MoveToTitleScreen());
        BackButton.enabled = false;
        StartButton.enabled = true;
        StartButton.Select();   
    }

    public void GoToLevelSelect()
    {
        StartCoroutine(MoveToLevelSelect());
        StartButton.enabled = false;
        BackButton.enabled = true;
        BackButton.Select();
    }

    //Private Members
    private List<GameObject> _buttons = new List<GameObject>();

    private void LoadLevelButtons()
    {
        for (int i = 2; i < SceneManager.sceneCountInBuildSettings; ++i)
        {
            if (i == 2)
            {
                _buttons[0].GetComponent<Button>().onClick.AddListener(() => { GameManager.Instance.LoadLevel(2); });
            }
            else
            {
                Button button = Instantiate(buttonCopy, levelSelectMenu.transform).GetComponent<Button>();
                button.onClick.AddListener(() => { GameManager.Instance.LoadLevel(i); });
                button.GetComponentInChildren<Text>().text = "Level " + (i - 1);
                _buttons.Add(button.gameObject);
            }
        }
    }

    private void SetPlayableLevels()
    {
        for (int i = 0; i < _buttons.Count; ++i)
        {
            if (i + 2 <= PlayerPrefs.GetInt("Level"))
                _buttons[i].GetComponent<Button>().interactable = true;
            else
                _buttons[i].GetComponent<Button>().interactable = false;
        }
    }

    private IEnumerator MoveToTitleScreen()
    {
        float startTime = Time.unscaledTime;
        float t = 0.0f;

        while (t <= 1.0f)
        {
            t = (Time.unscaledTime - startTime) / transitionDuration;

            anchorPosition.anchoredPosition = Vector3.Lerp(new Vector3(LevelSelectAnchorPosX, 0.0f, 0.0f), new Vector3(titleScreenAnchorPosX, 0.0f, 0.0f), t);

            yield return null;
        }
    }

    private IEnumerator MoveToLevelSelect()
    {
        float startTime = Time.unscaledTime;
        float t = 0.0f;

        while (t <= 1.0f)
        {
            t = (Time.unscaledTime - startTime) / transitionDuration;

            anchorPosition.anchoredPosition = Vector3.Lerp(new Vector3(titleScreenAnchorPosX, 0.0f, 0.0f), new Vector3(LevelSelectAnchorPosX, 0.0f, 0.0f), t);

            yield return null;
        }
    }
}
