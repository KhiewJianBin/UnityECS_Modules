using Unity.Entities;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Health;

    public Entity entityRef;

    EntityManager em;
    void Start()
    {
        em = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityQuery query = em.CreateEntityQuery(new ComponentType[] { typeof(PlayersSpawnerData) });
        var a = query.GetSingleton<PlayersSpawnerData>();
        a.PlayersToSpawn.Add(this);
    }
    void Update()
    {
        if (!em.HasComponent<HealthModule>(entityRef)) return;
        var hp = em.GetComponentData<HealthModule>(entityRef);
        Health = hp.BaseHealth;
    }
}