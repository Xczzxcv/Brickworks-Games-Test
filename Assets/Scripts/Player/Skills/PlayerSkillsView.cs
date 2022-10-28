using System.Collections.Generic;
using System.Numerics;

namespace Player.Skills
{
public struct PlayerSkillsView
{
    public List<PlayerSkillView> SkillViews;
    public List<PlayerSkillEdgeView> SkillEdges;
    public string SkillIdToSelect;
    public int PlayerSkillPoints;
}

public struct PlayerSkillEdgeView
{
    public string SrcSkillId;
    public string DestSkillId;
}
}