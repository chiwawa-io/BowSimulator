using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance {get; private set;}
    
    public PlayerData Data {get; private set;}

    private int _nextLevelHp;
    
    public int NextLevelHp => _nextLevelHp;
    private static bool _isNewPlayer;

    public static Action<string> OnPlayerDataError;
    
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
        string path = Path.Combine(Application.persistentDataPath, "PlayerData.json");

        if (File.Exists(path))
        {
            Data = JsonConvert.DeserializeObject<PlayerData>(File.ReadAllText(path));
            _nextLevelHp = (int)Data.MaxHp;
        }
        else
        {
            _isNewPlayer = true;
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
            string path = Path.Combine(Application.persistentDataPath, "PlayerData.json");

            var json = JsonConvert.SerializeObject(Data);
            
            File.WriteAllText(path, json);
        }

    }

    public void ChangeNewPlayerName(string playerName)
    {
        Data.Username = playerName;
        Data.CurrentXp = 0;
        Data.HasGreenOrb = false;
        Data.MaxHp = 2;
        Data.Level = 1;
        Data.LightOrbs = 0;
        
        SavePlayerData();
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
                _nextLevelHp = (int)Data.MaxHp;
                break;
            case 2:
                _nextLevelHp+=3;
                break;
            case 3:
                Data.HasGreenOrb = true;
                _nextLevelHp = (int)Data.MaxHp;
                
                break;
            case 4:
                // _hasDarkBow = true;
                _nextLevelHp = (int)Data.MaxHp;
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
        _nextLevelHp = (int)Data.MaxHp;
    }

    public void GreenOrbUsed()
    {
        Data.HasGreenOrb = false;
    }

    public bool CheckForNewPlayer()
    {
        return _isNewPlayer;
    }
}
