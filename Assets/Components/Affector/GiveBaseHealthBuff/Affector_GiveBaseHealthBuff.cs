using Unity.Entities;
using UnityEngine;

public class Affector_GiveBaseHealthBuff : MonoBehaviour
{
    [SerializeField]
    public BaseHealthBuff_Stackable buff;

    class Baker : Baker<Affector_GiveBaseHealthBuff>
    {
        public override void Bake(Affector_GiveBaseHealthBuff authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            authoring.buff.CameFromEntity = entity;
            AddComponent(entity, new GiveBaseHealthBuffData()
            {
                buff = authoring.buff
            });
            AddBuffer<GiveBuffEntityBuffer>(entity);
        }
    }
}

public struct GiveBaseHealthBuffData : IComponentData
{
    public BaseHealthBuff_Stackable buff;
}

[InternalBufferCapacity(16)]
public struct GiveBuffEntityBuffer : IBufferElementData
{
    public Entity GameBuffToEntity;
}