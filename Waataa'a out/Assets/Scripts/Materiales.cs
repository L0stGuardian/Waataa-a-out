// Sugerencia general de Clean Code: usar if de una sola línea con llaves cuando sea legible.

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
    private float _timer = 0f;
    private LayerMask _water;
    public LayerMask Water
    {
        get { return (LayerMask)_water; }
        set { _water = value; }
    }

    [Header("References")]
    [SerializeField] private int _currentHealth; // Respetar el orden de los miembros
    public int MaxHealth { get; } = 100;
    public int CurrentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = value; }
    }
    private float _lifePercentage; // Respetar el orden de los miembros
    private float _radio; // Respetar el orden de los miembros; Aclarar el significado o las unidades con Tooltip;
    private float _length; // Respetar el orden de los miembros
    private Renderer _objectRenderer; // Respetar el orden de los miembros; Usar inglés

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

    void Awake()
    {
        _currentHealth = MaxHealth;
        _lifePercentage = (_currentHealth * 100) / MaxHealth;
        _water = LayerMask.GetMask("Water");
    }
    void Start()
    {
        _objectRenderer = GetComponent<Renderer>();
        _radio = _length + 1; // Aclarar el significado o las unidades con Tooltip
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_isOnFire && _timer >= 1) // Revisar: usar una constante o configuración con nombre
        {
            DealFireDamage();
            _timer = 0f;
        }
        if (_currentHealth > 0 && _lifePercentage < 25f && _isOnFire) // Revisar: usar una constante o configuración con nombre
        {
            PassFire();
        }
        ChangeSprite(_isOnFire);
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

            if (ignitable != null && ignitableTargets.Add(ignitable)) // Revisar si se puede usar retorno temprano
            {
                if (_canBurn)
                {
                    ignitable.ChangeStatus(true);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // Respetar el orden de los miembros
    {
        if (collision.CompareTag("Water") && CanBurn)
        {
            ChangeStatus(false);
            StartCoroutine(TimeToBurn());
        }
    }

    public IEnumerator TimeToBurn() // Usar un verbo en el nombre del método
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
        if (fire)
        {
            _objectRenderer.material.color = Color.red;
        }
        else
        {
            _objectRenderer.material.color = Color.green;
        }
    }

    public void EstablishFireDamage(int fireDamage) // Respetar el orden de acceso y static; Corregir la ortografía del nombre
    {
        _fireDamage = fireDamage;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(this.transform.position, _radio);
    }
}
