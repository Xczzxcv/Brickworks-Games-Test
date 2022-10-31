using Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ConfigsManager configs;
    [SerializeField] private UIManager uiManager;

    private DataManager _dataManager;
    private PlayerManager _player;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        configs.Init();
        
        _dataManager = new DataManager();
        _dataManager.Init();
        
        _player = new PlayerManager(configs);
        _player.Init(_dataManager.PlayerData);

        uiManager.Init();
        uiManager.ShowPlayerSkills(_player, configs);
    }
}