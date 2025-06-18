using UnityEngine;

public class TargetScript : MonoBehaviour
{
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider _collider;
    void Start()
    {
        _spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (_spawnManager == null)
            Debug.LogError("SpawnManager component not found on the TargetScript GameObject.");
        if (_gameManager == null)
            Debug.LogError("GameManager component not found on the TargetScript GameObject.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            _spawnManager.SpawnTarget();
            _gameManager.UpdateTargetCount();
            Boom(collision);
        }
    }
    void Boom (Collision collision)
    {
        _animator.enabled = false;
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
        _collider.enabled = false;

        var impactPoint = collision.contacts[0].point;
        var forceDirection = collision.transform.forward;

        _rigidbody.AddForceAtPosition(forceDirection * 5f, impactPoint, ForceMode.Impulse);

        var randomTorque = new Vector3(Random.Range(-1f, 1f), Random.Range(0, 1f), Random.Range(-1f, 1f));

        _rigidbody.AddTorque(randomTorque, ForceMode.Impulse);

        Destroy(gameObject, 3f);
    }
}
