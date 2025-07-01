using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TargetScript : MonoBehaviour
{
    [SerializeField] private Rigidbody targetRigidbody;
    [SerializeField] private Animator targetAnimator;
    [SerializeField] private Collider targetCollider;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject particles;

    public static Action OnTargetHit;
    private void OnEnable()
    {
        particles.SetActive(false);
        particles.transform.position = transform.position;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            OnTargetHit?.Invoke();
            Boom(collision);
        }
    }
    void Boom (Collision collision)
    {
        targetAnimator.enabled = false;
        targetRigidbody.isKinematic = false;
        targetRigidbody.useGravity = true;
        targetCollider.enabled = false;
        
        audioSource.Play();
        
        var impactPoint = collision.contacts[0].point;
        var forceDirection = collision.transform.forward;

        particles.SetActive(true);
        particles.transform.position = impactPoint;
        
        targetRigidbody.AddForceAtPosition(forceDirection * 5f, impactPoint, ForceMode.Impulse);

        var randomTorque = new Vector3(Random.Range(-1f, 1f), Random.Range(0, 1f), Random.Range(-1f, 1f));

        targetRigidbody.AddTorque(randomTorque, ForceMode.Impulse);

        Destroy(gameObject, 3f);
    }
}
