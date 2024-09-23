using Unity.Entities;
using UnityEngine;

public class Affector_GravityOverride : MonoBehaviour
{
    public float value;

    class Baker : Baker<Affector_GravityOverride>
    {
        public override void Bake(Affector_GravityOverride authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new GravityOverrideData()
            {
                value = authoring.value
            });
        }
    }
}

public struct GravityOverrideData : IComponentData
{
    public float value;
}