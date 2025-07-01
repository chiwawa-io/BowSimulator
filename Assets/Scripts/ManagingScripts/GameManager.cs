using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static int _lightOrbs;
    private static int _level;
    private static int _currentXp;
    private static float _playerMaxHp = 2f;
    private static int _nextLevelHp = (int)_playerMaxHp;
    private static bool _hasGreenOrb;

    public static int LightOrbs {get => _lightOrbs; private set => _lightOrbs = value; }
    public static int Level {get => _level; private set => _level = value; }
    public static bool HasGreenOrb => _hasGreenOrb;

    [SerializeField] private int playerHealth = 2;
    [SerializeField] private int targetCount;
    private bool _isWon;
    private bool _isGameOver;
    private bool _changingTime;
    
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
        Store.onBoughtItem += OnBoughtItem;
        Player.onOutOfArrows += ChangeTime;
        
        playerHealth = _nextLevelHp;
        _gameDuration = Time.time;
        _startFixedDeltaTime = Time.fixedDeltaTime;
    }
    private void OnDisable()
    {
        TargetScript.OnTargetHit -= UpdateTargetCount;
        DamageTriggers.onPlayerHit -= UpdatePlayerHealth;
        Store.onBoughtItem -= OnBoughtItem;
    }
    void Update()
    {
        if (_isWon || _isGameOver)
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
            playerHealth++;
            _hasGreenOrb = false;
            showHideOrbUi?.Invoke(1);
            onLowHealth?.Invoke(1);
        }

    }

    private void UpdateTargetCount ()
    {
        targetCount++;
        _lightOrbs ++;
    }

    private void UpdatePlayerHealth ()
    {
        playerHealth--;
        if (playerHealth <= _playerMaxHp * 0.5f)
        {
            UiManager.Instance.UpdateHealthBar(1);
            onLowHealth?.Invoke(0);
            if (_hasGreenOrb) showHideOrbUi?.Invoke(0);
        }

        if (playerHealth <= 0) {
            UiManager.Instance.UpdateHealthBar(0);
            _isGameOver = true;
            _nextLevelHp = (int)_playerMaxHp;
            UiManager.Instance.LoseGame(CalculateScore());
            
            UnlockCursor();
            onGameOver?.Invoke();
            FreezeTime();
        }
    }

    public void OnBoughtItem(int id, int price)
    {
        LightOrbs -= price;
        switch (id) 
        {
            case 1:
                _currentXp++;
                CheckCurrentLevel();
                _nextLevelHp = (int)_playerMaxHp;
                break;
            case 2:
                _nextLevelHp+=3;
                break;
            case 3:
                _nextLevelHp = (int)_playerMaxHp;
                _hasGreenOrb = true;
                break;
            case 4:
                _nextLevelHp = (int)_playerMaxHp;
                // _hasDarkBow = true;
                break;
            default:
                break;
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
            UnlockCursor();
        }

    }

    public void EnterEndlessMode()
    {
        SceneManager.LoadScene(1);
        LockCursor();
    }

    public void EnterTrainingMode()
    {
        SceneManager.LoadScene(2);
        LockCursor();   
    }

    public void EnterStore()
    {
        SceneManager.LoadScene(3);
        UnlockCursor();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    void CheckCurrentLevel()
    {
        if (_currentXp >= 10)
        {
            _level++;
            _currentXp = 0;
            IncreaseHp();
        }
    }

    void IncreaseHp()
    {
        _playerMaxHp += 0.1f;
    }

    int CalculateScore()
    {
       return (int)(Time.time - _gameDuration) * targetCount;
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
}
