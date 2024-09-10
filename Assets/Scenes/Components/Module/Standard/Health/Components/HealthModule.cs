using Unity.Entities;

/// <summary>
/// This Entity has Health
/// </summary>
public struct HealthModule : IComponentData
{
    public float BaseHealth;
    public float CurrentHealth;

    public Entity e_BaseHealth;
    public Entity e_CurrentHealth;
}