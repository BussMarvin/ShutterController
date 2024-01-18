using System.Device.Gpio;
using Database.Contract.DataModels;
using Hardware.Contract.Interfaces.Components;
using Hardware.Contract.Interfaces.Components.Extension;
using Hardware.Contract.Interfaces.Extension;

namespace Hardware.Components;

internal class Gpio : IGpio, ISetGpioMode, ISetDescription<IGpio, Equipment_DataModel>, IDescription<Equipment_DataModel>
{
    private readonly IRaspberryPi _raspberryPi;

    public Gpio(IRaspberryPi raspberryPi)
    {
        _raspberryPi = raspberryPi ?? throw new ArgumentNullException(nameof(raspberryPi));
    }

    public Equipment_DataModel Description { get; private set; }
    public PinMode GpioMode { get; private set; }


    public bool ReadPinState()
    {
        if (GpioMode != PinMode.Input) throw new InvalidOperationException("Pin is not an Input");


        return (bool)_raspberryPi.Controller.OpenPin(Description.Gpio, GpioMode).Read();
    }


    public void SetPinHigh()
    {
        if (GpioMode != PinMode.Input) throw new InvalidOperationException("Pin is not an Output");


        _raspberryPi.Controller.OpenPin(Description.Gpio, GpioMode).Write(PinValue.High);
    }

    public void SetPinLow()
    {
        if (GpioMode != PinMode.Input) throw new InvalidOperationException("Pin is not an Output");


        _raspberryPi.Controller.OpenPin(Description.Gpio, GpioMode).Write(PinValue.Low);
    }

    public IGpio SetDescription(Equipment_DataModel description)
    {
        Description = description ?? throw new ArgumentNullException(nameof(description));
        return this;
    }

    public IGpio SetGpioMode(PinMode gpioMode)
    {
        GpioMode = gpioMode;
        return this;
    }
}