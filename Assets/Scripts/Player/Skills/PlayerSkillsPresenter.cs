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
    [SerializeField] private Button unlearnSkillBtn;
    [SerializeField] private Button unlearnAllSkillsBtn;
    [Space]
    [SerializeField] private TextMeshProUGUI playerSkillPointsLabel;
    [SerializeField] private Button addPlayerSkillPointsBtn;

    public event Action<PlayerSkillPresenter> SkillSelected;
    public event Action LearnSkillBtnClick;
    public event Action UnlearnSkillBtnClick;
    public event Action UnlearnAllSkillsBtnClick;
    public event Action AddPlayerSkillPointsBtnClick;

    private PlayerSkillsView _view;
    private Dictionary<string, PlayerSkillPresenter> _skillPresenters;

    protected override void Awake()
    {
        addPlayerSkillPointsBtn.onClick.AddListener(OnAddPlayerSkillPointsBtnClick);
        learnSkillBtn.onClick.AddListener(OnLearnSkillBtnClick);
        unlearnSkillBtn.onClick.AddListener(OnUnlearnSkillBtnClick);
        unlearnAllSkillsBtn.onClick.AddListener(OnUnlearnAllSkillsBtnClick);
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
        SetupSkillLearnButtons(selectionInfo.CanBeLearned, selectionInfo.CanBeUnlearned);
    }

    public struct SkillSelectionInfo
    {
        public PlayerSkillView SkillView;
        public bool CanBeLearned;
        public bool CanBeUnlearned;
    }

    private void SetupSkillPrice(int learningCost)
    {
        skillLearnPriceLabel.text = learningCost.ToString();
    }

    private void SetupSkillLearnButtons(bool canBeLearned, bool canBeUnlearned)
    {
        learnSkillBtn.interactable = canBeLearned;
        unlearnSkillBtn.interactable = canBeUnlearned;
        unlearnAllSkillsBtn.interactable = canBeLearned;
    }

    public void SetupPlayerPoints(int currentPoints)
    {
        playerSkillPointsLabel.text = currentPoints.ToString();
    }

    private void OnLearnSkillBtnClick() => LearnSkillBtnClick?.Invoke();

    private void OnUnlearnSkillBtnClick() => UnlearnSkillBtnClick?.Invoke();

    private void OnUnlearnAllSkillsBtnClick() => UnlearnAllSkillsBtnClick?.Invoke();

    private void OnAddPlayerSkillPointsBtnClick() => AddPlayerSkillPointsBtnClick?.Invoke();

    protected override void OnDestroy()
    {
        addPlayerSkillPointsBtn.onClick.RemoveListener(OnAddPlayerSkillPointsBtnClick);
        learnSkillBtn.onClick.RemoveListener(OnLearnSkillBtnClick);
        unlearnSkillBtn.onClick.RemoveListener(OnUnlearnSkillBtnClick);
        unlearnAllSkillsBtn.onClick.RemoveListener(OnUnlearnAllSkillsBtnClick);
    }
}
}