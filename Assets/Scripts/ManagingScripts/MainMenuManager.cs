using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{ 
    [SerializeField] private GameObject leaderboardUI;
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private TextMeshProUGUI errorText;

    private void OnEnable()
    {
        PlayerDataManager.onPlayerDataError += ShowErrorPanel;
    }
    
    private void OnDisable()
    {
        PlayerDataManager.onPlayerDataError -= ShowErrorPanel;
    }

    public void EnterEndlessMode()
    {
        SceneManager.LoadScene(2);
    }

    public void EnterTrainingMode()
    {
        SceneManager.LoadScene(3);
    }

    public void EnterStore()
    {
        SceneManager.LoadScene(4);
    }

    public void ReturnToMenu()
    {
        PlayerDataManager.Instance.SavePlayerData();
        SceneManager.LoadScene(1);
    }

    public void ShowHideLeaderboard(bool isOpen)
    {
        if (isOpen)
        {
            leaderboardUI.SetActive(true);
        }

        else leaderboardUI.SetActive(false);
    }

    public void ShowErrorPanel(string message)
    {
        errorPanel.SetActive(true);
        errorText.text = message;
    }
    public void Exit()
    {
        NetworkManager.Instance.HealthStatusCheckService.Deactivate();
        NetworkManager.Instance.WebSocketService.CloseConnection();
        Application.Quit();
    }
}
