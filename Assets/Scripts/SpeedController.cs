using UnityEngine;

public class SpeedController : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField]
    private Transform TrainFrontCollider;
    [SerializeField]
    private Transform TrainRearCollider;
    [SerializeField]
    private Transform Train;
    [SerializeField]
    private float TrainSpeed = 10.0f;

    private Vector2 mScreenBounds;
    private InputReciever mInputReciever;

    private void Start()
    {
        Camera MainCam = FindObjectOfType<Camera>();
        mScreenBounds = MainCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCam.transform.position.z));
        mInputReciever = GetComponent<InputReciever>();
    }

    private void Update()
    {
        float translation = mInputReciever.GetRawDirectionalInput().x * TrainSpeed * Time.deltaTime;
        Vector3 trainPos;
        if (TrainFrontCollider.position.x + translation >= mScreenBounds.x) // Check to the right side of the screen
        {
            trainPos = new Vector3(mScreenBounds.x - TrainFrontCollider.position.x, 0.0f, 0.0f);
        }
        else if (TrainRearCollider.position.x + translation <= -mScreenBounds.x) // Check to the left side of the screen
        {
            trainPos = new Vector3(mScreenBounds.x + TrainRearCollider.position.x, 0.0f, 0.0f);
        }
        else
        {
            trainPos = new Vector3(translation, 0.0f, 0.0f);
        }
        Train.Translate(trainPos);
    }
}