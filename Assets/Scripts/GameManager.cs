using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject playerControllerPrefab;
    //public int numOfPlayers;

    public Camera MainCamera;

    private List<PlayerController> mPlayerControllers;
    private GameObject train;

    public Vector3 ScreenBounds;

    private void Awake()
    {
        ScreenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));

        mPlayerControllers = new List<PlayerController>();
        train = GameObject.Find("Train");
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        //var player1 = PlayerInput.Instantiate(playerControllerPrefab, 0, "Gamepad", -1, Gamepad.current);
        var player2 = PlayerInput.Instantiate(playerControllerPrefab, 1, "KeyboardRight", -1, Keyboard.current);
        var player3 = PlayerInput.Instantiate(playerControllerPrefab, 2, "KeyboardLeft", -1, Keyboard.current);

        //var avatar1 = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, train.transform).GetComponent<Player>();
        var avatar2 = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, train.transform).GetComponent<Player>();
        var avatar3 = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, train.transform).GetComponent<Player>();
        //avatar1.GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
        avatar2.GetComponentInChildren<SpriteRenderer>().color = Color.cyan;
        avatar3.GetComponentInChildren<SpriteRenderer>().color = Color.magenta;

        //player1.GetComponent<PlayerController>().SetPlayer(ref avatar1);
        player2.GetComponent<PlayerController>().SetPlayer(ref avatar2);
        player3.GetComponent<PlayerController>().SetPlayer(ref avatar3);
    }
 
}
