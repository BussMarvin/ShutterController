using Hardware.Contract.Interfaces.Components;
using Hardware.Contract.Interfaces.Components.Watchdog;
using Timer = System.Timers.Timer;

namespace Hardware.Functiongroups;

internal class Watchdog : IWatchdog, ISetWatchdogTime, IControlWatchdog
{
    private readonly Timer _timer = new();


    private bool _sensorWasOccupied;

    public Watchdog()
    {
        _timer.AutoReset = true;
    }

    public void StartWatchdog()
    {
        _timer.Start();
    }

    public void StopWatchdog()
    {
        _timer.Stop();
    }


    public IWatchdog SetWatchdogTime(double time)
    {
        _timer.Interval = time;
        return this;
    }

    public event Action<bool> SensorIsTriggered;

    public IControlWatchdog SetWatchdogFunction(IOccupied monitoredSensor)
    {
        _timer.Elapsed += (_, _) =>
        {
            if (monitoredSensor.IsOccupied() && !_sensorWasOccupied)
                FireOccupied();
            else if (!monitoredSensor.IsOccupied() && _sensorWasOccupied)
                FireNotOccupied();
        };
        return this;
    }

    private void FireOccupied()
    {
        if (!_sensorWasOccupied)
        {
            _sensorWasOccupied = true;
            SensorIsTriggered?.Invoke(true);
        }
    }

    private void FireNotOccupied()
    {
        if (_sensorWasOccupied)
        {
            _sensorWasOccupied = false;
            SensorIsTriggered?.Invoke(false);
        }
    }
}