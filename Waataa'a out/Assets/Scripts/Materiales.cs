// Sugerencia general de Clean Code: usar if de una sola línea con llaves cuando sea legible.

using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class Materials : MonoBehaviour, IIgnitable
{
    [SerializeField] private FireState _actualFireState = FireState.CanBurn;
    public FireState ActualFireState
    {
        get { return _actualFireState; }
        set {  _actualFireState = value; }
    }

    [Header("Fire")]
    [SerializeField] private int _fireDamage;
    [SerializeField] private bool _isOnFire; // Respetar el orden de los miembros
    [SerializeField] private bool _canBurn;
    [SerializeField] private float _timerToBurnAgain; // Respetar el orden de los miembros
    [SerializeField] private float _timeToBurnAgain = 5f; // Respetar el orden de los miembros
    public float TimerToBurn
    {
        get { return _timerToBurnAgain; }
    }
    public int FireDamage
    {
        get { return _fireDamage; }
        set { _fireDamage = value; }
    }

    [Header("References")]
    public int MaxHealth { get; } = 100;
    [SerializeField] private int _currentHealth;

    [Tooltip("Limit at which the object will spread the fire")]
    [SerializeField] private int _percentageToBurn = 25;

    [Tooltip("Ratio for the fire to deal damage")] 
    [SerializeField] private float _damageRatio = 1f; 

    private float _timer = 0f;
    private float _lifePercentage;
    private LayerMask _water;
    public LayerMask Water
    {
        get { return (LayerMask)_water; }
        set { _water = value; }
    }

    [Header("Settings")]

    [Tooltip ("Radio of the afected area to burn")] 
    [SerializeField] private float _radius;
    private Renderer _objectRenderer; 
    public int CurrentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = value; }
    }
    void Awake()
    {
        _currentHealth = MaxHealth;
        _lifePercentage = (_currentHealth * 100) / MaxHealth;
        _water = LayerMask.GetMask("Water");
    }
    void Start()
    {
        _objectRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_actualFireState == FireState.OnFire && _timer >= _damageRatio)
        {
            DealFireDamage();
            _timer = 0f;
        }

        if (_currentHealth == 0 && _actualFireState != FireState.Extinguished) { ChangeStatus("extinguished"); }

        if (_currentHealth > 0 && _lifePercentage <= _percentageToBurn && _actualFireState == FireState.OnFire) { PassFire(); }

        if (_actualFireState == FireState.WaitingToBurnAgain) { _timerToBurnAgain += Time.deltaTime; }

        if (_timerToBurnAgain >= _timeToBurnAgain) 
        { 
            ChangeStatus("canburn"); 
            _timerToBurnAgain = 0f;
        }

        ChangeSprite();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water") && _actualFireState == FireState.OnFire)
        {
            ChangeStatus("waiting");
        }
    }

    public void DealFireDamage()
    {
        
        _currentHealth -= _fireDamage;
        _lifePercentage = (_currentHealth * 100) / MaxHealth;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, MaxHealth);
    }

    public void PassFire()
    {
        Collider2D[] objectsInRadio = Physics2D.OverlapCircleAll(this.transform.position, _radius); 
        HashSet<IIgnitable> ignitableTargets = new HashSet<IIgnitable>();
        foreach (Collider2D collider in objectsInRadio)
        {
            IIgnitable ignitable = collider.GetComponentInParent<IIgnitable>();

            if (ReferenceEquals(ignitable, this)) { continue; }
            
            if (ignitable != null && ignitableTargets.Add(ignitable)) { if (ignitable.ActualFireState == FireState.CanBurn) { ignitable.ChangeStatus("onfire"); } }
        }
    }

    public void ChangeStatus(string fireState)
    {
        Debug.Log(fireState);
        switch (fireState)
        {
            case "onfire":
                _actualFireState = FireState.OnFire;
                break;

            case "waiting":
                _actualFireState = FireState.WaitingToBurnAgain;
                break;

            case "canburn":
                _actualFireState = FireState.CanBurn;
                break;

            case "extinguished":
                _actualFireState = FireState.Extinguished;
                break;

            default:
                break;
        }
    }

    private void ChangeSprite()
    {
        switch (_actualFireState)
        {
            case FireState.OnFire:
                _objectRenderer.material.color = Color.red;
                break;

            case FireState.WaitingToBurnAgain:
                _objectRenderer.material.color = Color.blue;
                break;

            case FireState.CanBurn:
                _objectRenderer.material.color = Color.green;
                break;

            case FireState.Extinguished:
                _objectRenderer.material.color = Color.gray;
                break;

            default:
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, _radius);
    }
}
