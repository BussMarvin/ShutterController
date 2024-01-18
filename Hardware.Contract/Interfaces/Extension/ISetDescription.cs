namespace Hardware.Contract.Interfaces.Extension;

public interface ISetDescription<TReturnType, TDataModel>
{
    TReturnType SetDescription(TDataModel description);
}