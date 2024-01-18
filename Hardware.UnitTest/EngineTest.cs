using Autofac;
using Hardware.Contract.Exceptions;
using Hardware.Contract.Interfaces.Components;
using Hardware.UnitTest.Mapping;
using Moq;

namespace Hardware.UnitTest;

internal class EngineTest
{
    private readonly HardwareUnitTestMapping _hardwareMapping = new();
    private IContainer _container;

    [SetUp]
    public void Setup()
    {
        ContainerBuilder builder = new();
        builder.RegisterModule(_hardwareMapping);
        _container = builder.Build();
    }


    [Test]
    public void DriveLeftTest()
    {
        _hardwareMapping.RelayLeftGpio.Setup(x => x.SetPinHigh());
        _hardwareMapping.RelayLeftGpio.Setup(x => x.SetPinLow());

        IEngine engine = _container.Resolve<IEngine>();

        engine.DriveLeft();

        _hardwareMapping.RelayLeftGpio.Verify(x => x.SetPinHigh(), Times.Once);
        _hardwareMapping.RelayLeftGpio.Verify(x => x.SetPinLow(), Times.Never);
    }

    [Test]
    public void DriveRightTest()
    {
        _hardwareMapping.RelayRightGpio.Setup(x => x.SetPinHigh());
        _hardwareMapping.RelayRightGpio.Setup(x => x.SetPinLow());

        IEngine engine = _container.Resolve<IEngine>();

        engine.DriveRight();

        _hardwareMapping.RelayRightGpio.Verify(x => x.SetPinHigh(), Times.Once);
        _hardwareMapping.RelayRightGpio.Verify(x => x.SetPinLow(), Times.Never);
    }

    [Test]
    public void DriveRightAfterDriveLeftTest()
    {
        _hardwareMapping.RelayLeftGpio.Setup(x => x.SetPinHigh());
        _hardwareMapping.RelayLeftGpio.Setup(x => x.SetPinLow());

        _hardwareMapping.RelayRightGpio.Setup(x => x.SetPinHigh());
        _hardwareMapping.RelayRightGpio.Setup(x => x.SetPinLow());

        IEngine engine = _container.Resolve<IEngine>();

        engine.DriveLeft();

        Assert.Throws<EngineDriveException>(() => engine.DriveRight());


        _hardwareMapping.RelayLeftGpio.Verify(x => x.SetPinHigh(), Times.Once);
        _hardwareMapping.RelayLeftGpio.Verify(x => x.SetPinLow(), Times.Never);

        _hardwareMapping.RelayRightGpio.Verify(x => x.SetPinHigh(), Times.Never);
        _hardwareMapping.RelayRightGpio.Verify(x => x.SetPinLow(), Times.Never);
    }


    [Test]
    public void DriveLeftAfterDriveRightTest()
    {
        _hardwareMapping.RelayLeftGpio.Setup(x => x.SetPinHigh());
        _hardwareMapping.RelayLeftGpio.Setup(x => x.SetPinLow());

        _hardwareMapping.RelayRightGpio.Setup(x => x.SetPinHigh());
        _hardwareMapping.RelayRightGpio.Setup(x => x.SetPinLow());

        IEngine engine = _container.Resolve<IEngine>();

        engine.DriveRight();

        Assert.Throws<EngineDriveException>(() => engine.DriveLeft());

        _hardwareMapping.RelayLeftGpio.Verify(x => x.SetPinHigh(), Times.Never);
        _hardwareMapping.RelayLeftGpio.Verify(x => x.SetPinLow(), Times.Never);

        _hardwareMapping.RelayRightGpio.Verify(x => x.SetPinHigh(), Times.Once);
        _hardwareMapping.RelayRightGpio.Verify(x => x.SetPinLow(), Times.Never);
    }

    [Test]
    public void StopAfterDriveRightTest()
    {
        _hardwareMapping.RelayLeftGpio.Setup(x => x.SetPinHigh());
        _hardwareMapping.RelayLeftGpio.Setup(x => x.SetPinLow());

        _hardwareMapping.RelayRightGpio.Setup(x => x.SetPinHigh());
        _hardwareMapping.RelayRightGpio.Setup(x => x.SetPinLow());

        IEngine engine = _container.Resolve<IEngine>();

        engine.DriveRight();
        engine.Stop();

        _hardwareMapping.RelayLeftGpio.Verify(x => x.SetPinHigh(), Times.Never);
        _hardwareMapping.RelayLeftGpio.Verify(x => x.SetPinLow(), Times.Once);

        _hardwareMapping.RelayRightGpio.Verify(x => x.SetPinHigh(), Times.Once);
        _hardwareMapping.RelayRightGpio.Verify(x => x.SetPinLow(), Times.Once);
    }
}