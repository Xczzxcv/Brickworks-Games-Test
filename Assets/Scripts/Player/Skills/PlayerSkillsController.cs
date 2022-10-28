using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Player.Skills
{
public class PlayerSkillsController : UIBehaviour
{
    [SerializeField] private PlayerSkillController skillControllerPrefab;
    [SerializeField] private Transform skillViewsParent;
    [SerializeField] private ToggleGroup skillViewsToggleGroup;

    [SerializeField] private TextMeshProUGUI skillLearnPriceLabel;
    [SerializeField] private Button learnSkillBtn;
    [SerializeField] private Button unlearnSkillBtn;
    [SerializeField] private Button unlearnAllSkillsBtn;

    [SerializeField] private TextMeshProUGUI playerSkillPointsLabel;
    [SerializeField] private Button addPlayerSkillPointsBtn;

    protected override void Awake()
    {
        addPlayerSkillPointsBtn.onClick.AddListener(OnAddPlayerSkillPointsBtnClick);
        learnSkillBtn.onClick.AddListener(OnLearnSkillBtnClick);
        unlearnSkillBtn.onClick.AddListener(OnUnlearnSkillBtnClick);
        unlearnAllSkillsBtn.onClick.AddListener(OnUnlearnAllSkillsBtnClick);
    }

    public void Init(PlayerSkillsView view)
    {
        PlayerSkillController skillToSelect = null;
        foreach (var skillView in view.SkillViews)
        {
            var skillController = Instantiate(skillControllerPrefab, skillViewsParent);
            skillController.Init(skillViewsToggleGroup);
            skillController.Setup(skillView);
            skillController.SkillSelected += OnSkillSelected;

            if (skillView.SkillId == view.SkillIdToSelect)
            {
                skillToSelect = skillController;
            }
        }

        if (skillToSelect)
        {
            skillToSelect.GetComponent<Toggle>().isOn = true;
        }
        
        SetupPlayerPoints(view.PlayerSkillPoints);
    }

    private void OnSkillSelected(PlayerSkillController skillController)
    {
        SetupSkillSelectionView(skillController.SkillView);
    }

    private void SetupSkillSelectionView(PlayerSkillView skillView)
    {
        SetupSkillPrice(skillView.LearningCost);
        var canBeLearned = true;
        var canBeUnlearned = false;
        SetupSkillLearnButtons(canBeLearned, canBeUnlearned);
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

    private void SetupPlayerPoints(int currentPoints)
    {
        playerSkillPointsLabel.text = currentPoints.ToString();
    }

    private void OnLearnSkillBtnClick()
    {
        throw new System.NotImplementedException();
    }

    private void OnUnlearnSkillBtnClick()
    {
        throw new System.NotImplementedException();
    }

    private void OnUnlearnAllSkillsBtnClick()
    {
        throw new System.NotImplementedException();
    }

    private void OnAddPlayerSkillPointsBtnClick()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnDestroy()
    {
        addPlayerSkillPointsBtn.onClick.RemoveListener(OnAddPlayerSkillPointsBtnClick);
        learnSkillBtn.onClick.RemoveListener(OnLearnSkillBtnClick);
        unlearnSkillBtn.onClick.RemoveListener(OnUnlearnSkillBtnClick);
        unlearnAllSkillsBtn.onClick.RemoveListener(OnUnlearnAllSkillsBtnClick);
    }
}
}