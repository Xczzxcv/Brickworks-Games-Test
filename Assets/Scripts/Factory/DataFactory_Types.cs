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
        AddDataType<IPlayerSkill>(SomePlayerSkillConfig.ID, typeof(SomePlayerSkill));
        AddDataType<IPlayerSkill>(SomePlayerSkillConfig.ID1, typeof(SomePlayerSkill));
        AddDataType<IPlayerSkill>(SomePlayerSkillConfig.ID2, typeof(SomePlayerSkill));
        AddDataType<IPlayerSkill>(SomePlayerSkillConfig.ID3, typeof(SomePlayerSkill));
        AddDataType<IPlayerSkill>(SomePlayerSkillConfig.ID4, typeof(SomePlayerSkill));
        AddDataType<IPlayerSkill>(SomePlayerSkillConfig.ID5, typeof(SomePlayerSkill));
        AddDataType<IPlayerSkill>(SomePlayerSkillConfig.ID6, typeof(SomePlayerSkill));
        AddDataType<IPlayerSkill>(SomePlayerSkillConfig.ID7, typeof(SomePlayerSkill));
        AddDataType<IPlayerSkill>(SomePlayerSkillConfig.ID8, typeof(SomePlayerSkill));
        AddDataType<IPlayerSkill>(SomePlayerSkillConfig.ID9, typeof(SomePlayerSkill));
        AddDataType<IPlayerSkill>(SomePlayerSkillConfig.ID10, typeof(SomePlayerSkill));

        AddDataType<PlayerSkillConfig>(BasePlayerSkillConfig.ID, typeof(BasePlayerSkillConfig));
        AddDataType<PlayerSkillConfig>(JumpPlayerSkillConfig.ID, typeof(JumpPlayerSkillConfig));
        AddDataType<PlayerSkillConfig>(RunPlayerSkillConfig.ID, typeof(RunPlayerSkillConfig));
        AddDataType<PlayerSkillConfig>(FlyPlayerSkillConfig.ID, typeof(FlyPlayerSkillConfig));
        AddDataType<PlayerSkillConfig>(SomePlayerSkillConfig.ID, typeof(SomePlayerSkillConfig));
        AddDataType<PlayerSkillConfig>(SomePlayerSkillConfig.ID1, typeof(SomePlayerSkillConfig));
        AddDataType<PlayerSkillConfig>(SomePlayerSkillConfig.ID2, typeof(SomePlayerSkillConfig));
        AddDataType<PlayerSkillConfig>(SomePlayerSkillConfig.ID3, typeof(SomePlayerSkillConfig));
        AddDataType<PlayerSkillConfig>(SomePlayerSkillConfig.ID4, typeof(SomePlayerSkillConfig));
        AddDataType<PlayerSkillConfig>(SomePlayerSkillConfig.ID5, typeof(SomePlayerSkillConfig));
        AddDataType<PlayerSkillConfig>(SomePlayerSkillConfig.ID6, typeof(SomePlayerSkillConfig));
        AddDataType<PlayerSkillConfig>(SomePlayerSkillConfig.ID7, typeof(SomePlayerSkillConfig));
        AddDataType<PlayerSkillConfig>(SomePlayerSkillConfig.ID8, typeof(SomePlayerSkillConfig));
        AddDataType<PlayerSkillConfig>(SomePlayerSkillConfig.ID9, typeof(SomePlayerSkillConfig));
        AddDataType<PlayerSkillConfig>(SomePlayerSkillConfig.ID10, typeof(SomePlayerSkillConfig));
    }
}
}