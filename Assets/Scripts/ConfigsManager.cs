using System;
using System.Collections.Generic;
using System.IO;
using Player.Skills;
using UnityEditor;
using UnityEngine;

public class ConfigsManager : MonoBehaviour
{
    [SerializeField, Directory]
    [EditorButton(nameof(LinkSkillConfigs))]
    private string skillsDirPath;

    [SerializeField, SerializeReference, Disable]
    private List<PlayerSkillConfig> skillConfigs;
    [SerializeField, Disable]
    private List<PlayerSkillViewConfig> skillViewConfigs;
    
    [NonSerialized]
    public Configs<PlayerSkillConfig> PlayerSkills;
    [NonSerialized]
    public Configs<PlayerSkillViewConfig> PlayerSkillViews;

    public void Init()
    {
        /*
        PlayerSkills = new Configs<PlayerSkillConfig>()
        {
            { BasePlayerSkillConfig.ID, new BasePlayerSkillConfig
            {
                Id = BasePlayerSkillConfig.ID,
                LearningCost = 0,
                Neighbours = new[] {JumpPlayerSkillConfig.ID, RunPlayerSkillConfig.ID},
            } },
            { RunPlayerSkillConfig.ID, new RunPlayerSkillConfig {
                Id = RunPlayerSkillConfig.ID,
                LearningCost = 4,
                Neighbours = new[] {BasePlayerSkillConfig.ID, JumpPlayerSkillConfig.ID},
            } },
            { JumpPlayerSkillConfig.ID, new JumpPlayerSkillConfig {
                Id = JumpPlayerSkillConfig.ID,
                LearningCost = 2,
                Neighbours = new[] {RunPlayerSkillConfig.ID, FlyPlayerSkillConfig.ID},
            } },
            { FlyPlayerSkillConfig.ID, new FlyPlayerSkillConfig {
                Id = FlyPlayerSkillConfig.ID,
                LearningCost = 6,
                Neighbours = new[] {JumpPlayerSkillConfig.ID, BasePlayerSkillConfig.ID},
            } },
        };
        */
        
        PlayerSkills = new Configs<PlayerSkillConfig>();
        foreach (var playerSkillConfig in skillConfigs)
        {
            PlayerSkills.Add(playerSkillConfig.Id, playerSkillConfig);
        }

        PlayerSkillViews = new Configs<PlayerSkillViewConfig>();
        foreach (var playerSkillConfig in skillViewConfigs)
        {
            PlayerSkillViews.Add(playerSkillConfig.SkillId, playerSkillConfig);
        }
    }

    private void LinkSkillConfigs()
    {
        var path = $"Assets/{skillsDirPath}";
        // var path = Path.Combine(Application.dataPath, skillsDirPath);
        var skillAssetPaths = Directory.GetFiles(path);

        skillConfigs = new List<PlayerSkillConfig>();
        skillViewConfigs = new List<PlayerSkillViewConfig>();
        
        foreach (var skillAssetPath in skillAssetPaths)
        {
            var playerSkillAsset = AssetDatabase.LoadAssetAtPath<PlayerSkillScriptableO>(skillAssetPath);
            if (!playerSkillAsset)
            {
                continue;
            }

            skillConfigs.Add(playerSkillAsset.Config);
            skillViewConfigs.Add(playerSkillAsset.ViewConfig);
        }
    }
}