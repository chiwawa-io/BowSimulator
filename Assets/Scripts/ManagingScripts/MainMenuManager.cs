using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{ 
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private GameObject newPlayerPanel;
    [SerializeField] private TMP_InputField playerNameInput;

    [SerializeField] private GameObject mainMenuButton;

    private void OnEnable()
    {
        PlayerDataManager.OnPlayerDataError += ShowErrorPanel;
        if (PlayerDataManager.Instance.CheckForNewPlayer())
        {
            newPlayerPanel.SetActive(true);
        }
    }
    
    private void OnDisable()
    {
        PlayerDataManager.OnPlayerDataError -= ShowErrorPanel;
    }

    public void EnterEndlessMode()
    {
        InputManager.Instance.EnablePlayerInput();
        SceneManager.LoadScene(2);
    }

    public void EnterStore()
    {
        InputManager.Instance.EnablePlayerInput();
        SceneManager.LoadScene(3);
    }

    public void ReturnToMenu()
    {
        PlayerDataManager.Instance.SavePlayerData();
        SceneManager.LoadScene(1);
    }

    public void ShowErrorPanel(string message)
    {
        errorPanel.SetActive(true);
        errorText.text = message;
    }

    public void ChangeName()
    {
        var value = playerNameInput.text;
        Debug.Log(value); 
        PlayerDataManager.Instance.ChangeNewPlayerName(value);
    }

    public void CloseNamePanel()
    {
        newPlayerPanel.SetActive(false);
        UiManager.Instance.UpdatePlayerName();
    }
    public void Exit()
    {
        Application.Quit();
    }
}
