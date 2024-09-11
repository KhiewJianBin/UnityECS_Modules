using System;

[Flags]
public enum StackTypes
{
    None = 0,
    Value = 1, //Add together buff values 
    Duration = 2 //Add together buff duration
}