using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

[BurstCompile]
[DisableAutoCreation]
public partial struct Affector_GiveBuffSystem : ISystem, ISystemStartStop
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GiveBuffData>();
        state.RequireForUpdate<SimulationSingleton>();
    }
    public void OnDestroy(ref SystemState state) { }
    public void OnStartRunning(ref SystemState state) { }
    public void OnStopRunning(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {
        //var time = (float)SystemAPI.Time.ElapsedTime;

        //var world = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        //var collisionworld = world.CollisionWorld;

        //OverlapAabbInput input = new()
        //{
        //    Aabb = new Aabb(),
        //    Filter = new CollisionFilter()
        //};

        //NativeList<int> hits = new();
        //collisionworld.OverlapAabb(input, ref hits);

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);

        state.Dependency = new TriggerDeathJob
        {
            Ecb = ecb,
            GiveBuff_LU = SystemAPI.GetComponentLookup<GiveBuffData>(true),
            Health_LU = SystemAPI.GetComponentLookup<HealthModule>(true),
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);

        state.Dependency.Complete();

        ecb.Playback(state.EntityManager);
        ecb.Dispose();

        //state.CompleteDependency();

        // physicsWorldSingleton.CastRay(...);

        // It is also possible to access the PhysicsWorld
        //PhysicsWorld world = physicsWorldSingleton.PhysicsWorld;
    }

    [BurstCompile]
    struct TriggerDeathJob : ITriggerEventsJob
    {
        public EntityCommandBuffer Ecb;
        [ReadOnly] public ComponentLookup<GiveBuffData> GiveBuff_LU;
        [ReadOnly] public ComponentLookup<HealthModule> Health_LU;

        public void Execute(TriggerEvent triggerEvent)
        {
            if (GiveBuff_LU.TryGetComponent(triggerEvent.EntityA, out GiveBuffData buff))
            {
                if (Health_LU.HasComponent(triggerEvent.EntityB))
                {
                    var basehealthbuff = buff.buff;
                    basehealthbuff.Target = triggerEvent.EntityB;
                    var entity = Ecb.CreateEntity();
                    Ecb.AddComponent(entity, basehealthbuff);
                }
            }
            else if (GiveBuff_LU.TryGetComponent(triggerEvent.EntityB, out GiveBuffData buff2))
            {
                if (Health_LU.HasComponent(triggerEvent.EntityA))
                {
                    var basehealthbuff = buff2.buff;
                    basehealthbuff.Target = triggerEvent.EntityA;
                    var entity = Ecb.CreateEntity();
                    Ecb.AddComponent(entity, basehealthbuff);
                }
            }
        }
    }
}