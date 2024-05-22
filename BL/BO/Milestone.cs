namespace BO;

public class Milestone
{
    public int Id { get; init; }
    public string Alias { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAtDate { get; init; }
    public Status? Status { get; set; }
    public DateTime? ForecastDate { get; set; }
    public DateTime? DeadlineDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public double? CompletionPercentage { get; set; }
    public string? Remarks { get; set; }
    public List<TaskInList>? Dependencies { get; set; }

    public Milestone(int id, string alias, string description, DateTime createdAtDate, Status? status, DateTime? forecastDate, DateTime? deadlineDate, DateTime? completeDate, double? completionPercentage, string? remarks, List<TaskInList>? dependencies)
    {
        Id = id;
        Alias = alias;
        Description = description;
        CreatedAtDate = createdAtDate;
        Status = status;
        ForecastDate = forecastDate;
        DeadlineDate = deadlineDate;
        CompleteDate = completeDate;
        CompletionPercentage = completionPercentage;
        Remarks = remarks;
        Dependencies = dependencies;
    }

    public override string ToString()
    {
        string dependenciesToString = "";
        if (Dependencies != null)
        {
            foreach (var item in Dependencies)
            {
                dependenciesToString += $"{item.ToString()}\n";
            }
        }
        return ($"Id={Id} \nAlias={Alias} \nDescription={Description} \nCreated At Date={CreatedAtDate} " +
               $"\nStatus={(Status != null ? Status : "none")} \nForecast Date to complete={(ForecastDate != null ? ForecastDate : "none")}" +
               $"\nDeadline Date={(DeadlineDate != null ? DeadlineDate : "none")} \nComplete Date={(CompleteDate != null ? CompleteDate : "none")}" +
               $"\nPercentage of Task Completion={(CompletionPercentage != null ? CompletionPercentage : "none")}" +
               $"\nRemarks={(Remarks != null ? Remarks : "none")} \nDependencies={(Dependencies != null ? dependenciesToString : "none")}");
       
    }
}
