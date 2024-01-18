namespace Hardware.Contract.Interfaces.Components;

public interface IWatchdogActivator
{
    void ActivateWatchdog(double interval = 100);
}