using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    [SerializeField] private Image healthBar;
    [SerializeField] private Image progressBar;
    [SerializeField] private GameObject wolfImage;
    [SerializeField] private GameObject wolfText;
    [SerializeField] private GameObject wonText;
    [SerializeField] private GameObject tryAgainText;
    [SerializeField] private TextMeshProUGUI comboText;

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
    
    private int _comboCounter;

    private void OnEnable()
    {
        TargetScript.OnTargetHit += UpdateProgressBar;
        Store.onPressItem += ShowStorePopup;
        
        if (balance != null) balance.text = $"{GameManager.lightOrbs}"; 
    }
    private void OnDisable()
    {
        TargetScript.OnTargetHit -= UpdateProgressBar;
        Store.onPressItem -= ShowStorePopup;
    }

    void Start()
    {
        Instance = this;

        if (healthBar == null) Debug.Log("HealthBar is null");
        else healthBar.sprite = healthSprites[2];
        
        if (progressBar == null) Debug.Log("progressBar is null");
        else progressBar.sprite = progressSprites[0];
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

    public void WonGame()
    {
        wonText.SetActive(true);
        tryAgainText.SetActive(true);
        healthBar.gameObject.SetActive(false);
        progressBar.gameObject.SetActive(false);
        comboText.gameObject.SetActive(false);
    }

    public void LoseGame()
    {
        wolfText.SetActive(true);
        wolfImage.SetActive(true);
        tryAgainText.SetActive(true);
        healthBar.gameObject.SetActive(false);
        progressBar.gameObject.SetActive(false);
        comboText.gameObject.SetActive(false);
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
