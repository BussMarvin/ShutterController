using Autofac;
using AutoFixture;
using Database.Contract.DataModels;
using Hardware.Contract.Exceptions;
using Hardware.Contract.Interfaces.Extension;
using Hardware.Contract.Interfaces.Functiongroups;
using Hardware.Contract.Types;
using Hardware.UnitTest.Mapping;
using Moq;

namespace Hardware.UnitTest.Functiongroups;

internal class Functiongroup_HeTest
{
    private IContainer _container;
    private HardwareUnitTestMapping _hardwareMapping;

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
        fixture.Customize<FunctiongroupHe_DataModel>(x =>
            x.Without(y => y.ControlTimes)
                .Without(y => y.ControlTraces));

        fixture.Customize<Equipment_DataModel>(x =>
            x.Without(y => y.EngineNameNavigations)
                .Without(y => y.EngineRelayLeftNavigations)
                .Without(y => y.EngineRelayRightNavigations)
                .Without(y => y.FunctiongroupHbs)
                .Without(y => y.FunctiongroupHePositionSensorBottomNavigations)
                .Without(y => y.FunctiongroupHePositionSensorTopNavigations)
                .Without(y => y.FunctiongroupHePositionSensorTopNavigations));

        fixture.Customize<Engine_DataModel>(x =>
            x.Without(y => y.FunctiongroupHes));


        FunctiongroupHe_DataModel functiongroupDescription = fixture.Create<FunctiongroupHe_DataModel>();


        IFunctiongroup_He functiongroup = _container.Resolve<IFunctiongroup_He>();


        if (functiongroup is ISetDescription<IFunctiongroup_He, FunctiongroupHe_DataModel> setFunctiongroupDescription)
            setFunctiongroupDescription.SetDescription(functiongroupDescription);

        if (_hardwareMapping.RelayRightGpio is IDescription<Equipment_DataModel> setRelayRightEquipmentDescription)
            Assert.That(setRelayRightEquipmentDescription.Description, Is.EqualTo(functiongroupDescription.EngineNavigation.RelayRightNavigation));

        if (_hardwareMapping.RelayRightGpio is IDescription<Equipment_DataModel> setRelayLeftEquipmentDescription)
            Assert.That(setRelayLeftEquipmentDescription.Description, Is.EqualTo(functiongroupDescription.EngineNavigation.RelayLeftNavigation));


        if (_hardwareMapping.Bg11Gpio is IDescription<Equipment_DataModel> bg11)
            Assert.That(bg11.Description, Is.EqualTo(functiongroupDescription.PositionSensorBottomNavigation));

