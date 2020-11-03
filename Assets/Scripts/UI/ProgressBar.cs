using UnityEngine;
using UnityEngine.UI;
public class ProgressBar : MonoBehaviour
{
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
        miniTrain.transform.position += new Vector3((scaleX / levelManager.time)*Time.deltaTime, 0,0);
    }
}
