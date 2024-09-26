namespace Examples.GravityZones
{
    using UnityEngine;
    using Unity.Entities;

    public class GravityZones_Authoring : MonoBehaviour
    {
        class Baker : Baker<GravityZones_Authoring>
        {
            public override void Bake(GravityZones_Authoring authoring)
            {
                // 1. Create the initial systems in the world
                var healthSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<HealthModuleSystem>();

                var healthbuffSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<BaseHealthBuff_StackableSystem>();
                var BuffableFloatSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<BuffableFloatSystem>();

                var PlayerUpdateSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<PlayerUpdateSystem>();

                var GravityFactorSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<Affector_GravityFactorSystem>();
                var GiveGravityOverrideSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<Affector_GiveGravityOverrideSystem>();
                var gravityOverrideSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<GravityOverrideSystem>();

                // 2. Find Existing SystemGroup to insert the system into
                var InitSG = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>();
                var SimSG = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>();
                var PresentSG = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<PresentationSystemGroup>();

                var BuffStartSG = World.DefaultGameObjectInjectionWorld.CreateSystemManaged<BuffStartSystemGroup>();
                var ModuleSG = World.DefaultGameObjectInjectionWorld.CreateSystemManaged<ModuleSystemGroup>();
                var BuffResetSG = World.DefaultGameObjectInjectionWorld.CreateSystemManaged<BuffResetSystemGroup>();

                // 3. Add System to Appropriate Group

                // ========================  InitializationSystemGroup   ==============================
                InitSG.AddSystemToUpdateList(GravityFactorSystemHandle);
                InitSG.AddSystemToUpdateList(GiveGravityOverrideSystemHandle);
                //InitSG.AddSystemToUpdateList(gravityOverrideSystemHandle);

                // ===========================  SimulationSystemGroup       ===========================
                SimSG.AddSystemToUpdateList(BuffStartSG);
                {
                    BuffStartSG.AddSystemToUpdateList(healthbuffSystemHandle);
                    BuffStartSG.AddSystemToUpdateList(gravityOverrideSystemHandle);
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