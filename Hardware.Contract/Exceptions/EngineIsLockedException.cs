using Hardware.Contract.Types;

namespace Hardware.Contract.Exceptions;

public class EngineIsLockedException : Exception
{
    public EngineIsLockedException(LockedState lockedState)
    {
        State = lockedState;
    }

    public EngineIsLockedException(LockedState lockedState, string message)
        : base(message)
    {
        State = lockedState;
    }

    public EngineIsLockedException(LockedState lockedState, string message, Exception inner)
        : base(message, inner)
    {
        State = lockedState;
    }

    public LockedState State { get; }
}