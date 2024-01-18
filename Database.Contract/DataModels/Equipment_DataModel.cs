namespace Database.Contract.DataModels;

public class Equipment_DataModel
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public int Gpio { get; set; }

    public string? Comment { get; set; }

    public virtual ICollection<Engine_DataModel> EngineNameNavigations { get; set; } = new List<Engine_DataModel>();

    public virtual ICollection<Engine_DataModel> EngineRelayLeftNavigations { get; set; } = new List<Engine_DataModel>();

    public virtual ICollection<Engine_DataModel> EngineRelayRightNavigations { get; set; } = new List<Engine_DataModel>();

    public virtual ICollection<FunctiongroupHb_DataModel> FunctiongroupHbs { get; set; } = new List<FunctiongroupHb_DataModel>();

    public virtual ICollection<FunctiongroupHe_DataModel> FunctiongroupHePositionSensorBottomNavigations { get; set; } = new List<FunctiongroupHe_DataModel>();

    public virtual ICollection<FunctiongroupHe_DataModel> FunctiongroupHePositionSensorTopNavigations { get; set; } = new List<FunctiongroupHe_DataModel>();
}