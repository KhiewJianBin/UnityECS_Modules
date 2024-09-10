using Unity.Entities;

/// <summary>
/// Attached to an Entity that has a module which requires a float
/// </summary>
public struct FloatModule : IComponentData
{
    public float BaseValue;
}