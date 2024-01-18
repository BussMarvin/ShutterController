namespace Database.Contract.DataModels;

public class FunctiongroupHe_DataModel
{
    public long Id { get; set; }

    public long? PositionSensorBottom { get; set; }

    public long? PositionSensorTop { get; set; }

    public long? Engine { get; set; }

    public string? Comment { get; set; }

    public bool IsLocked { get; set; }

    public long? ControlMode { get; set; }

    public virtual ICollection<ControlTime_DataModel> ControlTimes { get; set; } = new List<ControlTime_DataModel>();

    public virtual ICollection<ControlTrace_DataModel> ControlTraces { get; set; } = new List<ControlTrace_DataModel>();

    public virtual Engine_DataModel? EngineNavigation { get; set; }

    public virtual Equipment_DataModel? PositionSensorBottomNavigation { get; set; }

    public virtual Equipment_DataModel? PositionSensorTopNavigation { get; set; }
}