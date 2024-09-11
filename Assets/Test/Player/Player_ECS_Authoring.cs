using UnityEngine;
using Unity.Entities;

public class Player_ECS_Authoring : MonoBehaviour
{
    /// <summary>
    /// Setup the Scene
    /// </summary>
    class Baker : Baker<Player_ECS_Authoring>
    {
        public override void Bake(Player_ECS_Authoring authoring)
        {
            // 1. Create the initial systems in the world
            var healthSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<HealthModuleSystem>();
            var healthbuffSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<BaseHealthBuffSystem>();
            var BuffableFloatSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<BuffableFloatSystem>();

            var PlayerSpawnerSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<PlayerSpawnerSystem>();

            // 2. Find Existing SystemGroup to insert the system into
            var InitSG = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>();

            var BuffStartSG = World.DefaultGameObjectInjectionWorld.CreateSystemManaged<BuffStartSystemGroup>();
            var ModuleSG = World.DefaultGameObjectInjectionWorld.CreateSystemManaged<ModuleSystemGroup>();
            var BuffResetSG = World.DefaultGameObjectInjectionWorld.CreateSystemManaged<BuffResetSystemGroup>();
            InitSG.AddSystemToUpdateList(BuffStartSG);
            InitSG.AddSystemToUpdateList(ModuleSG);
            InitSG.AddSystemToUpdateList(BuffResetSG);

            // 3. Add System to Appropriate Group
            InitSG.AddSystemToUpdateList(PlayerSpawnerSystemHandle);

            BuffStartSG.AddSystemToUpdateList(healthbuffSystemHandle);
            ModuleSG.AddSystemToUpdateList(healthSystemHandle);
            //BuffResetSG.AddSystemToUpdateList(BuffableFloatSystemHandle);
        }
    }
}