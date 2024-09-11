using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup))] // Set Parent Group
[UpdateAfter(typeof(BuffStartSystemGroup))]
[UpdateBefore(typeof(BuffResetSystemGroup))]
[DisableAutoCreation]
public partial class ModuleSystemGroup : ComponentSystemGroup
{
    protected override void OnUpdate()
    {
        base.OnUpdate();
    }
}