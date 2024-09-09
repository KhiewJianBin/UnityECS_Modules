using UnityEngine;
using Unity.Entities;

public class EmptyBuff_Authoring : MonoBehaviour
{
    /// <summary>
    /// Convert the GameObject
    /// </summary>
    class Baker : Baker<EmptyBuff_Authoring>
    {
        public override void Bake(EmptyBuff_Authoring authoring)
        {
            // 1. Get Enitity using only required TransformUsage Flag
            var entity = GetEntity(TransformUsageFlags.None);

            // 2. Add Entity Components (using data from authoring monobehaviour if needed)
            AddComponent(entity, new EmptyBuffComponent
            {
                PriorityQueue = 0,
                DurationTimer = 10,
                Value = 1,
            });

            // Option 1. Add additioanl Entity/+ Component if require
        }
    }
}