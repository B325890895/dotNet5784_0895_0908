using BO;
using System.Runtime.InteropServices;

namespace BlApi
{
    public interface ITask
    {
        public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null);
        public BO.Task? Read(int id);
        public void Create(int id, string alias, string description, List<BO.TaskInList>? dependencies, DateTime? scheduledDate, TimeSpan requiredEffortTime, DateTime? deadlineDate, string? deliverables, string? remarks, BO.EngineerExperience copmlexity);
        public void Delete(int id);
        public void Update(BO.Task task);
    }
}



   
             
             
           
             