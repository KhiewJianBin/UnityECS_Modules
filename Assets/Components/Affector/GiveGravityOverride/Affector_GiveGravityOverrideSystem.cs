using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

[BurstCompile]
[DisableAutoCreation]
public partial struct Affector_GiveGravityOverrideSystem : ISystem, ISystemStartStop
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GiveGravityOverrideData>();
        state.RequireForUpdate<SimulationSingleton>();
    }
    public void OnDestroy(ref SystemState state) { }
    public void OnStartRunning(ref SystemState state) { }
    public void OnStopRunning(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new(Allocator.TempJob);

        state.Dependency = new ApplyInTrigger_Job
        {
            Ecb = ecb,
            ggo_LU = SystemAPI.GetComponentLookup<GiveGravityOverrideData>(true),
            go_LU = SystemAPI.GetComponentLookup<GravityOverride>(true),
            pmo_LU = SystemAPI.GetComponentLookup<PhysicsMassOverride>(true),
            transform_LU = SystemAPI.GetComponentLookup<LocalTransform>(true),

        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);

        state.Dependency.Complete();

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    [BurstCompile]
    struct ApplyInTrigger_Job : ITriggerEventsJob
    {
        public EntityCommandBuffer Ecb;
        [ReadOnly] public ComponentLookup<GiveGravityOverrideData> ggo_LU;
        [ReadOnly] public ComponentLookup<GravityOverride> go_LU;
        [ReadOnly] public ComponentLookup<PhysicsMassOverride> pmo_LU;
        [ReadOnly] public ComponentLookup<LocalTransform> transform_LU;

        public void Execute(TriggerEvent triggerEvent)
        {
            if (ggo_LU.TryGetComponent(triggerEvent.EntityB, out GiveGravityOverrideData data))
            {
                if (pmo_LU.HasComponent(triggerEvent.EntityA) && !go_LU.HasComponent(triggerEvent.EntityA))
                {
                    var transform = transform_LU.GetRefRO(triggerEvent.EntityB);

                    var over = data.Override;
                    over.Direction = transform.ValueRO.Forward();

                    Ecb.AddComponent(triggerEvent.EntityA, over);
                }
            }
        }
    }
}