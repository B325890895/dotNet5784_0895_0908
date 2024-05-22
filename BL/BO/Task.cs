namespace BO;

public class Task
{
    public int Id { get; init; }
    public string Alias { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAtDate { get; init; }
    public Status? Status { get; set; }
    public List<TaskInList>? Dependencies { get; set; }
    public MilestoneInTask? Milestone { get; set; }
    public TimeSpan? RequiredEffortTime { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public DateTime? ForecastDate { get; set; }
    public DateTime? DeadlineDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public string? Deliverables { get; set; }
    public string? Remarks { get; set; }
    public EngineerInTask? Engineer { get; set; }
    public EngineerExperience? Copmlexity { get; set; }
   
    public Task(int id, string alias, string description, DateTime createdAtDate, Status? status, List<TaskInList>? dependencies, MilestoneInTask? milestone, TimeSpan? requiredEffortTime, DateTime? startDate, DateTime? scheduledDate, DateTime? forecastDate, DateTime? deadlineDate, DateTime? completeDate, string? deliverables, string? remarks, EngineerInTask? engineer, EngineerExperience? copmlexity)
    {
        Id = id;
        Alias = alias;
        Description = description;
        CreatedAtDate = createdAtDate;
        Status = status;
        Dependencies = dependencies;
        Milestone = milestone;
        RequiredEffortTime = requiredEffortTime;
        StartDate = startDate;
        ScheduledDate = scheduledDate;
        ForecastDate = forecastDate;
        DeadlineDate = deadlineDate;
        CompleteDate = completeDate;
        Deliverables = deliverables;
        Remarks = remarks;
        Engineer = engineer;
        Copmlexity = copmlexity;
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
        return ($"Id={Id} \nAlias={Alias} \nDescription={Description} \nCreated at date={CreatedAtDate}" +
               $"\nStatus={Status} \nDependencies={(Dependencies == null ? "none" : Dependencies.Any() ? dependenciesToString : "none")} \nMilestone={(Milestone != null ? Milestone : "none")} " +
               $"\nEffort Time the task Required={RequiredEffortTime} \nStart Date={StartDate} " +
               $"\nScheduled Date={ScheduledDate} \nForecast Date to complete={ForecastDate} " +
               $"\nDeadline Date={DeadlineDate} \nComplete Date={CompleteDate} \nDeliverables={Deliverables}" +
               $"\nRemarks={Remarks} \nEngineer assighn to the Task={(Engineer != null ? Engineer : "none")} \nCopmlexity of the Task={Copmlexity}");
    }

}
