using Unity.Burst;
using Unity.Entities;
using UnityEngine;

/// <summary>
/// Base Buff System
/// Unmanaged System (struct)
/// </summary>
[BurstCompile]
[DisableAutoCreation] // Unity will not this System Automatically
public partial struct HealthBuffSystem : ISystem, ISystemStartStop
{
    ComponentLookup<HealthModule> healthModules;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        EntityQuery query = state.GetEntityQuery(ComponentType.ReadWrite<HealthBuff>());
        state.RequireForUpdate(query);

        healthModules = state.GetComponentLookup<HealthModule>(false);
    }

    /// <summary>
    /// Called When System Is Destoryed
    /// </summary>
    public void OnDestroy(ref SystemState state) { }

    /// <summary>
    /// Called before the first OnUpdate, resuming after stopped or disabled
    /// </summary>
    public void OnStartRunning(ref SystemState state) { }

    /// <summary>
    /// Called when disabled or when OnUpdate is stopped called because of non matching queries
    /// </summary>
    public void OnStopRunning(ref SystemState state) { }

    /// <summary>
    /// Run Every Frame
    /// Schedule Jobs Here
    /// </summary>
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        // Used for add and removal of components, to be done at the end of simulation system (default world)
        var ecbs = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        EntityCommandBuffer ecb = ecbs.CreateCommandBuffer(state.WorldUnmanaged);

        // Get/Update all HealthModule
        healthModules.Update(ref state);

        foreach (var (buff, buffentity) in SystemAPI.Query<RefRW<HealthBuff>>().WithEntityAccess())
        {
            var rwBuff = buff.ValueRW;

            // Apply Buff
            if(healthModules.HasComponent(rwBuff.Target))
            {
                healthModules.GetRefRW(rwBuff.Target).ValueRW.Health += rwBuff.Value;
            }

            // Update Timer
            rwBuff.DurationTimer -= deltaTime;

            // Queue for Removal When Expired
            bool Expired = rwBuff.DurationTimer <= 0;
            if (Expired)
            {
                ecb.RemoveComponent<EmptyBuffComponent>(buffentity);
                Debug.Log("Expired!. Queued for Removal");
            }
        }
    }
}