using BlApi;


namespace BlImplementation;

internal class MilestoneImplementation : IMilestone
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
  
    public void creatingTheProjectSchedule()
    {
        
        IEnumerable<DO.Task> tasksList = _dal.Task.ReadAll();
        if(tasksList.Any())
        {
            foreach (DO.Task task in tasksList)
            {
                if (task.IsMilestone == true) throw new BO.BlScheduledException("you scheduled your project already!");
            }
        }
        IEnumerable<DO.Dependency> dependenciesList = _dal.Dependency.ReadAll();
        if (!dependenciesList.Any()) throw new BO.BlDoesNotExistException("There is no dependency for creating any schedule");
        if (!tasksList.Any()) throw new BO.BlDoesNotExistException("There is no task for creating any schedule");
        List<DO.Dependency> dependencies = _dal.Dependency.ReadAll().ToList() ?? throw new BO.BlDoesNotExistException("Faild to find any dependency");
        var tasksOfTasks = (from task in _dal.Task.ReadAll()
                            let prevTasks = (from dependency in _dal.Dependency.ReadAll()
                                             where dependency.DependentTask == task.Id
                                             select dependency.DependsOnTask)
                            where prevTasks.Any() && prevTasks is not null
                            select prevTasks?.Order().Distinct().ToList()).ToList();

       for (int i = 1;i< tasksOfTasks.Count(); i++)
        {
            if (tasksOfTasks.ElementAt(i - 1).Count() == tasksOfTasks.ElementAt(i).Count())
            {
                List<int> first = tasksOfTasks.ElementAt(i - 1);
                List<int> second = tasksOfTasks.ElementAt(i - 1);
                bool equal = true;
                for(int j =0;j< tasksOfTasks.ElementAt(i - 1).Count();j++)
                {
                    if (first.ElementAt(j) != second.ElementAt(j))
                    {
                        equal = false;
                    }
                }
                if (equal)
                { 
                    tasksOfTasks.RemoveAt(i--);
                }
            }
        }
        int index = 0;
        var groupsOfTasks = (from tasks in tasksOfTasks
                             group tasks by index++ into taskGroup
                             select taskGroup).ToList();

        var lastTasksId = (from task in _dal.Task.ReadAll()
                          let notFinalTask = (from dpn in _dal.Dependency.ReadAll()
                                              where dpn.DependsOnTask == task.Id
                                              select task.Id).FirstOrDefault()
                          where notFinalTask == default
                          select task.Id).ToList();
        IGrouping<int,List<int>> groupOfLastTasks = (from id in lastTasksId
                                group lastTasksId by index++ into taskGroup
                                select taskGroup).First();

        groupsOfTasks.Add(groupOfLastTasks);
        List<int> dependenciesIdToDelete = new List<int>();
        List<int> milestonesIdToDelete = new List<int>();
        DateTime lastTaskTime = DateTime.MinValue;
        DateTime? ProjectEndDate = _dal.ProjectEndDate ?? throw new BO.BlTheProjectIsUnscheduledException("לא תזמנת את זמני הפרויקט");
        DateTime? ProjectStartDate = _dal.ProjectStartDate ?? throw new BO.BlTheProjectIsUnscheduledException("לא תזמנת את זמני הפרויקט");
        DO.Task start = new(default, "Start", "this is start milestone", DateTime.Now, true, TimeSpan.Zero, null, null, ProjectStartDate, null, null, null, null, null);
        int lastMilestone= _dal.Task.Create(start);
        milestonesIdToDelete.Add(lastMilestone);
        foreach (var groupOfTasks in groupsOfTasks)//creation of the milestones!
        {
            int copmlexityNum = 0;
            DateTime? latestDeadlineDate = DateTime.MinValue;
            DateTime? earliestScheduledDate = DateTime.MaxValue;
            foreach (var tasks in groupOfTasks)
            {
                var tasksData = from taskId in tasks
                                let taskData = (from task in _dal.Task.ReadAll()
                                                where taskId == task.Id
                                                select new { scheduledDate = task.ScheduledDate, deadlineDate = task.DeadlineDate, copmlexity = task.Copmlexity }).ElementAt(0)
                                select taskData;
                int copmlexitySum = 0;
                foreach (var item in tasksData)
                {
                    copmlexitySum += item.copmlexity == DO.EngineerExperience.Beginner ? 0 : item.copmlexity == DO.EngineerExperience.AdvancedBeginner ? 1 : item.copmlexity == DO.EngineerExperience.Intermediate ? 2 : item.copmlexity == DO.EngineerExperience.Advanced ? 3 : 4;
                    if (item.deadlineDate == null || item.scheduledDate == null)
                    {
                        deleteDependenciesAndMilestoneInException(dependenciesIdToDelete, milestonesIdToDelete);
                        throw new BO.BlTheTasksAreUnscheduledException("לא תיזמנת את השעות של כל המשימות שלך בקשה תתזמן את הנשימות");

                    }
                    if (item.deadlineDate > latestDeadlineDate) latestDeadlineDate = item.deadlineDate;
                    if (item.scheduledDate < earliestScheduledDate) earliestScheduledDate = item.scheduledDate;
                }
                copmlexityNum = copmlexityNum /( tasksData.Count() == 0 ? 1 : tasksData.Count());
            }
            DO.EngineerExperience copmlexity = copmlexityNum == 0 ? DO.EngineerExperience.Beginner : copmlexityNum == 1 ? DO.EngineerExperience.AdvancedBeginner : copmlexityNum == 2 ? DO.EngineerExperience.Intermediate : copmlexityNum == 3 ? DO.EngineerExperience.Advanced : DO.EngineerExperience.Expert;
            string taskAlias = $"M{BO.Tools.NextMilestoneId}";
            if (groupOfTasks == groupsOfTasks.ElementAt(groupsOfTasks.Count()-1))
            {
                taskAlias = "End";
                if (latestDeadlineDate > ProjectEndDate) {
                    deleteDependenciesAndMilestoneInException(dependenciesIdToDelete, milestonesIdToDelete);
                    throw new BO.BlTheTasksAreUnscheduledException("הזמן שלך לא מתוזמן נכון המשימה ברבן דרך קודמת מתחילה לפני שהאבן דרך לפניך נגמרת!!");
                }
            }
            if (lastTaskTime > earliestScheduledDate)
            {
                deleteDependenciesAndMilestoneInException(dependenciesIdToDelete, milestonesIdToDelete);
                throw new BO.BlTheTasksAreUnscheduledException("הזמן שלך לא מתוזמן נכון המשימה ברבן דרך קודמת מתחילה לפני שהאבן דרך לפניך נגמרת!!"); 
            }
            if (groupsOfTasks.ElementAt(0) == groupOfTasks) 
            {
                if (earliestScheduledDate < ProjectStartDate)
                {
                    deleteDependenciesAndMilestoneInException(dependenciesIdToDelete, milestonesIdToDelete);
                    throw new BO.BlTheTasksAreUnscheduledException("התחלת את המשימות הראשונות לפני תחילת הפרויקט!!");
                }
            }
            lastTaskTime = (DateTime)latestDeadlineDate;
            DO.Task newMilestone = new DO.Task(default, taskAlias, $"this is milestone {taskAlias}", DateTime.Now, true, latestDeadlineDate - earliestScheduledDate, copmlexity, null, earliestScheduledDate, latestDeadlineDate, null, null, null, null);
            int newMilestoneId = _dal.Task.Create(newMilestone);
            milestonesIdToDelete.Add(newMilestoneId);
            foreach (var tasks in groupOfTasks)
            {
                foreach (var taskId in tasks)
                {
                    //themilestone dependent on his tasks:
                    DO.Dependency newDpn = new(default, newMilestoneId, taskId);
                    int depId = _dal.Dependency.Create(newDpn);
                    dependenciesIdToDelete.Add(depId);
                    //the tasks dependents on priviouse milestone
                    newDpn = new(default, taskId, lastMilestone);
                    depId = _dal.Dependency.Create(newDpn);
                    dependenciesIdToDelete.Add(depId);
                }

                lastMilestone = newMilestoneId;

            }
        }

    }

    public BO.Milestone Read(int id)
    {
        DO.Task doMilestone = _dal.Task.Read(id) ?? throw new Exception($"Mileston with ID:{id} did not found!");//Read the milestone from the task list.
        if(!doMilestone.IsMilestone) throw new Exception($"Mileston with ID:{id} did not found!");
        List<int> tasksID = (from dependency in _dal.Dependency.ReadAll()//take all the id of task that the part of the milestone.
                      where dependency.DependentTask == doMilestone.Id
                      select dependency.DependsOnTask).ToList();
        List<DO.Task> milestoneTasks = (from tsk in tasksID
                                       let taskData = _dal.Task.Read(tsk)
                                       select taskData).ToList();
        BO.Status status = !((from task in milestoneTasks
                               where task.StartDate != null
                               select task).Any()) ? BO.Status.Scheduled : !((from task in milestoneTasks
                                                                              where task.CompleteDate == null
                                                                              select task).Any()) ? BO.Status.Done : (from task in milestoneTasks
                                                                                                                      where task.CompleteDate == null && task.ScheduledDate + task.RequiredEffortTime < DateTime.Now
                                                                                                                     select task).Any() ? BO.Status.InJeopardy : BO.Status.OnTrack;
        double progressPercentage = ((from task in milestoneTasks
                              where task.CompleteDate != null
                              select task).Count() * 100) /( tasksID.Count() == 0 ? 1 : tasksID.Count());
        DateTime? complateDate = null;
        if (status == BO.Status.Done)
        {
            complateDate = (from task in milestoneTasks
                            where task.CompleteDate != null
                            select task.CompleteDate).Max();
        }

        List<BO.TaskInList> dependencies = (from task in tasksID
                                        let taskData = _dal.Task.Read(task)
                                        let taskStatus = taskData.StartDate==null?BO.Status.Scheduled:taskData.CompleteDate!=null?BO.Status.Done: taskData.ScheduledDate + taskData.RequiredEffortTime < DateTime.Now?BO.Status.InJeopardy:BO.Status.OnTrack
                                         select new BO.TaskInList(task,taskData.Description, taskData.Alias, taskStatus)).ToList();
        BO.Milestone boMilestone = new(id, doMilestone.Alias, doMilestone.Description, doMilestone.CreatedAtDate, status, doMilestone.ScheduledDate + doMilestone.RequiredEffortTime, doMilestone.DeadlineDate, complateDate, progressPercentage, doMilestone.Remarks, dependencies);
        return boMilestone;
    }

    public BO.Milestone Update(int id, string alias, string description, string? Remarks)
    {
        DO.Task milestone = _dal.Task.Read(id) ?? throw new BO.BlDoesNotExistException($"milestone with Id {id} was not found");
        if (!milestone.IsMilestone)
            throw new BO.BlDoesNotExistException($"milestone with Id {id} was not found");
        DO.Task updatedMilestone = new DO.Task(id, (alias == "" || alias == null) ? milestone.Alias : alias, (description == "" || description == null) ? milestone.Description : description, milestone.CreatedAtDate, true, null, milestone.Copmlexity, milestone.StartDate, milestone.ScheduledDate, milestone.DeadlineDate, milestone.CompleteDate, milestone.Deliverables,Remarks, milestone.EngineerId);
        try
        {
            _dal.Task.Update(updatedMilestone);
        }
        catch(DO.DalDoesNotExistException e)
        {
            throw new BO.BlDoesNotExistException("", e);
        }
        return Read(id);
    }
    private void deleteDependenciesAndMilestoneInException(List<int> dependenciesIdToDelete, List<int> milestonesIdToDelete)
    {
        foreach (var id in dependenciesIdToDelete)
        {
            _dal.Dependency.Delete(id);
        }
        foreach (var id in milestonesIdToDelete)
        {
            _dal.Task.Delete(id);
        }
    }
}


