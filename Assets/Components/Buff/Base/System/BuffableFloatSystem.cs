using Unity.Burst;
using Unity.Entities;
using UnityEngine;

/// <summary>
/// Base Buff System
/// </summary>
[BurstCompile]
[DisableAutoCreation] // Unity will not this System Automatically
public partial struct BuffableFloatSystem : ISystem, ISystemStartStop
{
    public void OnCreate(ref SystemState state) { }
    public void OnDestroy(ref SystemState state) { }
    public void OnStartRunning(ref SystemState state) { }
    public void OnStopRunning(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var bufffloat in SystemAPI.Query<RefRW<BuffableFloat>>())
        {
            bufffloat.ValueRW.BuffedValue = 0;
        }
    }
}