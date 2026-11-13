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

    public int bulletCount;
    
    [SerializeField]
    private float _fireRate;
    
    private float _lastFireTime;
    private bool _isFireActive = false;
    
    
    private BulletPool _bulletPool;
    
    private InputSystem_Actions _inputSystemActions;
    private InputAction _fireAction;
    
    public event Action FireEvent;
    public event Action CancelFireEvent;
    
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
        CancelFireEvent?.Invoke();
        StopCoroutine(FireCoroutine());
    }

    private void OnFire(InputAction.CallbackContext obj)
    {
       _isFireActive = true;
     
       StartCoroutine(FireCoroutine());
    }

    private IEnumerator FireCoroutine()
    {
        while (_isFireActive && bulletCount > 0)
        {
            if(bulletCount <= 0)
                yield return null;
            
            if (Time.time >= _lastFireTime + _fireRate)
            {
                FireEvent?.Invoke();
                _lastFireTime = Time.time;
                bulletCount--;
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
