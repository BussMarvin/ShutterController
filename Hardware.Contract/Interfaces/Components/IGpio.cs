using System.Device.Gpio;

namespace Hardware.Contract.Interfaces.Components;

public interface IGpio
{
    public PinMode GpioMode { get; }

    bool ReadPinState();
    void SetPinHigh();
    void SetPinLow();
}