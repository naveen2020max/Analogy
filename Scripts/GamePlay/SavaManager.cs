using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SavaManager 
{
    public static readonly string SavaPath = Application.dataPath + "/SaveData";

    public static void SavaData(PlayerProfile _playerProfile)
    {
        string Data;
        Data = JsonUtility.ToJson(_playerProfile);


        File.WriteAllText(SavaPath, Data);
    }

    public static PlayerProfile LoadData()
    {
        string data;
        if (Directory.Exists(SavaPath))
        {
            data = File.ReadAllText(SavaPath); 
        }

        PlayerProfile playerData;
        playerData = JsonUtility.FromJson<PlayerProfile>(SavaPath);

        return playerData;
    }

}
