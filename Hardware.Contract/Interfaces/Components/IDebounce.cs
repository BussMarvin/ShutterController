namespace Hardware.Contract.Interfaces.Components;

public interface IDebounce: IOccupied
{
    public bool OldStateWasOccupied { get; }
}