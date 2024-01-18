using Database.Contract.DataModels;
using Hardware.Contract.Interfaces.Components;
using Hardware.Contract.Interfaces.Components.Extension;
using Hardware.Contract.Interfaces.Extension;

namespace Hardware.Components;

internal class Debounce : IDebounce, ISetDebounceTime, ISetDescription<IDebounce, Equipment_DataModel>, IDescription<Equipment_DataModel>
{
    private readonly ISensor _sensor;
    private int _debounceTime = 30;

    public Debounce(ISensor sensor)
    {
        _sensor = sensor ?? throw new ArgumentNullException(nameof(sensor));
    }

    public bool OldStateWasOccupied => _sensor.OldStateWasOccupied;

    public bool IsOccupied()
    {
        if (_sensor.IsOccupied() && !_sensor.OldStateWasOccupied)
        {
            Thread.Sleep(_debounceTime);

            if (_sensor.IsOccupied()) return true;
        }

        return false;
    }

    public Equipment_DataModel Description { get; private set; }

    public IDebounce SetDebounceTime(int debounceTime)
    {
        _debounceTime = debounceTime;
        return this;
    }


    public IDebounce SetDescription(Equipment_DataModel description)
    {
        Description = description ?? throw new ArgumentNullException(nameof(description));
        if (_sensor is ISetDescription<ISensor, Equipment_DataModel> sensor)
            sensor.SetDescription(description);

        return this;
    }
}