using DDX;
using System;
using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    public int SelectedDifficultyIndex = 0;
    public GameDifficulty[] Difficulties;
    public GameDifficulty Difficulty => Difficulties[SelectedDifficultyIndex];
}

[Serializable]
public class GameDifficulty
{
    public string Name;

    [Header("Hand")]
    public HandSettings HandSettings;

    [Header("Girls")]
    public AngelConfig Angel;
    public DruidConfig Druid;
    public OrcConfig Orc;
    public PriestConfig Priest;
    public QueenConfig Queen;
    public SuccubConfig Succub;
    public WarriorConfig Warrior;

    public T GetGirlConfigByType<T>() where T : EnemyConfig
    {
        if (Angel is T) return (T)(EnemyConfig)Angel;
        if (Druid is T) return (T)(EnemyConfig)Druid;
        if (Orc is T) return (T)(EnemyConfig)Orc;
        if (Priest is T) return (T)(EnemyConfig)Priest;
        if (Queen is T) return (T)(EnemyConfig)Queen;
        if (Succub is T) return (T)(EnemyConfig)Succub;
        if (Warrior is T) return (T)(EnemyConfig)Warrior;

        throw new NotImplementedException();

    }
}
