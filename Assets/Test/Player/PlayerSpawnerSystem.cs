using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
[DisableAutoCreation]
public partial struct PlayerSpawnerSystem : ISystem, ISystemStartStop
{
    public void OnCreate(ref SystemState state)
    {
        var required = state.EntityManager.CreateEntity();
        state.EntityManager.AddComponentData(required, new PlayerSpawnerData());

        state.RequireForUpdate<PlayerSpawnerData>();
    }
    public void OnDestroy(ref SystemState state) { }
    public void OnStartRunning(ref SystemState state) { }
    public void OnStopRunning(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {
        var entityManager = state.EntityManager;
        var data = SystemAPI.ManagedAPI.GetSingleton<PlayerSpawnerData>();

        if (data == null) return;

        foreach (var player in data.PlayersToSpawn)
        {
            if (player == null) continue;

            var baseHealthEntity = entityManager.CreateEntity();
            entityManager.SetName(baseHealthEntity, nameof(baseHealthEntity));
            entityManager.AddComponentData(baseHealthEntity, new FloatModule()
            {
                BaseValue = Random.Range(0,1000000)
            });

            var currentHealthEntity = entityManager.CreateEntity();
            entityManager.SetName(currentHealthEntity, nameof(currentHealthEntity));
            entityManager.AddComponentData(currentHealthEntity, new FloatModule()
            {
                BaseValue = 99
            });

            var entityPlayer = entityManager.CreateEntity();
            entityManager.SetName(entityPlayer, nameof(entityPlayer));
            entityManager.AddComponentData(entityPlayer, new HealthModule()
            {
                BaseHealth = 1,
                e_BaseHealth = baseHealthEntity,
                e_CurrentHealth = currentHealthEntity,
            });

            player.entityRef = entityPlayer;
        }
        data.PlayersToSpawn.Clear();
    }
}

public class PlayerSpawnerData : IComponentData
{
    public List<Player> PlayersToSpawn = new();
}