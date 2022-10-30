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
        // Player.SkillsManager.ForgetSkill(JumpPlayerSkillConfig.ID);
        // Player.SkillsManager.ForgetSkill(FlyPlayerSkillConfig.ID);
        // Player.SkillsManager.ForgetSkill(RunPlayerSkillConfig.ID);
        // Player.SkillsManager.LearnSkill(JumpPlayerSkillConfig.ID);
    }

    private void Init()
    {
        Configs.Init();
        
        DataManager = new DataManager();
        DataManager.Init();
        
        Player = new PlayerManager(Configs);
        Player.Init(DataManager.PlayerData);

        UIManager.Init();
        UIManager.ShowPlayerSkills(Player, Configs);
    }
}