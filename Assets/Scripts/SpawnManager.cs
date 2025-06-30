using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> envirList;
    
    [SerializeField] private float deactivateDelay = 5f;

    [SerializeField] private GameObject targetPrefab;
    
    private Vector3 _targetPosition;
    private Vector3 _spawnPos;
    private GameObject _currentEnvir;
    private Movables _envirScript;


    private void OnEnable()
    {
        TargetScript.OnTargetHit += SpawnTarget;
    }
    private void OnDisable()
    {
        TargetScript.OnTargetHit -= SpawnTarget;
    }

    void Start()
    {
        envirList ??= new List<GameObject>();

        foreach (GameObject env in envirList)
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
        _envirScript = oldEnvir.GetComponent<Movables>();
        _spawnPos.z = _envirScript.GetNextRoad().position.z;
        
        
        _currentEnvir = GetEnvirFromPool();
        _currentEnvir.transform.position = _spawnPos;
        _currentEnvir.SetActive(true);

        if (oldEnvir != null)
        {
            StartCoroutine(DeactivatingRoutine(oldEnvir));
        }
    }

    private void SpawnTarget()
    {
        if (targetPrefab == null)
        {
            Debug.LogError("Target prefab is not assigned in the inspector.");
            return;
        }

        _targetPosition = new Vector3(Random.Range(-6f, 6f), Random.Range(1f, 3f), Random.Range(35f, 40f));
        GameObject target = Instantiate(targetPrefab, _targetPosition, Quaternion.identity);
        target.transform.SetParent(transform);
    }

    private GameObject GetEnvirFromPool()
    {
        foreach (GameObject env in envirList)
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
        yield return new WaitForSeconds(deactivateDelay);
        if (oldEnv != null) oldEnv.SetActive(false);
    }
}
