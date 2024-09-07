using System;
using Unity.Entities;

/// <summary>
/// Base Buff Component
/// </summary>
public struct StackableBuffComponent :
    IComponentData // Data Component
{
    public int PriorityQueue;
    public float DurationTimer;
    public float Value;

    public StackableBuffComponent(int PriorityQueue, float DurationTimer, float Value)
    {
        this.PriorityQueue = PriorityQueue;
        this.DurationTimer = DurationTimer;
        this.Value = Value;
        StackTypes = default;
    }

    public StackType StackTypes;

    [Flags]
    public enum StackType
    {
        None = 0,
        Value = 1, //Add together buff values 
        Duration = 2 //Add together buff duration
    }
}