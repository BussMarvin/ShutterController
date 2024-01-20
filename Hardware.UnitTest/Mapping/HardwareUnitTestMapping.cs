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

    protected override void RegisterGpio(ContainerBuilder builder)
    {
        MockGpio.As<ISetGpioMode>()
            .As<ISetDescription<IGpio, Equipment_DataModel>>()
            .As<IDescription<Equipment_DataModel>>();


        builder.RegisterInstance(MockGpio.Object).As<IGpio>().SingleInstance();
    }


    protected override void RegisterEngine(ContainerBuilder builder)
    {
        RegisterEngine(builder, RelayLeftGpio.Object, RelayRightGpio.Object);
    }


    protected override void RegisterFunctiongroups(ContainerBuilder builder)
    {
        RegisterFunctiongroupHb(builder);
    }
}