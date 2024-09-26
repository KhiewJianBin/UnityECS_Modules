using Unity.Entities;
using UnityEngine;

public class Affector_GiveGravityOverride : MonoBehaviour
{
    public GravityOverride Override;

    class Baker : Baker<Affector_GiveGravityOverride>
    {
        public override void Bake(Affector_GiveGravityOverride authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new GiveGravityOverrideData()
            {
                Override = authoring.Override
            });
        }
    }
}

public struct GiveGravityOverrideData : IComponentData
{
    public GravityOverride Override;
}