using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _envirPrefab;
    [SerializeField] private List<GameObject> _envirList;
    [SerializeField] private Vector3 _spawnPos = new Vector3(0f, 0f, 180.44f);
    [SerializeField] private float _deactivateDelay = 5f;

    [SerializeField] private GameObject _targetPrefab;
    private Vector3 _targetPosition;

    private GameObject _currentEnvir;

    void Start()
    {
        if (_envirList == null)
            _envirList = new List<GameObject>();

        if (_envirPrefab == null)
            Debug.LogError("Environment prefab is not assigned in the inspector.");

        foreach (GameObject env in _envirList)
        {
            if (env.activeInHierarchy)
            {
                _currentEnvir = env;
                break;
            }
        }
    }

    public void SpawnEnvironment()
    {
        var oldEnvir = _currentEnvir;

        _currentEnvir = GetEnvirFromPool();
        _currentEnvir.transform.position = _spawnPos;
        _currentEnvir.SetActive(true);

        if (oldEnvir != null)
        {
            StartCoroutine(DeactivatingRoutine(oldEnvir));
        }
    }

    public void SpawnTarget()
    {
        if (_targetPrefab == null)
        {
            Debug.LogError("Target prefab is not assigned in the inspector.");
            return;
        }

        _targetPosition = new Vector3(Random.Range(-6f, 6f), Random.Range(1f, 3f), Random.Range(35f, 40f));
        GameObject target = Instantiate(_targetPrefab, _targetPosition, Quaternion.identity);
        target.transform.SetParent(transform);
    }

    private GameObject GetEnvirFromPool()
    {
        foreach (GameObject env in _envirList)
        {
            if (!env.activeInHierarchy)
            {
                return env;
            }
        }

        return null;
    }

    IEnumerator DeactivatingRoutine(GameObject oldEnv)
    {
        yield return new WaitForSeconds(_deactivateDelay);
        if (oldEnv != null) oldEnv.SetActive(false);
    }
}
