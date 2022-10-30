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
    private ConfigsManager _configs;

    public void Init(RectTransform uiParent)
    {
        _uiParent = uiParent;
    }

    public void Setup(PlayerManager player, ConfigsManager configs)
    {
        _skillsManager = player.SkillsManager;
        _configs = configs;
        _playerSkillsPresenter = Instantiate(skillsPresenterPrefab, _uiParent);

        _skillsManager.SkillPointsUpdated += OnSkillPointsUpdated;
        _skillsManager.SkillsUpdated += OnSkillsUpdated;

        _playerSkillsPresenter.SkillSelected += OnSkillSelected;
        _playerSkillsPresenter.LearnSkillBtnClick += OnLearnSkillBtnClick;
        _playerSkillsPresenter.ForgetSkillBtnClick += OnForgetSkillBtnClick;
        _playerSkillsPresenter.ForgetAllSkillsBtnClick += OnForgetAllSkillsBtnClick;
        _playerSkillsPresenter.AddPlayerSkillPointsBtnClick += OnAddPlayerSkillPointsBtnClick;

        var skills = _skillsManager.GetSkills();
        var playerSkillViews = GetPlayerSkillViews(skills);
        var playerSkillEdgeViews = GetPlayerSkillEdgeViews(skills);
        var playerSkillsView = new PlayerSkillsView
        {
            SkillViews = playerSkillViews,
            SkillEdges = playerSkillEdgeViews,
            PlayerSkillPoints = _skillsManager.SkillPoints,
            SkillIdToSelect = BasePlayerSkillConfig.ID
        };
        _playerSkillsPresenter.Init(playerSkillsView);
    }

    private List<PlayerSkillView> GetPlayerSkillViews(List<IPlayerSkill> skills)
    {
        var playerSkillViews = new List<PlayerSkillView>();
        foreach (var playerSkill in skills)
        {
            var skillViewConfig = _configs.PlayerSkillViews[playerSkill.Id];
            var playerSkillView = PlayerSkillView.BuildFromSkill(playerSkill, skillViewConfig);
            playerSkillViews.Add(playerSkillView);
        }

        return playerSkillViews;
    }

    private List<PlayerSkillEdgeView> GetPlayerSkillEdgeViews(List<IPlayerSkill> skills)
    {
        var uniqueEdges = new HashSet<PlayerSkillEdgeView>();
        foreach (var skill in skills)
        {
            foreach (var neighbourSkillId in skill.BaseConfig.Neighbours)
            {
                var skillEdge = new PlayerSkillEdgeView
                {
                    SrcSkillId = skill.Id,
                    DestSkillId = neighbourSkillId
                };

                uniqueEdges.Add(skillEdge);
            }
        }

        return uniqueEdges.ToList();
    }

    private void OnSkillPointsUpdated()
    {
        _playerSkillsPresenter.SetupPlayerPoints(_skillsManager.SkillPoints);
        UpdateSelectedSkillInfo();
    }

    private void OnSkillsUpdated(List<IPlayerSkill> updatedSkills)
    {
        foreach (var updatedSkill in updatedSkills)
        {
            var skillViewConfig = _configs.PlayerSkillViews[updatedSkill.Id];
            var updatedSkillView = PlayerSkillView.BuildFromSkill(updatedSkill, skillViewConfig);
            _playerSkillsPresenter.SetSkillView(updatedSkillView);
        
            if (_currentSelectedSkill.SkillView.SkillId == updatedSkill.Id)
            {
                UpdateSelectedSkillInfo();
            }
        }
    }

    private void OnSkillSelected(PlayerSkillPresenter skillPresenter)
    {
        _currentSelectedSkill = skillPresenter;
        UpdateSelectedSkillInfo();
    }

    private void UpdateSelectedSkillInfo()
    {
        _playerSkillsPresenter.SetupSkillSelectionView(new PlayerSkillsPresenter.SkillSelectionInfo
        {
            SkillView = _currentSelectedSkill.SkillView,
            CanBeLearned = _skillsManager.CanLearnSkill(_currentSelectedSkill.SkillView.SkillId),
            CanBeForgot = _skillsManager.CanForgetSkill(_currentSelectedSkill.SkillView.SkillId),
        });
    }

    private void OnLearnSkillBtnClick()
    {
        _skillsManager.LearnSkill(_currentSelectedSkill.SkillView.SkillId);
    }

    private void OnForgetSkillBtnClick()
    {
        _skillsManager.ForgetSkill(_currentSelectedSkill.SkillView.SkillId);
    }

    private void OnForgetAllSkillsBtnClick()
    {
        _skillsManager.ForgetAllSkills();
    }

    private void OnAddPlayerSkillPointsBtnClick()
    {
        _skillsManager.AddSkillPoints(1);
    }
}