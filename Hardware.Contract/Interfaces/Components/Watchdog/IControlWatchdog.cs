namespace Hardware.Contract.Interfaces.Components.Watchdog;

public interface IControlWatchdog
{
    void StartWatchdog();

    void StopWatchdog();
}