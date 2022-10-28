using Player;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerSkillsController skillsController;
    [SerializeField] private RectTransform uiParent;

    public void Init()
    {
        skillsController.Init(uiParent);
    }

    public void ShowPlayerSkills(PlayerManager player, ConfigsManager configs)
    {
        skillsController.Setup(player, configs);
    }
}