using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireHandller : MonoBehaviour
{
    [SerializeField]
    private Transform _gunBarrel;
    [SerializeField]
    private GameObject _bulletPrefab;
    
    
    private InputSystem_Actions _inputSystemActions;
    private InputAction _fireAction;
    
    private void Awake()
    {
        _inputSystemActions = new InputSystem_Actions();
        _fireAction = _inputSystemActions.Player.Attack;
    }

    void OnEnable()
    {
        _inputSystemActions.Enable();
        _fireAction.performed += OnFire;
    }

    private void OnFire(InputAction.CallbackContext obj)
    {
        GameObject bullet = Instantiate(_bulletPrefab, _gunBarrel.position, _gunBarrel.rotation);
    }

    private void OnDisable()
    {
        if (_inputSystemActions == null)
            return;
        
        _fireAction.performed -= OnFire;
    }
    
    private void OnDestroy()
    {
        _inputSystemActions?.Dispose();
    }
}
