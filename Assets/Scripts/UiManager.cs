using UnityEngine.UI;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _progressBar;
    [SerializeField] private GameObject _wolfImage;
    [SerializeField] private GameObject _wolfText;
    [SerializeField] private GameObject _wonText;
    [SerializeField] private GameObject _tryAgainText;

    [SerializeField] private Sprite[] _healthSprites;
    [SerializeField] private Sprite[] _progressSprites;
    [SerializeField] private Sprite _invisible;

    void Start()
    {
        Instance = this;

        _healthBar.sprite = _healthSprites[2];
        _progressBar.sprite = _progressSprites[0];

        if (_healthBar == null) Debug.Log("HealthBar is null");
        if (_progressBar == null) Debug.Log("_progressBar is null");
    }

    public void UpdateHealthBar(int health) {
        _healthBar.sprite = _healthSprites[health];
    }

    public void UpdateProgressBar(int progress) {
        _progressBar.sprite = _progressSprites[progress];
    }

    public void WonGame()
    {
        _wonText.SetActive(true);
        _tryAgainText.SetActive(true);
        _healthBar.gameObject.SetActive(false);
        _progressBar.gameObject.SetActive(false);
    }

    public void LoseGame()
    {
        _wolfText.SetActive(true);
        _wolfImage.SetActive(true);
        _tryAgainText.SetActive(true);
        _healthBar.gameObject.SetActive(false);
        _progressBar.gameObject.SetActive(false);
    }
}
