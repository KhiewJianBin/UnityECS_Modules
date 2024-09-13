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

        // Add to a list which an ECS system will Spawn a copy of it as Entity+Data in ECS world
        EntityQuery query = em.CreateEntityQuery(new ComponentType[] { typeof(PlayersSpawnerData) });
        var a = query.GetSingleton<PlayersSpawnerData>();
        a.PlayersToSpawn.Add(this);
    }
}