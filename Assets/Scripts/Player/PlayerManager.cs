using Data;

namespace Player
{
public class PlayerManager
{
    public readonly PlayerSkillsManager SkillsManager;

    private PlayerData _data;

    public PlayerManager(ConfigsManager configs)
    {
        SkillsManager = new PlayerSkillsManager(configs.PlayerSkills);
    }

    public void Init(PlayerData data)
    {
        _data = data;
        SkillsManager.Init(_data.SkillsData);
    }
}
}