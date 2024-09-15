using System;
using Unity.Entities;

[Serializable]
public struct BaseHealthBuff : IComponentData
{
    public Entity Target;

    public float DurationTimer;
    public float Value;
    public BuffTypes BuffType;
}