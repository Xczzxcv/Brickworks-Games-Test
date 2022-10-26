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
        foreach (var playerSkillConfig in _playerSkillConfigs.Values)
        {
            if (!skillsData.Skills.TryGetValue(playerSkillConfig.Id, out var playerSkillData))
            {
                playerSkillData = new PlayerSkillData
                {
                    Id = playerSkillConfig.Id,
                    IsLearned = false
                };
                skillsData.Skills.Add(playerSkillConfig.Id, playerSkillData);
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

    public bool CanLearnSkill(string skillId)
    {
        return false;
    }

    public void LearnSkill(string skillId)
    {
        if (_data.Skills.TryGetValue(skillId, out var skillData))
        {
            
        }
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