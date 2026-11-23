using UnityEngine;

public class IIgnitableCount : MonoBehaviour
{
    public static IIgnitableCount Instance;

    public int IignitableCount { get; private set; } = 0;

    private void OnEnable()
    {
        EventBus.OnAddCountIgnitable += IgnitableCount;
    }

    private void OnDisable()
    {
        EventBus.OnAddCountIgnitable -= IgnitableCount;
    }

    private void IgnitableCount(int quantity)
    {
        IignitableCount += quantity;
    }
}