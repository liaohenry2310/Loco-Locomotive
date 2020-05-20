using UnityEngine;

public class SpeedController : MonoBehaviour
{
    public Transform trainFrontCollider;
    public Transform trainRearCollider;
    public Transform train;
    public float trainSpeed = 1.5f;

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
        float translation = mInputReciever.GetDirectionalInput().x * trainSpeed * Time.deltaTime;
        Vector3 trainPos;
        if (trainFrontCollider.position.x + translation >= mScreenBounds.x)
        {
            trainPos = new Vector3(mScreenBounds.x - trainFrontCollider.position.x, 0.0f, 0.0f);
        }
        else if (trainRearCollider.position.x + translation <= -mScreenBounds.x)
        {
            trainPos = new Vector3(mScreenBounds.x + trainRearCollider.position.x, 0.0f, 0.0f);
        }
        else
        {
            trainPos = new Vector3(translation, 0.0f, 0.0f);
        }
        train.Translate(trainPos);
    }
}