using UnityEngine;

namespace Player.Skills
{
[CreateAssetMenu(menuName = "PlayerSkill", fileName = "PlayerSkill", order = 0)]
public class PlayerSkillScriptableO : ScriptableObject
{
    [SerializeReference, ReferencePicker]
    public PlayerSkillConfig Config;
    public PlayerSkillViewConfig ViewConfig;
}
}