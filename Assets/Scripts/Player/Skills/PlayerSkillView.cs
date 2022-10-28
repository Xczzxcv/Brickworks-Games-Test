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

    public static PlayerSkillView BuildFromSkill(IPlayerSkill playerSkill, PlayerSkillViewConfig skillViewConfig)
    {
        return new PlayerSkillView
        {
            SkillId = playerSkill.BaseConfig.Id,
            Name =skillViewConfig.Name,
            Color = skillViewConfig.Color,
            LearningCost = playerSkill.BaseConfig.LearningCost,
            IsLearned = playerSkill.IsLearned,
            ScreenPosition = skillViewConfig.ScreenPosition,
        };
    }
}
}