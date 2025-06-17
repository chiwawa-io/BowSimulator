using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _envirPrefab;
    private List<GameObject> _envirList;

    [SerializeField] private GameObject firstStarterEnvir;
    [SerializeField] private GameObject secondStarterEnvir;

    void Start()
    {
        _envirList = new List<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            GameObject envir = Instantiate(_envirPrefab);
            envir.transform.SetParent(transform);
            _envirList.Add(envir);
            envir.SetActive(false);
        }

        if (_envirPrefab == null)
            Debug.LogError("Environment prefab is not assigned in the inspector.");
    }

    public void SpawnEnvironment()
    {
        StartCoroutine(DeactivatingRoutine());
        GetEnvirFromPool();
    }

    private void GetEnvirFromPool()
    {
        foreach (GameObject envir in _envirList)
        {
            if (!envir.activeInHierarchy)
            {
                envir.SetActive(true);
                envir.transform.position = new Vector3(-4.95f, -2.98f, 181.54f);
                return;
            }
        }
        // If no inactive environment is found, instantiate a new one
        GameObject newEnvir = Instantiate(_envirPrefab);
        _envirList.Add(newEnvir);
        newEnvir.SetActive(true);
    }

    IEnumerator DeactivatingRoutine()
    {
        yield return new WaitForSeconds(5f);
        if (firstStarterEnvir.activeSelf)
            firstStarterEnvir.SetActive(false);
        else if (secondStarterEnvir.activeSelf)
            secondStarterEnvir.SetActive(false);
        else
        {
            foreach (GameObject envir in _envirList)
            {
                if (envir.activeSelf)
                {
                    envir.SetActive(false);
                    break;
                }
            }
        }
    }
}
