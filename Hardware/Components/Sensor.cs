using System.Device.Gpio;
using Database.Contract.DataModels;
using Hardware.Contract.Interfaces.Components;
using Hardware.Contract.Interfaces.Components.Extension;
using Hardware.Contract.Interfaces.Extension;

namespace Hardware.Components;

internal class Sensor : ISensor, IInvertSignal, ISetDescription<ISensor, Equipment_DataModel>, IDescription<Equipment_DataModel>
{
    private readonly IGpio _gpio;
    private bool _invertSignal;

    public Sensor(IGpio gpio)
    {
        _gpio = gpio ?? throw new ArgumentNullException(nameof(gpio));

        if (_gpio is ISetGpioMode gpioMode) gpioMode.SetGpioMode(PinMode.Input);


        // Call the Method IsOccupied() to set the OldStateWasOccupied Property
        IsOccupied();
    }

    public Equipment_DataModel Description { get; private set; }

    public ISensor InvertSignal(bool invertSignal = true)
    {
        _invertSignal = invertSignal;
        return this;
    }

    public bool OldStateWasOccupied { get; private set; }

    public bool CurrentStateIsOccupied { get; private set; }

    public bool IsOccupied()
    {
        bool signalIsTrueAndNotInverted = !_invertSignal && _gpio.ReadPinState();
        bool signalIsFalseAndInverted = _invertSignal && !_gpio.ReadPinState();

        OldStateWasOccupied = CurrentStateIsOccupied;

        if (signalIsTrueAndNotInverted || signalIsFalseAndInverted)
        {
            CurrentStateIsOccupied = true;
            return true;
        }

        CurrentStateIsOccupied = false;
        return false;
    }


    public ISensor SetDescription(Equipment_DataModel description)
    {
        Description = description ?? throw new ArgumentNullException(nameof(description));
        if (_gpio is ISetDescription<IGpio, Equipment_DataModel> gpio)
            gpio.SetDescription(description);


        return this;
    }
}