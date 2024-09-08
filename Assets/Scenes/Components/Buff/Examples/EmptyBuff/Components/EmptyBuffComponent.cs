public struct EmptyBuffComponent : IBuffComponent
{
    public int PriorityQueue { get; set; }
    public float DurationTimer { get; set; }
    public float Value { get; set; }

    public EmptyBuffComponent(int PriorityQueue, float DurationTimer, float Value)
    {
        this.PriorityQueue = PriorityQueue;
        this.DurationTimer = DurationTimer;
        this.Value = Value;
    }
}