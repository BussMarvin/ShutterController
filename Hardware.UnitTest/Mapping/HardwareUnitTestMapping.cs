using Autofac;
using Database.Contract.DataModels;
using Hardware.Contract.Interfaces.Components;
using Hardware.Contract.Interfaces.Components.Extension;
using Hardware.Contract.Interfaces.Extension;
using Hardware.Mapping;
using Moq;

namespace Hardware.UnitTest.Mapping;

internal class HardwareUnitTestMapping : HardwareMapping
{
    public Mock<IGpio> MockGpio { get; } = new();

    public Mock<IGpio> RelayLeftGpio { get; } = new();

    public Mock<IGpio> RelayRightGpio { get; } = new();

    public Mock<IGpio> Bg11Gpio { get; } = new();

    public Mock<IGpio> Bg21Gpio { get; } = new();

    protected override void RegisterGpio(ContainerBuilder builder)
    {
        MockGpio.As<ISetGpioMode>()
            .As<ISetDescription<IGpio, Equipment_DataModel>>()
            .As<IDescription<Equipment_DataModel>>();

        RelayLeftGpio.As<ISetGpioMode>().As<ISetDescription<IGpio, Equipment_DataModel>>()
            .As<IDescription<Equipment_DataModel>>();

        RelayRightGpio.As<ISetGpioMode>().As<ISetDescription<IGpio, Equipment_DataModel>>()
            .As<IDescription<Equipment_DataModel>>();

        Bg11Gpio.As<ISetGpioMode>().As<ISetDescription<IGpio, Equipment_DataModel>>().As<IDescription<Equipment_DataModel>>();

        Bg21Gpio.As<ISetGpioMode>().As<ISetDescription<IGpio, Equipment_DataModel>>().As<IDescription<Equipment_DataModel>>();

        builder.RegisterInstance(MockGpio.Object).As<IGpio>().SingleInstance();
    }


    protected override void RegisterSensor(ContainerBuilder builder)
    {
        base.RegisterSensor(builder);
        RegisterSensor(builder, Bg11Gpio.Object, nameof(Bg11Gpio));
        RegisterSensor(builder, Bg21Gpio.Object, nameof(Bg21Gpio));
    }

    protected override void RegisterEngine(ContainerBuilder builder)
    {
        RegisterEngine(builder, RelayLeftGpio.Object, RelayRightGpio.Object);
    }


    protected override void RegisterFunctiongroups(ContainerBuilder builder)
    {
        RegisterFunctiongroupHb(builder);
        RegisterFunctiongroupHe(builder, nameof(Bg11Gpio), nameof(Bg21Gpio));
    }
}