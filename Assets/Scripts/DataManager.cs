using System.Collections.Generic;
using Data;
using Player.Skills;

public class DataManager
{
    public PlayerData PlayerData { get; private set; }

    public void Init()
    {
        PlayerData = new PlayerData
        {
            SkillsData = new PlayerSkillsData
            {
                Skills = new Dictionary<string, PlayerSkillData>
                {
                    {
                        BasePlayerSkillConfig.ID,
                        new PlayerSkillData {Id = BasePlayerSkillConfig.ID, IsLearned = true}
                    },
                },
                SkillPoints = 5
            }
        };
    }
}