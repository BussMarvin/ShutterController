namespace Database.Contract.DataModels;

public class ControlTime_DataModel
{
    public Guid Guid { get; set; } = new();

    public TimeOnly? Time { get; set; }

    public bool? IsActive { get; set; }

    public long? Function { get; set; }

    public long? Functiongroup { get; set; }

    public virtual Function_DataModel? FunctionNavigation { get; set; }

    public virtual FunctiongroupHe_DataModel? FunctiongroupNavigation { get; set; }
}