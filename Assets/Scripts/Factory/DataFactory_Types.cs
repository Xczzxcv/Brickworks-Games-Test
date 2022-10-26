using Player.Skills;

namespace Factory
{
public static partial class DataFactory
{
    private static void AddPlayerSkills()
    {
        AddDataType<IPlayerSkill>(BasePlayerSkillConfig.ID, typeof(BasePlayerSkill));
        AddDataType<IPlayerSkill>(JumpPlayerSkillConfig.ID, typeof(JumpPlayerSkill));
        AddDataType<IPlayerSkill>(RunPlayerSkillConfig.ID, typeof(RunPlayerSkill));
        AddDataType<IPlayerSkill>(FlyPlayerSkillConfig.ID, typeof(FlyPlayerSkill));

        AddDataType<PlayerSkillConfig>(BasePlayerSkillConfig.ID, typeof(BasePlayerSkillConfig));
        AddDataType<PlayerSkillConfig>(JumpPlayerSkillConfig.ID, typeof(JumpPlayerSkillConfig));
        AddDataType<PlayerSkillConfig>(RunPlayerSkillConfig.ID, typeof(RunPlayerSkillConfig));
        AddDataType<PlayerSkillConfig>(FlyPlayerSkillConfig.ID, typeof(FlyPlayerSkillConfig));
    }
}
}