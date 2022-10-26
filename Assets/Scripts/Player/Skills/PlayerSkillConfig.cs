namespace Player.Skills
{
public abstract class PlayerSkillConfig
{
    public string Id;
    public int LearningCost;
    public string[] Parents;
    public string[] Children;
}
}