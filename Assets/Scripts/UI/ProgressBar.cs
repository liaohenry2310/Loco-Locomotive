using UnityEngine;
using UnityEngine.UI;
public class ProgressBar : MonoBehaviour
{
    public Transform beginPoint;
    public Transform endPoint;
    public GameObject miniTrain;
    public GameObject Progressbar;
    public float scaleX = 0;
    private LevelManager levelManager;

    void Start()
    {
        levelManager = LevelManager.Instance;
    }

    void Update()
    {
        float t = levelManager.TimeRemaining / levelManager.TimeLimit;
        float x = Mathf.Lerp(endPoint.position.x, beginPoint.position.x, t);
        miniTrain.transform.position = new Vector3(x, Progressbar.transform.position.y, 0.0f);
    }
}
