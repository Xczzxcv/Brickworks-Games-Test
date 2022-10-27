using System;

namespace Player.Skills
{
[Serializable]
public abstract class PlayerSkillConfig
{
    public string Id;
    public int LearningCost;
    public string[] Neighbours;
}
}