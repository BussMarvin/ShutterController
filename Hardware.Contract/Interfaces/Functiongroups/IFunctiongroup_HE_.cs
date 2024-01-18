using Hardware.Contract.Enums;
using Hardware.Contract.Interfaces.Components;

namespace Hardware.Contract.Interfaces.Functiongroups;

public interface IFunctiongroup_HE_
{
    ISensor BG11 { get; }
    ISensor BG21 { get; }
    IEngine MA01 { get; }

    public bool EngineIsTemporaryLocked { get; set; }
    public DriveState CurrentState { get; }

    public Position CurrentPosition { get; }
    void OpenShutter();
    void CloseShutter();
    void StopDriving();
}