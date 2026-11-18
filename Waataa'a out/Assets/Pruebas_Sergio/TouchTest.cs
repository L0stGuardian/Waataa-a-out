using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TouchTest : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private float _maximumJumpStrength = 5f;
    [Tooltip("Modifies the velocity at which the jump strength increases")]
    [SerializeField] private float _jumpStrengthModifier = 5;
    [SerializeField] private Rigidbody2D _playerRigidBody;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private InputActionAsset _playerInputActions;
    [SerializeField] private Collider2D _triggerColliderWater;
    [SerializeField] private Slider _jumpSlider;

    [Header("Movement")]
    private Vector2 _moveValue;
    [SerializeField] private float _jumpStrength = 6f;
    private bool _onGround => Physics2D.OverlapCircle(_overlapCenter.transform.position, _radius, _ground);

    [Header("OverlapCircle")]
    [SerializeField] private GameObject _overlapCenter; 
    [SerializeField] private LayerMask _ground; 
    [Tooltip("The size of the circle to analyse if is on ground")][SerializeField] private float _radius;  

    [Header("Screen")]
    private int _screenWidth = Screen.width;

    [Header("Rotation")]
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
    private float _targetRotationZ = 0.2f;

    private const float _jumpLeftDirection = -1f; 
    private const float _jumpRightDirection = 1f; 
    [SerializeField] private float _smoothRotationSpeed = 100f; 
    [SerializeField] private float _rotationSpeed = 100f; 
    [SerializeField] private float _maxRandomImbalance = 120f;


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
        _playerAnimator = GetComponent<Animator>();
        _playerRigidBody = GetComponent<Rigidbody2D>();

        _jumpSlider.minValue = 0f;
        _jumpSlider.maxValue = _maximumJumpStrength;
        _jumpSlider.interactable = false;
        _jumpSlider.value = 0;
        ActivateAndDeactivateSlider(false);

        _jumpStrength = 0f;
    }


    private void Update()
    {
        int halfScreen = _screenWidth / 2;  
        if (Touchscreen.current == null) { return; }
        if (!Touchscreen.current.primaryTouch.press.isPressed) { return; }
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue(); 

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(Touchscreen.current.primaryTouch.touchId.ReadValue()))
        {
            return;
        }

        if (Touchscreen.current.primaryTouch.press.isPressed && _onGround)
        {
            CalculateJumpStrength();
            ActivateAndDeactivateSlider(true);
            CalculateStrengthSlider();
        }


        if (Touchscreen.current.primaryTouch.press.wasReleasedThisFrame && touchPosition.x > 0 && touchPosition.x < halfScreen && _onGround)
        {
            Jump(_jumpLeftDirection);
            ActivateAndDeactivateSlider(false);
        }
        if (Touchscreen.current.primaryTouch.press.wasReleasedThisFrame && touchPosition.x >= halfScreen && touchPosition.x < _screenWidth && _onGround)
        {
            Jump(_jumpRightDirection);
            ActivateAndDeactivateSlider(false);
        }
        if (touchPosition.x > 0 && touchPosition.x < halfScreen && !_onGround) {_targetRotationZ += _rotationSpeed * Time.deltaTime;}

        if (touchPosition.x >= halfScreen && touchPosition.x < _screenWidth && !_onGround) {_targetRotationZ += -_rotationSpeed * Time.deltaTime;}

        if (_onGround) { _targetRotationZ = 0f; }

        Rotate();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            collision.isTrigger = false;
            collision.tag = "WaterInPlayer";
        }
    }

    private void OnTriggerStay2D(Collider2D collision) 
    {
        if (collision.CompareTag("Water")) { collision.tag = "WaterInPlayer";}
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("WaterInPlayer"))
        {
            collision.tag = "Water";
            collision.isTrigger = true;
        }
    }
    private void Jump(float direction = 0)
    {
        float targetHorizontalSpeed = _moveValue.x * _jumpStrength;
        _playerRigidBody.linearVelocity = new Vector2(targetHorizontalSpeed, 0f);
        _playerRigidBody.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Impulse);

        float imbalanceDirection;
        if (_moveValue.x > 0.01f) { imbalanceDirection = 1f; }

        else { imbalanceDirection = -1f; }

        float randomAngle = Random.Range(1f, _maxRandomImbalance);
        _targetRotationZ += (randomAngle * imbalanceDirection * _jumpStrength);
        _jumpStrength = 0f;

    }

    private void Rotate()
    {
        Quaternion rotation = Quaternion.Euler(0f, 0f, _targetRotationZ);

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, _smoothRotationSpeed * Time.deltaTime);
    }

    private void CalculateJumpStrength()
    {
        _jumpStrength += Time.deltaTime * _jumpStrengthModifier;
        _jumpStrength = Mathf.Clamp(_jumpStrength, 0, _maximumJumpStrength);
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
        Gizmos.DrawSphere(_overlapCenter.transform.position, _radius);
    }
}
