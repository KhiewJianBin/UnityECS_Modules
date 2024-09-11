using Unity.Burst;
using Unity.Entities;

/// <summary>
/// Standard Health Module System
/// Unmanaged System (struct)
/// </summary>
[BurstCompile]
[DisableAutoCreation] // Unity will not this System Automatically
public partial struct HealthModuleSystem : ISystem
{
    ComponentLookup<FloatModule> float_LU;
    ComponentLookup<BuffableFloat> bufffloat_LU;
    public void OnCreate(ref SystemState state)
    {
        float_LU = state.GetComponentLookup<FloatModule>(true);
        bufffloat_LU = state.GetComponentLookup<BuffableFloat>(true);
    }
    public void OnDestroy(ref SystemState state) { }
    public void OnStartRunning(ref SystemState state) { }
    public void OnStopRunning(ref SystemState state) { }

    /// <summary>
    /// Run Every Frame
    /// Schedule Jobs Here
    /// </summary>
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float_LU.Update(ref state);
        bufffloat_LU.Update(ref state);

        foreach (var module in SystemAPI.Query<RefRW<HealthModule>>())
        {
            module.ValueRW.BaseHealth = float.NaN;
            module.ValueRW.CurrentHealth = float.NaN;

            var ebasehealth = module.ValueRW.e_BaseHealth;
            if (!float_LU.HasComponent(ebasehealth)) return;

            var value = float_LU.GetRefRO(ebasehealth).ValueRO;
            module.ValueRW.BaseHealth = value.BaseValue;

            if (bufffloat_LU.HasComponent(ebasehealth))
            {
                var buff = bufffloat_LU.GetRefRO(ebasehealth).ValueRO;
                module.ValueRW.BaseHealth += buff.BuffedValue;
            }
        }
    }
}