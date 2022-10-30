using System;
using System.Collections.Generic;
using System.IO;
using Player.Skills;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ConfigsManager : MonoBehaviour
{
    [SerializeField, Directory]
    [EditorButton(nameof(LinkSkillConfigs))]
    private string skillsDirPath;
    [SerializeField]
    private float screenPosModifier;

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
#if UNITY_EDITOR
        var path = $"Assets/{skillsDirPath}";
        var skillAssetPaths = Directory.GetFiles(path);

        skillConfigs = new List<PlayerSkillConfig>();
        skillViewConfigs = new List<PlayerSkillViewConfig>();

        var linkedCounter = 0;
        foreach (var skillAssetPath in skillAssetPaths)
        {
            var playerSkillAsset = AssetDatabase.LoadAssetAtPath<PlayerSkillScriptableO>(skillAssetPath);
            if (!playerSkillAsset)
            {
                continue;
            }

            skillConfigs.Add(playerSkillAsset.Config);
            playerSkillAsset.ViewConfig.ScreenPosition *= screenPosModifier;
            skillViewConfigs.Add(playerSkillAsset.ViewConfig);
            linkedCounter++;
        }

        EditorUtility.SetDirty(this);
        
        Debug.Log($"{linkedCounter} skill assets were linked.");
#endif
    }
}