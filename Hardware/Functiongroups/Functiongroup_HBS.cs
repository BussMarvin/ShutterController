//using Hardware.Contract.Interfaces.Components;
//using Hardware.Contract.Interfaces.Functiongroups;

//namespace Hardware.Functiongroups;

//internal class Functiongroup_HBS : IFunctiongroup_HBS
//{
//    public Functiongroup_HBS(IDebounce s_01, FunctiongroupHbs_DataModel description)
//    {
//        S_01 = s_01 ?? throw new ArgumentNullException(nameof(s_01));
//        Description = description ?? throw new ArgumentNullException(nameof(description));
//    }


//    public IDebounce S_01 { get; }
//    public FunctiongroupHbs_DataModel Description { get; }

//    public bool ControlButtonIsOccupied()
//    {
//        return S_01.IsOccupied();
//    }


//}

