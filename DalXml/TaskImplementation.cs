namespace Dal;
using DalApi;
using DO;
using System.Xml;
using System.Xml.Linq;


internal class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        Task newItem = (item.Id != default) ? item : item with { Id = Config.NextTaskId };
        XDocument doc = XDocument.Load(@"..\xml\tasks.xml");
        doc.Root!.Add(new XElement("Task",
                                new XElement("Id", newItem.Id),
                                new XElement("Alias", newItem.Alias),
                                new XElement("Description", newItem.Description),
                                new XElement("CreatedAtDate", newItem.CreatedAtDate),
                                new XElement("IsMilestone", newItem.IsMilestone)));
        if (!newItem.IsMilestone)
        {
            doc.Descendants("Task").FirstOrDefault(task => task.Element("Id")!.Value.Equals(Convert.ToString(newItem.Id)))!.
                Add(new XElement("RequiredEffortTime", newItem.RequiredEffortTime));
        }
        if(newItem.Copmlexity != null)
        {
            doc.Descendants("Task").FirstOrDefault(task => task.Element("Id")!.Value.Equals(Convert.ToString(newItem.Id)))!.
                Add(new XElement("Copmlexity", newItem.Copmlexity));
        }
        if (newItem.StartDate != null)
        {
            doc.Descendants("Task").FirstOrDefault(task => task.Element("Id")!.Value.Equals(Convert.ToString(newItem.Id)))!.
                Add(new XElement("StartDate", newItem.StartDate));
        }
        if (newItem.ScheduledDate != null)
        {
            doc.Descendants("Task").FirstOrDefault(task => task.Element("Id")!.Value.Equals(Convert.ToString(newItem.Id)))!.
                Add(new XElement("ScheduledDate", newItem.ScheduledDate));
        }
        if (newItem.DeadlineDate != null)
        {
            doc.Descendants("Task").FirstOrDefault(task => task.Element("Id")!.Value.Equals(Convert.ToString(newItem.Id)))!.
                Add(new XElement("DeadlineDate", newItem.DeadlineDate));
        }
        if (newItem.CompleteDate != null)
        {
            doc.Descendants("Task").FirstOrDefault(task => task.Element("Id")!.Value.Equals(Convert.ToString(newItem.Id)))!.
                Add(new XElement("CompleteDate", newItem.CompleteDate));
        }
        if (newItem.Deliverables != null)
        {
            doc.Descendants("Task").FirstOrDefault(task => task.Element("Id")!.Value.Equals(Convert.ToString(newItem.Id)))!.
                Add(new XElement("Deliverables", newItem.Deliverables));
        }
        if (newItem.Remarks != null)
        {
            doc.Descendants("Task").FirstOrDefault(task => task.Element("Id")!.Value.Equals(Convert.ToString(newItem.Id)))!.
                Add(new XElement("Remarks", newItem.Remarks));
        }
        if (newItem.EngineerId != null)
        {
            doc.Descendants("Task").FirstOrDefault(task => task.Element("Id")!.Value.Equals(Convert.ToString(newItem.Id)))!.
                Add(new XElement("EngineerId", newItem.EngineerId));
        }
        doc.Save(@"..\xml\tasks.xml");
        return newItem.Id;
    }

    public void Delete(int id)
    {
        XDocument doc = XDocument.Load(@"..\xml\tasks.xml");
        var taskToDelete = doc.Descendants("Task").
            FirstOrDefault(task => task.Element("Id")!.Value.Equals(Convert.ToString(id)))
            ?? throw new DalDoesNotExistException($"Task with ID={id} does Not exist");
        taskToDelete.Remove();
        doc.Save(@"..\xml\tasks.xml");
    }
    
    public Task? Read(int id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        return tasks.Where(task => task.Id == id).FirstOrDefault();

    }

    public Task? Read(Func<Task, bool> filter)
    {
        List<Task>? tasksList = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        return tasksList!.FirstOrDefault(filter);
    }

    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null)
    {
        IEnumerable<Task> tasksList = XMLTools.LoadListFromXMLSerializer<Task>("tasks");

        if (filter != null)
            return tasksList!.Where(filter);
        else
            return tasksList!.Select(item => item);
    }

    public void Update(Task item)
    {
        XDocument doc = XDocument.Load(@"..\xml\tasks.xml");
        var taskToDelete = doc.Descendants("Task").
            FirstOrDefault(task => task.Element("Id")!.Value.Equals(Convert.ToString(item.Id)))
            ?? throw new DalDoesNotExistException($"Task with ID={item.Id} does Not exist");
        taskToDelete.Remove();
        doc.Save(@"..\xml\tasks.xml");
        Create(item);
    }
}

