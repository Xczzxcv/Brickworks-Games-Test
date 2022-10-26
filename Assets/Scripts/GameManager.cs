using System;
using Player;
using Player.Skills;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public struct SkillView : IEquatable<SkillView>
    {
        public string Name;
        public bool IsLearned;
        public int LearningCost;

        public bool Equals(SkillView other)
        {
            return Equals(this, other);
        }

        public override string ToString()
        {
            return $"N: {Name}  L: {(IsLearned ? "1" : "0")} C: {LearningCost}";
        }
    }

    public DataManager DataManager;
    public ConfigsManager Configs;
    public PlayerManager Player;

    void Start()
    {
        var graph = new Graph_Linked.Graph<SkillView>();
        var skill1 = new SkillView {Name = "1"};
        var skill2 = new SkillView {Name = "2"};
        var skill3 = new SkillView {Name = "3"};
        var skill4 = new SkillView {Name = "4"};
        graph.Init(skill1);
        graph.AddNode(graph.Head, skill2);
        graph.AddNode(graph.Head, skill3);
        if (graph.TryGetNode(skill3, out var skill3Node))
        {
            graph.AddNode(skill3Node, skill4);
        }

        Debug.Log(graph);
        Init();
        Debug.Log($"{Player.SkillsManager.SkillPoints}");
        Player.SkillsManager.AddSkillPoints(10);
        Debug.Log($"{Player.SkillsManager.SkillPoints}");

        Debug.Log($"{Player.SkillsManager.CanLearnSkill(JumpPlayerSkillConfig.ID)}");
        Debug.Log($"{Player.SkillsManager.CanLearnSkill(FlyPlayerSkillConfig.ID)}");
        Player.SkillsManager.LearnSkill(JumpPlayerSkillConfig.ID);

        Debug.Log($"{Player.SkillsManager.CanLearnSkill(JumpPlayerSkillConfig.ID)}");
        Debug.Log($"{Player.SkillsManager.CanLearnSkill(FlyPlayerSkillConfig.ID)}");
    }

    public void Init()
    {
        DataManager = new DataManager();
        DataManager.Init();
        
        Configs = new ConfigsManager();
        Configs.Init();
        
        Player = new PlayerManager(Configs);
        Player.Init(DataManager.PlayerData);
    }
}