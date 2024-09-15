using Unity.Entities;
using UnityEngine;

public class Affector_GiveBuff : MonoBehaviour
{
    class Baker : Baker<Affector_GiveBuff>
    {
        public override void Bake(Affector_GiveBuff authoring)
        {
            var entityPlayer = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entityPlayer, new GiveBuffData()
            {
                buff = new BaseHealthBuff()
                {
                    DurationTimer = 1,
                    Value = 1,
                    BuffType = BuffTypes.ValueAdd,
                }
            });
        }
    }
}

public struct GiveBuffData : IComponentData
{
    public BaseHealthBuff buff;
}