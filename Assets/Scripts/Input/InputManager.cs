using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public static InputActionsAsset InputActions { get; private set; }


    void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
            InputActions = new InputActionsAsset();
        }
        else Destroy(this);
        
        InputActions.Enable();
    }

    void OnDisable()
    {
        InputActions.Disable();
        InputActions.Player.Disable();
        InputActions.Ui.Disable();
    }

    public void EnablePlayerInput()
    {
        InputActions.Player.Enable();
        InputActions.Ui.Disable();
    }

    public void EnableUiInput()
    {
        InputActions.Player.Disable();
        InputActions.Ui.Enable();
    }
}
