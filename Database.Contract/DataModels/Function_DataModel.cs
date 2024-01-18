namespace Database.Contract.DataModels;

public class Function_DataModel
{
    public long Id { get; set; }

    public string? PlainText { get; set; }

    public virtual ICollection<ControlTime_DataModel> ControlTimes { get; set; } = new List<ControlTime_DataModel>();

    public virtual ICollection<ControlTrace_DataModel> ControlTraces { get; set; } = new List<ControlTrace_DataModel>();
}