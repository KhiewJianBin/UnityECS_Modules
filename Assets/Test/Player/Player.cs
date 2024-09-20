using Unity.Entities;
using Unity.Physics;
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
            var baseHealthEntity = CreateAdditionalEntity(TransformUsageFlags.None, false, "Base Health");
            AddComponent(baseHealthEntity, new FloatModule()
            {
                BaseValue = authoring.BaseHealth
            });
            AddComponent(baseHealthEntity, new BuffableFloat());

            var currentHealthEntity = CreateAdditionalEntity(TransformUsageFlags.None, false, "Current Health");
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

            AddComponentObject(entityPlayer, new PlayerData()
            {
                player_ref = authoring
            });

            AddComponent(entityPlayer, new PhysicsGravityFactor() { Value = 1 });
        }
    }
}

public class PlayerData : IComponentData
{
    public Player player_ref;
}