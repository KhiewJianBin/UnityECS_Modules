using UnityEngine;
using Unity.Entities;

public class Player_AuthoringSystem : MonoBehaviour
{
    /// <summary>
    /// Setup the Scene
    /// </summary>
    class Baker : Baker<Player_AuthoringSystem>
    {
        public override void Bake(Player_AuthoringSystem authoring)
        {
            // 1. Create the initial systems in the world
            var healthSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<HealthModuleSystem>();
            var healthbuffSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<BaseHealthBuff_StackableSystem>();
            var BuffableFloatSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<BuffableFloatSystem>();

            var PlayerSpawnerSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<PlayerSpawnerSystem>();
            var PlayerUpdateSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<PlayerUpdateSystem>();

            var GiveBuffSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<Affector_GiveBaseHealthBuffSystem>();

            var GravityFactorHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<Affector_GravityFactorSystem>();
            
            // 2. Find Existing SystemGroup to insert the system into
            var InitSG = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>();
            var SimSG = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>();
            var PresentSG = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<PresentationSystemGroup>();

            var BuffStartSG = World.DefaultGameObjectInjectionWorld.CreateSystemManaged<BuffStartSystemGroup>();
            var ModuleSG = World.DefaultGameObjectInjectionWorld.CreateSystemManaged<ModuleSystemGroup>();
            var BuffResetSG = World.DefaultGameObjectInjectionWorld.CreateSystemManaged<BuffResetSystemGroup>();

            // 3. Add System to Appropriate Group

            // ========================  InitializationSystemGroup   ==============================
            InitSG.AddSystemToUpdateList(PlayerSpawnerSystemHandle);
            InitSG.AddSystemToUpdateList(GiveBuffSystemHandle);
            InitSG.AddSystemToUpdateList(GravityFactorHandle);

            // ===========================  SimulationSystemGroup       ===========================
            SimSG.AddSystemToUpdateList(BuffStartSG);
            {
                BuffStartSG.AddSystemToUpdateList(healthbuffSystemHandle);
            }
            SimSG.AddSystemToUpdateList(ModuleSG);
            {
                ModuleSG.AddSystemToUpdateList(healthSystemHandle);
            }
            SimSG.AddSystemToUpdateList(BuffResetSG);
            {
                BuffResetSG.AddSystemToUpdateList(BuffableFloatSystemHandle);
            }

            // ===========================  PresentationSystemGroup  ===========================
            PresentSG.AddSystemToUpdateList(PlayerUpdateSystemHandle);
        }
    }
}