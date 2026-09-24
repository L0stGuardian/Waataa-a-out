using UnityEngine;

public interface IIgnitable
{
    static bool OnFire { get; }
    int MaxHealth { get; }
    int CurrentHealth { get; }
    void FireDamage();
    void PassFire();
    void ChangeStatus(bool onFire);
}
