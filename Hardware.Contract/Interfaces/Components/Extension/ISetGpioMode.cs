using System.Device.Gpio;

namespace Hardware.Contract.Interfaces.Components.Extension;

public interface ISetGpioMode
{
    IGpio SetGpioMode(PinMode gpioMode);
}