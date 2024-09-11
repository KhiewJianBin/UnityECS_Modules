using Unity.Entities;

public struct EmptyStackableBuffComponent : IComponentData
{
    public float DurationTimer;
    public float Value;
    public BuffTypes BuffType;

    public StackTypes StackType;

    public void Stack(EmptyStackableBuffComponent esbc)
    {
        
    }
}