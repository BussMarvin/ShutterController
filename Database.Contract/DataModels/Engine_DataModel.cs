namespace Database.Contract.DataModels;

public class Engine_DataModel
{
    public long Id { get; set; }

    public long? Name { get; set; }

    public long? RelayRight { get; set; }

    public long? RelayLeft { get; set; }

    public virtual ICollection<FunctiongroupHe_DataModel> FunctiongroupHes { get; set; } = new List<FunctiongroupHe_DataModel>();

    public virtual Equipment_DataModel? NameNavigation { get; set; }

    public virtual Equipment_DataModel? RelayLeftNavigation { get; set; }

    public virtual Equipment_DataModel? RelayRightNavigation { get; set; }
}