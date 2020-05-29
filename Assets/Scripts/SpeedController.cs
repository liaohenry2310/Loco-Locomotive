using UnityEngine;

[RequireComponent(typeof(FuelController))]
public class SpeedController : MonoBehaviour
{
    [Header("Properties")]
    public Transform trainFrontCollider;
    public Transform trainRearCollider;
    public Transform train;
    public float trainSpeed = 1.5f;
    public float SpendFuel = 0.5f;

    private InputReciever mInputReciever;
    private FuelController mFuelController;
    private Vector3 mScreenBounds;

    private void Start()
    {
        mScreenBounds = FindObjectOfType<GameManager>().ScreenBounds;
        mInputReciever = GetComponent<InputReciever>();
        mFuelController = GetComponent<FuelController>();
    }

    private void Update()
    {
        float translation = mInputReciever.GetDirectionalInput().x * trainSpeed * Time.deltaTime;
        if (translation != 0f)
        {
            UseFuel(SpendFuel);
        }
     
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

    private void UseFuel(float fuel)
    {
        mFuelController.CurrentFuel(fuel);
    }

}