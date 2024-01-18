namespace Database.Contract.DataModels;

public class FunctiongroupHb_DataModel
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public long? Button { get; set; }

    public string? Comment { get; set; }

    public bool IsLocked { get; set; }

    public long? ControlMode { get; set; }

    public virtual Equipment_DataModel? ButtonNavigation { get; set; }

    public virtual ICollection<ControlTrace_DataModel> ControlTraces { get; set; } = new List<ControlTrace_DataModel>();
}