using Hardware.Contract.Interfaces.Components;

namespace Hardware.Contract.Interfaces.Functiongroups;

public interface IFunctiongroup_Hb
{
    public IDebounce S01 { get; }

    bool ButtonIsOccupied { get; }

    event Action ButtonOccupied;

}