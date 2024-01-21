using Database.Contract.DataModels;

namespace Database.Contract.Interfaces;

/// <summary>
///     Interface for UnitOfWork.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    public IGenericRepository<ControlTime_DataModel> ControlTimeRepository { get; }


    public IGenericRepository<ControlTrace_DataModel> ControlTracesRepository { get; }

    public IGenericRepository<Engine_DataModel> EnginesRepository { get; }

    public IGenericRepository<Equipment_DataModel> EquipmentRepository { get; }

    public IGenericRepository<Function_DataModel> FunctionsRepository { get; }

    public IGenericRepository<FunctiongroupHb_DataModel> FunctiongroupHbRepository { get; }

    public IGenericRepository<FunctiongroupHe_DataModel> FunctiongroupHeRepository { get; }


    /// <summary>
    ///     Saves the changes.
    /// </summary>
    void SaveChanges();
}