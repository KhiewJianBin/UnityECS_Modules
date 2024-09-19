using System.Collections.Generic;
using System.Linq;

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

/// <summary>
/// Base Buff System
/// Unmanaged System (struct)
/// </summary>
[BurstCompile]
[DisableAutoCreation] // Unity will not this System Automatically
public partial struct BaseHealthBuff_StackableSystem : ISystem, ISystemStartStop
{
    ComponentLookup<HealthModule> healthModule_LU;
    ComponentLookup<FloatModule> float_LU;
    ComponentLookup<BuffableFloat> bufffloat_LU;
    BufferLookup<RemoveEntityBuffer> RB_LU;
    public void OnCreate(ref SystemState state)
    {
        healthModule_LU = state.GetComponentLookup<HealthModule>(true);
        float_LU = state.GetComponentLookup<FloatModule>(true);
        bufffloat_LU = state.GetComponentLookup<BuffableFloat>(false);
        RB_LU = state.GetBufferLookup<RemoveEntityBuffer>();
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

        // Stack Buff
        if (BaseHealthBuff_Stackable.CanStack)
        {
            EntityCommandBuffer ecb2 = new(Allocator.TempJob);
            HashSet<(BaseHealthBuff_Stackable buff, Entity entity)> existing = new();
            foreach (var (buff, buffentity) in SystemAPI.Query<RefRW<BaseHealthBuff_Stackable>>().WithEntityAccess())
            {
                var existingbuff = existing.FirstOrDefault(bhb => bhb.buff.Target == buff.ValueRO.Target);

                if (existingbuff.entity == default)
                {
                    existingbuff.buff = buff.ValueRW.Stack(existingbuff.buff);
                    existingbuff.entity = buffentity;
                    existing.Add(existingbuff);
                }
                else
                {
                    existing.Remove(existingbuff);
                    ecb2.DestroyEntity(existingbuff.entity);

                    existingbuff.buff = buff.ValueRW.Stack(existingbuff.buff);
                    existing.Add(existingbuff);
                }
            }

            ecb2.Playback(state.EntityManager);
            ecb2.Dispose();
        }

        healthModule_LU.Update(ref state);
        float_LU.Update(ref state);
        bufffloat_LU.Update(ref state);
        RB_LU.Update(ref state);

        //// Used for add and removal of components, to be done at the end of simulation system (default world)
        var ecbs = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        EntityCommandBuffer ecb = ecbs.CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (buff, buffentity) in SystemAPI.Query<RefRW<BaseHealthBuff_Stackable>>().WithEntityAccess())
        {
            var target = buff.ValueRO.Target;

            if (!healthModule_LU.HasComponent(target)) return;

            var healthModule = healthModule_LU.GetRefRO(target);
            var bufftarget = healthModule.ValueRO.e_BaseHealth;

            if (!float_LU.HasComponent(bufftarget) || !bufffloat_LU.HasComponent(bufftarget)) return;

            var reffloat = float_LU.GetRefRO(bufftarget);
            var refbufffloat = bufffloat_LU.GetRefRW(bufftarget);

            // Apply Buff, store into bufffloat
            switch (BaseHealthBuff_Stackable.BuffType)
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
                if (RB_LU.TryGetBuffer(buff.ValueRO.AppliedByEntity, out var buffer))
                {
                    buffer.Add(new RemoveEntityBuffer() { GameBuffToEntity = target });
                }

                ecb.DestroyEntity(buffentity);
                
                //Debug.Log("Expired, Removing" + buffentity);
            }
        }
    }
}