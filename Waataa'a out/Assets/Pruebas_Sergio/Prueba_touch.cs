// Sugerencia general de Clean Code: usar if de una sola línea con llaves cuando sea legible.

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Prueba_touch : MonoBehaviour // Usar PascalCase; Usar inglés
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _playerRigidBody;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private InputActionAsset _playerInputActions;
    [SerializeField] private Collider2D _triggerColliderWater;

    [Header("Movement")]
    [SerializeField] private float _jumpStrength = 6f;
    private bool _onGround => Physics2D.OverlapCircle(_overlapCenter.transform.position, _radio, _ground);
    private Vector2 _moveValue; // Respetar el orden de los miembros


    [Header("Rotation")]
    [SerializeField] private float _maxRandomImbalance = 120f; // Respetar el orden de los miembros
    [SerializeField] private float _smoothRotationSpeed = 100f; // Respetar el orden de los miembros
    [SerializeField] private float _rotationSpeed = 100f; // Respetar el orden de los miembros

    private float _targetRotationZ = 0.2f; // Respetar el orden de los miembros

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

    private int _screenWidth = Screen.width; // Respetar el orden de los miembros
    [Header("OverlapCircle")]
    [SerializeField] private float _radio; // Respetar el orden de los miembros; Aclarar el significado o las unidades con Tooltip; Usar inglés
    [SerializeField] private GameObject _overlapCenter; // Respetar el orden de los miembros
    [SerializeField] private LayerMask _ground; // Respetar el orden de los miembros

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
        Vector2 posicionToque = Touchscreen.current.primaryTouch.position.ReadValue(); // Usar inglés

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(Touchscreen.current.primaryTouch.touchId.ReadValue()))
        {
            return;
        }

        if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame && posicionToque.x > 0 && posicionToque.x < _halfScreen && _onGround)
        {
            JumpLeft();
        }
        if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame && posicionToque.x >= _halfScreen && posicionToque.x < _screenWidth && _onGround)
        {
            JumpRight();
        }
        if (posicionToque.x > 0 && posicionToque.x < _halfScreen && !_onGround)
        {
            _targetRotationZ += _rotationSpeed * Time.deltaTime;
        }
        if (posicionToque.x >= _halfScreen && posicionToque.x < _screenWidth && !_onGround)
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
        //_playerAnimator.SetBool("Jump", true);

        float _imbalanceDirection = 1f; // Usar camelCase sin _


        float _randomAngle = UnityEngine.Random.Range(1f, _maxRandomImbalance); // Usar camelCase sin _
        _targetRotationZ += (_randomAngle * _imbalanceDirection);
    }
    private void JumpLeft() // Revisar el código duplicado
    {
        _moveValue = new Vector2(-1f, 0f);
        float targetHorizontalSpeed = _moveValue.x * _jumpStrength;
        _playerRigidBody.linearVelocity = new Vector2(targetHorizontalSpeed, 0f);
        _playerRigidBody.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Impulse);
        //_playerAnimator.SetBool("Jump", true);

        float _imbalanceDirection = -1f; // Usar camelCase sin _


        float _randomAngle = UnityEngine.Random.Range(-1f, _maxRandomImbalance); // Usar camelCase sin _
        _targetRotationZ += (_randomAngle * _imbalanceDirection);
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
