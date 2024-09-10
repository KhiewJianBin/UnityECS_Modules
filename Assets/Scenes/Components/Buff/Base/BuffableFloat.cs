using Unity.Entities;

/// <summary>
/// Attached to an Entity that has a float module to be "buffed"
/// </summary>
public struct BuffableFloat : IComponentData
{
    public float BuffedValue;
}