using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadMainMenu());
    }
    IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(1);
    }
}
