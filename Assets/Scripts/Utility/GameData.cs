using UnityEngine;
public static class GameData
{
    public static int Money
    {
        get => PlayerPrefs.GetInt("MONEY", 0);
        set => PlayerPrefs.SetInt("MONEY", value);
    }

    public static int HiScore
    {
        get => PlayerPrefs.GetInt("HISCORE", 0);
        set => PlayerPrefs.SetInt("HISCORE", value);
    }

    public static int Rewards
    {
        get => PlayerPrefs.GetInt("REWARDS", 0);
        set => PlayerPrefs.SetInt("REWARDS", value);
    }

    public static int PlayerIndex
    {
        get => PlayerPrefs.GetInt("PLAYERINDEX", 0);
        set => PlayerPrefs.SetInt("PLAYERINDEX", value);
    }

    public static int TerrainIndex
    {
        get => PlayerPrefs.GetInt("TERRAININDEX", 0);
        set => PlayerPrefs.SetInt("TERRAININDEX", value);
    }

    public static int EnemyIndex
    {
        get => PlayerPrefs.GetInt("ENEMYINDEX", 0);
        set => PlayerPrefs.SetInt("ENEMYINDEX", value);
    }
}
