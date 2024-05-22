namespace BO;

public class TaskInList
{
    public int Id { get; init; }
    public string Description { get; set; }
    public string Alias { get; set; }
    public Status? Status { get; set; }

    public TaskInList(int id, string description, string alias, Status? status)
    {
        Id = id;
        Description = description;
        Alias = alias;
        Status = status;
    }

    public override string ToString()
    {
        return ($"   Id:{Id} Description:{Description} Alias:{Alias} Status:{Status}");
    }
}
