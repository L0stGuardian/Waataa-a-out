using UnityEngine;

public class IIgnitableCount : MonoBehaviour
{
    [Header ("References")]
    public static IIgnitableCount Instance;

    [Header("Variables")]
    public int IignitableCount { get; private set; } = 0;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else if (Instance != this) { Destroy(this.gameObject); }
        IignitableCount = 0;
    }

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