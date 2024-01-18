namespace Database.Contract.DataModels;

public class ControlTrace_DataModel
{
    public Guid Guid { get; set; } = new();

    public DateTime TimeStamp { get; set; }

    public long? Function { get; set; }

    public long? ControlMode { get; set; }

    public long? FunctiongroupHe { get; set; }

    public long? FunctiongroupHbs { get; set; }

    public string? Comment { get; set; }

    public virtual Function_DataModel? FunctionNavigation { get; set; }

    public virtual FunctiongroupHb_DataModel? FunctiongroupHbsNavigation { get; set; }

    public virtual FunctiongroupHe_DataModel? FunctiongroupHeNavigation { get; set; }
}