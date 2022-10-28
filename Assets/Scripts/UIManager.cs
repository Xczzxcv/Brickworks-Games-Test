using System.Collections.Generic;
using Player;
using Player.Skills;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerSkillsController skillsControllerPrefab;
    [SerializeField] private RectTransform uiParent;

    public void ShowPlayerSkills(PlayerManager player, ConfigsManager configs)
    {
        var playerSkillViews = new List<PlayerSkillView>();
        
        var skills = player.SkillsManager.GetSkills();
        foreach (var playerSkill in skills)
        {
            var skillViewConfig = configs.PlayerSkillViews[playerSkill.BaseConfig.Id];
            var playerSkillView = PlayerSkillView.BuildFromSkill(playerSkill, skillViewConfig);
            playerSkillViews.Add(playerSkillView);
        }
        
        var playerSkillsController = Instantiate(skillsControllerPrefab, uiParent);
        var playerSkillsView = new PlayerSkillsView
        {
            SkillViews = playerSkillViews,
            PlayerSkillPoints = player.SkillsManager.SkillPoints,
            SkillIdToSelect = BasePlayerSkillConfig.ID
        };
        playerSkillsController.Init(playerSkillsView);
    }
}