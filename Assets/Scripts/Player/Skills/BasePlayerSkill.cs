using Data;
using UnityEngine;

namespace Player.Skills
{
public class BasePlayerSkill : PlayerSkill<BasePlayerSkillConfig, PlayerSkillData>
{
    public BasePlayerSkill(BasePlayerSkillConfig skillConfig, PlayerSkillData skillData) 
        : base(skillConfig, skillData)
    { }

    public override void Use()
    {
        Debug.Log("BASED");
    }
}
}