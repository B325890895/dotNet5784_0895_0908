namespace BO;

public class TaskInEngineer
{
    public int Id {  get; set; }
    public string Alias { get; set; }

    public TaskInEngineer (int id,string alias)
    {
        Id = id;
        Alias = alias;
    }

    public override string ToString()
    {
        return ($"  Id:{Id} Alias:{Alias}");
    }
}
