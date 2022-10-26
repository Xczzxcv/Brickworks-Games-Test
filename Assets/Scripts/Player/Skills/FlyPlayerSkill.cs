using Data;
using UnityEngine;

namespace Player.Skills
{
public class FlyPlayerSkill : PlayerSkill<FlyPlayerSkillConfig, PlayerSkillData>
{
    public FlyPlayerSkill(FlyPlayerSkillConfig skillConfig, PlayerSkillData skillData) 
        : base(skillConfig, skillData)
    { }

    public override void Use()
    {
        Debug.Log("FLY");
    }
}
}