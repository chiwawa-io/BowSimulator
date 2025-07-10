using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    public static bool HasInitialized;
    void Start()
    {
        if (HasInitialized) Destroy(gameObject);
        else
        {
            GetPlayerData();
            HasInitialized = true;
        }
    }
    void GetPlayerData()
    {
        PlayerDataManager.Instance.LoadPlayerData();
    }
}
