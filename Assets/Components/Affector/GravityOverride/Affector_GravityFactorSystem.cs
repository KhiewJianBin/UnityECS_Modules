using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

[BurstCompile]
[DisableAutoCreation]
public partial struct Affector_GravityFactorSystem : ISystem, ISystemStartStop
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GravityFactorData>();
    }
    public void OnDestroy(ref SystemState state) { }
    public void OnStartRunning(ref SystemState state) { }
    public void OnStopRunning(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {
        // Give buff to collision trigger entities that matches
        state.Dependency = new ApplyInTrigger_Job
        {
            GFD_LU = SystemAPI.GetComponentLookup<GravityFactorData>(true),
            PG_LU = SystemAPI.GetComponentLookup<PhysicsGravityFactor>(),

        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);

        state.Dependency.Complete();
    }

    [BurstCompile]
    struct ApplyInTrigger_Job : ITriggerEventsJob
    {
        [ReadOnly] public ComponentLookup<GravityFactorData> GFD_LU;
        public ComponentLookup<PhysicsGravityFactor> PG_LU;

        public void Execute(TriggerEvent triggerEvent)
        {
            if (GFD_LU.TryGetComponent(triggerEvent.EntityA, out GravityFactorData data))
            {
                if (PG_LU.HasComponent(triggerEvent.EntityB))
                {
                    var gravity = PG_LU.GetRefRW(triggerEvent.EntityB);
                    gravity.ValueRW.Value = data.value;
                    
                }
            }
            else if (GFD_LU.TryGetComponent(triggerEvent.EntityB, out GravityFactorData data2))
            {
                if (PG_LU.HasComponent(triggerEvent.EntityA))
                {
                    var gravity = PG_LU.GetRefRW(triggerEvent.EntityA);
                    gravity.ValueRW.Value = data2.value;
                }
            }
        }
    }
}