using UnityEngine;

public interface IIgnitable
{
    enum FireStates { onFire, waitingToBurnAgain, canBurn, extinguished }; // Mover el enum público a su propio archivo | Usar un sustantivo singular para el enum | Usar PascalCase en onFire | Usar PascalCase en waitingToBurnAgain | Usar PascalCase en canBurn | Usar PascalCase en extinguished
    FireStates ActualFireState { get; }
    int MaxHealth { get; }
    int CurrentHealth { get; }
    int FireDamage { get; }
    void EstablishFireDamage(int fireDamage);
    void DealFireDamage();
    void PassFire();
    void ChangeStatus(string fireState);
}
