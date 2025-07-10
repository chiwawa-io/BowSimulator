using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    [SerializeField] private Image healthBar;
    [SerializeField] private Image progressBar;
    [SerializeField] private TextMeshProUGUI scoreNumber;
    [SerializeField] private GameObject pauseMenu;
    
    [SerializeField] private GameObject scoreText;
    [SerializeField] private GameObject tryAgainText;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private GameObject reloadButton;
    [SerializeField] private GameObject greenOrbButton;
    [SerializeField] private GameObject greenOrbOverlay;

    [SerializeField] private Sprite[] healthSprites;
    [SerializeField] private Sprite[] progressSprites;

    [SerializeField] private float smallComboCooldown;
    [SerializeField] private float mediumComboCooldown;
    [SerializeField] private float highComboCooldown;
    
    [SerializeField] private TextMeshProUGUI balance;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private TextMeshProUGUI itemPrice;
    [SerializeField] private Image itemSprite;
    [SerializeField] private GameObject items;
    [SerializeField] private GameObject buyMenuPopUp;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject exitToMenu;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject playerProfile;
    [SerializeField] private TextMeshProUGUI playerNameInMainMenu;
    [SerializeField] private TextMeshProUGUI playerNameInProfile;
    [SerializeField] private TextMeshProUGUI playerLevelInMainMenu;
    [SerializeField] private TextMeshProUGUI playerLevelInProfile;
    [SerializeField] private GameObject closePlayerProfileButton;
    [SerializeField] private GameObject endlessButton;
    [SerializeField] private EventSystem eventSystem;
    private int _comboCounter;

    private void OnEnable()
    {
        TargetScript.OnTargetHit += UpdateProgressBar;
        Store.onPressItem += ShowStorePopup;
        Player.onOutOfArrows += OnOutOfArrows;
        GameManager.ShowHideOrbUi += OnLowHealth;
        GameManager.OnPause += OnPause;
    }
    private void OnDisable()
    {
        TargetScript.OnTargetHit -= UpdateProgressBar;
        Store.onPressItem -= ShowStorePopup;
        Player.onOutOfArrows -= OnOutOfArrows;
        GameManager.ShowHideOrbUi -= OnLowHealth;
        GameManager.OnPause -= OnPause;
    }

    void Start()
    {
        Instance = this;

        if (healthBar == null) Debug.Log("HealthBar is null");
        else healthBar.sprite = healthSprites[2];
        
        if (progressBar == null) Debug.Log("progressBar is null");
        else progressBar.sprite = progressSprites[0];
        
        DisplayPlayerData();
    }

    public void UpdateHealthBar(int health) {
        healthBar.sprite = healthSprites[health];
    }

    public void UpdateProgressBar()
    {
        StopAllCoroutines();
        switch (_comboCounter)
        {
            case < 2:
                comboText.gameObject.SetActive(true);
                StartCoroutine(ComboCooldownRoutine(smallComboCooldown));
                break;
            case < 4:
                StartCoroutine(ComboCooldownRoutine(mediumComboCooldown));
                comboText.gameObject.SetActive(true);
                break;
            case > 4:
                StartCoroutine(ComboCooldownRoutine(highComboCooldown));
                comboText.gameObject.SetActive(true);
                break;
            default:
                StartCoroutine(ComboCooldownRoutine(smallComboCooldown));
                comboText.gameObject.SetActive(true);
                break;
        }
        {
            
        }
    }

    private void ShowStorePopup(string Name, string description, int price, Sprite sprite)
    {
        items.SetActive(false);
        buyMenuPopUp.SetActive(true);
        
        itemSprite.sprite = sprite;
        itemName.text = Name;
        itemDescription.text = description;
        itemPrice.text = $"{price} light orbs";
    }

    public void ExitPopUp()
    {
        buyMenuPopUp.SetActive(false);
        items.SetActive(true);
    }

    void OnOutOfArrows(int id)
    {
        switch (id)
        {
            case 0:
                reloadButton.SetActive(true);
                break;
            case 1:
                reloadButton.SetActive(false);
                break;
            default:
                break;
        }

    }

    void OnLowHealth(int id)
    {
        switch (id)
        {
            case 0:
                greenOrbButton.SetActive(true);
                break;
            case 1:
                greenOrbButton.SetActive(false);
                greenOrbOverlay.SetActive(false);
                break;
            default:
                break;
        }

    }

    void OnPause(bool paused)
    {
        if (pauseMenu != null){
            if (paused) pauseMenu.SetActive(true);
            else pauseMenu.SetActive(false);
        }
    }

    public void LoseGame(int score)
    {
        scoreText.SetActive(true);
        scoreNumber.gameObject.SetActive(true);
        scoreNumber.text = score.ToString();
        tryAgainText.SetActive(true);
        progressBar.gameObject.SetActive(false);
        comboText.gameObject.SetActive(false);
        Debug.Log(score);
    }

    public void OpenClosePlayerProfile(bool isOpen)
    {
        if (isOpen)
        {
            playerProfile.SetActive(true);
            mainMenu.SetActive(false);
        }
        else
        {
            playerProfile.SetActive(false);
            mainMenu.SetActive(true);
        }
    }

    public void UpdateLightOrbs()
    {
        if (balance != null) balance.text = $"{PlayerDataManager.Instance.Data.LightOrbs}";
    }

    public void UpdatePlayerName()
    {
        playerNameInMainMenu.text = PlayerDataManager.Instance.Data.Username;
        playerNameInProfile.text = PlayerDataManager.Instance.Data.Username;
    }


    void DisplayPlayerData()
    {
        if (PlayerDataManager.Instance.Data.HasGreenOrb && greenOrbOverlay != null) 
        {
            greenOrbOverlay.SetActive(true);
            greenOrbButton.SetActive(false);
        }
        if (balance != null) balance.text = $"{PlayerDataManager.Instance.Data.LightOrbs}";
        if (playerProfile != null)
        {
            playerLevelInMainMenu.text = $"{PlayerDataManager.Instance.Data.Level}";   
            playerLevelInProfile.text = $"{PlayerDataManager.Instance.Data.Level}";   
            UpdatePlayerName();
        }
    }
    IEnumerator ComboCooldownRoutine(float waitTime)
    {
        _comboCounter++;
        comboText.text = "x" + _comboCounter;
        var comboBar = 5;
        while (comboBar > -1)
        {
            yield return new WaitForSeconds(waitTime);
            progressBar.sprite = progressSprites[comboBar];
            comboBar--;
            if (comboBar <= -1)
            {
                _comboCounter = 0;
                comboText.gameObject.SetActive(false);
                break;
            }
        }
        
    }
}
