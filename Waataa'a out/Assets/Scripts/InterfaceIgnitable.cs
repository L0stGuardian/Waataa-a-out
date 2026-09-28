using UnityEngine;

public interface IIgnitable
{
    static bool OnFire { get; set; }
    int MaxHealth { get; }
    int CurrentHealth { get; }
    void FireDamage()
    {

    }
    void PassFire()
        {

    }
}
