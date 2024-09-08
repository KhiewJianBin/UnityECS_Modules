using System;

/// <summary>
/// Base Buff Component
/// </summary>
public interface IStackableBuffComponent : IBuffComponent
{
    public StackType StackTypes { get; set; }

    [Flags]
    public enum StackType
    {
        None = 0,
        Value = 1, //Add together buff values 
        Duration = 2 //Add together buff duration
    }
}