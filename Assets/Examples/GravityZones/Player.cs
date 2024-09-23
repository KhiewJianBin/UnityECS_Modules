namespace Examples.GravityZones
{
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
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new HealthModule()
                {
                    e_BaseHealth = AddBuffableBaseHealthFloatModule(authoring),
                    e_CurrentHealth = AddBuffableCurrentHealthFloatModule(authoring),
                });
                AddComponentObject(entity, new PlayerData()
                {
                    player_ref = authoring
                });

                AddComponent(entity, new PhysicsGravityFactor()
                {
                    Value = 1
                });

                AddComponent(entity, new PhysicsMassOverride()
                {
                    IsKinematic = 0x00
                });
            }

            Entity AddBuffableBaseHealthFloatModule(Player authoring)
            {
                var entity = CreateAdditionalEntity(TransformUsageFlags.None, false, "Base Health");
                AddComponent(entity, new FloatModule()
                {
                    BaseValue = authoring.BaseHealth
                });
                AddComponent(entity, new BuffableFloat());

                return entity;
            }
            Entity AddBuffableCurrentHealthFloatModule(Player authoring)
            {
                var entity = CreateAdditionalEntity(TransformUsageFlags.None, false, "Current Health");
                AddComponent(entity, new FloatModule()
                {
                    BaseValue = authoring.CurrentHealth
                });
                AddComponent(entity, new BuffableFloat());

                return entity;
            }
        }
    }

    public class PlayerData : IComponentData
    {
        public Player player_ref;
    }
}