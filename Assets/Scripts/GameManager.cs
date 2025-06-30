using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int playerHealth;
    [SerializeField] private int targetCount;


    private bool _isWon;
    private bool _isGameOver;
    
    public static Action onLowHealth;
    private void OnEnable()
    {
        TargetScript.OnTargetHit += UpdateTargetCount;
        DamageTriggers.onPlayerHit += UpdatePlayerHealth;
    }
    private void OnDisable()
    {
        TargetScript.OnTargetHit -= UpdateTargetCount;
        DamageTriggers.onPlayerHit -= UpdatePlayerHealth;
    }
    void Update()
    {
        if (_isWon || _isGameOver) RestartGame();
    }

    private void UpdateTargetCount ()
    {
        targetCount++;
    }

    private void UpdatePlayerHealth ()
    {
        playerHealth--;
        if (playerHealth <= 3 && playerHealth > 2)
        {
            UiManager.Instance.UpdateHealthBar(1);
            onLowHealth?.Invoke();
        }

        if (playerHealth <= 0) {
            UiManager.Instance.UpdateHealthBar(0);
            _isGameOver = true;
            UiManager.Instance.LoseGame();
        }
    }
    private void RestartGame()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0); 
    }

    
}
