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
            var entityPlayer = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entityPlayer, new GiveBaseHealthBuffData()
            {
                buff = authoring.buff
            });
        }
    }
}

public struct GiveBaseHealthBuffData : IComponentData
{
    public BaseHealthBuff_Stackable buff;

    //public Entity 
}