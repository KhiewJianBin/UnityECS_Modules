using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

[BurstCompile]
[DisableAutoCreation]
public partial struct Affector_GiveBaseHealthBuffSystem : ISystem, ISystemStartStop
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GiveBaseHealthBuffData>();
        state.RequireForUpdate<SimulationSingleton>();
    }
    public void OnDestroy(ref SystemState state) { }
    public void OnStartRunning(ref SystemState state) { }
    public void OnStopRunning(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new(Allocator.TempJob);

        state.Dependency = new GiveBuffInTrigger_Job
        {
            Ecb = ecb,
            GiveBuff_LU = SystemAPI.GetComponentLookup<GiveBaseHealthBuffData>(true),
            GiveBuff_BLU = SystemAPI.GetBufferLookup<GiveBuffEntityBuffer>(),
            Health_LU = SystemAPI.GetComponentLookup<HealthModule>(true),

        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);

        state.Dependency.Complete();

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    [BurstCompile]
    struct GiveBuffInTrigger_Job : ITriggerEventsJob
    {
        public EntityCommandBuffer Ecb;
        [ReadOnly] public ComponentLookup<GiveBaseHealthBuffData> GiveBuff_LU;
        public BufferLookup<GiveBuffEntityBuffer> GiveBuff_BLU;
        [ReadOnly] public ComponentLookup<HealthModule> Health_LU;

        public void Execute(TriggerEvent triggerEvent)
        {
            if (GiveBuff_LU.TryGetComponent(triggerEvent.EntityA, out GiveBaseHealthBuffData buff))
            {
                if (Health_LU.HasComponent(triggerEvent.EntityB))
                {
                    GiveBuff_BLU.TryGetBuffer(triggerEvent.EntityA, out DynamicBuffer<GiveBuffEntityBuffer> buffdbuffer);

                    for (int i = 0; i < buffdbuffer.Length; i++)
                    {
                        if (buffdbuffer[i].GameBuffToEntity == triggerEvent.EntityB)
                        {
                            Debug.Log("This entity already has been buff by this givebuffaffector");
                            return;
                        }
                    }

                    buffdbuffer.Add(new GiveBuffEntityBuffer() { GameBuffToEntity = triggerEvent.EntityB });

                    var basehealthbuff = buff.buff;
                    basehealthbuff.Target = triggerEvent.EntityB;
                    var entity = Ecb.CreateEntity();
                    Ecb.AddComponent(entity, basehealthbuff);
                }
            }
            else if (GiveBuff_LU.TryGetComponent(triggerEvent.EntityB, out GiveBaseHealthBuffData buff2))
            {
                if (Health_LU.HasComponent(triggerEvent.EntityA))
                {
                    GiveBuff_BLU.TryGetBuffer(triggerEvent.EntityB, out DynamicBuffer<GiveBuffEntityBuffer> buffdbuffer);

                    for (int i = 0; i < buffdbuffer.Length; i++)
                    {
                        if (buffdbuffer[i].GameBuffToEntity == triggerEvent.EntityA)
                        {
                            Debug.Log("This entity already has been buff by this givebuffaffector");
                            return;
                        }
                    }

                    buffdbuffer.Add(new GiveBuffEntityBuffer() { GameBuffToEntity = triggerEvent.EntityA });

                    var basehealthbuff = buff2.buff;
                    basehealthbuff.Target = triggerEvent.EntityA;
                    var entity = Ecb.CreateEntity();
                    Ecb.AddComponent(entity, basehealthbuff);
                }
            }
        }
    }
}