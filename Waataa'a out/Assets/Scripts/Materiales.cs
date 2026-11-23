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
    [SerializeField] private float _timerToBurnAgain;
    [SerializeField] private float _timeToBurnAgain = 5f;
    [SerializeField] private bool _isOnFire; 
    [SerializeField] private bool _canBurn;
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
    public int MaxHealth { get; private set; }
    [SerializeField] private int _currentHealth;

    [Tooltip("Limit at which the object will spread the fire")]
    [SerializeField] private int _percentageToIgnite = 25;

    [Tooltip("Ratio for the fire to deal damage")] 
    [SerializeField] private float _damageRatio = 1f;
    [SerializeField] private Material _material;
    

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
        MaxHealth = _material.MaxHealth;
        _fireDamage = _material.FireDamage;
        _percentageToIgnite = _material.PercentageToIgnite;
        _timeToBurnAgain = _material.TimeToBurn;

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

        if (_currentHealth == 0 && _actualFireState != FireState.Extinguished) { ChangeStatus(FireState.Extinguished); }

        if (_currentHealth > 0 && _lifePercentage <= _percentageToIgnite && _actualFireState == FireState.OnFire) { PassFire(); }

        if (_actualFireState == FireState.WaitingToBurnAgain) { _timerToBurnAgain += Time.deltaTime; }

        if (_timerToBurnAgain >= _timeToBurnAgain) 
        { 
            ChangeStatus(FireState.CanBurn); 
            _timerToBurnAgain = 0f;
        }

        ChangeSprite();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water") && _actualFireState == FireState.OnFire)
        {
            ChangeStatus(FireState.WaitingToBurnAgain);
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
            
            if (ignitable != null && ignitableTargets.Add(ignitable)) { if (ignitable.ActualFireState == FireState.CanBurn) { ignitable.ChangeStatus(FireState.OnFire); } }
        }
    }

    public void ChangeStatus(FireState fireState)
    {
        switch (fireState)
        {
            case FireState.OnFire:
                _actualFireState = FireState.OnFire;
                EventBus.ExtinguishedFires(-1);
                break;

            case FireState.WaitingToBurnAgain:
                _actualFireState = FireState.WaitingToBurnAgain;
                EventBus.ExtinguishedFires(1);
                break;

            case FireState.CanBurn:
                _actualFireState = FireState.CanBurn;
                break;

            case FireState.Extinguished:
                _actualFireState = FireState.Extinguished;
                EventBus.BurnedOutFires(1);
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
