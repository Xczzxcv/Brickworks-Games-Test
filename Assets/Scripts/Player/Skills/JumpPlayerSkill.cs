using Data;
using UnityEngine;

namespace Player.Skills
{
public class JumpPlayerSkill : PlayerSkill<JumpPlayerSkillConfig, PlayerSkillData>
{
    public JumpPlayerSkill(JumpPlayerSkillConfig skillConfig, PlayerSkillData skillData) 
        : base(skillConfig, skillData)
    { }

    public override void Use()
    {
        Debug.Log("JUMP");
    }
}
}