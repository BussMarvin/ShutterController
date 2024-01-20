using Database.Contract.DataModels;
using Hardware.Contract.Interfaces.Components;
using Hardware.Contract.Interfaces.Components.Watchdog;
using Hardware.Contract.Interfaces.Extension;
using Hardware.Contract.Interfaces.Functiongroups;

namespace Hardware.Functiongroups;

internal class Functiongroup_Hb : IFunctiongroup_Hb, ISetDescription<IFunctiongroup_Hb, FunctiongroupHb_DataModel>, IDescription<FunctiongroupHb_DataModel>,
    IWatchdogActivator
{
    private readonly ISetWatchdogTime _watchdogTime;

    public Functiongroup_Hb(ISetWatchdogTime watchdogTime, IDebounce s01)
    {
        _watchdogTime = watchdogTime ?? throw new ArgumentNullException(nameof(watchdogTime));
        S01 = s01 ?? throw new ArgumentNullException(nameof(s01));
    }

    public FunctiongroupHb_DataModel Description { get; private set; }
    public IDebounce S01 { get; }
    public bool ButtonIsOccupied => S01.IsOccupied();

    public event Action? ButtonOccupied;


    public IFunctiongroup_Hb SetDescription(FunctiongroupHb_DataModel description)
    {
        Description = description ?? throw new ArgumentNullException(nameof(description));

        if (S01 is ISetDescription<IDebounce, Equipment_DataModel> s01)
            s01.SetDescription(description.ButtonNavigation);
        return this;
    }

    public void ActivateWatchdog(double interval = 100)
    {
        IWatchdog watchdog = _watchdogTime.SetWatchdogTime(interval);

        watchdog.SensorIsTriggered += WatchdogTriggered;
        watchdog.SetWatchdogFunction(S01).StartWatchdog();
    }

    private void WatchdogTriggered(bool state)
    {
        if (state) ButtonOccupied?.Invoke();
    }
}