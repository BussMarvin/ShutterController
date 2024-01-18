namespace Hardware.Contract.Interfaces.Components;

public interface IDebounce
{
    public bool OldStateWasOccupied { get; }
    bool IsOccupied();
}