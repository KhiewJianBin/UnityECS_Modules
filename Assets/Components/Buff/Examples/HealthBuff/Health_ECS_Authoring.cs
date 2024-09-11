using UnityEngine;
using Unity.Entities;

public class Health_ECS_Authoring : MonoBehaviour
{
    /// <summary>
    /// Setup the Scene
    /// </summary>
    class Baker : Baker<Health_ECS_Authoring>
    {
        public override void Bake(Health_ECS_Authoring authoring)
        {
            // 1. Create the initial systems in the world
            var healthSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<HealthModuleSystem>();
            var healthbuffSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<BaseHealthBuffSystem>();
            var BuffableFloatSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<BuffableFloatSystem>();

            // 2. Find Existing SystemGroup to insert the system into
            var SimSG = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>();

            // 3. Add System to Appropriate Group
            if (SimSG != null)
            {
                SimSG.AddSystemToUpdateList(healthSystemHandle);
                SimSG.AddSystemToUpdateList(healthbuffSystemHandle);
                SimSG.AddSystemToUpdateList(BuffableFloatSystemHandle);
            }
        }
    }
}