using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct GravityOverride : IComponentData
{
    public float DurationTimer;
    public float3 Direction;
}