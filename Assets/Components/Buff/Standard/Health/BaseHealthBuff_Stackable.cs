using System;
using Unity.Entities;

[Serializable]
public struct BaseHealthBuff_Stackable : IComponentData
{
    public Entity Target;
    public float DurationTimer;
    public float Value;

    public static BuffTypes BuffType = BuffTypes.ValueAdd;
    public static StackTypes StackType = StackTypes.Value;
    public static bool CanStack => StackType != 0;

    public Entity AppliedByEntity;

    public BaseHealthBuff_Stackable Stack(BaseHealthBuff_Stackable other)
    {
        if (StackType.HasFlag(StackTypes.Duration)) DurationTimer += other.DurationTimer;

        if (StackType.HasFlag(StackTypes.Value)) Value += other.Value;

        return this;
    }
}