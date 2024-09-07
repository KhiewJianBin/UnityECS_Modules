using Unity.Entities;

/// <summary>
/// Base Buff Component
/// </summary>
public struct BuffComponent : 
    IComponentData // Data Component
{
    public int PriorityQueue;
    public float DurationTimer;
    public float Value;

    public BuffComponent(int PriorityQueue, float DurationTimer, float Value)
    {
        this.PriorityQueue = PriorityQueue;
        this.DurationTimer = DurationTimer;
        this.Value = Value;
    }
}