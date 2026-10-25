using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireHandller : MonoBehaviour
{
    [SerializeField]
    private Transform _gunBarrel;

    [SerializeField] 
    private GameObject _bulletPoolObject;
    
    [SerializeField]
    private float _fireRate;
    
    private float _lastFireTime;
    private bool _isFireActive = false;
    
    
    private BulletPool _bulletPool;
    
    private InputSystem_Actions _inputSystemActions;
    private InputAction _fireAction;
    
    private void Awake()
    {
        _inputSystemActions = new InputSystem_Actions();
        _fireAction = _inputSystemActions.Player.Attack;
    }

    void OnEnable()
    {
        _bulletPool = _bulletPoolObject.GetComponent<BulletPool>();
        _inputSystemActions.Enable();
        _fireAction.performed += OnFire;
        _fireAction.canceled += OnFireCancel;
    }

    private void OnFireCancel(InputAction.CallbackContext obj)
    {
        _isFireActive = false;
        StopCoroutine(FireCoroutine());
    }

    private void OnFire(InputAction.CallbackContext obj)
    {
       _isFireActive = true;
       StartCoroutine(FireCoroutine());
    }

    private IEnumerator FireCoroutine()
    {
        while (_isFireActive)
        {
            if (Time.time >= _lastFireTime + _fireRate)
            {
                _lastFireTime = Time.time;
                _bulletPool.GetBullet(_gunBarrel.position, _gunBarrel.rotation);
            }
            yield return null; 
        }
    }


    private void OnDisable()
    {
        if (_inputSystemActions == null)
            return;
        
        _fireAction.performed -= OnFire;
        _fireAction.canceled -= OnFireCancel;
        _inputSystemActions.Disable();
    }
    
    private void OnDestroy()
    {
        _inputSystemActions?.Dispose();
    }
}
