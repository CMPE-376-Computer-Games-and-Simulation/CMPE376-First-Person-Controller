using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    
    [SerializeField]
    private InputActionAsset _inputActionAsset;
    
    [SerializeField]
    private InputActionReference _moveActionReference;
    [SerializeField]
    private InputActionReference _rotateActionReference;
    [SerializeField]
    private InputActionReference _jumpActionReference;
    
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _rotateSpeed;
    [SerializeField]
    private float _headSpeed;
    [SerializeField]
    private float _jumpSpeed;
    
    private InputAction _moveAction;
    private InputAction _rotateAction;
    private InputAction _jumpAction;
    
    private Vector2 _moveDirection;
    private Vector2 _rotateDirection;
    private float _jumpInput;
    
    private Rigidbody _rigidbody;

    private void OnEnable()
    {
        _inputActionAsset.Enable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        _rigidbody = GetComponent<Rigidbody>();
        _moveAction = _moveActionReference.action;
        _rotateAction = _rotateActionReference.action;
        _jumpAction = _jumpActionReference.action;
    }

    // Update is called once per frame
    void Update()
    {
         _moveDirection = _moveAction.ReadValue<Vector2>();
         Vector3 move = (transform.forward*_moveDirection.y  + transform.right*_moveDirection.x).normalized;
         transform.position += move * _moveSpeed * Time.deltaTime;

         _rotateDirection = _rotateAction.ReadValue<Vector2>();
         Quaternion rot = transform.localRotation;
         Vector3 eulerAngles = rot.eulerAngles;
         eulerAngles.y += _rotateSpeed * Time.deltaTime * _rotateDirection.x;  
         transform.localRotation = Quaternion.Euler(eulerAngles);
         
         Vector3 headAngles = _camera.transform.localRotation.eulerAngles;
         float currentAngle = headAngles.x;
         currentAngle = (currentAngle > 180) ? currentAngle - 360 : currentAngle;
         currentAngle -= _headSpeed * _rotateDirection.y * Time.deltaTime;
         currentAngle = Mathf.Clamp(currentAngle, -45, 30);
         _camera.transform.localRotation = Quaternion.Euler(new Vector3(currentAngle, headAngles.y, headAngles.z));

         _jumpInput = _jumpAction.ReadValue<float>();
         if (_jumpInput > 0)
         {
             _rigidbody.AddForce(Vector3.up * _jumpSpeed, ForceMode.Impulse);
         }

    }

    private void OnDestroy()
    {
        Disable();
    }

    private void OnApplicationQuit()
    {
        Disable();
    }

    private void OnDisable()
    {
        Disable();
    }
    
    private void Disable()
    {
        _inputActionAsset.Disable();
    }
}
