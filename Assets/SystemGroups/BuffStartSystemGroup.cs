using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup))] // Set Parent Group
[DisableAutoCreation]
public partial class BuffStartSystemGroup : ComponentSystemGroup
{
    protected override void OnUpdate()
    {
        base.OnUpdate();
    }
}