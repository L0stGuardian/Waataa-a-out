using UnityEngine;
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}

public class Materiales : MonoBehaviour, IIgnitable
{
    [Header("Fire")]
    [SerializeField] private int _fireDamage;
    [SerializeField] private bool _onFire;
    public bool OnFire
    {
        get { return _onFire; }
        set { _onFire = value; }
    }

    [Header("References")]
    public int MaxHealth { get; } = 100;
    [SerializeField] private int _currentHealth;
    public int CurrentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = value; }
    }
    private float _lifePercentage => _currentHealth / MaxHealth * 100;
    private float _radio;
    private float _length;
    private Renderer _objetoRenderer;

    void Start()
    {
        _objetoRenderer = GetComponent<Renderer>();
        _radio = _length + 1;
        _currentHealth = MaxHealth;
    }

    void Update()
    {
        if(_onFire)
        {
            FireDamage();
        }
        if (_currentHealth > 0 && _lifePercentage < 25f && _onFire)
        {
            PassFire();
        }
    }

    private void FireDamage()
    {
        
        _currentHealth -= _fireDamage;
        _objetoRenderer.material.color = Color.red;
        Mathf.Clamp(_currentHealth, 0, MaxHealth);
    }

    private void PassFire()
    {
        Collider2D[] _objectsInRadio = Physics2D.OverlapCircleAll(this.transform.position, _radio);
        foreach (Collider2D collider in _objectsInRadio)
        {
            if(TryGetComponent<IIgnitable> (out IIgnitable valor))
            {
                IIgnitable.OnFire = true;
            }
            else
            {
                continue;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(this.transform.position, _radio);
    }
}
