using UnityEngine;

public class Materiales : MonoBehaviour
{
    private bool _onFire;
    public bool OnFire
    {
        get { return _onFire; }
        set { _onFire = value; }
    }

    [SerializeField] private int _currentHealth;

    public int MaxHealth { get; }
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void FireDamage()
    {

    }
}
