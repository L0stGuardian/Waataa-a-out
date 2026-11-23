using UnityEngine;
using System;

public static class EventBus
{
    #region Events
    public static event Action<int> OnAddCountIgnitable;
    #endregion

    #region EventCall
    public static void AddCountIgnitable(int quantity)
    {
        OnAddCountIgnitable?.Invoke(quantity);
    }
    #endregion
}
