using UnityEngine;

public interface IIgnitable
{
    bool _onFire { get; }
    int MaxHealth { get; }
    int _actualHealth { get; }
    void OnFire()
    {

    }
    void PassFire()
    {

    }
}
