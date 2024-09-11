using Unity.Burst;
using Unity.Entities;
using UnityEngine;

/// <summary>
/// Base Buff System
/// Unmanaged System (struct)
/// </summary>
[BurstCompile]
[DisableAutoCreation] // Unity will not this System Automatically
public partial struct EmptyStackableBuffSystem : ISystem, ISystemStartStop
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        EntityQuery query = state.GetEntityQuery(ComponentType.ReadWrite<EmptyStackableBuffComponent>());
        state.RequireForUpdate(query);
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

        foreach (var (buff, buff2, entity) in SystemAPI.Query<RefRW<EmptyStackableBuffComponent>, RefRW<EmptyStackableBuffComponent>>().WithEntityAccess())
        {
            // Apply Buff
            Debug.Log("Apply Buff");

            // Update Timer
            buff.ValueRW.DurationTimer -= deltaTime;

            // Queue for Removal When Expired
            bool Expired = buff.ValueRO.DurationTimer <= 0;
            if(Expired)
            {
                ecb.RemoveComponent<EmptyStackableBuffComponent>(entity);
                Debug.Log("Expired!. Queued for Removal");
            }
        }
    }
}