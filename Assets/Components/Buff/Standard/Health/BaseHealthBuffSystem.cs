using Unity.Burst;
using Unity.Entities;
using UnityEngine;

/// <summary>
/// Base Buff System
/// Unmanaged System (struct)
/// </summary>
[BurstCompile]
[DisableAutoCreation] // Unity will not this System Automatically
public partial struct BaseHealthBuffSystem : ISystem, ISystemStartStop
{
    ComponentLookup<HealthModule> healthModule_LU;
    ComponentLookup<FloatModule> float_LU;
    ComponentLookup<BuffableFloat> bufffloat_LU;
    public void OnCreate(ref SystemState state)
    {
        healthModule_LU = state.GetComponentLookup<HealthModule>(true);
        float_LU = state.GetComponentLookup<FloatModule>(true);
        bufffloat_LU = state.GetComponentLookup<BuffableFloat>(false);
    }
    public void OnDestroy(ref SystemState state) { }
    public void OnStartRunning(ref SystemState state) { }
    public void OnStopRunning(ref SystemState state) { }

    /// <summary>
    /// Run Every Frame
    /// Schedule Jobs Here
    /// </summary>
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        //// Used for add and removal of components, to be done at the end of simulation system (default world)
        var ecbs = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        EntityCommandBuffer ecb = ecbs.CreateCommandBuffer(state.WorldUnmanaged);

        healthModule_LU.Update(ref state);
        float_LU.Update(ref state);
        bufffloat_LU.Update(ref state);

        foreach (var (buff, buffentity) in SystemAPI.Query<RefRW<BaseHealthBuff>>().WithEntityAccess())
        {
            var target = buff.ValueRO.Target;

            if (!healthModule_LU.HasComponent(target)) return;

            var healthModule = healthModule_LU.GetRefRO(target);
            var bufftarget = healthModule.ValueRO.e_BaseHealth;

            if (!float_LU.HasComponent(bufftarget) || !bufffloat_LU.HasComponent(bufftarget)) return;

            var reffloat = float_LU.GetRefRO(bufftarget);
            var refbufffloat = bufffloat_LU.GetRefRW(bufftarget);

            // Apply Buff, store into bufffloat
            switch (buff.ValueRO.BuffType)
            {
                case BuffTypes.ValueAdd:
                    refbufffloat.ValueRW.BuffedValue += buff.ValueRO.Value;
                    break;
                case BuffTypes.ValueMultiply:
                    refbufffloat.ValueRW.BuffedValue += reffloat.ValueRO.BaseValue * buff.ValueRO.Value;
                    break;
                default:
                    break;
            }

            // Update Timer
            buff.ValueRW.DurationTimer -= deltaTime;

            // Queue for Removal When Expired
            bool Expired = buff.ValueRW.DurationTimer <= 0;
            if (Expired)
            {
                ecb.DestroyEntity(buffentity);
                //Debug.Log("Expired, Removing" + buffentity);
            }
        }
    }
}