using Autofac;
using AutoFixture;
using Database.Contract.DataModels;
using Hardware.Contract.Interfaces.Components;
using Hardware.Contract.Interfaces.Extension;
using Hardware.Contract.Interfaces.Functiongroups;
using Hardware.UnitTest.Mapping;

namespace Hardware.UnitTest.Functiongroups;

internal class Functiongroup_HbTest
{
    private IContainer _container;
    private HardwareUnitTestMapping _hardwareMapping = new();

    [SetUp]
    public void Setup()
    {
        _hardwareMapping = new HardwareUnitTestMapping();

        ContainerBuilder builder = new();
        builder.RegisterModule(_hardwareMapping);
        _container = builder.Build();
    }


    [Test]
    public void SetDescriptionTest()
    {
        Fixture fixture = new();
        fixture.Customize<FunctiongroupHb_DataModel>(x =>
            x.Without(y => y.ButtonNavigation)
                .Without(y => y.ControlTraces));

        fixture.Customize<Equipment_DataModel>(x =>
            x.Without(y => y.EngineNameNavigations)
                .Without(y => y.EngineRelayLeftNavigations)
                .Without(y => y.EngineRelayRightNavigations)
                .Without(y => y.FunctiongroupHbs)
                .Without(y => y.FunctiongroupHePositionSensorBottomNavigations)
                .Without(y => y.FunctiongroupHePositionSensorTopNavigations)
                .Without(y => y.FunctiongroupHePositionSensorTopNavigations)
        );


        FunctiongroupHb_DataModel functiongroupDescription = fixture.Create<FunctiongroupHb_DataModel>();

        Equipment_DataModel buttonDescription = fixture.Create<Equipment_DataModel>();

        functiongroupDescription.ButtonNavigation = buttonDescription;

        IFunctiongroup_Hb engine = _container.Resolve<IFunctiongroup_Hb>();


        if (engine is ISetDescription<IFunctiongroup_Hb, FunctiongroupHb_DataModel> setFunctiongroupDescription)
            setFunctiongroupDescription.SetDescription(functiongroupDescription);

        if (_hardwareMapping.MockGpio is IDescription<Equipment_DataModel> setEquipmentDescription)
            Assert.That(setEquipmentDescription.Description, Is.EqualTo(functiongroupDescription.ButtonNavigation));
    }


    [Test]
    public void ButtonIsOccupiedTest()
    {
        var gpioMock = _hardwareMapping.MockGpio;
        gpioMock.Setup(x => x.ReadPinState()).Returns(true);

        IFunctiongroup_Hb functiongroupHb = _container.Resolve<IFunctiongroup_Hb>();

        Assert.That(functiongroupHb.ButtonIsOccupied, Is.True);
    }


    [Test]
    public void ButtonIsNotOccupiedTest()
    {
        var gpioMock = _hardwareMapping.MockGpio;
        gpioMock.Setup(x => x.ReadPinState()).Returns(false);

        IFunctiongroup_Hb functiongroupHb = _container.Resolve<IFunctiongroup_Hb>();

        Assert.That(functiongroupHb.ButtonIsOccupied, Is.False);
    }


    [Test]
    public void WatchdogIsOccupiedTest()
    {
        int triggeredValue = 0;

        var gpioMock = _hardwareMapping.MockGpio;
        gpioMock.Setup(x => x.ReadPinState()).Returns(false);


        IFunctiongroup_Hb functiongroupHb = _container.Resolve<IFunctiongroup_Hb>();
        functiongroupHb.ButtonOccupied += () => { triggeredValue++; };

        if (functiongroupHb is IWatchdogActivator watchdogActivator)
            watchdogActivator.ActivateWatchdog();

        Assert.That(triggeredValue, Is.EqualTo(0));

        gpioMock.Setup(x => x.ReadPinState()).Returns(true);

        Assert.That(() => triggeredValue, Is.EqualTo(1).After(2).Seconds.PollEvery(100));
    }


    [Test]
    public void WatchdogIsNotOccupiedTest()
    {
        int triggeredValue = 0;

        var gpioMock = _hardwareMapping.MockGpio;
        gpioMock.Setup(x => x.ReadPinState()).Returns(true);


        IFunctiongroup_Hb functiongroupHb = _container.Resolve<IFunctiongroup_Hb>();
        functiongroupHb.ButtonOccupied += () => { triggeredValue++; };

        if (functiongroupHb is IWatchdogActivator watchdogActivator)
            watchdogActivator.ActivateWatchdog(10);

        Assert.That(() => triggeredValue, Is.EqualTo(1).After(2).Seconds.PollEvery(100));

        gpioMock.Setup(x => x.ReadPinState()).Returns(false);

        Assert.That(() => triggeredValue, Is.EqualTo(1).After(2).Seconds.PollEvery(100));
    }

    [TearDown]
    public void TearDown()
    {
        _container.Dispose();
    }
}