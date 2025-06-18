using UnityEngine;

public class SpawnRoadTrigger : MonoBehaviour
{
    private SpawnManager spawnManager;
    void Start()
    {
        var spawnManagerObject = GameObject.FindGameObjectWithTag("SpawnManager");
        spawnManager = spawnManagerObject.GetComponent<SpawnManager>();

        if (spawnManager == null)
        {
            Debug.Log("SpawnManager component not found on the SpawnManager GameObject.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spawnManager.SpawnEnvironment();
        }
    }
}
