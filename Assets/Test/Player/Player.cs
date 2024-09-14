using Unity.Entities;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Health;

    public Entity entityRef;

    void Start()
    {
        //em = World.DefaultGameObjectInjectionWorld.EntityManager;

        //// Add to a list which an ECS system will Spawn a copy of it as Entity+Data in ECS world
        //EntityQuery query = em.CreateEntityQuery(new ComponentType[] { typeof(PlayersSpawnerData) });
        //var a = query.GetSingleton<PlayersSpawnerData>();
        //a.PlayersToSpawn.Add(this);
    }

    class Baker : Baker<Player>
    {
        public override void Bake(Player authoring)
        {
            var baseHealthEntity = CreateAdditionalEntity(TransformUsageFlags.None, false, nameof(FloatModule));
            AddComponent(baseHealthEntity, new FloatModule()
            {
                BaseValue = Random.Range(0, 1000000)
            });
            AddComponent(baseHealthEntity, new BuffableFloat());

            var currentHealthEntity = CreateAdditionalEntity(TransformUsageFlags.None, false, nameof(BuffableFloat));
            AddComponent(currentHealthEntity, new FloatModule()
            {
                BaseValue = 99
            });

            var entityPlayer = GetEntity(TransformUsageFlags.None);
            AddComponent(entityPlayer, new HealthModule()
            {
                BaseHealth = 1,
                e_BaseHealth = baseHealthEntity,
                e_CurrentHealth = currentHealthEntity,
            });

            var buff = CreateAdditionalEntity(TransformUsageFlags.None, false, nameof(BaseHealthBuff));
            AddComponent(buff, new BaseHealthBuff()
            {
                Target = entityPlayer,

                DurationTimer = float.PositiveInfinity,
                Value = 1,
                BuffType = BuffTypes.ValueAdd,
            });

            AddComponentObject(entityPlayer, new PlayerData()
            {
                player_ref = authoring
            });
        }
    }
}