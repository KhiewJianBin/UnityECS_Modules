using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
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

        var playersData = SystemAPI.ManagedAPI.GetSingleton<PlayersManagedData>();
        var healths = SystemAPI.GetComponentLookup<HealthModule>();

        foreach ( var player in playersData.Players)
        {
            if (healths.TryGetComponent(player.entityRef, out var health))
            {
                player.Health = health.BaseHealth;
            }

            //player.transform.position = new Vector3(math.sin(3 * time), player.transform.position.y, player.transform.position.z);
        }
    }
}