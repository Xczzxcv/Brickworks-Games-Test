using System;

namespace Player.Skills
{
public interface IPlayerSkill : IEquatable<IPlayerSkill>
{
    string Id { get; }
    bool IsLearned { get; }
    PlayerSkillConfig BaseConfig { get; }

    void Use();
}
}