using UnityEngine;
using Unity.Entities;

public class EmptyStackableBuff_ECS : MonoBehaviour
{
    /// <summary>
    /// Setup the Scene
    /// </summary>
    class Baker : Baker<EmptyStackableBuff_ECS>
    {
        public override void Bake(EmptyStackableBuff_ECS authoring)
        {
            // 1. Create the initial systems in the world
            var buffSystemHandle = World.DefaultGameObjectInjectionWorld.CreateSystem<EmptyStackableBuffSystem>();

            // 2. Find Existing SystemGroup to insert the system into
            var SimSG = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>();

            // 3. Add System to Appropriate Group
            if (SimSG != null)
            {
                SimSG.AddSystemToUpdateList(buffSystemHandle);
            }
        }
    }
}