using Unity.Entities;

public struct BaseHealthBuff : IComponentData
{
    public Entity Target;

    public float DurationTimer;
    public float Value;
    public BuffTypes BuffType;
}