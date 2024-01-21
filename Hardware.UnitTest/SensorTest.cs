using Autofac;
using Database.Contract.DataModels;
using Hardware.Contract.Interfaces.Components;
using Hardware.Contract.Interfaces.Components.Extension;
using Hardware.Contract.Interfaces.Extension;
using Hardware.UnitTest.Mapping;

namespace Hardware.UnitTest;

public class SensorTest
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
    public void SensorSetDescriptionTest()
    {
        Equipment_DataModel description = new()
        {
            Gpio = 10
        };

        ISensor sensor = _container.Resolve<ISensor>();

        if (sensor is ISetDescription<ISensor, Equipment_DataModel> setSensorDescription)
            setSensorDescription.SetDescription(description);

        if (sensor is IDescription<Equipment_DataModel> sensorDescription)
            Assert.That(description, Is.EqualTo(sensorDescription.Description));

        if (_hardwareMapping.MockGpio is IDescription<Equipment_DataModel> gpioDescription)
            Assert.That(description, Is.EqualTo(gpioDescription.Description));
    }

    [Test]
    public void SensorWithoutInvertAndTrueSignalTest()
    {
        _hardwareMapping.MockGpio.Setup(x => x.ReadPinState()).Returns(true);

        ISensor sensor = _container.Resolve<ISensor>();

        Assert.True(sensor.IsOccupied());
        Assert.True(sensor.CurrentStateIsOccupied);
    }


    [Test]
    public void SensorWithInvertAndTrueSignalTest()
    {
        _hardwareMapping.MockGpio.Setup(x => x.ReadPinState()).Returns(true);

        ISensor sensor = _container.Resolve<ISensor>();

        if (sensor is IInvertSignal invertSignal)
            invertSignal.InvertSignal();

        Assert.False(sensor.IsOccupied());
        Assert.False(sensor.CurrentStateIsOccupied);
    }

    [Test]
    public void SensorWithoutInvertAndFalseSignalTest()
    {
        _hardwareMapping.MockGpio.Setup(x => x.ReadPinState()).Returns(false);

        ISensor sensor = _container.Resolve<ISensor>();

        Assert.False(sensor.IsOccupied());
        Assert.False(sensor.CurrentStateIsOccupied);
    }


    [Test]
    public void SensorWithInvertAndFalseSignalTest()
    {
        _hardwareMapping.MockGpio.Setup(x => x.ReadPinState()).Returns(false);

        ISensor sensor = _container.Resolve<ISensor>();

        if (sensor is IInvertSignal invertSignal)
            invertSignal.InvertSignal();

        Assert.True(sensor.IsOccupied());
        Assert.True(sensor.CurrentStateIsOccupied);
    }

    [TearDown]
    public void TearDown()
    {
        _container.Dispose();
    }
}