using Data;
using UnityEngine;

namespace Player.Skills
{
public class SomePlayerSkill : PlayerSkill<SomePlayerSkillConfig, PlayerSkillData>
{
    public SomePlayerSkill(SomePlayerSkillConfig skillConfig, PlayerSkillData skillData) 
        : base(skillConfig, skillData)
    { }

    public override void Use()
    {
        Debug.LogError("SOME");
    }
}
}