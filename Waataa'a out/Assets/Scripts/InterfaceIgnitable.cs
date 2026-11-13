using UnityEngine;

public interface IIgnitable
{
    FireState ActualFireState { get; }
    int MaxHealth { get; }
    int CurrentHealth { get; }
    int FireDamage { get; }
    void DealFireDamage();
    void PassFire();
    void ChangeStatus(string fireState);
}
