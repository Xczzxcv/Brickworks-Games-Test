using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Player.Skills
{
public class PlayerSkillsPresenter : UIBehaviour
{
    [SerializeField] private PlayerSkillPresenter skillPresenterPrefab;
    [SerializeField] private Transform skillViewsParent;
    [SerializeField] private ToggleGroup skillViewsToggleGroup;
    [Space]
    [SerializeField] private PlayerSkillEdgePresenter skillEdgePresenterPrefab;
    [SerializeField] private Transform edgeViewsParent;
    [Space]
    [SerializeField] private TextMeshProUGUI skillLearnPriceLabel;
    [SerializeField] private Button learnSkillBtn;
    [SerializeField] private Button forgetSkillBtn;
    [SerializeField] private Button forgetAllSkillsBtn;
    [Space]
    [SerializeField] private TextMeshProUGUI playerSkillPointsLabel;
    [SerializeField] private Button addPlayerSkillPointsBtn;

    public event Action<PlayerSkillPresenter> SkillSelected;
    public event Action LearnSkillBtnClick;
    public event Action ForgetSkillBtnClick;
    public event Action ForgetAllSkillsBtnClick;
    public event Action AddPlayerSkillPointsBtnClick;

    private PlayerSkillsView _view;
    private Dictionary<string, PlayerSkillPresenter> _skillPresenters;

    protected override void Awake()
    {
        addPlayerSkillPointsBtn.onClick.AddListener(OnAddPlayerSkillPointsBtnClick);
        learnSkillBtn.onClick.AddListener(OnLearnSkillBtnClick);
        forgetSkillBtn.onClick.AddListener(OnForgetSkillBtnClick);
        forgetAllSkillsBtn.onClick.AddListener(OnForgetAllSkillsBtnClick);
    }

    public void Init(PlayerSkillsView view)
    {
        _view = view;
        _skillPresenters = new Dictionary<string, PlayerSkillPresenter>();
        
        PlayerSkillPresenter skillToSelect = null;
        foreach (var skillView in _view.SkillViews)
        {
            var skillPresenter = Instantiate(skillPresenterPrefab, skillViewsParent);
            skillPresenter.Init(skillViewsToggleGroup);
            skillPresenter.Setup(skillView);
            skillPresenter.SkillSelected += OnSkillSelected;
            
            _skillPresenters.Add(skillView.SkillId, skillPresenter);

            if (skillView.SkillId == _view.SkillIdToSelect)
            {
                skillToSelect = skillPresenter;
            }
        }

        foreach (var skillEdge in _view.SkillEdges)
        {
            var srcSkillPresenter = _skillPresenters[skillEdge.SrcSkillId];
            var destSkillPresenter = _skillPresenters[skillEdge.DestSkillId];

            var edgePresenter = Instantiate(skillEdgePresenterPrefab, edgeViewsParent);
            edgePresenter.Setup(srcSkillPresenter, destSkillPresenter);
        }

        if (skillToSelect)
        {
            skillToSelect.GetComponent<Toggle>().isOn = true;
        }
        
        SetupPlayerPoints(_view.PlayerSkillPoints);
    }

    private void OnSkillSelected(PlayerSkillPresenter skillPresenter)
    {
        SkillSelected?.Invoke(skillPresenter);
    }

    public void SetupSkillSelectionView(SkillSelectionInfo selectionInfo)
    {
        SetupSkillPrice(selectionInfo.SkillView.LearningCost);
        SetupSkillLearnButtons(selectionInfo.CanBeLearned, selectionInfo.CanBeForgot);
    }

    public struct SkillSelectionInfo
    {
        public PlayerSkillView SkillView;
        public bool CanBeLearned;
        public bool CanBeForgot;
    }

    private void SetupSkillPrice(int learningCost)
    {
        skillLearnPriceLabel.text = learningCost.ToString();
    }

    private void SetupSkillLearnButtons(bool canBeLearned, bool canBeForgot)
    {
        learnSkillBtn.interactable = canBeLearned;
        forgetSkillBtn.interactable = canBeForgot;
        forgetAllSkillsBtn.interactable = true;
    }

    public void SetupPlayerPoints(int currentPoints)
    {
        playerSkillPointsLabel.text = currentPoints.ToString();
    }

    public void SetSkillView(PlayerSkillView skillView)
    {
        if (!_skillPresenters.TryGetValue(skillView.SkillId, out var skillPresenter))
        {
            Debug.LogError($"You just tried to update skill view for absent presenter ({skillView.SkillId})");
            return;
        }

        skillPresenter.Setup(skillView);
    }

    private void OnLearnSkillBtnClick() => LearnSkillBtnClick?.Invoke();

    private void OnForgetSkillBtnClick() => ForgetSkillBtnClick?.Invoke();

    private void OnForgetAllSkillsBtnClick() => ForgetAllSkillsBtnClick?.Invoke();

    private void OnAddPlayerSkillPointsBtnClick() => AddPlayerSkillPointsBtnClick?.Invoke();

    protected override void OnDestroy()
    {
        addPlayerSkillPointsBtn.onClick.RemoveListener(OnAddPlayerSkillPointsBtnClick);
        learnSkillBtn.onClick.RemoveListener(OnLearnSkillBtnClick);
        forgetSkillBtn.onClick.RemoveListener(OnForgetSkillBtnClick);
        forgetAllSkillsBtn.onClick.RemoveListener(OnForgetAllSkillsBtnClick);
    }
}
}