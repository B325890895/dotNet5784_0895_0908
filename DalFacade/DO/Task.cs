namespace DO;

public record Task //this record save the information about the Task.
(
    int Id,
    string Alias,
    string Description,
    DateTime CreatedAtDate,
    bool IsMilestone,
    TimeSpan? RequiredEffortTime,
    EngineerExperience? Copmlexity,
    DateTime? StartDate,
    DateTime? ScheduledDate,
    DateTime? DeadlineDate,
    DateTime? CompleteDate,
    string? Deliverables,
    string? Remarks,
    int? EngineerId

/*
 Id int [pk, increment]
Alias string [not null, unique]
Description string [not null]
CreatedAtDate datetime [not null, note: 'Date when the task was added to the system']
RequiredEffortTime TimeSpan [note: 'how many men-days needed for the task (for MS it is null)']
IsMilestone bool [not null]
Copmlexity DO.EngineerExperience [note: 'task: minimum expirience for engineer to assign']
StartDate datetime [note: 'the real start date']
ScheduledDate datetime [note: 'the planned start date']
DeadlineDate datetime [note: 'the latest complete date']
CompleteDate datetime [note: 'task: real completion date']
Deliverables string [note: 'task: description of deliverables for MS copmletion']
Remarks string [note: 'free remarks from project meetings']
EngineerId int [ref: < DOE.Id]
 */
)
{ public Task() : this(default,"","",DateTime.Now,false,null,null,null,null,null,null,null,null,null) { } }//empty ctor (the parametrize ctoer was already defined)

