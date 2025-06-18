using UnityEngine.UI;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _progressBar;

    [SerializeField] private Sprite[] _healthSprites;
    [SerializeField] private Sprite[] _progressSprites;

    void Start()
    {
        Instance = this;

        if (_healthBar == null) Debug.Log("HealthBar is null");
        if (_progressBar == null) Debug.Log("_progressBar is null");
    }

    public void UpdateHealthBar(int health) {
        _healthBar.sprite = _healthSprites[health];
    }

    public void UpdateProgressBar(int progress) {
        _progressBar.sprite = _progressSprites[progress];
    }
}
