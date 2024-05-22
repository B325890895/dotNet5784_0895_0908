namespace Dal;
using DalApi;
using System;

sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();
    private DalList() { }

    public IDependency Dependency =>  new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    DateTime? IDal.ProjectStartDate => DataSource.Config.ProjectStartDate;

    DateTime? IDal.ProjectEndDate => DataSource.Config.ProjectEndDate;


    public void Reset()
    {
        if (DataSource.Engineers != null) DataSource.Engineers.Clear();
        if (DataSource.Dependencies != null) DataSource.Dependencies.Clear();
        if (DataSource.Tasks != null) DataSource.Tasks.Clear();
    }
}