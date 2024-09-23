namespace Examples.GravityZones
{
    using Unity.Burst;
    using Unity.Entities;

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
            foreach (var (player, health) in SystemAPI.Query<PlayerData, RefRO<HealthModule>>())
            {
                player.player_ref.baseHealth = health.ValueRO.BaseHealth;
                player.player_ref.currentHleath = health.ValueRO.CurrentHealth;
            }
        }
    }
}