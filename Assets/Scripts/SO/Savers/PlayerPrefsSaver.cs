using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsSaver : ISaver
{
    const string PrefsKey = "playerData"; 
    
    public void SavePlayer(Player savedSO)
    {
        PlayerPrefs.SetString(PrefsKey, JsonUtility.ToJson(savedSO));
        PlayerPrefs.Save();
    }

    public void LoadPlayerTo(Player targetSO)
    {
        JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(PrefsKey, ""), targetSO);
    }

    public bool IsAvailable()
    {
        return PlayerPrefs.HasKey(PrefsKey) && !string.IsNullOrEmpty(PlayerPrefs.GetString(PrefsKey, ""));
    }
}
