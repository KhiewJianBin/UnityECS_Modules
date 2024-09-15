using Unity.Entities;
using UnityEngine;

public class Affector_GiveBuff : MonoBehaviour
{
    [SerializeField]
    public BaseHealthBuff buff;

    class Baker : Baker<Affector_GiveBuff>
    {
        public override void Bake(Affector_GiveBuff authoring)
        {
            var entityPlayer = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entityPlayer, new GiveBuffData()
            {
                //buff = new BaseHealthBuff()
                //{
                //    DurationTimer = 1,
                //    Value = 1,
                //    BuffType = BuffTypes.ValueAdd,
                //}

                buff = authoring.buff
            });
        }
    }
}

public struct GiveBuffData : IComponentData
{
    public BaseHealthBuff buff;
}