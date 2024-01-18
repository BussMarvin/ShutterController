using System.Device.Gpio;
using Database.Contract.DataModels;
using Hardware.Contract.Interfaces.Components;
using Hardware.Contract.Interfaces.Extension;

namespace Hardware.Components;

internal class RaspberryPi : IRaspberryPi, ISetDescription<IRaspberryPi, Equipment_DataModel>, IDescription<Equipment_DataModel>, IDisposable
{
    public Equipment_DataModel Description { get; private set; }

    public void Dispose()
    {
        Controller.Dispose();
    }

    public GpioController Controller { get; } = new();


    public IRaspberryPi SetDescription(Equipment_DataModel description)
    {
        Description = description ?? throw new ArgumentNullException(nameof(description));
        return this;
    }
}