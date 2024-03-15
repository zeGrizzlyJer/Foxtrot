using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public List<PlayerUnlockData> characterUnlockData = new List<PlayerUnlockData>();
    public List<PlayerUnlockData> enemyUnlockData = new List<PlayerUnlockData>();
    public List<PlayerUnlockData> terrainUnlockData = new List<PlayerUnlockData>();

    public void LoadData(PlayerUnlockData data)
    {
        data.isUnlocked = PlayerPrefs.GetInt(data.unlockName, 0) == 1;
    }

    public void SaveData(PlayerUnlockData data)
    {
        PlayerPrefs.SetInt(data.unlockName, data.isUnlocked ? 1 : 0);

        PlayerPrefs.Save();
    }

    public void ResetPlayerData()
    {
        PlayerPrefs.DeleteAll();

        foreach (PlayerUnlockData data in characterUnlockData)
        {
            if (data.cost == -1) data.isUnlocked = true;
            else data.isUnlocked = false;
        }
        foreach (PlayerUnlockData data in enemyUnlockData)
        {
            if (data.cost == -1) data.isUnlocked = true;
            else data.isUnlocked = false;
        }
        foreach (PlayerUnlockData data in terrainUnlockData)
        {
            if (data.cost == -1) data.isUnlocked = true;
            else data.isUnlocked = false;
        }
    }
}
