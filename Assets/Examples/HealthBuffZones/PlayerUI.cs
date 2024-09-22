namespace Examples.HealthBuffZones
{
    using System.Collections.Generic;
    using TMPro;
    using Unity.Collections;
    using Unity.Entities;
    using Unity.Transforms;
    using UnityEngine;

    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] GameObject HealthUIPrefab;

        List<GameObject> HealthUIs = new();

        void Update()
        {
            var em = World.DefaultGameObjectInjectionWorld.EntityManager;

            var queryDesc = new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<LocalTransform>(),
                    ComponentType.ReadOnly<HealthModule>(),
                }
            };
            EntityQuery query = em.CreateEntityQuery(queryDesc);
            var playerEntities = query.ToEntityArray(Allocator.TempJob);


            

            int moreSpawnCount = playerEntities.Length - HealthUIs.Count;
            for (int i = 0; i < moreSpawnCount; i++)
            {
                var newUI = Instantiate(HealthUIPrefab, transform);
                HealthUIs.Add(newUI);
            }
            for (int i = playerEntities.Length - 1; i < HealthUIs.Count; i++)
            {
                HealthUIs[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < playerEntities.Length; i++)
            {
                var entity = playerEntities[i];
                var localtransform = em.GetComponentData<LocalTransform>(entity);
                var health = em.GetComponentData<HealthModule>(entity);

                var healthUI = HealthUIs[i];

                healthUI.gameObject.SetActive(true);
                healthUI.transform.position = localtransform.Position;

                healthUI.GetComponentInChildren<TMP_Text>().text = $"Health: {health.CurrentHealth}/{health.BaseHealth}";
            }
        }
    }
}