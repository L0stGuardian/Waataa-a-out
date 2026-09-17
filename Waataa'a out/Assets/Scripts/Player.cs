using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionAsset _playerInputSystem;
    [SerializeField] private Rigidbody2D _playerRigidBody;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private InputActionAsset _playerInputActions;

    [Header("Movement")]
    private InputAction _jumpAction;
    [SerializeField] private float _jumpStrength;
    private InputAction _moveAction;
    private Vector2 _moveValue;
    private bool _onGround => Physics2D.OverlapCircle(_overlapCenter.transform.position, _radio, _ground);

    [Header("OverlapCircle")]
    [SerializeField] private float _radio;
    [SerializeField] private GameObject _overlapCenter;
    [SerializeField] private LayerMask _ground;

    private void OnEnable()
    {
        _playerInputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        _playerInputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _moveAction = InputSystem.actions.FindAction("Move");

        _playerAnimator = GetComponent<Animator>();
        _playerRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _moveValue = _moveAction.ReadValue<Vector2>();
        if(_jumpAction.WasPressedThisFrame() && _moveAction.IsPressed() && _onGround)
        {
            Jump();
        }

        if(_moveAction.IsPressed() && !_onGround)
        {

        }
    }
    
    private void Jump()
    {
        float targetHorizontalSpeed = _moveValue.x * _jumpStrength;
        _playerRigidBody.linearVelocity = new Vector2(targetHorizontalSpeed, 0f);

        _playerRigidBody.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Impulse);
        //_playerAnimator.SetBool("Jump", true);
    }

    private void Rotate()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_overlapCenter.transform.position, _radio);
    }
}
