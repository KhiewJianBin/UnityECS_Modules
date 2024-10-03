using System.Collections.Generic;

using UnityEngine;

public class VectorMovementModule : MovementModule
{
    public SpeedModule speedmodule;
    [SerializeField] float speed;

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
        speed = speedmodule.Speed;

        if (Forward.value)
        {
            transform.Translate(new Vector3(0, 0, 1) * speed * Time.deltaTime);
        }
        if (Backward.value)
        {
            transform.Translate(new Vector3(0, 0, -1) * speed * Time.deltaTime);
        }
        if (Left.value)
        {
            transform.Translate(new Vector3(-1, 0, 0) * speed * Time.deltaTime);
        }
        if (Right.value)
        {
            transform.Translate(new Vector3(1, 0, 0) * speed * Time.deltaTime);
        }
        if (Up.value)
        {
            transform.Translate(new Vector3(0, 1, 0) * speed * Time.deltaTime);
        }
        if (Down.value)
        {
            transform.Translate(new Vector3(0, -1, 0) * speed * Time.deltaTime);
        }
    }
}