namespace Hardware.Contract.Interfaces.Extension;

public interface IDescription<TReturnType>
{
    TReturnType Description { get; }
}