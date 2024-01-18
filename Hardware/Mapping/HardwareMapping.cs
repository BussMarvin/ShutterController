using Autofac;
using Autofac.Core;
using Hardware.Components;
using Hardware.Contract.Interfaces.Components;
using Hardware.Contract.Interfaces.Components.Watchdog;
using Hardware.Functiongroups;

namespace Hardware.Mapping;

public class HardwareMapping : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterGpio(builder);
        RegisterRaspberryPi(builder);
        RegisterSensor(builder);
        RegisterEngine(builder);
        RegisterDebounce(builder);
        RegisterWatchdog(builder);
    }


    protected virtual void RegisterGpio(ContainerBuilder builder)
    {
        builder.RegisterType<Gpio>().As<IGpio>().InstancePerDependency();
    }


    protected virtual void RegisterRaspberryPi(ContainerBuilder builder)
    {
        builder.RegisterType<RaspberryPi>().As<IRaspberryPi>().SingleInstance();
    }

    protected virtual void RegisterSensor(ContainerBuilder builder)
    {
        builder.RegisterType<Sensor>().As<ISensor>().InstancePerDependency();
    }

    protected virtual void RegisterEngine(ContainerBuilder builder)
    {
        builder.RegisterType<Engine>().As<IEngine>().InstancePerDependency();
    }

    protected void RegisterEngine(ContainerBuilder builder, IGpio relayLeftRotation, IGpio relayRightRotation)
    {
        builder.RegisterType<Engine>().As<IEngine>()
            .WithParameter(new ResolvedParameter(
                (pi, _) => pi.ParameterType == typeof(IGpio) && pi.Name.Contains("Left"),
                (_, _) => relayLeftRotation))
            .WithParameter(new ResolvedParameter(
                (pi, _) => pi.ParameterType == typeof(IGpio) && pi.Name.Contains("Right"),
                (_, _) => relayRightRotation))
            .InstancePerDependency();
    }

    protected virtual void RegisterDebounce(ContainerBuilder builder)
    {
        builder.RegisterType<Debounce>().As<IDebounce>().InstancePerDependency();
    }


    protected void RegisterWatchdog(ContainerBuilder builder)
    {
        builder.RegisterType<Watchdog>()
            .As<ISetWatchdogTime>()
            .As<IWatchdog>()
            .InstancePerDependency();
    }
}