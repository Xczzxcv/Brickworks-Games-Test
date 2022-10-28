using System;
using System.Collections.Generic;
using Data;
using Factory;
using Player.Skills;
using UnityEngine;

namespace Player
{
public class PlayerSkillsManager
{
    public int SkillPoints => _data.SkillPoints;
    public event Action SkillPointsUpdated;

    private readonly Configs<PlayerSkillConfig> _playerSkillConfigs;
    private readonly Dictionary<string, IPlayerSkill> _skills = new Dictionary<string, IPlayerSkill>();
    private PlayerSkillsData _data;

    public PlayerSkillsManager(Configs<PlayerSkillConfig> playerSkillConfigs)
    {
        _playerSkillConfigs = playerSkillConfigs;
    }

    public void Init(PlayerSkillsData skillsData)
    {
        _data = skillsData;

        foreach (var playerSkillConfig in _playerSkillConfigs.Values)
        {
            if (!_data.Skills.TryGetValue(playerSkillConfig.Id, out var playerSkillData))
            {
                playerSkillData = new PlayerSkillData
                {
                    Id = playerSkillConfig.Id,
                    IsLearned = false
                };
                _data.Skills.Add(playerSkillConfig.Id, playerSkillData);
            }

            var playerSkill = EntitiesFactory.BuildPlayerSkill(playerSkillConfig, playerSkillData);
            _skills.Add(playerSkillConfig.Id, playerSkill);
        }
    }

    private bool TryGetSkill(string skillId, out IPlayerSkill skill)
    {
        if (!_skills.TryGetValue(skillId, out skill))
        {
            Debug.LogError($"Can't find skill {skillId}");
            return false;
        }

        return true;
    }

    private bool TryGetSkillData(string skillId, out PlayerSkillData skillData)
    {
        if (!_data.Skills.TryGetValue(skillId, out skillData))
        {
            Debug.LogError($"Can't find skill data {skillId}");
            return false;
        }

        return true;
    }

    public bool CanLearnSkill(string skillId)
    {
        if (!TryGetSkill(skillId, out var skill))
        {
            return false;
        }

        var haveEnoughPoints = skill.BaseConfig.LearningCost > SkillPoints;
        if (haveEnoughPoints)
        {
            return false;
        }

        foreach (var neighbourSkillId in skill.BaseConfig.Neighbours)
        {
            if (!TryGetSkill(neighbourSkillId, out var neighbourSkill))
            {
                continue;
            }

            if (!neighbourSkill.IsLearned)
            {
                continue;
            }

            return true;
        }

        return false;
    }

    public void LearnSkill(string skillId)
    {
        Debug.Assert(CanLearnSkill(skillId), $"Actually you can't learn {skillId}");

        if (!TryGetSkill(skillId, out var skill))
        {
            return;
        }

        if (!TryGetSkillData(skillId, out var skillData))
        {
            return;
        }

        skillData.IsLearned = true;
        DeductSkillPoints(skill.BaseConfig.LearningCost);
    }
    
    public bool CanUnlearnSkill(string skillId)
    {
        if (!TryGetSkill(skillId, out var skill))
        {
            return false;
        }

        if (skill.BaseConfig.Id == BasePlayerSkillConfig.ID)
        {
            return false;
        }

        foreach (var childSkillId in skill.BaseConfig.Neighbours)
        {
            if (!TryGetSkill(childSkillId, out var childSkill))
            {
                continue;
            }

            if (!childSkill.IsLearned)
            {
                continue;
            }

            if (!IsConnectedToParent(childSkillId, BasePlayerSkillConfig.ID, 
                    skillId))
            {
                return false;
            }
        }
        
        return true;
    }

    public bool IsConnectedToParent(string skillId, string targetSkillId, string forbiddenSkillId = "")
    {
        var searchQueue = new Queue<string>();
        searchQueue.Enqueue(skillId);

        while (searchQueue.Count > 0)
        {
            var skillIdToCheck = searchQueue.Dequeue();

            if (skillIdToCheck == forbiddenSkillId)
            {
                continue;
            }

            if (skillIdToCheck == targetSkillId)
            {
                return true;
            }

            if (!TryGetSkill(skillIdToCheck, out var skillToCheck))
            {
                continue;
            }

            if (!skillToCheck.IsLearned)
            {
                continue;
            }

            foreach (var neighbourSkillId in skillToCheck.BaseConfig.Neighbours)
            {
                searchQueue.Enqueue(neighbourSkillId);
            }
        }

        return false;
    }

    public void UnlearnSkill(string skillId)
    {
        Debug.Assert(CanUnlearnSkill(skillId), $"Actually you can't unlearn {skillId}");

        if (!TryGetSkill(skillId, out var skill))
        {
            return;
        }

        if (!TryGetSkillData(skillId, out var skillData))
        {
            return;
        }

        skillData.IsLearned = false;
        AddSkillPoints(skill.BaseConfig.LearningCost);
    }

    public void AddSkillPoints(int increaseAmount)
    {
        UpdateSkillPoints(SkillPoints + increaseAmount);
    }

    public void DeductSkillPoints(int decreaseAmount)
    {
        UpdateSkillPoints(SkillPoints - decreaseAmount);
    }

    private void UpdateSkillPoints(int newAmount)
    {
        _data.SkillPoints = newAmount;
        SkillPointsUpdated?.Invoke();
    }

    public List<IPlayerSkill> GetSkills()
    {
        return new List<IPlayerSkill>(_skills.Values);
    }

    public void UnlearnAllSkills()
    {
        throw new System.NotImplementedException();
    }
}
}