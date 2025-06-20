using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _playerHealth = 0;
    [SerializeField] private int _targerCount = 0;
    [SerializeField] private int _targetsWinCount = 0;

    private bool _isWon = false;
    private bool _isGameOver = false;
    void Update()
    {
        if (_isWon || _isGameOver) RestartGame();
    }

    public void UpdateTargetCount ()
    {
        _targerCount++;
        UiManager.Instance.UpdateProgressBar(_targerCount);
        if (_targerCount >= _targetsWinCount) {
            _isWon = true;
            UiManager.Instance.WonGame();
        }
    }

    public void UpdatePlayerHealth ()
    {
        _playerHealth--;
        if (_playerHealth <= 3 && _playerHealth > 2) UiManager.Instance.UpdateHealthBar(1);
        if (_playerHealth <= 0) {
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
