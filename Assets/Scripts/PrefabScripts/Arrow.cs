using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    void Awake()
    {
        if (_rigidbody == null)
            _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
            Debug.Log("Rigidbody component is not assigned in the inspector.");
    }

    private void OnEnable()
    {
        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = true; 
        }
        transform.localPosition = Vector3.zero; 
        transform.localRotation = Quaternion.identity;
    }

    public void Shoot(Vector3 direction, float force)
    {
        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(direction * force, ForceMode.Impulse);
        }
        else
        {
            Debug.Log("Rigidbody is not assigned or found on the arrow object.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Wait3SEconds();
    }

    IEnumerator Wait3SEconds()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}
