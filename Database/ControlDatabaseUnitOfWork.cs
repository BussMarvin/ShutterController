using Database.Contract.DataModels;
using Database.Contract.Interfaces;

namespace Database;

internal class ControlDatabaseUnitOfWork : IUnitOfWork
{
    private readonly ControlDatabaseContext _dbContext;


    public ControlDatabaseUnitOfWork(ControlDatabaseContext dbContext,
                                     IGenericRepository<ControlTime_DataModel> controlTimeRepository,
                                     IGenericRepository<ControlTrace_DataModel> controlTracesRepository,
                                     IGenericRepository<Engine_DataModel> enginesRepository,
                                     IGenericRepository<Equipment_DataModel> equipmentRepository,
                                     IGenericRepository<FunctiongroupHb_DataModel> functiongroupHbRepository,
                                     IGenericRepository<FunctiongroupHe_DataModel> functiongroupHeRepository,
                                     IGenericRepository<Function_DataModel> functionsRepository)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        ControlTimeRepository = controlTimeRepository ?? throw new ArgumentNullException(nameof(controlTimeRepository));
        ControlTracesRepository = controlTracesRepository ?? throw new ArgumentNullException(nameof(controlTracesRepository));
        EnginesRepository = enginesRepository ?? throw new ArgumentNullException(nameof(enginesRepository));
        EquipmentRepository = equipmentRepository ?? throw new ArgumentNullException(nameof(equipmentRepository));
        FunctiongroupHbRepository = functiongroupHbRepository ?? throw new ArgumentNullException(nameof(functiongroupHbRepository));
        FunctiongroupHeRepository = functiongroupHeRepository ?? throw new ArgumentNullException(nameof(functiongroupHeRepository));
        FunctionsRepository = functionsRepository ?? throw new ArgumentNullException(nameof(functionsRepository));
    }

    public IGenericRepository<ControlTime_DataModel> ControlTimeRepository { get; }
    public IGenericRepository<ControlTrace_DataModel> ControlTracesRepository { get; }
    public IGenericRepository<Engine_DataModel> EnginesRepository { get; }
    public IGenericRepository<Equipment_DataModel> EquipmentRepository { get; }
    public IGenericRepository<Function_DataModel> FunctionsRepository { get; }
    public IGenericRepository<FunctiongroupHb_DataModel> FunctiongroupHbRepository { get; }
    public IGenericRepository<FunctiongroupHe_DataModel> FunctiongroupHeRepository { get; }

    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}