using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    public static bool hasInitialized;
    void Start()
    {
        if (hasInitialized) Destroy(gameObject);
        else
        {
            
            NetworkManager.Instance.WebSocketService.ConnectToServer(OnConnectionSuccess, OnConnectionFail);
            hasInitialized = true;
        }
    }
    void OnConnectionSuccess()
    {
        NetworkManager.Instance.HealthStatusCheckService.Activate();
        PlayerDataManager.Instance.LoadPlayerData();
    }

    void OnConnectionFail()
    {
        Debug.Log("Connection Failed");
    }
}
