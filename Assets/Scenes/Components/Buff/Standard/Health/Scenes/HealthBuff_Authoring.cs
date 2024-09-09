using UnityEngine;
using Unity.Entities;

public class HealthBuff_Authoring : MonoBehaviour
{
    /// <summary>
    /// Convert the GameObject
    /// </summary>
    class Baker : Baker<HealthBuff_Authoring>
    {
        public override void Bake(HealthBuff_Authoring authoring)
        {
            // 1. Get Enitity using only required TransformUsage Flag
            var entity = GetEntity(TransformUsageFlags.None);

            // 2. Add Entity Components (using data from authoring monobehaviour if needed)
            AddComponent(entity, new HealthModule()
            {
                Health = 100
            });
            
            // Option 1. Add additioanl Entity/+ Component if require
            var buffEntity = CreateAdditionalEntity(TransformUsageFlags.None, bakingOnlyEntity: false, entityName: "Health Buff");
            AddComponent(buffEntity, new HealthBuff()
            {
                Target = entity,

                DurationTimer = 10,
                Value = 1,
                BuffType = BuffTypes.ValueAdd,
            });
        }
    }
}