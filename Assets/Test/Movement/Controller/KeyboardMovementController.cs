using AEM.Inputs;
using UnityEngine;

/// <summary>
/// keyboard WASD+QE movement
/// </summary>
public class KeyboardMovementController : AEMController
{
    public AEMModule Module;
    public KeyCode ForwardKey = KeyCode.W;
    public KeyCode BackwardKey = KeyCode.S;
    public KeyCode LeftKey = KeyCode.A;
    public KeyCode RightKey = KeyCode.D;
    public KeyCode UpKey = KeyCode.Q;
    public KeyCode DownKey = KeyCode.E;

    void Start ()
    {
        if (!Module)
            Debug.LogWarning(this + "No Module assigned");
    }

    void Update()
    {
        if (Module)
            Control();
    }
    protected override void Control()
    {
        Module.InputTriggers["Forward"].value = InputManager.GetKeyboardPress("Forward", ForwardKey);
        Module.InputTriggers["Backward"].value = InputManager.GetKeyboardPress("Backward", BackwardKey);
        Module.InputTriggers["Left"].value = InputManager.GetKeyboardPress("Left", LeftKey);
        Module.InputTriggers["Right"].value = InputManager.GetKeyboardPress("Right", RightKey);
        Module.InputTriggers["Up"].value = InputManager.GetKeyboardPress("Up", UpKey);
        Module.InputTriggers["Down"].value = InputManager.GetKeyboardPress("Down", DownKey);
    }
    public override void Remove()
    {
        InputManager.RemoveInput("Forward");
        InputManager.RemoveInput("Backward");
        InputManager.RemoveInput("Left");
        InputManager.RemoveInput("Right");
        Destroy(this);
    }
}