using Unity.Entities;

public struct EmptyBuffComponent : IComponentData
{
    public float DurationTimer;
    public float Value;
    public BuffTypes BuffType;
}