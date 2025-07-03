using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static bool _hasGreenOrb;
    private int _playerHealth = 2;
    private int _playerMaxHp;
    private int _targetCount;
    private bool _isGameOver;
    private bool _changingTime;

    [SerializeField] private GameObject errorPanel;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private GameObject retryLevelBeginButton;
    [SerializeField] private GameObject retryLevelEndButton;
    
    private float _gameDuration;
    private float _targetTimeScale = 1f;
    private float _startFixedDeltaTime;
    
    public static Action<int> onLowHealth;
    public static Action<int> showHideOrbUi;
    public static Action<bool> onPause;
    public static Action onGameOver;
    private void OnEnable()
    {
        TargetScript.OnTargetHit += UpdateTargetCount;
        DamageTriggers.onPlayerHit += UpdatePlayerHealth;
        Player.onOutOfArrows += ChangeTime;
        
        _playerMaxHp = (int)PlayerDataManager.Instance.Data.MaxHp;
        _playerHealth = PlayerDataManager.Instance.nextLevelHp;
        _hasGreenOrb = PlayerDataManager.Instance.Data.HasGreenOrb;
        _gameDuration = Time.time;
        _startFixedDeltaTime = Time.fixedDeltaTime;
        
        FreezeTime();

        NetworkManager.Instance.WebSocketCommandHandler.SendLevelBeginRequestCommand(0, OnLevelBeginSuccess, OnLevelBeginFail);
    }
    private void OnDisable()
    {
        TargetScript.OnTargetHit -= UpdateTargetCount;
        DamageTriggers.onPlayerHit -= UpdatePlayerHealth;
        Player.onOutOfArrows -= ChangeTime; 
    }
    void Update()
    {
            if (_isGameOver)
            {
                RestartGame();
            }

            if (InputManager.InputActions.Player.Exit.WasPressedThisFrame())
            {
                UnlockCursor();
                onPause?.Invoke(true);
                FreezeTime();
            }

            if (_changingTime) SmoothChangeTime();

            if (InputManager.InputActions.Player.UseItem.WasPressedThisFrame() && _hasGreenOrb)
            {
                _playerHealth++;
                _hasGreenOrb = false;
                showHideOrbUi?.Invoke(1);
                onLowHealth?.Invoke(1);
            }
        

    }

    private void UpdateTargetCount ()
    {
        _targetCount++;
        PlayerDataManager.Instance.IncrementLightOrbs();
    }

    private void UpdatePlayerHealth ()
    {
        _playerHealth--;
        if (_playerHealth <= _playerMaxHp * 0.5f)
        {
            UiManager.Instance.UpdateHealthBar(1);
            onLowHealth?.Invoke(0);
            if (_hasGreenOrb) showHideOrbUi?.Invoke(0);
        }

        if (_playerHealth <= 0) {
            UiManager.Instance.UpdateHealthBar(0);
            _isGameOver = true;
            PlayerDataManager.Instance.ResetNextLevelHp();
            UiManager.Instance.LoseGame(CalculateScore());
            
            _changingTime = false;
            UnlockCursor();
            onGameOver?.Invoke();
            FreezeTime();
        }
    }
    public void Continue()
    {
        LockCursor();
        UnFreezeTime();
        onPause?.Invoke(false);
    }


    private void ChangeTime(int id)
    {
        if (id == 0) _targetTimeScale = 0.5f;
        else if (id == 1) _targetTimeScale = 1f;
        _changingTime = true;
    }
    private void FreezeTime() => Time.timeScale = 0;
    private void UnFreezeTime() => Time.timeScale = 1;
    
    private void SmoothChangeTime()
    {
        Time.timeScale = Mathf.Lerp(Time.timeScale, _targetTimeScale, Time.unscaledDeltaTime * 5f);

        Time.fixedDeltaTime = _startFixedDeltaTime * Time.timeScale;
        
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }

    private void RestartGame()
    {
        if (InputManager.InputActions.Player.Reload.WasPressedThisFrame())
        {
            NetworkManager.Instance.WebSocketCommandHandler.SendLevelEndRequestCommand(0, CalculateScore(),  OnLevelEndSuccess, OnLevelEndFail);
            UnlockCursor();
        }

    }

    int CalculateScore()
    {
       return (int)(Time.time - _gameDuration) * _targetCount;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void OnLevelBeginSuccess()
    {
        Continue();   
    }

    void OnLevelBeginFail(int code, string message)
    {
        UnlockCursor();
        errorPanel.SetActive(true);
        errorText.text = message;
    }

    void OnLevelEndSuccess()
    {
        PlayerDataManager.Instance.SavePlayerData();
        SceneManager.LoadScene(1);
    }

    public void RetryLevelBegin()
    {
        NetworkManager.Instance.WebSocketCommandHandler.SendLevelBeginRequestCommand(0, OnLevelBeginSuccess, OnLevelBeginFail);
    }

    public void RetryLevelEnd()
    {
        NetworkManager.Instance.WebSocketCommandHandler.SendLevelEndRequestCommand(0, CalculateScore(),  OnLevelEndSuccess, OnLevelEndFail);
    }

    void OnLevelEndFail(int code, string message)
    {
        UnlockCursor();
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


