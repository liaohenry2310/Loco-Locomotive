using UnityEngine;

public class SpeedController : MonoBehaviour
{
    public enum Directions
    {
        Stand, Left, Right
    }

    private Directions directions;

    [SerializeField]
    private Transform TrainFrontCollider;

    [SerializeField]
    private Transform TrainRearCollider;

    [SerializeField]
    private Transform Train;


    [SerializeField]
    private float TrainSpeed = 10f;

    private Camera MainCam;
    private Vector2 screenBounds;
    private int directionCount = 2;

    private void Awake()
    {
        directions = Directions.Stand;
        MainCam = FindObjectOfType<Camera>();
        // Check View Bounding
        screenBounds = MainCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));//MainCam.transform.position.z));
    }


    private void Update()
    {
        if (directions == Directions.Right)
        {
            if (TrainFrontCollider.position.x <= screenBounds.x)
            {
                Train.transform.Translate(Vector2.right * TrainSpeed * Time.deltaTime);
            }
        }

        if (directions == Directions.Left)
        {
            if (TrainRearCollider.position.x >= -screenBounds.x)
            {
                Train.transform.Translate(Vector2.left * TrainSpeed * Time.deltaTime);
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                directionCount--;
                if (directionCount == 2)
                {
                    directions = Directions.Left;
                }

                if (directionCount == 1)
                {
                    directions = Directions.Right;
                }

                if (directionCount == 0)
                {
                    directionCount = 3;
                    directions = Directions.Stand;
                }
                Debug.Log($"[OnTriggerStay2D] {directions}");
            }

        }

    }
}