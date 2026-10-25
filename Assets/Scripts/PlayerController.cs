using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _rotateSpeed;
    [SerializeField]
    private float _headSpeed;
    [SerializeField]
    private float _jumpHeight;

    private InputSystem_Actions _inputSystemActions;
    
    private InputAction _moveAction;
    private InputAction _rotateAction;
    private InputAction _jumpAction;
    
    private Vector2 _moveDirection;
    private Vector2 _rotateDirection;
    private bool _isJump;
    
    private readonly float _gravity = -9.81f;
    private float _verticalVelocity;
    private Vector3 _moveVelocity;
    
    private CharacterController _characterController;

    private void Awake()
    {
        _inputSystemActions = new InputSystem_Actions();
        _moveAction = _inputSystemActions.Player.Move;
        _rotateAction = _inputSystemActions.Player.Look;
        _jumpAction = _inputSystemActions.Player.Jump;
    }

    void OnEnable()
    {
        _inputSystemActions.Enable();
        _characterController = GetComponent<CharacterController>();
        _moveAction.performed += OnMove;
        _moveAction.canceled += OnMoveCancel;
        _rotateAction.performed += OnRotateAround;
        _rotateAction.canceled += OnRotateAroundCancel;
        _jumpAction.performed += OnJump;
        Cursor.visible = false;
    }
    
    void Update()
    {
         MoveCharacter();
         JumpMove();
         ApplyMovement();
         RotateAround();
         RotateHead();
    }

    private void RotateHead()
    {
        Vector3 headAngles = _camera.transform.localRotation.eulerAngles;
        float currentAngle = headAngles.x;
        currentAngle = (currentAngle > 180) ? currentAngle - 360 : currentAngle;
        currentAngle -= _headSpeed * _rotateDirection.y * Time.deltaTime;
        currentAngle = Mathf.Clamp(currentAngle, -45, 30);
        _camera.transform.localRotation = Quaternion.Euler(new Vector3(currentAngle, headAngles.y, headAngles.z));
    }

    private void RotateAround()
    {
        Quaternion rot = transform.localRotation;
        Vector3 eulerAngles = rot.eulerAngles;
        eulerAngles.y += _rotateSpeed * Time.deltaTime * _rotateDirection.x;  
        transform.localRotation = Quaternion.Euler(eulerAngles);
    }

    private void MoveCharacter()
    {
        _moveVelocity = (transform.forward*_moveDirection.y  + transform.right*_moveDirection.x).normalized;
        _moveVelocity *= _moveSpeed;
    }

    private void JumpMove()
    {
        if (_characterController.isGrounded && _verticalVelocity < 0f)
        {
            _verticalVelocity = -2f;
        }

        if (_isJump && _characterController.isGrounded)
        {
            _verticalVelocity =
                Mathf.Sqrt(_jumpHeight * -2f * _gravity);

            _isJump = false;
        }

        _verticalVelocity += _gravity * Time.deltaTime;
        _moveVelocity.y = _verticalVelocity;
    }

    private void ApplyMovement()
    {
        _characterController.Move(_moveVelocity  * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
    }

    public void OnMoveCancel(InputAction.CallbackContext context)
    {
        _moveDirection = Vector2.zero;
    }
    
    public void OnRotateAround(InputAction.CallbackContext context)
    {
        _rotateDirection = context.ReadValue<Vector2>();
    }

    public void OnRotateAroundCancel(InputAction.CallbackContext context)
    {
        _rotateDirection = Vector2.zero;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        _isJump = true;
    }
    
    private void OnDisable()
    {
        if (_inputSystemActions == null)
            return;

        _moveAction.performed -= OnMove;
        _moveAction.canceled -= OnMoveCancel;

        _rotateAction.performed -= OnRotateAround;
        _rotateAction.canceled -= OnRotateAroundCancel;

        _jumpAction.performed -= OnJump;

        _inputSystemActions.Disable();
    }

    private void OnDestroy()
    {
        _inputSystemActions?.Dispose();
    }
}
