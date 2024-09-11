using UnityEngine;
using Unity.Entities;

public class EmptyStackableBuff_Authoring : MonoBehaviour
{
    /// <summary>
    /// Convert the GameObject
    /// </summary>
    class Baker : Baker<EmptyStackableBuff_Authoring>
    {
        public override void Bake(EmptyStackableBuff_Authoring authoring)
        {
            // 1. Get/Create Enitities using only required TransformUsage Flag
            var entity = GetEntity(TransformUsageFlags.None);
            var additionalA = CreateAdditionalEntity(TransformUsageFlags.None, bakingOnlyEntity: false, entityName: "Empty Buff");
            var additionalB = CreateAdditionalEntity(TransformUsageFlags.None, bakingOnlyEntity: false, entityName: "Empty Buff");

            // 2. Add Entity Components (using data from authoring monobehaviour if needed)
            AddComponent(additionalA, new EmptyStackableBuffComponent
            {
                DurationTimer = 10,
                Value = 1,
                BuffType = BuffTypes.ValueAdd,

                StackType = StackTypes.Value
            });
            AddComponent(additionalB, new EmptyStackableBuffComponent
            {
                DurationTimer = 10,
                Value = 1,
                BuffType = BuffTypes.ValueAdd,

                StackType = StackTypes.Value
            });

            // Option 1. Add additioanl Entity/+ Component if require
        }
    }
}