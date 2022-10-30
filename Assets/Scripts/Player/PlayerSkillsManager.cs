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
    public event Action<List<IPlayerSkill>> SkillsUpdated;

    private readonly Configs<PlayerSkillConfig> _playerSkillConfigs;
    private readonly Dictionary<string, IPlayerSkill> _skills = new Dictionary<string, IPlayerSkill>();
    private PlayerSkillsData _data;
    
    private readonly List<string> _skillIdsToUpdateCache = new List<string>();
    private readonly List<IPlayerSkill> _updatedSkillsCache = new List<IPlayerSkill>();
    private readonly HashSet<string> _alreadyCheckedSkillIdsCached = new HashSet<string>();

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

        if (skill.IsLearned)
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
        
        _updatedSkillsCache.Clear();
        _updatedSkillsCache.Add(skill);
        SkillsUpdated?.Invoke(_updatedSkillsCache);
    }
    
    public bool CanForgetSkill(string skillId)
    {
        if (!TryGetSkill(skillId, out var skill))
        {
            return false;
        }

        if (!skill.IsLearned)
        {
            return false;
        }

        if (skill.Id == BasePlayerSkillConfig.ID)
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

            if (!AreSkillsConnected(neighbourSkillId, BasePlayerSkillConfig.ID, 
                    skillId))
            {
                return false;
            }
        }
        
        return true;
    }

    private bool AreSkillsConnected(string skillId, string targetSkillId, string forbiddenSkillId = "")
    {
        var searchQueue = new Queue<string>();
        searchQueue.Enqueue(skillId);

        _alreadyCheckedSkillIdsCached.Clear();

        while (searchQueue.Count > 0)
        {
            var skillIdToCheck = searchQueue.Dequeue();

            var needToCheckSkillWithSuchId = skillIdToCheck != forbiddenSkillId 
                                             && !_alreadyCheckedSkillIdsCached.Contains(skillIdToCheck);
            if (!needToCheckSkillWithSuchId)
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

            _alreadyCheckedSkillIdsCached.Add(skillIdToCheck);
        }

        return false;
    }

    public void ForgetSkill(string skillId)
    {
        Debug.Assert(CanForgetSkill(skillId), $"Actually you can't forget {skillId}");

        _skillIdsToUpdateCache.Clear();
        _skillIdsToUpdateCache.Add(skillId);
        ForgetInternal();
    }

    public void ForgetAllSkills()
    {
        _skillIdsToUpdateCache.Clear();
        foreach (var skillId in _data.Skills.Keys)
        {
            var noNeedToForget = skillId == BasePlayerSkillConfig.ID;
            if (noNeedToForget)
            {
                continue;
            }

            _skillIdsToUpdateCache.Add(skillId);
        }

        ForgetInternal();
    }

    private void ForgetInternal()
    {
        _updatedSkillsCache.Clear();
        var skillPointsReturnedTotal = 0;

        foreach (var skillId in _skillIdsToUpdateCache)
        {
            if (!TryGetSkill(skillId, out var skill))
            {
                continue;
            }

            if (!TryGetSkillData(skillId, out var skillData))
            {
                continue;
            }

            if (!skillData.IsLearned)
            {
                continue;
            }

            skillData.IsLearned = false;
            skillPointsReturnedTotal += skill.BaseConfig.LearningCost;
            _updatedSkillsCache.Add(skill);
        }

        AddSkillPoints(skillPointsReturnedTotal);
        SkillsUpdated?.Invoke(_updatedSkillsCache);
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
}
}