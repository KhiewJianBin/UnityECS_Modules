using Unity.Entities;

public struct HealthBuff : IComponentData
{
    public Entity Target;

    public float DurationTimer;
    public float Value;
    public BuffTypes BuffType;
}