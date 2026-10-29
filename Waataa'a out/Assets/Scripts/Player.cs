using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private float _maxJumpStrength = 5f;
    [Tooltip("Modifies the velocity at which the jump strength increases")]
    [SerializeField] private float _jumpStrengthModifier = 5;
    [SerializeField] private Rigidbody2D _playerRigidBody;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private InputActionAsset _playerInputActions;
    [SerializeField] private Collider2D _triggerColliderWater;
    [SerializeField] private Slider _jumpSlider;

    [Header("Movement")]
    [SerializeField] private float _jumpStrength = 0f   ;

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

        _jumpSlider.minValue = 0f;
        _jumpSlider.maxValue = _maxJumpStrength;
        _jumpSlider.interactable = false;
        _jumpSlider.value = 0;
        ActivateAndDeactivateSlider(false);

        _jumpStrength = 0f;
    }

    private void Update()
    {
        _moveValue = _moveAction.ReadValue<Vector2>();

        if (_jumpAction.IsPressed() && _onGround) 
        {
            CalculateJumpStrength();
            ActivateAndDeactivateSlider(true);
            CalculateStrengthSlider();
        }

        if (_jumpAction.WasReleasedThisFrame() && _onGround) 
        { 
            Jump();
            ActivateAndDeactivateSlider(false);
        }

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
        _jumpStrength = 0f;
    }

    private void Rotate()
    {
        Quaternion rotation = Quaternion.Euler(0f, 0f, _targetRotationZ);

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, _smoothRotationSpeed  * Time.deltaTime);
    }

    private void CalculateJumpStrength()
    {
            _jumpStrength += Time.deltaTime * _jumpStrengthModifier;
            _jumpStrength = Mathf.Clamp(_jumpStrength, 0, _maxJumpStrength);
    }
    
    private void CalculateStrengthSlider()
    {
        _jumpSlider.value = _jumpStrength;
    }

    private void ActivateAndDeactivateSlider(bool isActive)
    {
        _jumpSlider.gameObject.SetActive(isActive);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_overlapCenter.transform.position, _radio);
    }
}
