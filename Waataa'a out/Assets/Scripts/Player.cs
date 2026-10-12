using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _playerRigidBody;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private InputActionAsset _playerInputActions;
    [SerializeField] private Collider2D _triggerColliderWater;

    [Header("Movement")]
    [SerializeField] private float _jumpStrength;

    private InputAction _jumpAction;
    private InputAction _moveAction;

    private Vector2 _moveValue;

    private bool _onGround => Physics2D.OverlapCircle(_overlapCenter.transform.position, _radio, _ground);


    [Header("Rotation")]
    private float _targetRotationZ = 0f;

    [SerializeField] private float _maxRandomImbalance = 90f; 
    [SerializeField] private float _smoothRotationSpeed = 50f; 
    [SerializeField] private float _rotationSpeed = 50f; 
    public float MaxRandomImbalance
    {
        get { return _maxRandomImbalance; }
        set { _maxRandomImbalance = value; }
    }
    public float RotationSpeed
    {
        get { return _rotationSpeed; }
        set { _rotationSpeed = value; }
    }


    [Header("OverlapCircle")]
    [SerializeField] private GameObject _overlapCenter; 

    [Tooltip ("Radio of the collision check")] [SerializeField] private float _radio;
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
        if (_jumpAction.WasPressedThisFrame() && _moveAction.IsPressed() && _onGround) { Jump(); }

        if (_moveAction.IsPressed() && !_onGround) { _targetRotationZ += _moveValue.x * _rotationSpeed * Time.deltaTime; }

        if (_onGround) { _targetRotationZ = 0f; }

        Rotate();
    }
    //Checks collision with the water and desactivates his trigger mode // Explicar el porqué, no el cómo
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            collision.isTrigger = false;
            collision.tag = "WaterInPlayer";
        }
    }

    //Checks collision with the water and ensures it stays as WaterInPlayer while in collision // Explicar el porqué, no el cómo
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Water")) { collision.tag = "WaterInPlayer"; }
    }

    //Checks collision with water to make it trigger when it gets out of the player // Explicar el porqué, no el cómo
    //Also changes its tag to Water
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("WaterInPlayer"))
        {
            collision.tag = "Water";
            collision.isTrigger = true;
        }
    }

    private void Jump()
    {
        float targetHorizontalSpeed = _moveValue.x * _jumpStrength;
        _playerRigidBody.linearVelocity = new Vector2(targetHorizontalSpeed, 0f);
        _playerRigidBody.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Impulse);

        float imbalanceDirection;
        if (_moveValue.x > 0.01f) { imbalanceDirection = 1f; }

        else { imbalanceDirection = -1f; }

        float randomAngle = UnityEngine.Random.Range(1f, _maxRandomImbalance);
        _targetRotationZ += (randomAngle * imbalanceDirection);
    }

    private void Rotate()
    {
        Quaternion rotation = Quaternion.Euler(0f, 0f, _targetRotationZ);

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, _smoothRotationSpeed  * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_overlapCenter.transform.position, _radio);
    }
}
