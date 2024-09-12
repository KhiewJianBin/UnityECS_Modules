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
        state.EntityManager.AddComponentData(required, new PlayersSpawnerData());
        state.EntityManager.AddComponentData(required, new PlayersManagedData());

        state.RequireForUpdate<PlayersSpawnerData>();
    }
    public void OnDestroy(ref SystemState state) { }
    public void OnStartRunning(ref SystemState state) { }
    public void OnStopRunning(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {
        var entityManager = state.EntityManager;
        var spawnerData = SystemAPI.ManagedAPI.GetSingleton<PlayersSpawnerData>();
        var playersData = SystemAPI.ManagedAPI.GetSingleton<PlayersManagedData>();

        foreach (var player in spawnerData.PlayersToSpawn)
        {
            var baseHealthEntity = entityManager.CreateEntity();
            entityManager.AddComponentData(baseHealthEntity, new FloatModule()
            {
                BaseValue = Random.Range(0,1000000)
            });
            entityManager.AddComponentData(baseHealthEntity, new BuffableFloat());

            var currentHealthEntity = entityManager.CreateEntity();
            entityManager.AddComponentData(currentHealthEntity, new FloatModule()
            {
                BaseValue = 99
            });

            var entityPlayer = entityManager.CreateEntity();
            entityManager.AddComponentData(entityPlayer, new HealthModule()
            {
                BaseHealth = 1,
                e_BaseHealth = baseHealthEntity,
                e_CurrentHealth = currentHealthEntity,
            });

            player.entityRef = entityPlayer;

            var buffEntity = entityManager.CreateEntity();
            entityManager.AddComponentData(buffEntity, new BaseHealthBuff()
            {
                Target = entityPlayer,

                DurationTimer = float.PositiveInfinity,
                Value = 1,
                BuffType = BuffTypes.ValueAdd,
            });

            //playersData.Players.Add(player);
        }

        spawnerData.PlayersToSpawn.Clear();
    }
}

public class PlayersSpawnerData : IComponentData
{
    public List<Player> PlayersToSpawn = new();
}

public class PlayersManagedData : IComponentData
{
    public List<Player> Players = new();
}