using Data;
using UnityEngine;

namespace Player.Skills
{
public class RunPlayerSkill : PlayerSkill<RunPlayerSkillConfig, PlayerSkillData>
{
    public RunPlayerSkill(RunPlayerSkillConfig skillConfig, PlayerSkillData skillData)
        : base(skillConfig, skillData)
    { }

    public override void Use()
    {
        Debug.Log("RUN");
    }
}
}