using System.Linq;
using Player.Skills;

public class ConfigsManager
{
    public Configs<PlayerSkillConfig> PlayerSkills;

    public void Init()
    {
        PlayerSkills = new Configs<PlayerSkillConfig>()
        {
            { BasePlayerSkillConfig.ID, new BasePlayerSkillConfig
            {
                Id = BasePlayerSkillConfig.ID,
                LearningCost = 0,
                Children = new[] {JumpPlayerSkillConfig.ID, RunPlayerSkillConfig.ID},
                Parents = (string[]) Enumerable.Empty<string>(),
            } },
            { JumpPlayerSkillConfig.ID, new JumpPlayerSkillConfig {
                Id = JumpPlayerSkillConfig.ID,
                LearningCost = 2,
                Children = new[] {FlyPlayerSkillConfig.ID},
                Parents = new[] {BasePlayerSkillConfig.ID},
            } },
            { RunPlayerSkillConfig.ID, new RunPlayerSkillConfig {
                Id = RunPlayerSkillConfig.ID,
                LearningCost = 0,
                Children = (string[]) Enumerable.Empty<string>(),
                Parents = new[] {BasePlayerSkillConfig.ID},
            } },
            { FlyPlayerSkillConfig.ID, new FlyPlayerSkillConfig {
                Id = FlyPlayerSkillConfig.ID,
                LearningCost = 0,
                Children = (string[]) Enumerable.Empty<string>(),
                Parents = new[] {JumpPlayerSkillConfig.ID},
            } },
        };
    }
}