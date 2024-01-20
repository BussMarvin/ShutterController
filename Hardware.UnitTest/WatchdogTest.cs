using Autofac;
using Hardware.Contract.Interfaces.Components;
using Hardware.Contract.Interfaces.Components.Watchdog;
using Hardware.UnitTest.Mapping;
using Moq;

namespace Hardware.UnitTest;

internal class WatchdogTest
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
    public void EventTriggerTest()
    {
        bool triggeredValue = false;
        Mock<IOccupied> occupiedMock = new();
        occupiedMock.Setup(x => x.IsOccupied()).Returns(false);

        IWatchdog watchdog = _container.Resolve<IWatchdog>();

        watchdog.SensorIsTriggered += value => triggeredValue = value;
        watchdog.SetWatchdogFunction(occupiedMock.Object).StartWatchdog();

        Assert.That(triggeredValue, Is.False);

        occupiedMock.Setup(x => x.IsOccupied()).Returns(true);

        Assert.That(() => triggeredValue, Is.False.After(2).Seconds.PollEvery(100));
    }


    [Test]
    public void CheckEventTriggerdOnceTest()
    {
        int triggeredValue = 0;
        Mock<IOccupied> occupiedMock = new();
        occupiedMock.Setup(x => x.IsOccupied()).Returns(false);

        IWatchdog watchdog = _container.Resolve<IWatchdog>();
        watchdog.SensorIsTriggered += _ => triggeredValue++;

        watchdog.SetWatchdogFunction(occupiedMock.Object).StartWatchdog();

        Assert.That(triggeredValue, Is.EqualTo(0));

        occupiedMock.Setup(x => x.IsOccupied()).Returns(true);

        Assert.That(() => triggeredValue, Is.EqualTo(1).After(1).Seconds);
    }
}