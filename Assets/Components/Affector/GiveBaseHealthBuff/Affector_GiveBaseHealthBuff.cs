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
            authoring.buff.AppliedByEntity = entity;
            AddComponent(entity, new GiveBaseHealthBuffData()
            {
                buff = authoring.buff
            });
            AddBuffer<ApplyEntityBuffer>(entity);
            AddBuffer<RemoveEntityBuffer>(entity);
        }
    }
}

public struct GiveBaseHealthBuffData : IComponentData
{
    public BaseHealthBuff_Stackable buff;
}

// Use to keep track of the entities that this affector have given
[InternalBufferCapacity(16)]
public struct ApplyEntityBuffer : IBufferElementData
{
    public Entity GameBuffToEntity;
}

// Use to queue entities that the affector has given for removal
[InternalBufferCapacity(16)]
public struct RemoveEntityBuffer : IBufferElementData
{
    public Entity GameBuffToEntity;
}