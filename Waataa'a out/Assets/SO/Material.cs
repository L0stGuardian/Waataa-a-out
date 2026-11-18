using UnityEngine;

[CreateAssetMenu(fileName = "Material", menuName = "Scriptable Objects/Material")]
public class Material : ScriptableObject
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _fireDamage;
    [SerializeField] private int _percentageToIgnite;
    [SerializeField] private float _timeToBurn;

    public int MaxHealth { get { return _maxHealth; } }
    public int FireDamage { get { return _fireDamage; } }
    public int PercentageToIgnite { get { return _percentageToIgnite; } }
    public float TimeToBurn { get { return _timeToBurn; } }
}
