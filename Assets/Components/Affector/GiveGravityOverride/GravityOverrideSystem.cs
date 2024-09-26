using System.Collections.Generic;
using System.Linq;

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;

[BurstCompile]
[DisableAutoCreation]
public partial struct GravityOverrideSystem : ISystem, ISystemStartStop
{
    ComponentLookup<PhysicsMassOverride> healthModule_LU;
    ComponentLookup<FloatModule> float_LU;
    ComponentLookup<BuffableFloat> bufffloat_LU;
    BufferLookup<RemoveEntityBuffer> RB_LU;
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GravityOverride>();

        //PM_LU = SystemAPI.GetComponentLookup<PhysicsMassOverride>();
        //velocity_LU = SystemAPI.GetComponentLookup<PhysicsVelocity>();
        //mass_LU = SystemAPI.GetComponentLookup<PhysicsMass>();

        //localTransform = SystemAPI.GetComponentLookup<LocalTransform>();
    }
    public void OnDestroy(ref SystemState state) { }
    public void OnStartRunning(ref SystemState state) { }
    public void OnStopRunning(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        //// Used for add and removal of components, to be done at the end of simulation system (default world)
        var ecbs = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        EntityCommandBuffer ecb = ecbs.CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (gravity, pmo, pvel, pmass, entity) in SystemAPI.Query<RefRW<GravityOverride>, RefRW<PhysicsMassOverride>, RefRW<PhysicsVelocity>, RefRO<PhysicsMass>>().WithEntityAccess())
        {
            pmo.ValueRW.IsKinematic = 1;

            pvel.ValueRW.Linear = 0;
            pvel.ValueRW.Angular = 0;

            var mass = pmass.ValueRO;
            var force = gravity.ValueRO.Direction * 10;

            pvel.ValueRW.ApplyLinearImpulse(in mass, in force);


            // Update Timer
            gravity.ValueRW.DurationTimer -= deltaTime;

            // Queue for Removal When Expired
            bool Expired = gravity.ValueRO.DurationTimer <= 0;
            if (Expired)
            {
                pmo.ValueRW.IsKinematic = 0;

                ecb.RemoveComponent<GravityOverride>(entity);
            }
        }
    }
}