using UnityEngine;

public interface IIgnitable
{
<<<<<<< HEAD
    static bool OnFire { get; }
    int MaxHealth { get; }
    int CurrentHealth { get; }
    int FireDamage { get; }
    bool CanBurn { get; }
    void StablishFireDamage(int fireDamage);
    void DealFireDamage();
    void PassFire();
    void ChangeStatus(bool onFire);
=======
    static bool OnFire { get; set; }
    int MaxHealth { get; }
    int CurrentHealth { get; }
    void FireDamage()
    {

    }
    void PassFire()
        {

    }
>>>>>>> origin/main
}
