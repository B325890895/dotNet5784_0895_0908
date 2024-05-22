
namespace Dal;
using DalApi;
using DO;


internal class TaskImplementation : ITask//The class that implements the ITask interface
{
    public int Create(Task item)//Creates new Task object in DAL
    {
        Task newItem = item with { Id = DataSource.Config.NextTaskId };
        DataSource.Tasks!.Add(newItem);
        return newItem.Id;
    }

    public void Delete(int id)//Deletes a Task (its active field==false) object by its Id
    {
        Task? removeItem = DataSource.Tasks!.Find(current => current.Id == id);
        if (removeItem != null)
        {
            DataSource.Tasks.Remove(removeItem);
        }
        else
        {
            throw new DalDoesNotExistException($"Task with ID={id} does Not exist");
        }
    }

    public Task? Read(int id)////Reads all Task objects
    {
        var taskItem = from item in DataSource.Tasks
                       where item.Id == id
                       select item;
        return taskItem.FirstOrDefault();
    }

    public Task? Read(Func<Task, bool> filter)//stage 2 , Reads task object by function
    {
        var taskItem = from item in DataSource.Tasks
                       where filter(item)
                       select item;
        return taskItem.FirstOrDefault();
    }

    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null) //stage 2, Reading all tasks that establish a certain condition or all of them (if no condition is sent)
    {

        if (filter != null)
        {
            return from item in DataSource.Tasks
                   where filter(item)
                   select item;
        }

        return from item in DataSource.Tasks
               select item;
    }

    public void Update(Task item)//Updates Task object
    {
        Task? removeItem = DataSource.Tasks!.Find(current => current.Id == item.Id);
        if (removeItem != null)
        {
            DataSource.Tasks.Remove(removeItem);
            DataSource.Tasks.Add(item);
        }
        else
        {
            throw new DalDoesNotExistException($"Task with ID={item.Id} does Not exist");
        }
    }
}