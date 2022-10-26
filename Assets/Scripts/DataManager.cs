using System.Collections.Generic;
using Data;

public class DataManager
{
    public PlayerData PlayerData { get; private set; }

    public void Init()
    {
        PlayerData = new PlayerData
        {
            SkillsData = new PlayerSkillsData
            {
                Skills = new Dictionary<string, PlayerSkillData>(),
                SkillPoints = 5
            }
        };
    }
}