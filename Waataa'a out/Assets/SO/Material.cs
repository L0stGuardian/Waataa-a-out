using UnityEngine;

[CreateAssetMenu(fileName = "Material", menuName = "Scriptable Objects/Material")]
public class Material : ScriptableObject
{
    public int MaxHealth { get; private set; }
    public int FireDamage { get; private set; }
    public int PercentageToIgnite { get; private set; }
    public float TimeToBurn { get; private set; }
}
