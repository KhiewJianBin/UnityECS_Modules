using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[DisableAutoCreation]
public partial struct Affector_GravityOverrideSystem : ISystem, ISystemStartStop
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GravityOverrideData>();
    }
    public void OnDestroy(ref SystemState state) { }
    public void OnStartRunning(ref SystemState state) { }
    public void OnStopRunning(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {
        // Give buff to collision trigger entities that matches
        state.Dependency = new ApplyInTrigger_Job
        {
            GOD_LU = SystemAPI.GetComponentLookup<GravityOverrideData>(true),

            PM_LU = SystemAPI.GetComponentLookup<PhysicsMassOverride>(),
            velocity_LU = SystemAPI.GetComponentLookup<PhysicsVelocity>(),
            mass_LU = SystemAPI.GetComponentLookup<PhysicsMass>(),

            localTransform = SystemAPI.GetComponentLookup<LocalTransform>(),


        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);

        state.Dependency.Complete();
    }

    [BurstCompile]
    struct ApplyInTrigger_Job : ITriggerEventsJob
    {
        [ReadOnly] public ComponentLookup<GravityOverrideData> GOD_LU;
        public ComponentLookup<PhysicsMassOverride> PM_LU;
        public ComponentLookup<PhysicsVelocity> velocity_LU;
        [ReadOnly] public ComponentLookup<PhysicsMass> mass_LU;
        [ReadOnly] public ComponentLookup<LocalTransform> localTransform;

        public void Execute(TriggerEvent triggerEvent)
        {
            if (GOD_LU.TryGetComponent(triggerEvent.EntityB, out GravityOverrideData data2))
            {
                if (PM_LU.HasComponent(triggerEvent.EntityA))
                {
                    var gravity = PM_LU.GetRefRW(triggerEvent.EntityA);
                    gravity.ValueRW.IsKinematic = 0x01;

                    var vel = velocity_LU.GetRefRW(triggerEvent.EntityA);
                    var mass = mass_LU.GetRefRO(triggerEvent.EntityA).ValueRO;

                    var t = localTransform.GetRefRO(triggerEvent.EntityB).ValueRO;
                    var f = t.Forward() * 10;

                    vel.ValueRW.Linear = 0;
                    vel.ValueRW.Angular = 0;

                    vel.ValueRW.ApplyLinearImpulse(in mass, in f);
                }
            }
        }
    }
}