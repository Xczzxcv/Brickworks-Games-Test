using System;
using System.Collections.Generic;
using Data;
using Factory;
using Graph_Linked;
using Player.Skills;
using UnityEngine;

namespace Player
{
public class PlayerSkillsManager
{
    public int SkillPoints => _data.SkillPoints;

    private readonly Graph<IPlayerSkill> _graph;
    private readonly Configs<PlayerSkillConfig> _playerSkillConfigs;
    private readonly Dictionary<string, IPlayerSkill> _skills = new Dictionary<string, IPlayerSkill>();
    private PlayerSkillsData _data;

    public PlayerSkillsManager(Configs<PlayerSkillConfig> playerSkillConfigs)
    {
        _graph = new Graph<IPlayerSkill>();
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

        var basePlayerSkill = _skills[BasePlayerSkillConfig.ID];
        _graph.Init(basePlayerSkill);
        
        var addingQueue = new Queue<IPlayerSkill>();
        EnqueueChildren(basePlayerSkill, addingQueue);

        var parentNodesContent = new List<IPlayerSkill>();
        while (addingQueue.Count > 0)
        {
            var skillToAdd = addingQueue.Dequeue();
            EnqueueChildren(skillToAdd, addingQueue);

            parentNodesContent.Clear();
            foreach (var parentSkillId in skillToAdd.BaseConfig.Parents)
            {
                var parentSkill = _skills[parentSkillId];
                parentNodesContent.Add(parentSkill);                
            }

            var newNode = _graph.TryAddNode(parentNodesContent, skillToAdd);
            if (newNode == null)
            {
                throw new ArgumentException($"{skillToAdd.BaseConfig.Id}");
            }
        }
        
        Debug.Log($"[test113] {_graph}");
    }

    private void EnqueueChildren(IPlayerSkill playerSkill, Queue<IPlayerSkill> addingQueue)
    {
        foreach (var childSkillId in playerSkill.BaseConfig.Children)
        {
            var childSkill = _skills[childSkillId];
            addingQueue.Enqueue(childSkill);
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

        if (skill.BaseConfig.LearningCost > SkillPoints)
        {
            return false;
        }

        foreach (var parentSkillId in skill.BaseConfig.Parents)
        {
            if (!TryGetSkill(parentSkillId, out var parentSkill))
            {
                continue;
            }

            if (!parentSkill.IsLearned)
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

        foreach (var childSkillId in skill.BaseConfig.Children)
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

    public bool IsConnectedToParent(string skillId, string targetParentSkillId, string forbiddenSkillId = "")
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

            if (skillIdToCheck == targetParentSkillId)
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

            foreach (var parentSkillId in skillToCheck.BaseConfig.Parents)
            {
                searchQueue.Enqueue(parentSkillId);
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
        _data.SkillPoints += increaseAmount;
    }

    public void DeductSkillPoints(int decreaseAmount)
    {
        _data.SkillPoints -= decreaseAmount;
    }
}
}