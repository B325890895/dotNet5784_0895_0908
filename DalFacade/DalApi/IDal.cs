namespace DalApi;

public interface IDal//Generic intefrace for all entities lists
{
    IDependency Dependency { get; }
    IEngineer Engineer { get; }
    ITask Task { get; }
    void Reset();
    public DateTime? ProjectStartDate { get; }
    public DateTime? ProjectEndDate { get;  } 
}

