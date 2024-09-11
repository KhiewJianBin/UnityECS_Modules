using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup))] // Set Parent Group
[DisableAutoCreation]
public partial class BuffResetSystemGroup : ComponentSystemGroup
{
    protected override void OnUpdate()
    {
        base.OnUpdate();
    }
}