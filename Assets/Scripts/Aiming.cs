using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private GameObject _arrowPrefab;
    private List<GameObject> _arrows;

    [SerializeField] private Vector3 arrowOffsets = new Vector3(0, 0, 0f);

    private float mouseInputX;
    private float mouseInputY;
    void Start()
    {
        _animator = GetComponent<Animator>();

        InitPoolOfArrows();

        if (_arrowPrefab == null)
            Debug.LogError("Arrow prefab is not assigned in the inspector.");
        if (_animator == null)
            Debug.LogError("Animator component is not assigned in the inspector.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            _animator.SetBool("Aiming", true);
            StartCoroutine(FireArrow());
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
            _animator.SetBool("Aiming", false);

        Movement();
    }
    
    void Movement()
    {
        mouseInputX = Input.GetAxis("Mouse X");
        if (mouseInputX != 0)
        {
            Vector3 rotationX = transform.rotation.eulerAngles;
            rotationX.y += mouseInputX * 2f;
            transform.rotation = Quaternion.Euler(rotationX);
        }

        if (Input.GetKey(KeyCode.W))
        {
            Vector3 rotationY = transform.rotation.eulerAngles;
            rotationY.x -= 0.2f;
            transform.rotation = Quaternion.Euler(rotationY);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 rotationY = transform.rotation.eulerAngles;
            rotationY.x += 0.2f;
            transform.rotation = Quaternion.Euler(rotationY);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * 0.1f, Space.World);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * 0.1f, Space.World);
        }
    }

    void InitPoolOfArrows()
    {
        _arrows = new List<GameObject>();

        for (int i = 0; i < 10; i++)
        {
            GameObject arrow = Instantiate(_arrowPrefab, transform.position, Quaternion.identity);
            arrow.transform.SetParent(transform); 
            arrow.SetActive(false);
            _arrows.Add(arrow);
        }
    }

    void GetArrowFromPool()
    {
        foreach (GameObject arrow in _arrows)
        {
            if (!arrow.activeInHierarchy)
            {
                arrow.SetActive(true);
                arrow.transform.position = transform.TransformPoint(arrowOffsets);
                return;
            }
        }
        
        Debug.LogWarning("No available arrows in the pool.");
    }

    IEnumerator FireArrow()
    {
        yield return new WaitForSeconds(0.2f);
        GetArrowFromPool();
    }
    
}
