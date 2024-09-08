using Unity.Entities;

[ConverterVersion("unity", 1)]
public class DoohickeyConversionSystem : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((Doohickey doohickey) =>
        {
            AddHybridComponent(doohickey);
        });
    }
}