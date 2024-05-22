using DalApi;
using DO;
using System.Xml.Linq;
namespace Dal;

internal class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        //Checking that these tasks exist in the system
        Dependency newItem = (item.Id != 0) ? item : item with { Id = Config.NextDependencyId };

        //A check that the same dependency did not exist in the system
        Dependency? itemOfDependentTask = Read((item) => { return item.DependentTask == newItem.DependentTask; });
        Dependency? itemOfNextDependencyId= Read((item) => { return item.DependsOnTask == newItem.DependsOnTask; });
        if (itemOfDependentTask != null && itemOfNextDependencyId != null &&
            itemOfDependentTask.DependentTask == newItem.DependentTask && itemOfNextDependencyId.DependsOnTask == newItem.DependsOnTask)
            throw new DalExistException("This dependency already exists in the system");

       

        XDocument doc = XDocument.Load(@"..\xml\dependencies.xml");
        doc.Root!.Add(new XElement("Dependency",
                                new XElement("Id", newItem.Id),
                                new XElement("DependentTask", newItem.DependentTask),
                                new XElement("DependsOnTask", newItem.DependsOnTask)));
        doc.Save(@"..\xml\dependencies.xml");
        return newItem.Id;
    }

    public void Delete(int id)
    {
         XDocument doc = XDocument.Load(@"..\xml\dependencies.xml");
        var dependencyToDelete = doc.Descendants("Dependency").
            FirstOrDefault(dependency => dependency.Element("Id")!.Value.Equals(Convert.ToString(id))) 
            ?? throw new DalDoesNotExistException($"Dependency with ID={id} does Not exist");
        dependencyToDelete.Remove();
        doc.Save(@"..\xml\dependencies.xml"); 
        
    }

    public Dependency? Read(int id)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>("dependencies");
        return dependencies.Where(dependency => dependency.Id == id).FirstOrDefault();
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        XDocument doc = XDocument.Load(@"..\xml\dependencies.xml");
        var dependencyList = doc.Descendants("Dependency")
                            .Select(dependency => new Dependency(
                                    Convert.ToInt32(dependency.Element("Id")!.Value),
                                    Convert.ToInt32(dependency.Element("DependentTask")!.Value),
                                    Convert.ToInt32(dependency.Element("DependsOnTask")!.Value))).ToList();
        var dependencyfilterList = from item in dependencyList
                                   where filter(item)
                                   select item;
        var dependencyItem = dependencyList.FirstOrDefault();
        return dependencyItem;
    }

    public IEnumerable<Dependency>? ReadAll(Func<Dependency, bool>? filter = null)
    {
        XDocument doc = XDocument.Load(@"..\xml\dependencies.xml");
        var dependencyList = doc.Descendants("Dependency")
                                .Select(dependency => new Dependency(
                                        Convert.ToInt32(dependency.Element("Id")!.Value),
                                        Convert.ToInt32(dependency.Element("DependentTask")!.Value),
                                        Convert.ToInt32(dependency.Element("DependsOnTask")!.Value)))
                                .ToList();
        if (filter != null)
        {
            var dependencyFilterList = from item in dependencyList
                                       where filter(item)
                                       select item;
            return dependencyFilterList;
        }
        return dependencyList;
    }

    public void Update(Dependency item)
    {
        XDocument doc = XDocument.Load(@"..\xml\dependencies.xml");
        var dependencyToDelete = doc.Descendants("Dependency").
            FirstOrDefault(dependency => dependency.Element("Id")!.Value.Equals(Convert.ToString(item.Id)))
            ?? throw new DalDoesNotExistException($"Dependency with ID={item.Id} does Not exist");
        dependencyToDelete.Remove();
        doc.Save(@"..\xml\dependencies.xml");
        Create(item);
    }
}
