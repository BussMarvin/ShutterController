using Database.Contract.DataModels;
using Hardware.Contract.Exceptions;
using Hardware.Contract.Interfaces.Components;
using Hardware.Contract.Interfaces.Functiongroups;
using Hardware.Contract.Types;

namespace Hardware.Functiongroups;

public class Functiongroup_HE_ : IFunctiongroup_HE_
{
    public Functiongroup_HE_(ISensor bg11, ISensor bg21, IEngine ma01, FunctiongroupHe_DataModel description)
    {
        Description = description;
        BG11 = bg11;
        BG21 = bg21;
        MA01 = ma01;
    }

    public FunctiongroupHe_DataModel Description { get; }

    public DriveState CurrentState { get; private set; }

    public Position CurrentPosition => SetCurrentPosition();


    public ISensor BG11 { get; }
    public ISensor BG21 { get; }

    public IEngine MA01 { get; }

    public bool EngineIsTemporaryLocked { get; set; }

    public void OpenShutter()
    {
        CheckIfEngineIsLocked();

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
        CheckIfEngineIsLocked();

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


    private void CheckIfEngineIsLocked()
    {
        if (Description.IsLocked) throw new EngineIsLockedException(LockedState.Locked);

        if (EngineIsTemporaryLocked) throw new EngineIsLockedException(LockedState.TemporaryLocked);
    }


    private Position SetCurrentPosition()
    {
        if (BG11.IsOccupied() && !BG21.IsOccupied()) return Position.Closed;

        if (!BG11.IsOccupied() && BG21.IsOccupied()) return Position.Open;

        return Position.NotDefined;
    }
}