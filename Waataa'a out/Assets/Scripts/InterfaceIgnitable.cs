using UnityEngine;

public interface IIgnitable
{
    enum FireStates { onFire, waitingToBurnAgain, canBurn, extinguished };
    FireStates ActualFireState { get; }
    int MaxHealth { get; }
    int CurrentHealth { get; }
    int FireDamage { get; }
    void EstablishFireDamage(int fireDamage);
    void DealFireDamage();
    void PassFire();
    void ChangeStatus(string fireState);
}
