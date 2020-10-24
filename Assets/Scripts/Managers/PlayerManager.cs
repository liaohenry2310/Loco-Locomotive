using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerSpawnPod = null;
    
    private Train _train;
    private uint _playerCount = 0;

    private void Awake()
    {
        _train = FindObjectOfType<Train>();
    }

    public void PlayerRespawnPod(PlayerInput input)
    {
        if (++_playerCount > 1)
        {
            _train.PlayerCount++;
        }
        PlayerV1 player = input.GetComponent<PlayerV1>();
        player.transform.SetParent(_train.transform);
        player.transform.position = _playerSpawnPod.transform.position;
        player.RespawnPoint = _playerSpawnPod.transform.position;
    }

}
