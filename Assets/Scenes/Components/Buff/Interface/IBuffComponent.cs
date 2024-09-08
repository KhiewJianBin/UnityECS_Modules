using Unity.Entities;

/// <summary>
/// Base Buff Component
/// </summary>
public interface IBuffComponent : IComponentData
{
    public int PriorityQueue { get; set; }
    public float DurationTimer { get; set; }
    public float Value { get; set; }
}