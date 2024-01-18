namespace Hardware.Contract.Interfaces.Components;

public interface ISensor
{
    public bool OldStateWasOccupied { get; }
    public bool CurrentStateIsOccupied { get; }

    bool IsOccupied();
}