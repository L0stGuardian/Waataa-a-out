using UnityEngine;

public interface IIgnitable
{
    static bool OnFire { get; set; }
    int MaxHealth { get; }
    int CurrentHealth { get; }
    int FireDamage { get; }
    bool CanBurn { get; }
    void StablishFireDamage(int fireDamage);
    void DealFireDamage();
    void PassFire();
    void ChangeStatus(bool onFire);
}
