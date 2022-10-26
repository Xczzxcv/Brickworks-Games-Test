using System;

namespace Player.Skills
{
public interface IPlayerSkill : IEquatable<IPlayerSkill>
{
    public bool IsLearned { get; }
    public PlayerSkillConfig BaseConfig { get; }

    void Use();
}
}