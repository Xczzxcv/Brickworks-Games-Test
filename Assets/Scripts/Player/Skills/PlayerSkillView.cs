using UnityEngine;

namespace Player.Skills
{
public struct PlayerSkillView
{
    public string SkillId;
    public string Name;
    public Color Color;
    public Vector3 ScreenPosition;
    public int LearningCost;
    public bool IsLearned;

    public static PlayerSkillView BuildFromSkill(IPlayerSkill playerSkill)
    {
        return new PlayerSkillView
        {
            SkillId = playerSkill.BaseConfig.Id,
            Name = "4325 asd",
            Color = Color.green,
            LearningCost = playerSkill.BaseConfig.LearningCost,
            IsLearned = playerSkill.IsLearned,
            ScreenPosition = Vector3.zero,
        };
    }
}
}