using Hardware.Contract.Interfaces.Components;

namespace Hardware.Contract.Interfaces.Functiongroups;

public interface IFunctiongroup_HBS
{
    public IDebounce S_01 { get; }

    bool ControlButtonIsOccupied();
}