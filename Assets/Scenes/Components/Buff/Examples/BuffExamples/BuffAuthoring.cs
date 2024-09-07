using UnityEngine;
using Unity.Entities;

public class BuffAuthoring : MonoBehaviour
{
    /// <summary>
    /// Convert the GameObject
    /// </summary>
    class Baker : Baker<BuffAuthoring>
    {
        public override void Bake(BuffAuthoring authoring)
        {
            // 1. Get Enitity using only required TransformUsage Flag
            var entity = GetEntity(TransformUsageFlags.None);

            // 2. Add Entity Components (using data from authoring monobehaviour if needed)
            AddComponent(entity, new BuffComponent
            {
                PriorityQueue = 0,
                DurationTimer = 10,
                Value = 1,
            });

            // Option 1. Add additioanl Entity/+ Component if require
        }
    }
}