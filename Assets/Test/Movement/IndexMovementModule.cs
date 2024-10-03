using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// 3D Grid Movement
/// </summary>
public class IndexMovementModule : MovementModule
{
    public int xPos = 0;
    public int yPos = 0;
    public int zPos = 0;
    public Vector3 Index_To_Unity_Pos_Scale = Vector3.one;

    InputTrigger Forward = new InputTrigger();
    InputTrigger Backward = new InputTrigger();
    InputTrigger Left = new InputTrigger();
    InputTrigger Right = new InputTrigger();
    InputTrigger Up = new InputTrigger();
    InputTrigger Down = new InputTrigger();

    void Awake()
    {
        InputTriggers = new Dictionary<string, InputTrigger>();
        InputTriggers.Add("Forward", Forward);
        InputTriggers.Add("Backward", Backward);
        InputTriggers.Add("Left", Left);
        InputTriggers.Add("Right", Right);
        InputTriggers.Add("Up", Up);
        InputTriggers.Add("Down", Down);
    }

    protected override void MovementModuleLogic()
    {
        if (Forward.value)
        {
            zPos++;
        }
        else if (Backward.value)
        {
            zPos--;
        }
        else if (Left.value)
        {
            xPos--;
        }
        else if (Right.value)
        {
            xPos++;
        }
        else if (Up.value)
        {
            yPos++;
        }
        else if (Down.value)
        {
            yPos--;
        }

        transform.position = GetUnityPos(xPos,yPos,zPos, Index_To_Unity_Pos_Scale);
    }

    public Vector3 GetUnityPos(int ix,int iy,int iz, Vector3 scale)
    {
        return new Vector3(
           ix * scale.x,
           iy * scale.y,
           iz * scale.z);
    }
}
