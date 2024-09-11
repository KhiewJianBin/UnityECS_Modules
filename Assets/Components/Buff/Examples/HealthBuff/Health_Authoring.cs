using UnityEngine;
using Unity.Entities;

public class Health_Authoring : MonoBehaviour
{
    /// <summary>
    /// Convert the GameObject
    /// </summary>
    class Baker : Baker<Health_Authoring>
    {
        public override void Bake(Health_Authoring authoring)
        {
            // 1. Get Enitity using only required TransformUsage Flag
            var entity = GetEntity(TransformUsageFlags.None);

            // 2. Create sub entity/components (using data from authoring monobehaviour if needed)
            var baseHealthEntity = CreateAdditionalEntity(TransformUsageFlags.None, bakingOnlyEntity: false, entityName: "BaseHealth Value");
            AddComponent(baseHealthEntity, new FloatModule()
            {
                BaseValue = 100
            });
            AddComponent(baseHealthEntity, new BuffableFloat()
            {
                BuffedValue = 0
            });

            var currentHealthEntity = CreateAdditionalEntity(TransformUsageFlags.None, bakingOnlyEntity: false, entityName: "CurrentHealth Value");

            // 3. Create & Add Main Component
            AddComponent(entity, new HealthModule()
            {
                BaseHealth = 1,
                e_BaseHealth = baseHealthEntity,
                e_CurrentHealth = currentHealthEntity,
            });


            var buffEntity = CreateAdditionalEntity(TransformUsageFlags.None, bakingOnlyEntity: false, entityName: "Health Buff");
            AddComponent(buffEntity, new BaseHealthBuff()
            {
                Target = entity,

                DurationTimer = 10,
                Value = 5,
                BuffType = BuffTypes.ValueAdd,
            });

            var buffEntity2 = CreateAdditionalEntity(TransformUsageFlags.None, bakingOnlyEntity: false, entityName: "Health Buff");
            AddComponent(buffEntity2, new BaseHealthBuff()
            {
                Target = entity,

                DurationTimer = 10,
                Value = 0.5f,
                BuffType = BuffTypes.ValueMultiply,
            });
        }
    }
}