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
    public void OnCreate(ref SystemState state) { }
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
        var float_LU = state.GetComponentLookup<FloatModule>(true);
        var bufffloat_LU = state.GetComponentLookup<BuffableFloat>(true);

        foreach (var refmodule in SystemAPI.Query<RefRW<HealthModule>>())
        {
            refmodule.ValueRW.BaseHealth = float.NaN;
            refmodule.ValueRW.CurrentHealth = float.NaN;

            var ebasehealth = refmodule.ValueRW.e_BaseHealth;
            if (!float_LU.HasComponent(ebasehealth)) return;

            var value = float_LU.GetRefRO(ebasehealth).ValueRO;
            refmodule.ValueRW.BaseHealth = value.BaseValue;

            if (bufffloat_LU.HasComponent(ebasehealth))
            {
                var buff = bufffloat_LU.GetRefRO(ebasehealth).ValueRO;
                refmodule.ValueRW.BaseHealth += buff.BuffedValue;
            }
        }
    }
}