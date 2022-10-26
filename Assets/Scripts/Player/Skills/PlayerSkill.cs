using Data;

namespace Player.Skills
{
public abstract class PlayerSkill<TConfig, TData> : IPlayerSkill
    where TConfig : PlayerSkillConfig
    where TData : PlayerSkillData
{
    public bool IsLearned => _skillData.IsLearned;
    public PlayerSkillConfig BaseConfig => Config;

    public readonly TConfig Config;
    private readonly TData _skillData;

    protected PlayerSkill(TConfig skillConfig, TData skillData)
    {
        Config = skillConfig;
        _skillData = skillData;
    }

    public abstract void Use();

    public bool Equals(IPlayerSkill other)
    {
        return other != null && BaseConfig.Id == other.BaseConfig.Id;
    }
}
}