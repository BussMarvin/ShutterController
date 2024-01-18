namespace Hardware.Contract.Interfaces.Components.Watchdog;

public interface IWatchdog
{
    event Action<bool> SensorIsTriggered;

    IControlWatchdog SetWatchdogFunction(IOccupied monitoredSensor);
}