using Database.Contract.DataModels;
using Hardware.Contract.Exceptions;
using Hardware.Contract.Interfaces.Components;
using Hardware.Contract.Interfaces.Components.Watchdog;
using Hardware.Contract.Interfaces.Extension;
using Hardware.Contract.Interfaces.Functiongroups;
using Hardware.Contract.Types;

namespace Hardware.Functiongroups;

public class Functiongroup_He : IFunctiongroup_He, ISetDescription<IFunctiongroup_He, FunctiongroupHe_DataModel>, IDescription<FunctiongroupHe_DataModel>,
    IWatchdogActivator
{
    private readonly ISetWatchdogTime _watchdogBg11;
    private readonly ISetWatchdogTime _watchdogBg21;

    public Functiongroup_He(ISensor bg11,
                             ISensor bg21,
                             IEngine ma01,
                             ISetWatchdogTime watchdogBg11,
                             ISetWatchdogTime watchdogBg21)
    {
        _watchdogBg11 = watchdogBg11 ?? throw new ArgumentNullException(nameof(watchdogBg11));
        _watchdogBg21 = watchdogBg21 ?? throw new ArgumentNullException(nameof(watchdogBg21));
        BG11 = bg11 ?? throw new ArgumentNullException(nameof(bg11));
        BG21 = bg21 ?? throw new ArgumentNullException(nameof(bg21));
        MA01 = ma01 ?? throw new ArgumentNullException(nameof(ma01));
    }

    public FunctiongroupHe_DataModel Description { get; private set; }

    public DriveState CurrentState { get; private set; }

    public Position CurrentPosition => GetCurrentPosition();
    public event Action? BG11IsOccupied;
    public event Action? BG21IsOccupied;


    public ISensor BG11 { get; }
    public ISensor BG21 { get; }

    public IEngine MA01 { get; }

    public bool EngineIsTemporaryLocked { get; set; }

    public void OpenShutter()
    {
        CheckEngineIsLocked();

        if (BG21.IsOccupied())
        {
            MA01.Stop();
            return;
        }

        try
        {
            MA01.DriveRight();
            CurrentState = DriveState.IsDriving;
        }
        catch (EngineDriveException e)
        {
            throw new EngineDriveException("Drive Up is not Possible", e);
        }
    }

    public void CloseShutter()
    {
        CheckEngineIsLocked();

        if (BG11.IsOccupied())
        {
            MA01.Stop();
            return;
        }


        try
        {
            MA01.DriveLeft();
            CurrentState = DriveState.IsDriving;
        }
        catch (EngineDriveException e)
        {
            throw new EngineDriveException("Drive Down is not Possible", e);
        }
    }

    public void StopDriving()
    {
        MA01.Stop();
        CurrentState = DriveState.IsNotDriving;
    }

    public IFunctiongroup_He SetDescription(FunctiongroupHe_DataModel description)
    {
        Description = description ?? throw new ArgumentNullException(nameof(description));

        if (BG11 is ISetDescription<ISensor, Equipment_DataModel> bg11)
            bg11.SetDescription(description.PositionSensorBottomNavigation);

        if (BG21 is ISetDescription<ISensor, Equipment_DataModel> bg21)
            bg21.SetDescription(description.PositionSensorTopNavigation);

        if (MA01 is ISetDescription<IEngine, Engine_DataModel> ma01)
            ma01.SetDescription(description.EngineNavigation);

        return this;
    }

    public void ActivateWatchdog(double interval = 100)
    {
        IWatchdog watchdogBg11 = _watchdogBg11.SetWatchdogTime(interval);

        watchdogBg11.SensorIsTriggered += Bg11Triggered;
        watchdogBg11.SetWatchdogFunction(BG11).StartWatchdog();

        IWatchdog watchdogBg21 = _watchdogBg21.SetWatchdogTime(interval);

        watchdogBg21.SensorIsTriggered += Bg21Triggered;
        watchdogBg21.SetWatchdogFunction(BG21).StartWatchdog();
    }


    private void CheckEngineIsLocked()
    {
        if (Description.IsLocked) throw new EngineIsLockedException(LockedState.Locked);

        if (EngineIsTemporaryLocked) throw new EngineIsLockedException(LockedState.TemporaryLocked);
    }


    private Position GetCurrentPosition()
    {
        if (BG11.IsOccupied() && !BG21.IsOccupied()) return Position.Closed;

        if (!BG11.IsOccupied() && BG21.IsOccupied()) return Position.Open;

        return Position.NotDefined;
    }

    private void Bg21Triggered(bool state)
    {
        if (state) BG21IsOccupied?.Invoke();
    }

    private void Bg11Triggered(bool state)
    {
        if (state) BG11IsOccupied?.Invoke();
    }
}