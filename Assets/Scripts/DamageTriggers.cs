using UnityEngine;

public class DamageTriggers : MonoBehaviour
{
    private GameManager _gameManager;
    [SerializeField] private Rigidbody _rigidbody;
    void Start()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (_gameManager == null) Debug.Log("GameManager on DamageTrigger is not assigned"); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) _gameManager.UpdatePlayerHealth();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow")) Boom(collision);

    }

    private void Boom(Collision collision) {
        _rigidbody.isKinematic = false;

        var impactPoint = collision.contacts[0].point;
        var forceDirection = collision.transform.forward;

        _rigidbody.AddForceAtPosition(forceDirection * 5f, impactPoint, ForceMode.Impulse);

        var randomTorque = new Vector3(Random.Range(-1f, 1f), Random.Range(0, 1f), Random.Range(-1f, 1f));

        _rigidbody.AddTorque(randomTorque, ForceMode.Impulse);

        Destroy(gameObject, 3f);
    }
}
