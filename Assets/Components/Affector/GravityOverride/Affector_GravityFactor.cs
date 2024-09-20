using Unity.Entities;
using UnityEngine;

public class Affector_GravityFactor : MonoBehaviour
{
    public float value;

    class Baker : Baker<Affector_GravityFactor>
    {
        public override void Bake(Affector_GravityFactor authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new GravityFactorData()
            {
                value = authoring.value
            });
        }
    }
}

public struct GravityFactorData : IComponentData
{
    public float value;
}