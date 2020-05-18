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
    private float TrainSpeed = 10f;

    private PlayerController player = null;
    private Vector2 screenBounds;
    private InputManager imputManager;
    private bool isPlayerOnTrain = false;

    private void Start()
    {
        imputManager = FindObjectOfType<InputManager>();
        Camera MainCam = FindObjectOfType<Camera>();
        // Check View Bounding
        screenBounds = MainCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCam.transform.position.z));
    }

    private void Update()
    {
        if (player != null && Input.GetKeyDown(KeyCode.E))
        {
            isPlayerOnTrain = !isPlayerOnTrain;
            imputManager.InputCommand = isPlayerOnTrain ?
                    InputManager.EInputCommand.Train :
                    InputManager.EInputCommand.Player;
            Debug.Log($"[OnTriggerStay] Player is controll the train = {isPlayerOnTrain}");
        }

        if (imputManager.InputCommand == InputManager.EInputCommand.Train)
        {
            float inputHorizontal = Input.GetAxisRaw("Horizontal");
            float translation = inputHorizontal * TrainSpeed * Time.deltaTime;
            Vector3 trainPos;
            if (TrainFrontCollider.position.x + translation >= screenBounds.x) // Check to the right side of the screen
            {
                trainPos = new Vector3(screenBounds.x - TrainFrontCollider.position.x, 0.0f, 0.0f);
            }
            else if (TrainRearCollider.position.x + translation <= -screenBounds.x) // Check to the left side of the screen
            {
                trainPos = new Vector3(screenBounds.x + TrainRearCollider.position.x, 0.0f, 0.0f);
            }
            else
            {
                trainPos = new Vector3(translation, 0.0f, 0.0f);
            }
            Train.Translate(trainPos);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;
        }
    }
}