using System.Device.Gpio;
using Database.Contract.DataModels;
using Hardware.Contract.Exceptions;
using Hardware.Contract.Interfaces.Components;
using Hardware.Contract.Interfaces.Components.Extension;
using Hardware.Contract.Interfaces.Extension;

namespace Hardware.Components;

internal class Engine : IEngine, ISetDescription<IEngine, Engine_DataModel>, IDescription<Engine_DataModel>
{
    private readonly IGpio _relayLeftRotation;
    private readonly IGpio _relayRightRotation;

    private bool _engineIsDriveLeft;
    private bool _engineIsDriveRight;

    public Engine(IGpio relayLeftRotation, IGpio relayRightRotation)
    {
        _relayLeftRotation = relayLeftRotation ?? throw new ArgumentNullException(nameof(relayLeftRotation));
        _relayRightRotation = relayRightRotation ?? throw new ArgumentNullException(nameof(relayRightRotation));

        if (_relayLeftRotation is ISetGpioMode relayLeftGpio) relayLeftGpio.SetGpioMode(PinMode.Output);

        if (_relayRightRotation is ISetGpioMode relayRightGpio) relayRightGpio.SetGpioMode(PinMode.Output);
    }


    public Engine_DataModel Description { get; private set; }

    public void DriveRight()
    {
        if (_engineIsDriveLeft) throw new EngineDriveException("Engine is Drive Left");
        _engineIsDriveRight = true;
        _relayRightRotation.SetPinHigh();
    }

    public void DriveLeft()
    {
        if (_engineIsDriveRight) throw new EngineDriveException("Engine is Drive Right");
        _engineIsDriveLeft = true;
        _relayLeftRotation.SetPinHigh();
    }

    public void Stop()
    {
        _relayRightRotation.SetPinLow();
        _relayLeftRotation.SetPinLow();
        _engineIsDriveLeft = false;
        _engineIsDriveRight = false;
    }

    public IEngine SetDescription(Engine_DataModel description)
    {
        Description = description ?? throw new ArgumentNullException(nameof(description));

        if (_relayLeftRotation is ISetDescription<IGpio, Equipment_DataModel> relayLeftDescription)
            relayLeftDescription.SetDescription(description.RelayLeftNavigation);

        if (_relayRightRotation is ISetDescription<IGpio, Equipment_DataModel> relayRightDescription)
            relayRightDescription.SetDescription(description.RelayRightNavigation);

        return this;
    }
}