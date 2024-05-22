using BlApi;
using BO;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// Creates a new task with the provided information and optional dependencies.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <param name="alias">The alias of the task.</param>
    /// <param name="description">The description of the task.</param>
    /// <param name="dependencies">Optional list of dependencies for the task.</param>
    /// <param name="scheduledDate">The scheduled date of the task.</param>
    /// <param name="requiredEffortTime">The required effort time for the task.</param>
    /// <param name="deadlineDate">The deadline date for the task.</param>
    /// <param name="deliverables">Optional deliverables for the task.</param>
    /// <param name="remarks">Optional remarks for the task.</param>
    /// <param name="complexity">The complexity level of the task.</param>
    public void Create(int id, string alias, string description, List<BO.TaskInList>? dependencies, DateTime? scheduledDate, TimeSpan requiredEffortTime, DateTime? deadlineDate, string? deliverables, string? remarks, BO.EngineerExperience copmlexity)
    {
        int taskId;
        if (scheduledDate is not null && deadlineDate is not null && deadlineDate < scheduledDate) throw new BO.BlDataIsInvalidException("scheduled Date is later then the deadline Date");
        if (id > 0 || alias != "" || alias != null)
        {
            DO.Task doTask = new(id, alias, description, DateTime.Now, false, requiredEffortTime, (DO.EngineerExperience)Enum.Parse(typeof(DO.EngineerExperience), copmlexity.ToString()), null, scheduledDate, deadlineDate, null, deliverables, remarks, null);
            taskId = _dal.Task.Create(doTask);
        }
        else
        {
            throw new Exception("Can't add Task without valid details");
        }
        if (dependencies != null && dependencies.Any())
        {
            foreach (var dpn in dependencies)
            {
                var taskExist = _dal.Task.Read(dpn.Id) ?? throw new BO.BlDoesNotExistException($"Faild to found the Depenets on task num{dpn.Id}!");
                DO.Dependency dependency = new DO.Dependency(default, taskId, dpn.Id);
                try
                {
                    _dal.Dependency.Create(dependency);
                }
                catch (DO.DalDoesNotExistException ex)
                {
                    throw new BO.BlDoesNotExistException($"faild to add dependency between task id={id} and task = {dpn}", ex);
                }
                catch (DO.DalExistException ex)
                {
                    throw new BO.BlExistException($"The depedency is already exist", ex);

                }
            }
        }
    }

    /// <summary>
    /// Deletes a task with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the task to be deleted.</param>
    public void Delete(int id)
    {
        List<int> dependenciesToDeleteId = new List<int>();  
        foreach (var item in _dal.Dependency.ReadAll())
        {
            if (item!.DependentTask == id && _dal.Task.Read(item.DependsOnTask)!.IsMilestone)
            {
                throw new BO.BlDeletionIsProhibitedException($"Deletion Of Task {id} Is Prohibited after create the schedule");
            }
            if (item!.DependsOnTask == id)
                throw new BO.BlDeletionIsProhibitedException($"Deletion Of Task {id} Is Prohibited");
            if(item!.DependentTask == id)
            {
                dependenciesToDeleteId.Add(item.Id);
            }
        }
        foreach (var item in dependenciesToDeleteId)
        {
            _dal.Dependency.Delete(item);
        }
        try
        {
            _dal.Task.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"faild to delete task {id}", ex);
        }
    }

    public BO.Task? Read(int id)
    {
        DO.Task doTask = _dal.Task.Read(id) ?? throw new BO.BlDoesNotExistException($"Faild to find Task num={id}");
        if (doTask.IsMilestone) throw new BO.BlDoesNotExistException($"Faild to find Task num={id}");
        IEnumerable<DO.Dependency>? dependencies = _dal.Dependency.ReadAll();

        List<BO.TaskInList>? previousTasks = null;
        BO.MilestoneInTask? milestone = null;
        BO.Status status = BO.Status.Unscheduled;
        if (dependencies.Any())
        {
            List<int> previousTasksId = (from item in dependencies
                                         where item.DependentTask == id
                                         select item.DependsOnTask).ToList();

            previousTasks = previousTasksId.Any() ? (from depId in previousTasksId
                                                     let myTask = _dal.Task.Read(depId)
                                                     select new BO.TaskInList(myTask.Id, myTask.Description, myTask.Alias, 0)).ToList() : new List<BO.TaskInList>();
            milestone = previousTasksId.Any() ? (from depId in previousTasksId
                                                 let myTask = _dal.Task.Read(depId)
                                                 where myTask.IsMilestone == true
                                                 select new BO.MilestoneInTask(myTask.Id, myTask.Alias)).FirstOrDefault() ?? null : null;
            status = milestone != null ? doTask.CompleteDate != null ? BO.Status.Done : (DateTime.Now + doTask.RequiredEffortTime > doTask.DeadlineDate) ? BO.Status.InJeopardy : doTask.StartDate != null ? BO.Status.OnTrack : BO.Status.Scheduled : BO.Status.Unscheduled;
        }
        BO.EngineerInTask? engineerInTask = null;
        if (doTask.EngineerId != null && doTask.EngineerId != default)
        {
            DO.Engineer? engineer = _dal.Engineer.Read((int)doTask.EngineerId) ?? throw new BO.BlDoesNotExistException($"Faild to found engineer {doTask.EngineerId} that assign to that task!");
            engineerInTask = new BO.EngineerInTask(engineer.Id, engineer.Name);
        }

        BO.Task boTask = new BO.Task(doTask.Id, doTask.Alias, doTask.Description, doTask.CreatedAtDate, status, previousTasks, milestone, (TimeSpan)doTask.RequiredEffortTime!, doTask.StartDate, doTask.ScheduledDate, doTask.StartDate + doTask.RequiredEffortTime, doTask.DeadlineDate, doTask.CompleteDate, doTask.Deliverables, doTask.Remarks, engineerInTask, (BO.EngineerExperience)Enum.Parse(typeof(BO.EngineerExperience), doTask.Copmlexity.ToString()!));
        return boTask;
    }

    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        IEnumerable<DO.Dependency> dependencies = _dal.Dependency.ReadAll();
        IEnumerable<DO.Task> tasks = _dal.Task.ReadAll();
        IEnumerable<DO.Engineer> engineers = _dal.Engineer.ReadAll();
        IEnumerable<BO.Task> taskList = tasks.Any() ? from task in tasks
                                                      let previousTasksId = dependencies.Any() ? from dpn in dependencies
                                                                                                 where dpn.DependentTask == task.Id
                                                                                                 select new { Id = dpn.DependsOnTask } : null
                                                      let previousTasks = dependencies.Any() && previousTasksId.Any() ? (from item in previousTasksId
                                                                                                                         let myTask = _dal.Task.Read(item.Id)
                                                                                                                         select new BO.TaskInList(myTask.Id, myTask.Description, myTask.Alias, 0)).ToList() : new List<BO.TaskInList>()
                                                      let milestone = dependencies.Any() && previousTasksId.Any() ? (from item in previousTasksId
                                                                                                                    let myTask = _dal.Task.Read(item.Id)
                                                                                                                    where myTask.IsMilestone == true
                                                                                                                    select new BO.MilestoneInTask(myTask.Id, myTask.Alias)).FirstOrDefault() ?? null : null 
                                                      let status = dependencies.Any() && milestone != null ? task.CompleteDate != null ? BO.Status.Done : (DateTime.Now + task.RequiredEffortTime > task.DeadlineDate) ? BO.Status.InJeopardy : task.StartDate != null ? BO.Status.OnTrack : BO.Status.Scheduled : BO.Status.Unscheduled 
                                                      let engineerInTask = engineers.Any()&& task.EngineerId is not null ? (from engineer in engineers
                                                                                              where engineer.Id == task.EngineerId
                                                                                              select engineer).First() ?? null : null
                                                      where task.IsMilestone == false
                                                      select new BO.Task(task.Id,task.Alias, task.Description, task.CreatedAtDate, status, previousTasks.ToList(),
                                                      milestone, (TimeSpan)task.RequiredEffortTime!, task.StartDate, task.ScheduledDate,
                                                      task.StartDate + task.RequiredEffortTime, task.DeadlineDate, task.CompleteDate,
                                                      task.Deliverables, task.Remarks, engineerInTask != null ? new BO.EngineerInTask(engineerInTask.Id, engineerInTask.Name) : null,
                                                      (BO.EngineerExperience)Enum.Parse(typeof(BO.EngineerExperience), task.Copmlexity.ToString()!)) : new List<BO.Task>();

        if (filter != null)
        {
            return from item in taskList
                   where filter(item)
                   select item;
        }
        return taskList;
    }


    public void Update(BO.Task task)
    {

        if (task.Id > 0 || task.Alias != "" || task.Alias != default)
        {
            DO.Task doTask = new DO.Task(task.Id, task.Alias, task.Description, task.CreatedAtDate, false, task.RequiredEffortTime, (DO.EngineerExperience)Enum.Parse(typeof(DO.EngineerExperience), task.Copmlexity.ToString()!), task.StartDate, task.ScheduledDate, task.DeadlineDate, task.CompleteDate, task.Deliverables, task.Remarks, task.Engineer != null ? task.Engineer.EngineerId : null);
            try
            {
                _dal.Task.Update(doTask);
            }
            catch (Exception ex)
            {
                throw new BO.BlDoesNotExistException($"Faild to find task {task.Id}", ex);
            }
            return;
        }
        throw new BO.BlDataIsInvalidException($"Invalid data to update task {task.Id}");
    }

}

