using System.Collections;
using UnityEngine;

public interface IExtinguishable
{
    bool OnFire { get; }
    float TimerToBurn { get; }
    bool CanBurn { get; }
    IEnumerator TimeToBurn();
    void ChangeStatus(bool onFire);
}
