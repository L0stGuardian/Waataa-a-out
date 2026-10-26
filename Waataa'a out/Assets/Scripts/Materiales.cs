using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEditor.Search;
public class Materials : MonoBehaviour, IIgnitable
{
    [SerializeField] private IIgnitable.FireStates _actualFireState = IIgnitable.FireStates.canBurn;
    public IIgnitable.FireStates ActualFireState
    {
        get { return _actualFireState; }
        set {  _actualFireState = value; }
    }

    [Header("Fire")]
    [SerializeField] private int _fireDamage;
    [SerializeField] private bool _isOnFire;
    [SerializeField] private bool _canBurn;
    [SerializeField] private float _timerToBurnAgain;
    [SerializeField] private float _timeToBurnAgain = 5f;
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
    [Tooltip("Ratio for the fire to deal damage")] 
    [SerializeField] private float _damageRatio = 1f;
    [Tooltip("Limit at which the object will spread the fire")]
    [SerializeField] private int _percentageToBurn = 25;
    [SerializeField] private int _currentHealth;

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
    [SerializeField] private float _radio;
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
        if (_actualFireState == IIgnitable.FireStates.onFire && _timer >= _damageRatio)
        {
            DealFireDamage();
            _timer = 0f;
        }

        if (_currentHealth == 0 && _actualFireState != IIgnitable.FireStates.extinguished) { ChangeStatus("extinguished"); }

        if (_currentHealth > 0 && _lifePercentage <= _percentageToBurn && _actualFireState == IIgnitable.FireStates.onFire) { PassFire(); }

        if (_actualFireState == IIgnitable.FireStates.waitingToBurnAgain) { _timerToBurnAgain += Time.deltaTime; }

        if (_timerToBurnAgain >= _timeToBurnAgain) 
        { 
            ChangeStatus("canburn"); 
            _timerToBurnAgain = 0f;
        }

        ChangeSprite();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water") && _actualFireState == IIgnitable.FireStates.onFire)
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
        Collider2D[] objectsInRadio = Physics2D.OverlapCircleAll(this.transform.position, _radio); 
        HashSet<IIgnitable> ignitableTargets = new HashSet<IIgnitable>();
        foreach (Collider2D collider in objectsInRadio)
        {
            IIgnitable ignitable = collider.GetComponentInParent<IIgnitable>();

            if (ReferenceEquals(ignitable, this)) { continue; }
            
            if (ignitable != null && ignitableTargets.Add(ignitable)) { if (ignitable.ActualFireState == IIgnitable.FireStates.canBurn) { ignitable.ChangeStatus("onfire"); } }
        }
    }

    public void ChangeStatus(string fireState)
    {
        Debug.Log(fireState);
        switch (fireState)
        {
            case "onfire":
                _actualFireState = IIgnitable.FireStates.onFire;
                break;

            case "waiting":
                _actualFireState = IIgnitable.FireStates.waitingToBurnAgain;
                break;

            case "canburn":
                _actualFireState = IIgnitable.FireStates.canBurn;
                break;

            case "extinguished":
                _actualFireState = IIgnitable.FireStates.extinguished;
                break;

            default:
                break;
        }
    }

    private void ChangeSprite()
    {
        switch (_actualFireState)
        {
            case IIgnitable.FireStates.onFire:
                _objectRenderer.material.color = Color.red;
                break;

            case IIgnitable.FireStates.waitingToBurnAgain:
                _objectRenderer.material.color = Color.blue;
                break;

            case IIgnitable.FireStates.canBurn:
                _objectRenderer.material.color = Color.green;
                break;

            case IIgnitable.FireStates.extinguished:
                _objectRenderer.material.color = Color.gray;
                break;

            default:
                break;
        }
    }

    public void EstablishFireDamage(int fireDamage)
    {
        _fireDamage = fireDamage;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(this.transform.position, _radio);
    }
}
