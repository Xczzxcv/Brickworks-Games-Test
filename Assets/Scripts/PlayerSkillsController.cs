using System.Collections.Generic;
using System.Linq;
using Player;
using Player.Skills;
using UnityEngine;

public class PlayerSkillsController : MonoBehaviour
{
    [SerializeField] private PlayerSkillsPresenter skillsPresenterPrefab;

    private Transform _uiParent;
    private PlayerSkillsManager _skillsManager;
    private PlayerSkillsPresenter _playerSkillsPresenter;
    private PlayerSkillPresenter _currentSelectedSkill;

    public void Init(RectTransform uiParent)
    {
        _uiParent = uiParent;
    }

    public void Setup(PlayerManager player, ConfigsManager configs)
    {
        _skillsManager = player.SkillsManager;

        var skills = _skillsManager.GetSkills();
        var playerSkillViews = GetPlayerSkillViews(skills, configs);
        var playerSkillEdgeViews = GetPlayerSkillEdgeViews(skills);
        var playerSkillsView = new PlayerSkillsView
        {
            SkillViews = playerSkillViews,
            SkillEdges = playerSkillEdgeViews,
            PlayerSkillPoints = _skillsManager.SkillPoints,
            SkillIdToSelect = BasePlayerSkillConfig.ID
        };
        _playerSkillsPresenter = Instantiate(skillsPresenterPrefab, _uiParent);
        _playerSkillsPresenter.Init(playerSkillsView);


        _skillsManager.SkillPointsUpdated += OnSkillPointsUpdated;

        _playerSkillsPresenter.SkillSelected += OnSkillSelected;
        _playerSkillsPresenter.LearnSkillBtnClick += OnLearnSkillBtnClick;
        _playerSkillsPresenter.UnlearnSkillBtnClick += OnUnlearnSkillBtnClick;
        _playerSkillsPresenter.UnlearnAllSkillsBtnClick += OnUnlearnAllSkillsBtnClick;
        _playerSkillsPresenter.AddPlayerSkillPointsBtnClick += OnAddPlayerSkillPointsBtnClick;
    }

    private List<PlayerSkillEdgeView> GetPlayerSkillEdgeViews(IEnumerable<IPlayerSkill> skills)
    {
        var uniqueEdges = new HashSet<PlayerSkillEdgeView>();
        foreach (var skill in skills)
        {
            foreach (var neighbourSkillId in skill.BaseConfig.Neighbours)
            {
                var skillEdge = new PlayerSkillEdgeView
                {
                    SrcSkillId = skill.BaseConfig.Id,
                    DestSkillId = neighbourSkillId
                };

                uniqueEdges.Add(skillEdge);
            }
        }

        return uniqueEdges.ToList();
    }

    private List<PlayerSkillView> GetPlayerSkillViews(IEnumerable<IPlayerSkill> skills, 
        ConfigsManager configs)
    {
        var playerSkillViews = new List<PlayerSkillView>();
        foreach (var playerSkill in skills)
        {
            var skillViewConfig = configs.PlayerSkillViews[playerSkill.BaseConfig.Id];
            var playerSkillView = PlayerSkillView.BuildFromSkill(playerSkill, skillViewConfig);
            playerSkillViews.Add(playerSkillView);
        }

        return playerSkillViews;
    }

    private void OnSkillPointsUpdated()
    {
        _playerSkillsPresenter.SetupPlayerPoints(_skillsManager.SkillPoints);
    }

    private void OnSkillSelected(PlayerSkillPresenter skillPresenter)
    {
        _currentSelectedSkill = skillPresenter;
        _playerSkillsPresenter.SetupSkillSelectionView(new PlayerSkillsPresenter.SkillSelectionInfo
        {
            SkillView = skillPresenter.SkillView,
            CanBeLearned = _skillsManager.CanLearnSkill(skillPresenter.SkillView.SkillId),
            CanBeUnlearned = _skillsManager.CanUnlearnSkill(skillPresenter.SkillView.SkillId),
        });
    }

    private void OnLearnSkillBtnClick()
    {
        _skillsManager.LearnSkill(_currentSelectedSkill.SkillView.SkillId);
    }

    private void OnUnlearnSkillBtnClick()
    {
        _skillsManager.UnlearnSkill(_currentSelectedSkill.SkillView.SkillId);
    }

    private void OnUnlearnAllSkillsBtnClick()
    {
        _skillsManager.UnlearnAllSkills();
    }

    private void OnAddPlayerSkillPointsBtnClick()
    {
        _skillsManager.AddSkillPoints(1);
    }
}