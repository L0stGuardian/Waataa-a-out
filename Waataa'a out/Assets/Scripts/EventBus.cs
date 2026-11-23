using UnityEngine;
using System;

public static class EventBus
{
    #region Events
    public static event Action<int> OnAddCountIgnitable;
    public static event Action<int> OnBurnedOut;
    public static event Action<int> OnExtinguished;
    #endregion

    #region EventCall
    public static void AddCountIgnitable(int quantity)
    {
        OnAddCountIgnitable?.Invoke(quantity);
    }

    public static void ExtinguishedFires(int quantity)
    {
        OnExtinguished?.Invoke(quantity);
    }

    public static void BurnedOutFires(int quantity)
    {
        OnBurnedOut?.Invoke(quantity);
    }
    #endregion
}
