using TMPro;
using Unity.Entities;
using UnityEngine;

public class Player : MonoBehaviour
{
    public TMP_Text healthtext;

    public Entity entityRef;

    EntityManager em;
    void Start()
    {
        em = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityQuery colorTablesQ = em.CreateEntityQuery(new ComponentType[] { typeof(PlayerSpawnerData) });
        var a = colorTablesQ.GetSingleton<PlayerSpawnerData>();
        a.PlayersToSpawn.Add(this);
    }
    void Update()
    {
        if (!em.HasComponent<HealthModule>(entityRef)) return;

        var hp = em.GetComponentData<HealthModule>(entityRef); 
        healthtext.text = "Your Health is " + hp.BaseHealth.ToString();
    }
}
