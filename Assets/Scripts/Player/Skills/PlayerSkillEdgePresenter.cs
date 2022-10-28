using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Player.Skills
{
public class PlayerSkillEdgePresenter : UIBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private RectTransform rectTransform;

    public void Setup(PlayerSkillPresenter srcSkill, PlayerSkillPresenter destSkill)
    {
        var srcSkillRect = srcSkill.GetComponent<RectTransform>();
        var destSkillRect = destSkill.GetComponent<RectTransform>();

        var positionDiff = destSkillRect.position - srcSkillRect.position;
        var edgeRotation = Quaternion.FromToRotation(Vector3.right, positionDiff);

        rectTransform.SetPositionAndRotation(srcSkillRect.position, edgeRotation);
        var edgeSize = rectTransform.sizeDelta;
        edgeSize.x = positionDiff.magnitude;
        rectTransform.sizeDelta = edgeSize;
    }
}
}