        if (_hardwareMapping.Bg21Gpio is IDescription<Equipment_DataModel> bg21)
            Assert.That(bg21.Description, Is.EqualTo(functiongroupDescription.PositionSensorTopNavigation));
    }


    [Test]
    public void CloseShutterTest()
    {
        IFunctiongroup_He functiongroup = _container.Resolve<IFunctiongroup_He>();
        functiongroup = SetFunctiongroupDescription(functiongroup);

        functiongroup.CloseShutter();
        _hardwareMapping.RelayLeftGpio.Verify(x => x.SetPinHigh(), Times.Once);
        _hardwareMapping.RelayRightGpio.Verify(x => x.SetPinHigh(), Times.Never);
    }


    [Test]
    public void OpenShutterTest()
    {
        IFunctiongroup_He functiongroup = _container.Resolve<IFunctiongroup_He>();
        functiongroup = SetFunctiongroupDescription(functiongroup);

        functiongroup.OpenShutter();
        _hardwareMapping.RelayRightGpio.Verify(x => x.SetPinHigh(), Times.Once);
        _hardwareMapping.RelayLeftGpio.Verify(x => x.SetPinHigh(), Times.Never);
    }

    [Test]
    public void CloseShutterAndOccupiedBg11Test()
    {
        IFunctiongroup_He functiongroup = _container.Resolve<IFunctiongroup_He>();
        functiongroup = SetFunctiongroupDescription(functiongroup);

        functiongroup.CloseShutter();
        _hardwareMapping.RelayLeftGpio.Verify(x => x.SetPinHigh(), Times.Once);

        _hardwareMapping.Bg11Gpio.Setup(x => x.ReadPinState()).Returns(true);
        functiongroup.CloseShutter();

        _hardwareMapping.RelayLeftGpio.Verify(x => x.SetPinLow(), Times.Once);
    }

    [Test]
    public void OpenShutterAndOccupiedBg21Test()
    {
        IFunctiongroup_He functiongroup = _container.Resolve<IFunctiongroup_He>();
        functiongroup = SetFunctiongroupDescription(functiongroup);

        functiongroup.OpenShutter();
        _hardwareMapping.RelayRightGpio.Verify(x => x.SetPinHigh(), Times.Once);

        _hardwareMapping.Bg21Gpio.Setup(x => x.ReadPinState()).Returns(true);
        functiongroup.OpenShutter();

        _hardwareMapping.RelayRightGpio.Verify(x => x.SetPinLow(), Times.Once);
    }


    [Test]
    public void CloseShutterAndOccupiedBg21Test()
    {
        IFunctiongroup_He functiongroup = _container.Resolve<IFunctiongroup_He>();
        functiongroup = SetFunctiongroupDescription(functiongroup);


        _hardwareMapping.Bg21Gpio.Setup(x => x.ReadPinState()).Returns(true);
        functiongroup.CloseShutter();

        _hardwareMapping.RelayLeftGpio.Verify(x => x.SetPinHigh(), Times.Once);
        _hardwareMapping.RelayLeftGpio.Verify(x => x.SetPinLow(), Times.Never);
    }

    [Test]
    public void OpenShutterAndOccupiedBg11Test()
    {
        IFunctiongroup_He functiongroup = _container.Resolve<IFunctiongroup_He>();
        functiongroup = SetFunctiongroupDescription(functiongroup);


        _hardwareMapping.Bg11Gpio.Setup(x => x.ReadPinState()).Returns(true);
        functiongroup.OpenShutter();

        _hardwareMapping.RelayRightGpio.Verify(x => x.SetPinHigh(), Times.Once);
        _hardwareMapping.RelayRightGpio.Verify(x => x.SetPinLow(), Times.Never);
    }

    [Test]
    public void CloseShutterDuringOpenShutterTest()
    {
        IFunctiongroup_He functiongroup = _container.Resolve<IFunctiongroup_He>();
        functiongroup = SetFunctiongroupDescription(functiongroup);

        bool isClosed = false;

        Task.Run(() =>
        {
            while (!isClosed)
            {
                functiongroup.CloseShutter();
            }
        });

        Assert.Throws<EngineDriveException>(() => functiongroup.OpenShutter());
        isClosed = true;
    }


    [Test]
    public void OpenShutterDuringCloseShutterTest()
    {
        HardwareUnitTestMapping hardwareMapping = new();

        IFunctiongroup_He functiongroup = _container.Resolve<IFunctiongroup_He>();
        functiongroup = SetFunctiongroupDescription(functiongroup);

        bool isOpen = false;


        Task.Run(() =>
        {
            while (!isOpen)
            {
                functiongroup.OpenShutter();
            }
        });

        Assert.Throws<EngineDriveException>(() => functiongroup.CloseShutter());
        isOpen = true;
    }

    [Test]
    public void MoveShutterWhenFunctiongroupIsLockedTest()
    {
        IFunctiongroup_He functiongroup = _container.Resolve<IFunctiongroup_He>();
        functiongroup = SetFunctiongroupDescription(functiongroup);

        if (functiongroup is IDescription<FunctiongroupHe_DataModel> functiongroupModel)
            functiongroupModel.Description.IsLocked = true;


        Assert.Throws<EngineIsLockedException>(() => functiongroup.CloseShutter());

        Assert.Throws<EngineIsLockedException>(() => functiongroup.OpenShutter());
    }

    [Test]
    public void MoveShutterWhenFunctiongroupIsTemporaryLockedTest()
    {
        IFunctiongroup_He functiongroup = _container.Resolve<IFunctiongroup_He>();
        functiongroup = SetFunctiongroupDescription(functiongroup);
        functiongroup.EngineIsTemporaryLocked = true;

        Assert.Throws<EngineIsLockedException>(() => functiongroup.CloseShutter());
        Assert.Throws<EngineIsLockedException>(() => functiongroup.OpenShutter());
    }

    [Test]
    public void CurrentPositionIsClosedTest()
    {
        var bg11Mock = _hardwareMapping.Bg11Gpio;
        var bg21Mock = _hardwareMapping.Bg21Gpio;

        IFunctiongroup_He functiongroup = _container.Resolve<IFunctiongroup_He>();
        functiongroup = SetFunctiongroupDescription(functiongroup);

        bg11Mock.Setup(x => x.ReadPinState()).Returns(true);
        bg21Mock.Setup(x => x.ReadPinState()).Returns(false);

        Assert.That(functiongroup.CurrentPosition, Is.EqualTo(Position.Closed));
    }

    [Test]
    public void CurrentPositionIsOpenTest()
    {
        var bg11Mock = _hardwareMapping.Bg11Gpio;
        var bg21Mock = _hardwareMapping.Bg21Gpio;

        IFunctiongroup_He functiongroup = _container.Resolve<IFunctiongroup_He>();
        functiongroup = SetFunctiongroupDescription(functiongroup);

        bg11Mock.Setup(x => x.ReadPinState()).Returns(false);
        bg21Mock.Setup(x => x.ReadPinState()).Returns(true);

        Assert.That(functiongroup.CurrentPosition, Is.EqualTo(Position.Open));
    }

    [Test]
    public void CurrentPositionIsNotDefinedTest()
    {
        var bg11Mock = _hardwareMapping.Bg11Gpio;
        var bg21Mock = _hardwareMapping.Bg21Gpio;

        IFunctiongroup_He functiongroup = _container.Resolve<IFunctiongroup_He>();
        functiongroup = SetFunctiongroupDescription(functiongroup);

        bg11Mock.Setup(x => x.ReadPinState()).Returns(false);
        bg21Mock.Setup(x => x.ReadPinState()).Returns(false);

        Assert.That(functiongroup.CurrentPosition, Is.EqualTo(Position.NotDefined));
    }


    [Test]
    public void StateIsDrivingTest()
    {
        IFunctiongroup_He functiongroup = _container.Resolve<IFunctiongroup_He>();
        functiongroup = SetFunctiongroupDescription(functiongroup);

        Task.Run(() =>
        {
            while (true)
            {
                functiongroup.CloseShutter();
            }
        });

        Assert.That(functiongroup.CurrentState, Is.EqualTo(DriveState.IsDriving));
    }

    [Test]
    public void StateIsNotDrivingTest()
    {
        IFunctiongroup_He functiongroup = _container.Resolve<IFunctiongroup_He>();
        functiongroup = SetFunctiongroupDescription(functiongroup);
        functiongroup.StopDriving();


        Assert.That(functiongroup.CurrentState, Is.EqualTo(DriveState.IsNotDriving));
    }

    private IFunctiongroup_He SetFunctiongroupDescription(IFunctiongroup_He functiongroup)
    {
        if (functiongroup is ISetDescription<IFunctiongroup_He, FunctiongroupHe_DataModel> setFunctiongroupDescription)
        {
            setFunctiongroupDescription.SetDescription(new FunctiongroupHe_DataModel
            {
                PositionSensorBottomNavigation = new Equipment_DataModel(),
                PositionSensorTopNavigation = new Equipment_DataModel(),


                EngineNavigation = new Engine_DataModel
                {
                    RelayLeftNavigation = new Equipment_DataModel(),
                    RelayRightNavigation = new Equipment_DataModel()
                }
            });
        }


        return functiongroup;
    }


    [TearDown]
    public void TearDown()
    {
        _container.Dispose();
    }
}