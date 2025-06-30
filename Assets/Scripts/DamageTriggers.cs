using System;
using UnityEngine;

public class DamageTriggers : MonoBehaviour
{
    [SerializeField] private Rigidbody targetRigidbody;
    
    public static Action onPlayerHit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) onPlayerHit?.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow")) Boom(collision);

    }

    private void Boom(Collision collision) {
        targetRigidbody.isKinematic = false;

        var impactPoint = collision.contacts[0].point;
        var forceDirection = collision.transform.forward;

        targetRigidbody.AddForceAtPosition(forceDirection * 5f, impactPoint, ForceMode.Impulse);

        var randomTorque = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(-1f, 1f));

        targetRigidbody.AddTorque(randomTorque, ForceMode.Impulse);

        Destroy(gameObject, 3f);
    }
}
