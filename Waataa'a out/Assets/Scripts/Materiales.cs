using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}

public class Materiales : MonoBehaviour, IIgnitable, IExtinguishable
{
    [Header("Fire")]
    [SerializeField] private int _fireDamage;
    public int FireDamage 
    { 
        get { return _fireDamage; }
        set { _fireDamage = value; }
    }
    [SerializeField] private bool _onFire;
    public bool OnFire
    {
        get { return _onFire; }
        set { _onFire = value; }
    }
    [SerializeField] private bool _canBurn;
    public bool CanBurn
    {
        get { return _canBurn; }
        set { _canBurn = value; }
    }

    [Header("References")]
    private LayerMask _water;
    public LayerMask Water
    {
        get { return (LayerMask)_water; }
        set { _water = value; }
    }
    private float _timer = 0f;
    public int MaxHealth { get; } = 100;
    [SerializeField] private int _currentHealth = 100;
    public int CurrentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = value; }
    }
    private float _lifePercentage;
    private float _radio;
    private float _length;
    private Renderer _objetoRenderer;

    void Awake()
    {
        _currentHealth = MaxHealth;
        _lifePercentage = (_currentHealth * 100) / MaxHealth;
        _water = LayerMask.GetMask("Water");
    }
    void Start()
    {
        _objetoRenderer = GetComponent<Renderer>();
        _radio = _length + 1;
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if(_onFire && _timer >= 1)
        {
            DealFireDamage();
            _timer = 0f;
        }
        if (_currentHealth > 0 && _lifePercentage < 25f && _onFire)
        {
            PassFire();
        }
        ChangeSprite(_onFire);
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

            if(ignitable != null && ignitableTargets.Add(ignitable))
            {
                if (_canBurn)
                {
                    ignitable.ChangeStatus(true);
                }
            }
        }
    }

    public void ChangeStatus(bool onFire)
    {
        this.OnFire = onFire;
    }

    private void ChangeSprite(bool fire)
    {
        if(fire)
        {
            _objetoRenderer.material.color = Color.red;
        }
        else
        {
            _objetoRenderer.material.color = Color.green;
        }
    }

    public void StablishFireDamage(int fireDamage)
    {
        _fireDamage = fireDamage;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(this.transform.position, _radio);
    }
}
