using UnityEngine;
using UnityEngine.UI;
public class ProgressBar : MonoBehaviour
{
    public GameObject miniTrain;
    public Slider slider;
    private LevelManager levelManager;
    private float endPos;
    private Vector3 startPos;
    void Start()
    {
        levelManager = LevelManager.Instance;
        startPos = miniTrain.transform.position;
        endPos = slider.GetComponent<RectTransform>().sizeDelta.x;

    }

    void Update()
    {
        slider.value = 1-levelManager.timeRemaining / levelManager.time;
        miniTrain.transform.position = startPos + new Vector3(slider.value * 12.8f, 0.0f);
    }
}
