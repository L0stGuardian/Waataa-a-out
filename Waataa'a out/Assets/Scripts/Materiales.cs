using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
public class Materials : MonoBehaviour, IIgnitable
{
    [Header("Fire")]
    [SerializeField] private int _fireDamage;
    [SerializeField] private bool _isOnFire;
    [SerializeField] private bool _canBurn;
    [SerializeField] private float _timerToBurn;
    public bool OnFire
    {
        get { return _isOnFire; }
        set { _isOnFire = value; }
    }
    public bool CanBurn
    {
        get { return _canBurn; }
        set { _canBurn = value; }
    }
    public float TimerToBurn
    {
        get { return _timerToBurn; }
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
        if (_isOnFire && _timer >= _damageRatio)
        {
            DealFireDamage();
            _timer = 0f;
        }

        if (_currentHealth > 0 && _lifePercentage < _percentageToBurn && _isOnFire) { PassFire(); }

        ChangeSprite(_isOnFire);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water") && CanBurn)
        {
            ChangeStatus(false);
            StartCoroutine(WaitTimeToBurn());
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

            if (ignitable != null && ignitableTargets.Add(ignitable)) { if (_canBurn) { ignitable.ChangeStatus(true); } }
        }
    }


    public IEnumerator WaitTimeToBurn()
    {
        _canBurn = false;
        yield return new WaitForSeconds(_timerToBurn);
        _canBurn = true;
    }    

    public void ChangeStatus(bool onFire)
    {
        this.OnFire = onFire;
    }

    private void ChangeSprite(bool fire)
    {
        if (fire) { _objectRenderer.material.color = Color.red; }

        else { _objectRenderer.material.color = Color.green; }
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
