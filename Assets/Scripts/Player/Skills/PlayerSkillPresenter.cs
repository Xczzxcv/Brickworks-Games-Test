using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Player.Skills
{
public class PlayerSkillPresenter : UIBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Toggle toggle;
    [SerializeField] private GameObject skillLearnedMark;

    public event Action<PlayerSkillPresenter> SkillSelected;
    public PlayerSkillView SkillView { get; private set; }

    public void Init(ToggleGroup parentToggleGroup)
    {
        toggle.group = parentToggleGroup;
    }

    public void Setup(PlayerSkillView skillView)
    {
        SkillView = skillView;

        rectTransform.localPosition = SkillView.ScreenPosition;
        background.color = SkillView.Color;
        nameLabel.text = SkillView.Name;
        skillLearnedMark.SetActive(SkillView.IsLearned);
        
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool newValue)
    {
        if (newValue)
        {
            SkillSelected?.Invoke(this);
        }
    }
}
}