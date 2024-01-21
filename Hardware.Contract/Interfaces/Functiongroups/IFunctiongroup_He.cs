using Hardware.Contract.Interfaces.Components;
using Hardware.Contract.Types;

namespace Hardware.Contract.Interfaces.Functiongroups;

public interface IFunctiongroup_He
{
    ISensor BG11 { get; }

    ISensor BG21 { get; }
    IEngine MA01 { get; }


    public bool EngineIsTemporaryLocked { get; set; }
    public DriveState CurrentState { get; }

    public Position CurrentPosition { get; }
    event Action BG11IsOccupied;
    event Action BG21IsOccupied;
    void OpenShutter();
    void CloseShutter();
    void StopDriving();
}