using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TouchTest : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _playerRigidBody;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private InputActionAsset _playerInputActions;
    [SerializeField] private Collider2D _triggerColliderWater;
    
    [Header("Movement")]
    private Vector2 _moveValue;
    [SerializeField] private float _jumpStrength = 6f;
    private bool _onGround => Physics2D.OverlapCircle(_overlapCenter.transform.position, _radio, _ground);

    [Header("OverlapCircle")]
    [SerializeField] private GameObject _overlapCenter; 
    [SerializeField] private LayerMask _ground; 
    [Tooltip("The size of the circle to analyse if is on ground")][SerializeField] private float _radio; 

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
    [SerializeField] private float _smoothRotationSpeed = 100f; 
    [SerializeField] private float _rotationSpeed = 100f; 
    [SerializeField] private float _maxRandomImbalance = 120f;
    private const float _jumpLeftDirection = -1f;
    private const float _jumpRightDirection = 1f;


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
    }


    private void Update()
    {
        int HalfScreen = _screenWidth / 2; 
        if (Touchscreen.current == null) { return; }
        if (!Touchscreen.current.primaryTouch.press.isPressed) { return; }
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue(); 

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(Touchscreen.current.primaryTouch.touchId.ReadValue()))
        {
            return;
        }

        if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame && touchPosition.x > 0 && touchPosition.x < HalfScreen && _onGround)
        {
            Jump(_jumpLeftDirection);
        }
        if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame && touchPosition.x >= HalfScreen && touchPosition.x < _screenWidth && _onGround)
        {
            Jump(_jumpRightDirection);
        }
        if (touchPosition.x > 0 && touchPosition.x < HalfScreen && !_onGround)
        {
            _targetRotationZ += _rotationSpeed * Time.deltaTime;
        }
        if (touchPosition.x >= HalfScreen && touchPosition.x < _screenWidth && !_onGround)
        {
            _targetRotationZ += -_rotationSpeed * Time.deltaTime;
        }

        if (_onGround)
        {
            _targetRotationZ = 0f;
        }

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
        if (collision.CompareTag("Water"))
        {
            collision.tag = "WaterInPlayer";
        }
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
        _moveValue = new Vector2(direction, 0f);
        float targetHorizontalSpeed = _moveValue.x * _jumpStrength;
        _playerRigidBody.linearVelocity = new Vector2(targetHorizontalSpeed, 0f);
        _playerRigidBody.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Impulse);

        float imbalanceDirection = direction;


        float randomAngle = UnityEngine.Random.Range(1f, _maxRandomImbalance);
        _targetRotationZ += (randomAngle * imbalanceDirection);
    
    }

    private void Rotate()
    {
        Quaternion Rotation = Quaternion.Euler(0f, 0f, _targetRotationZ); 

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Rotation, _smoothRotationSpeed * Time.deltaTime);
    }

    
   

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_overlapCenter.transform.position, _radio);
    }
}
