using System.Device.Gpio;

namespace Hardware.Contract.Interfaces.Components;

public interface IRaspberryPi
{
    GpioController Controller { get; }
}