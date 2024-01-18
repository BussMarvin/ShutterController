namespace Hardware.Contract.Interfaces.Components.Extension;

public interface IInvertSignal
{
    ISensor InvertSignal(bool invertSignal = true);
}