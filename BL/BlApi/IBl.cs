using System.Reflection.Metadata.Ecma335;

namespace BlApi;

public interface IBl
{
    public ITask Task { get; }
    public IEngineer Engineer { get; }
    public IMilestone Milestone { get; }
    public void InitializeDB ();
    public void ResetDB ();
}
