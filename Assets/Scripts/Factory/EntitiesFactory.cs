using System;
using Data;
using Player.Skills;

namespace Factory
{
public static class EntitiesFactory
{
    public static IPlayerSkill BuildPlayerSkill(PlayerSkillConfig config, PlayerSkillData skillData)
    {
        return GeneralBuild<IPlayerSkill>(config.Id, config, skillData);
    }

    private static T GeneralBuild<T>(string typeStr, params object[] args)
    {
        var generalEntityType = DataFactory.GetType<T>(typeStr);
        return MostGeneralBuild<T>(generalEntityType, args);
    }

    private static T MostGeneralBuild<T>(Type type, params object[] args)
    {
        return (T) Activator.CreateInstance(type, args);
    }
}
}