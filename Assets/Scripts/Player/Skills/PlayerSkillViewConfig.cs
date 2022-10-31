using System;
using UnityEngine;

namespace Player.Skills
{
[Serializable]
public struct PlayerSkillViewConfig
{
    public string SkillId;
    public string Name;
    public Color Color;
    public Vector3 ScreenPosition;
}
}