using Unity.Burst;
using Unity.Entities;
using UnityEngine;

/// <summary>
/// Base Buff System
/// Unmanaged System (struct)
/// </summary>
[BurstCompile]
[DisableAutoCreation] // Unity will not this System Automatically
public partial struct FloatBuffSystem : ISystem, ISystemStartStop
{
    public void OnCreate(ref SystemState state) { }
    public void OnDestroy(ref SystemState state) { }
    public void OnStartRunning(ref SystemState state) { }
    public void OnStopRunning(ref SystemState state) { }

    /// <summary>
    /// Run Every Frame
    /// Schedule Jobs Here
    /// </summary>
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        //// Used for add and removal of components, to be done at the end of simulation system (default world)
        var ecbs = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        EntityCommandBuffer ecb = ecbs.CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (refvalue, refbuffvalue, refbuff, buffentity) in SystemAPI.Query<RefRO<FloatModule>, RefRW<BuffableFloat>, RefRW<HealthBuff>>().WithEntityAccess())
        {
            var value = refvalue.ValueRO;
            var buffvalue = refbuffvalue.ValueRW;
            var buff = refbuff.ValueRW;

            // Apply Buff, store into buffvalue
            switch (buff.BuffType)
            {
                case BuffTypes.ValueAdd:
                    buffvalue.BuffedValue += buff.Value;
                    break;
                case BuffTypes.ValueMultiply:
                    buffvalue.BuffedValue *= buff.Value;
                    break;
                default:
                    break;
            }

            // Update Timer
            buff.DurationTimer -= deltaTime;

            // Queue for Removal When Expired
            bool Expired = buff.DurationTimer <= 0;
            if (Expired)
            {
                ecb.RemoveComponent<EmptyBuffComponent>(buffentity);
                Debug.Log("Expired!. Queued for Removal");
            }
        }
    }
}