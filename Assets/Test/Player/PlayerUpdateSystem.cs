using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[DisableAutoCreation]
public partial struct PlayerUpdateSystem : ISystem, ISystemStartStop
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayersManagedData>();
    }
    public void OnDestroy(ref SystemState state) { }
    public void OnStartRunning(ref SystemState state) { }
    public void OnStopRunning(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {
        var time = (float)SystemAPI.Time.ElapsedTime;

        foreach (var (player, health) in SystemAPI.Query<PlayerData, RefRO<HealthModule>>())
        {
            player.player_ref.baseHealth = health.ValueRO.BaseHealth;
            player.player_ref.currentHleath = health.ValueRO.CurrentHealth;
            //transform.ValueRW.Position = new Vector3(10 * math.sin(time), transform.ValueRW.Position.y, transform.ValueRW.Position.z);
        }
    }
}