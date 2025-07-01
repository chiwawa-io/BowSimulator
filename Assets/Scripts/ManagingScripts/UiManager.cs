using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

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

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject playerProfile;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI playerLevelInMainMenu;
    [SerializeField] private TextMeshProUGUI playerLevelInProfile;
    
    private int _comboCounter;

    private void OnEnable()
    {
        TargetScript.OnTargetHit += UpdateProgressBar;
        Store.onPressItem += ShowStorePopup;
        Store.onBoughtItem += UpdateLightOrbs;
        Player.onOutOfArrows += OnOutOfArrows;
        GameManager.showHideOrbUi += OnLowHealth;
        GameManager.onPause += OnPause;
        
        if (balance != null) balance.text = $"{GameManager.LightOrbs}";
        if (playerProfile != null)
        {
         playerLevelInMainMenu.text = $"{GameManager.Level}";   
         playerLevelInProfile.text = $"{GameManager.Level}";   
        }
    }
    private void OnDisable()
    {
        TargetScript.OnTargetHit -= UpdateProgressBar;
        Store.onPressItem -= ShowStorePopup;
        Store.onBoughtItem -= UpdateLightOrbs;
        Player.onOutOfArrows -= OnOutOfArrows;
        GameManager.showHideOrbUi -= OnLowHealth;
        GameManager.onPause -= OnPause;
    }

    void Start()
    {
        Instance = this;

        if (healthBar == null) Debug.Log("HealthBar is null");
        else healthBar.sprite = healthSprites[2];
        
        if (progressBar == null) Debug.Log("progressBar is null");
        else progressBar.sprite = progressSprites[0];

        if (GameManager.HasGreenOrb && greenOrbOverlay != null) 
        {
            greenOrbOverlay.SetActive(true);
            greenOrbButton.SetActive(false);
        }

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

    public void OpenClosePlayerProfile(int id)
    {
        switch (id)
        {
            case 1:
                mainMenu.SetActive(false);
                playerProfile.SetActive(true);
                break;
            case 2:
                mainMenu.SetActive(true);
                playerProfile.SetActive(false);
                break;
            default:
                mainMenu.SetActive(true);
                playerProfile.SetActive(false);
                break;
        }
    }

    void UpdateLightOrbs(int a, int b)
    {
        if (balance != null) balance.text = $"{GameManager.LightOrbs}";
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
