using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerSpawnPod = null;
    [SerializeField] private TrainData _trainData = null; 

    private uint _playerCount = 0;

    public void PlayerRespawnPod(PlayerInput input)
    {
        if (++_playerCount > 1)
        {
            _trainData.PlayerCount++;
        }
        PlayerV1 player = input.GetComponent<PlayerV1>();
        player.transform.SetParent(_trainData.TrainTransform);
        player.transform.position = _playerSpawnPod.transform.position;
        player.RespawnPoint = _playerSpawnPod.transform.position;
    }

}
