using Unity.Entities;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Authoring")]
    public float BaseHealth;
    public float CurrentHealth;

    [Header("Output")]
    public float baseHealth;
    public float currentHleath;

    class Baker : Baker<Player>
    {
        public override void Bake(Player authoring)
        {
            var baseHealthEntity = CreateAdditionalEntity(TransformUsageFlags.None, false, nameof(FloatModule));
            AddComponent(baseHealthEntity, new FloatModule()
            {
                BaseValue = authoring.BaseHealth
            });
            AddComponent(baseHealthEntity, new BuffableFloat());

            var currentHealthEntity = CreateAdditionalEntity(TransformUsageFlags.None, false, nameof(FloatModule));
            AddComponent(currentHealthEntity, new FloatModule()
            {
                BaseValue = authoring.CurrentHealth
            });
            AddComponent(currentHealthEntity, new BuffableFloat());

            var entityPlayer = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entityPlayer, new HealthModule()
            {
                e_BaseHealth = baseHealthEntity,
                e_CurrentHealth = currentHealthEntity,
            });

            //var buff = CreateAdditionalEntity(TransformUsageFlags.None, false, nameof(BaseHealthBuff));
            //AddComponent(buff, new BaseHealthBuff()
            //{
            //    Target = entityPlayer,

            //    DurationTimer = float.PositiveInfinity,
            //    Value = 1,
            //    BuffType = BuffTypes.ValueAdd,
            //});

            AddComponentObject(entityPlayer, new PlayerData()
            {
                player_ref = authoring
            });
        }
    }
}

public class PlayerData : IComponentData
{
    public Player player_ref;
}