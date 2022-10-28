using Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ConfigsManager config;
    [SerializeField] private UIManager uiManager;
    
    public DataManager DataManager;
    public ConfigsManager Configs => config;
    public PlayerManager Player;
    public UIManager UIManager => uiManager;

    void Start()
    {
        Init();
        // Debug.Log($"{Player.SkillsManager.SkillPoints}");
        // Player.SkillsManager.AddSkillPoints(15);
        // Debug.Log($"{Player.SkillsManager.SkillPoints}");
        //
        // Player.SkillsManager.LearnSkill(RunPlayerSkillConfig.ID);
        // Player.SkillsManager.LearnSkill(JumpPlayerSkillConfig.ID);
        // Player.SkillsManager.LearnSkill(FlyPlayerSkillConfig.ID);
        //
        // Player.SkillsManager.UnlearnSkill(JumpPlayerSkillConfig.ID);
        // Player.SkillsManager.UnlearnSkill(FlyPlayerSkillConfig.ID);
        // Player.SkillsManager.UnlearnSkill(RunPlayerSkillConfig.ID);
        // Player.SkillsManager.LearnSkill(JumpPlayerSkillConfig.ID);
    }

    public void Init()
    {
        DataManager = new DataManager();
        DataManager.Init();
        
        Configs.Init();
        
        Player = new PlayerManager(Configs);
        Player.Init(DataManager.PlayerData);

        UIManager.Init();
        UIManager.ShowPlayerSkills(Player, Configs);
    }
}