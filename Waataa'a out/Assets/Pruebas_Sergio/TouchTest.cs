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
    [SerializeField] private float _radio; // Respetar el orden de los miembros; Aclarar el significado o las unidades con Tooltip; Usar inglés
    [SerializeField] private GameObject _overlapCenter; // Respetar el orden de los miembros
    [SerializeField] private LayerMask _ground; // Respetar el orden de los miembros

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
        int _halfScreen = _screenWidth / 2; // Usar camelCase sin _
        if (Touchscreen.current == null) { return; }
        if (!Touchscreen.current.primaryTouch.press.isPressed) { return; }
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue(); 

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(Touchscreen.current.primaryTouch.touchId.ReadValue()))
        {
            return;
        }

        if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame && touchPosition.x > 0 && touchPosition.x < _halfScreen && _onGround)
        {
            JumpLeft();
        }
        if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame && touchPosition.x >= _halfScreen && touchPosition.x < _screenWidth && _onGround)
        {
            JumpRight();
        }
        if (touchPosition.x > 0 && touchPosition.x < _halfScreen && !_onGround)
        {
            _targetRotationZ += _rotationSpeed * Time.deltaTime;
        }
        if (touchPosition.x >= _halfScreen && touchPosition.x < _screenWidth && !_onGround)
        {
            _targetRotationZ += -_rotationSpeed * Time.deltaTime;
        }

        if (_onGround)
        {
            _targetRotationZ = 0f;
        }

        Rotate();
    }
    private void JumpRight()
    {
        _moveValue = new Vector2(1f, 0f);
        float targetHorizontalSpeed = _moveValue.x * _jumpStrength;
        _playerRigidBody.linearVelocity = new Vector2(targetHorizontalSpeed, 0f);
        _playerRigidBody.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Impulse);

        float imbalanceDirection = 1f; 


        float randomAngle = UnityEngine.Random.Range(1f, _maxRandomImbalance); 
        _targetRotationZ += (randomAngle * imbalanceDirection);
    }
    private void JumpLeft() // Revisar el código duplicado
    {
        _moveValue = new Vector2(-1f, 0f);
        float targetHorizontalSpeed = _moveValue.x * _jumpStrength;
        _playerRigidBody.linearVelocity = new Vector2(targetHorizontalSpeed, 0f);
        _playerRigidBody.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Impulse);

        float imbalanceDirection = -1f; 


        float randomAngle = UnityEngine.Random.Range(-1f, _maxRandomImbalance);
        _targetRotationZ += (randomAngle * imbalanceDirection);
    }

    private void Rotate()
    {
        Quaternion _rotation = Quaternion.Euler(0f, 0f, _targetRotationZ); // Usar camelCase sin _

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, _rotation, _smoothRotationSpeed * Time.deltaTime);
    }

    //Checks collision with the water and desactivates his trigger mode // Explicar el porqué, no el cómo
    private void OnTriggerEnter2D(Collider2D collision) // Respetar el orden de los miembros; Revisar el código duplicado
    {
        if (collision.CompareTag("Water"))
        {
            collision.isTrigger = false;
            collision.tag = "WaterInPlayer";
        }
    }

    //Checks collision with the water and ensures it stays as WaterInPlayer while in collision // Explicar el porqué, no el cómo
    private void OnTriggerStay2D(Collider2D collision) // Respetar el orden de los miembros
    {
        if (collision.CompareTag("Water"))
        {
            collision.tag = "WaterInPlayer";
        }
    }

    //Checks collision with water to make it trigger when it gets out of the player // Explicar el porqué, no el cómo
    //Also changes its tag to Water
    private void OnTriggerExit2D(Collider2D collision) // Respetar el orden de los miembros
    {
        if (collision.CompareTag("WaterInPlayer"))
        {
            collision.tag = "Water";
            collision.isTrigger = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_overlapCenter.transform.position, _radio);
    }
}
