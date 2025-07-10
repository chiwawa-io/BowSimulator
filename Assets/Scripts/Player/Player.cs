using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private GameObject onLowHealthVolume;
    [SerializeField] private GameObject onRechargingVolume;
    [SerializeField] private AudioClip lowHealthClip;
    [SerializeField] private AudioClip normalStateClip;
    [SerializeField] private AudioSource audioSource;
    
    [SerializeField] private SimulateTrajectory simulatedTr;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject arrowParent;
    [SerializeField] private Vector3 arrowOffsets;
    [SerializeField] private float arrowForce;
    [SerializeField] private float mouseSensitivity;
    private List<Arrow> _arrows;
    private Arrow _currentArrowScript;
    private bool _isHoldingArrow;

    private float _mouseInputX;
    private float _movementX;
    private float _movementY;
    private Vector3 _simulationLaunchPosition;

    private bool _noArrowsLeft;
    private bool _shootPressed;
    private bool _aimPressed;
    private bool _isPaused;
    private int _recharging;
    
    private string _animatorAiming = "Aiming";

    public static Action<int> onOutOfArrows;
    
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(0.2f);
    private void OnEnable()
    {
        GameManager.OnLowHealth += OnLowHealth;
        GameManager.OnPause += OnPause;
        GameManager.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameManager.OnLowHealth -= OnLowHealth;
        GameManager.OnPause -= OnPause;
        GameManager.OnGameOver -= OnGameOver;
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
        if (!_isPaused)
        {
            Movement();

            _aimPressed = InputManager.InputActions.Player.Aim.WasPressedThisFrame();
            if (_aimPressed && !_isHoldingArrow && !_noArrowsLeft)
            {
                Aiming();

            }

            _shootPressed = InputManager.InputActions.Player.Shoot.WasPressedThisFrame();
            if (_shootPressed && _isHoldingArrow && !_noArrowsLeft)
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
    }

    void Movement()
    {
        _mouseInputX = InputManager.InputActions.Player.Horizontal.ReadValue<float>();
        if (_mouseInputX != 0)
        {
            Vector3 rotationX = transform.rotation.eulerAngles;
            rotationX.y += _mouseInputX * mouseSensitivity;
            transform.rotation = Quaternion.Euler(rotationX);
            if (_isHoldingArrow) Simulate();

        }
        
        _movementY = InputManager.InputActions.Player.Vertical.ReadValue<float>();
        if (_movementY > 0)
        {
            Vector3 rotationY = transform.rotation.eulerAngles;
            rotationY.x -= 0.2f;
            transform.rotation = Quaternion.Euler(rotationY);
            if (_isHoldingArrow) Simulate();
        }
        if (_movementY < 0)
        {
            Vector3 rotationY = transform.rotation.eulerAngles;
            rotationY.x += 0.2f;
            transform.rotation = Quaternion.Euler(rotationY);
            if (_isHoldingArrow) Simulate();
        }

        _movementX = InputManager.InputActions.Player.Movement.ReadValue<float>();
        if (_movementX < 0)
        {
            if (transform.position.x > -6f) transform.Translate(Vector3.left * 0.1f, Space.World);
            if (_isHoldingArrow) Simulate();
        }
        if (_movementX > 0)
        {
            if (transform.position.x < 6f) transform.Translate(Vector3.right * 0.1f, Space.World);
            if(_isHoldingArrow) Simulate();
        }

    }
    void Aiming()
    {
        _animator.SetBool(_animatorAiming, true);
        StartCoroutine(WaitTimeRoutine(1));
    }

    void Shoot()
    {
        _animator.SetBool(_animatorAiming, false);
        ShootArrow();
    }

    void ReloadArrows()
    {
        onOutOfArrows?.Invoke(0);
        onRechargingVolume.SetActive(true);
        audioSource.pitch = Time.timeScale;
        if (InputManager.InputActions.Player.Reload.WasPressedThisFrame()) {
            _recharging++;
            if (_recharging > 3) {
                _noArrowsLeft = false;
                _recharging = 0;
                onOutOfArrows?.Invoke(1);
                onRechargingVolume.SetActive(false);
                DeactivateAllArrows();
                StartCoroutine(WaitTimeRoutine(2));
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

    private void OnLowHealth(int id)
    {
        switch (id)
        {
            case 0:
                onLowHealthVolume?.SetActive(true);    
                audioSource.clip = lowHealthClip;
                audioSource.Play();
                break;
            case 1:
                onLowHealthVolume?.SetActive(false);
                audioSource.clip = normalStateClip;
                audioSource.Play();
                break;
            default:
                break;
        }
    }

    private void OnPause(bool pause)
    {
        _isPaused = pause;
        if (pause) audioSource.Pause();
        else audioSource.Play();
    }

    private void OnGameOver()
    {
        OnPause(true);
    }

    IEnumerator WaitTimeRoutine(int id)
    {
        yield return _waitForSeconds;
        if (id ==1 ) GetArrowFromPool();
        else audioSource.pitch = Time.timeScale;
    }

}
