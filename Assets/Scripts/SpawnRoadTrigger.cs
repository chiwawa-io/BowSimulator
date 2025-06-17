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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player collided with SpawnRoadTrigger.");
            spawnManager.SpawnEnvironment();
        }
    }
}
