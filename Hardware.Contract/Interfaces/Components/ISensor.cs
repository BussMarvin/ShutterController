namespace Hardware.Contract.Interfaces.Components;

public interface ISensor: IOccupied
{
    public bool OldStateWasOccupied { get; }
    public bool CurrentStateIsOccupied { get; }

}