public abstract class MovementModule : AEMModule
{
    void Update()
    {
        MovementModuleLogic();
    }

    protected abstract void MovementModuleLogic();
}
