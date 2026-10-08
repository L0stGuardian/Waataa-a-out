<<<<<<< HEAD
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections;
=======
using UnityEngine;
>>>>>>> origin/main
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}

<<<<<<< HEAD
public class Materiales : MonoBehaviour, IIgnitable, IExtinguishable
{
    [Header("Fire")]
    [SerializeField] private int _fireDamage;
    public int FireDamage 
    { 
        get { return _fireDamage; }
        set { _fireDamage = value; }
    }
=======
public class Materiales : MonoBehaviour, IIgnitable
{
    [Header("Fire")]
    [SerializeField] private int _fireDamage;
>>>>>>> origin/main
    [SerializeField] private bool _onFire;
    public bool OnFire
    {
        get { return _onFire; }
        set { _onFire = value; }
    }
<<<<<<< HEAD
    [SerializeField] private bool _canBurn;
    public bool CanBurn
    {
        get { return _canBurn; }
        set { _canBurn = value; }
    }
    [SerializeField] private float _timerToBurn;
    public float TimerToBurn
    {
        get { return _timerToBurn; }
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
=======

    [Header("References")]
    public int MaxHealth { get; } = 100;
    [SerializeField] private int _currentHealth;
>>>>>>> origin/main
    public int CurrentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = value; }
    }
<<<<<<< HEAD
    private float _lifePercentage;
=======
    private float _lifePercentage => _currentHealth / MaxHealth * 100;
>>>>>>> origin/main
    private float _radio;
    private float _length;
    private Renderer _objetoRenderer;

<<<<<<< HEAD
    void Awake()
    {
        _currentHealth = MaxHealth;
        _lifePercentage = (_currentHealth * 100) / MaxHealth;
        _water = LayerMask.GetMask("Water");
    }
=======
>>>>>>> origin/main
    void Start()
    {
        _objetoRenderer = GetComponent<Renderer>();
        _radio = _length + 1;
<<<<<<< HEAD
=======
        _currentHealth = MaxHealth;
>>>>>>> origin/main
    }

    void Update()
    {
<<<<<<< HEAD
        _timer += Time.deltaTime;
        if(_onFire && _timer >= 1)
        {
            DealFireDamage();
            _timer = 0f;
=======
        if(_onFire)
        {
            FireDamage();
>>>>>>> origin/main
        }
        if (_currentHealth > 0 && _lifePercentage < 25f && _onFire)
        {
            PassFire();
        }
<<<<<<< HEAD
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
=======
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
>>>>>>> origin/main
            }
        }
    }

<<<<<<< HEAD
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water") && CanBurn)
        {
            ChangeStatus(false);
            StartCoroutine(TimeToBurn());
        }
    }

    public IEnumerator TimeToBurn()
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

=======
>>>>>>> origin/main
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(this.transform.position, _radio);
    }
}
