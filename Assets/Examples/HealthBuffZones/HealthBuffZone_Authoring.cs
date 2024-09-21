namespace Examples.HealthBuffZones
{
    using UnityEngine;
    using Unity.Entities;

    public class HealthBuffZone_Authoring : MonoBehaviour
    {
        class Baker : Baker<HealthBuffZone_Authoring>
        {
            public override void Bake(HealthBuffZone_Authoring authoring)
            {
                // 1. Create the initial systems in the world
                var healthSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<HealthModuleSystem>();

                var healthbuffSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<BaseHealthBuff_StackableSystem>();
                var BuffableFloatSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<BuffableFloatSystem>();

                var PlayerUpdateSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<PlayerUpdateSystem>();

                var GiveBuffSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<Affector_GiveBaseHealthBuffSystem>();

                // 2. Find Existing SystemGroup to insert the system into
                var InitSG = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>();
                var SimSG = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>();
                var PresentSG = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<PresentationSystemGroup>();

                var BuffStartSG = World.DefaultGameObjectInjectionWorld.CreateSystemManaged<BuffStartSystemGroup>();
                var ModuleSG = World.DefaultGameObjectInjectionWorld.CreateSystemManaged<ModuleSystemGroup>();
                var BuffResetSG = World.DefaultGameObjectInjectionWorld.CreateSystemManaged<BuffResetSystemGroup>();

                // 3. Add System to Appropriate Group

                // ========================  InitializationSystemGroup   ==============================
                InitSG.AddSystemToUpdateList(GiveBuffSystemHandle);

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
}