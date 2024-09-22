namespace Examples.HealthBuffZones
{
    using Unity.Burst;
    using Unity.Entities;
    using Unity.Transforms;
    using UnityEngine;
    using Unity.Mathematics;

    [BurstCompile]
    [DisableAutoCreation]
    public partial struct PlayerUpdateSystem : ISystem, ISystemStartStop
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerData>();
        }
        public void OnDestroy(ref SystemState state) { }
        public void OnStartRunning(ref SystemState state) { }
        public void OnStopRunning(ref SystemState state) { }

        public void OnUpdate(ref SystemState state)
        {
            var time = (float)SystemAPI.Time.ElapsedTime;

            foreach (var (player, health, transform) in SystemAPI.Query<PlayerData, RefRO<HealthModule>, RefRW<LocalTransform>>())
            {
                player.player_ref.baseHealth = health.ValueRO.BaseHealth;
                player.player_ref.currentHleath = health.ValueRO.CurrentHealth;

                transform.ValueRW.Position = new float3(transform.ValueRW.Position.x, transform.ValueRW.Position.y, 10 * Mathf.Sin(time));
            }
        }
    }
}