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
}
