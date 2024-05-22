namespace BlImplementation;
using BlApi;
internal class Bl : IBl
{
    public ITask Task => new TaskImplementation();

    public IEngineer Engineer => new EngineerImplementation();  

    public IMilestone Milestone => new MilestoneImplementation();

    public void InitializeDB() => DalTest.Initialization.Do();

    public void ResetDB() => DalTest.Initialization.Reset();
}
