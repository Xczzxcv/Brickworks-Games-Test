using Player;
using Player.Skills;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DataManager DataManager;
    public ConfigsManager Configs;
    public PlayerManager Player;

    void Start()
    {
        Init();
        Debug.Log($"{Player.SkillsManager.SkillPoints}");
        Player.SkillsManager.AddSkillPoints(15);
        Debug.Log($"{Player.SkillsManager.SkillPoints}");

        Player.SkillsManager.LearnSkill(RunPlayerSkillConfig.ID);
        Player.SkillsManager.LearnSkill(JumpPlayerSkillConfig.ID);
        Player.SkillsManager.LearnSkill(FlyPlayerSkillConfig.ID);

        Player.SkillsManager.UnlearnSkill(JumpPlayerSkillConfig.ID);
        Player.SkillsManager.UnlearnSkill(FlyPlayerSkillConfig.ID);
        Player.SkillsManager.UnlearnSkill(RunPlayerSkillConfig.ID);
        Player.SkillsManager.LearnSkill(JumpPlayerSkillConfig.ID);

        // Debug.Log($"{Player.SkillsManager.CanLearnSkill(RunPlayerSkillConfig.ID)}");
        // Player.SkillsManager.LearnSkill(RunPlayerSkillConfig.ID);
        //
        // Debug.Log($"{Player.SkillsManager.CanLearnSkill(JumpPlayerSkillConfig.ID)}");
        // Player.SkillsManager.LearnSkill(JumpPlayerSkillConfig.ID);
        //
        // Debug.Log($"{Player.SkillsManager.CanLearnSkill(JumpPlayerSkillConfig.ID)}");
        // Player.SkillsManager.LearnSkill(JumpPlayerSkillConfig.ID);
        //
        // Debug.Log($"{Player.SkillsManager.CanLearnSkill(FlyPlayerSkillConfig.ID)}");
        // Player.SkillsManager.LearnSkill(FlyPlayerSkillConfig.ID);
    }

    public void Init()
    {
        DataManager = new DataManager();
        DataManager.Init();
        
        Configs.Init();
        
        Player = new PlayerManager(Configs);
        Player.Init(DataManager.PlayerData);
    }
}