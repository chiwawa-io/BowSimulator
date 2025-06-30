using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private GameObject onLowHealthVolume;
    [SerializeField] private AudioClip lowHealthClip;
    [SerializeField] private AudioClip normalStateClip;
    [SerializeField] private AudioSource audioSource;
    
    [SerializeField] private SimulateTrajectory simulatedTr;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject arrowParent;
    [SerializeField] private Vector3 arrowOffsets;
    [SerializeField] private float arrowForce;
    private List<Arrow> _arrows;
    private Arrow _currentArrowScript;
    private bool _isHoldingArrow;

    private float _mouseInputX;
    private Vector3 _simulationLaunchPosition;

    private bool _noArrowsLeft;
    private int _recharging;
    
    private string _animatorAiming = "Aiming";

    private void OnEnable()
    {
        GameManager.onLowHealth += OnLowHealth;
    }

    private void OnDisable()
    {
        GameManager.onLowHealth -= OnLowHealth;
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        onLowHealthVolume?.SetActive(false);

        InitPoolOfArrows();

        if (arrowPrefab == null)
            Debug.LogError("Arrow prefab is not assigned in the inspector.");
        if (_animator == null)
            Debug.LogError("Animator component is not assigned in the inspector.");
    }

    void Update()
    {
        Movement();
        
        if (Input.GetKeyDown(KeyCode.Mouse1) && !_isHoldingArrow && !_noArrowsLeft)
        {
            Aiming();

        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && _isHoldingArrow && !_noArrowsLeft)
        {
            Shoot();
        }
        
        if (_isHoldingArrow) Simulate();
        else if (!_isHoldingArrow) simulatedTr.gameObject.SetActive(false);


        if (_noArrowsLeft)
        {
            ReloadArrows();
        }
    }

    void Movement()
    {
        _mouseInputX = Input.GetAxis("Mouse X");
        if (_mouseInputX != 0)
        {
            Vector3 rotationX = transform.rotation.eulerAngles;
            rotationX.y += _mouseInputX * 2f;
            transform.rotation = Quaternion.Euler(rotationX);
            if (_isHoldingArrow) Simulate();

        }

        if (Input.GetKey(KeyCode.W))
        {
            Vector3 rotationY = transform.rotation.eulerAngles;
            rotationY.x -= 0.2f;
            transform.rotation = Quaternion.Euler(rotationY);
            if (_isHoldingArrow) Simulate();
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 rotationY = transform.rotation.eulerAngles;
            rotationY.x += 0.2f;
            transform.rotation = Quaternion.Euler(rotationY);
            if (_isHoldingArrow) Simulate();
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (transform.position.x > -6f) transform.Translate(Vector3.left * 0.1f, Space.World);
            if (_isHoldingArrow) Simulate();
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (transform.position.x < 6f) transform.Translate(Vector3.right * 0.1f, Space.World);
            if(_isHoldingArrow) Simulate();
        }

    }
    void Aiming()
    {
        _animator.SetBool(_animatorAiming, true);
        StartCoroutine(FireArrow());
    }

    void Shoot()
    {
        _animator.SetBool(_animatorAiming, false);
        ShootArrow();
    }

    void ReloadArrows()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            _recharging++;
            if (_recharging > 3) {
                _noArrowsLeft = false;
                _recharging = 0;
                DeactivateAllArrows();
            }
        }
    }

    void Simulate()
    {
        _simulationLaunchPosition = transform.TransformPoint(arrowOffsets);
        simulatedTr.gameObject.SetActive(true);
        simulatedTr.SimulateTr(transform.forward * arrowForce, _simulationLaunchPosition);
    }

    void InitPoolOfArrows()
    {
        _arrows = new List<Arrow>();

        for (int i = 0; i < 10; i++)
        {
            var arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            arrow.transform.SetParent(transform);
            arrow.SetActive(false);
            var arrowScript = arrow.GetComponent<Arrow>();
            _arrows.Add(arrowScript);
        }
    }

    private void GetArrowFromPool()
    {
        foreach (var arrow in _arrows)
        {
            if (!arrow.gameObject.activeInHierarchy)
            {
                arrow.gameObject.SetActive(true);
                arrow.transform.position = transform.TransformPoint(arrowOffsets);
                _currentArrowScript = arrow;
                _isHoldingArrow = true;
                return;
            }
        }

        _noArrowsLeft = true;
        _animator.SetBool(_animatorAiming, false);
    }

    private void DeactivateAllArrows()
    {
        foreach (var arrow in _arrows)
        {
            arrow.gameObject.SetActive(false);
            arrow.transform.SetParent(transform);
        }
    }

    private void ShootArrow()
    {
        if (_isHoldingArrow)
        {
            Vector3 direction = transform.forward;
            _currentArrowScript.Shoot(direction, arrowForce);
            _currentArrowScript.transform.SetParent(arrowParent.transform);
            _currentArrowScript = null;
            _isHoldingArrow = false;
        }
    }

    private void OnLowHealth()
    {
        onLowHealthVolume?.SetActive(true);    
        audioSource.clip = lowHealthClip;
        audioSource.Play();
    }
    IEnumerator FireArrow()
    {
        yield return new WaitForSeconds(0.2f);
        GetArrowFromPool();
    }

}
