namespace Hardware.Contract.Interfaces.Components.Watchdog;

public interface ISetWatchdogTime
{
    IWatchdog SetWatchdogTime(double time = 100);
}