using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;

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

        // Give buff to collision trigger entities that matches
        state.Dependency = new GiveBuffInTrigger_Job
        {
            Ecb = ecb,
            GiveBuff_LU = SystemAPI.GetComponentLookup<GiveBaseHealthBuffData>(true),
            GiveBuff_BLU = SystemAPI.GetBufferLookup<ApplyEntityBuffer>(),
            Health_LU = SystemAPI.GetComponentLookup<HealthModule>(true),

        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);

        state.Dependency.Complete();

        // Update Dynamic Buffer
        foreach (var (addbuff, removebuff) in SystemAPI.Query<DynamicBuffer<ApplyEntityBuffer>, DynamicBuffer<RemoveEntityBuffer>>())
        {
            for (int i = removebuff.Length - 1; i >= 0; i--)
            {
                for (int j = addbuff.Length - 1; j >= 0; j--)
                {
                    if (addbuff[j].GameBuffToEntity == removebuff[i].GameBuffToEntity)
                    {
                        addbuff.RemoveAt(j);
                        break;
                    }
                }
                removebuff.RemoveAt(i);
            }
        }
        
        
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    [BurstCompile]
    struct GiveBuffInTrigger_Job : ITriggerEventsJob
    {
        public EntityCommandBuffer Ecb;
        [ReadOnly] public ComponentLookup<GiveBaseHealthBuffData> GiveBuff_LU;
        public BufferLookup<ApplyEntityBuffer> GiveBuff_BLU;
        [ReadOnly] public ComponentLookup<HealthModule> Health_LU;

        public void Execute(TriggerEvent triggerEvent)
        {
            if (GiveBuff_LU.TryGetComponent(triggerEvent.EntityA, out GiveBaseHealthBuffData buff))
            {
                if (Health_LU.HasComponent(triggerEvent.EntityB))
                {
                    GiveBuff_BLU.TryGetBuffer(triggerEvent.EntityA, out DynamicBuffer<ApplyEntityBuffer> buffdbuffer);

                    for (int i = 0; i < buffdbuffer.Length; i++)
                    {
                        if (buffdbuffer[i].GameBuffToEntity == triggerEvent.EntityB)
                        {
                            //Debug.Log("This entity already has been buff by this givebuffaffector");
                            return;
                        }
                    }

                    buffdbuffer.Add(new ApplyEntityBuffer() { GameBuffToEntity = triggerEvent.EntityB });

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
                    GiveBuff_BLU.TryGetBuffer(triggerEvent.EntityB, out DynamicBuffer<ApplyEntityBuffer> buffdbuffer);

                    for (int i = 0; i < buffdbuffer.Length; i++)
                    {
                        if (buffdbuffer[i].GameBuffToEntity == triggerEvent.EntityA)
                        {
                            //Debug.Log("This entity already has been buff by this givebuffaffector");
                            return;
                        }
                    }

                    buffdbuffer.Add(new ApplyEntityBuffer() { GameBuffToEntity = triggerEvent.EntityA });

                    var basehealthbuff = buff2.buff;
                    basehealthbuff.Target = triggerEvent.EntityA;
                    var entity = Ecb.CreateEntity();
                    Ecb.AddComponent(entity, basehealthbuff);
                }
            }
        }
    }
}