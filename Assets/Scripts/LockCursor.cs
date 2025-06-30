using UnityEngine;

public class CursorController : MonoBehaviour
{
    void Start()
    {
        LockCursor();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (Cursor.lockState == CursorLockMode.Locked)
            {
                UnlockCursor();
            }
            else
            {
                LockCursor();
            }
        }
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}