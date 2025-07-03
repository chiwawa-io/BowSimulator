using Luxodd.Game.Scripts.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance {get; private set;}
    
    public PlayerData Data {get; private set;}

    public int nextLevelHp;

    public static Action<string> onPlayerDataError;
    
    void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            Data = new PlayerData();
            
        }
    }
    
    public void LoadPlayerData()
    {
        NetworkManager.Instance.WebSocketCommandHandler.SendGetUserDataRequestCommand(OnLoadPlayerDataSuccess, OnLoadPlayerDataFail);
    }

    void OnLoadPlayerDataSuccess(object response)
    {
        var userDataPayload = (UserDataPayload)response;
        var userDataRaw = userDataPayload.Data;
        var userDataObject = (JObject)userDataRaw;

        if (userDataObject != null)
        {
            Data = JsonConvert.DeserializeObject<PlayerData>(userDataObject["user_data"]?.ToString() ?? string.Empty);
            nextLevelHp = (int)Data.MaxHp;
        }
        else
        {
            NetworkManager.Instance.WebSocketCommandHandler.SendProfileRequestCommand(OnPlayerNameGetSuccess, OnPlayerNameGetFail);
        }
    }


    public void SavePlayerData()
    {
        if (Data == null)
        {
            Debug.Log("PlayerDataManager: Data is null");
        }
        else
        {
            var json = JsonConvert.SerializeObject(Data);
            NetworkManager.Instance.WebSocketCommandHandler.SendSetUserDataRequestCommand(json, OnSavePlayerDataSuccess, OnSavePlayerDataFail);
        }

    }

    void OnSavePlayerDataSuccess()
    {
        Debug.Log("PlayerDataManager: SavePlayerDataSuccess");
    }

    void OnPlayerNameGetSuccess(string playerName)
    {
        Data.Username = playerName;
        Data.CurrentXp = 0;
        Data.HasGreenOrb = false;
        Data.MaxHp = 2;
        Data.Level = 1;
        Data.LightOrbs = 0;
        
        SavePlayerData();
    }

    void OnPlayerNameGetFail(int code, string msg)
    {
        onPlayerDataError?.Invoke(msg);
    }
    void OnLoadPlayerDataFail(int code, string msg)
    {
        onPlayerDataError?.Invoke(msg);
    }

    void OnSavePlayerDataFail(int code, string msg)
    {
        onPlayerDataError?.Invoke(msg);
    }
    
    public void OnBoughtItem(int id, int price)
    {
        UiManager.Instance.UpdateLightOrbs();
        Data.LightOrbs -= price;
        switch (id) 
        {
            case 1:
                Data.CurrentXp++;
                CheckCurrentLevel();
                nextLevelHp = (int)Data.MaxHp;
                break;
            case 2:
                nextLevelHp+=3;
                break;
            case 3:
                Data.HasGreenOrb = true;
                nextLevelHp = (int)Data.MaxHp;
                
                break;
            case 4:
                // _hasDarkBow = true;
                nextLevelHp = (int)Data.MaxHp;
                break;
            default:
                break;
        }
    }
    
    void CheckCurrentLevel()
    {
        if (Data.CurrentXp >= 10)
        {
            Data.Level++;
            Data.CurrentXp = 0;
            IncreaseHp();
        }
    }
    
    void IncreaseHp()
    {
        Data.MaxHp += 0.1f;
    }

    public void IncrementLightOrbs()
    {
        Data.LightOrbs++;
    }

    public void ResetNextLevelHp()
    {
        nextLevelHp = (int)Data.MaxHp;
    }
}
