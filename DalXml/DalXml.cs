using DalApi;
using System.Xml.Linq;

namespace Dal;

//stage 3
sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }

    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    DateTime? IDal.ProjectStartDate => Convert.ToDateTime(XDocument.Load(@"..\xml\data-config.xml").Descendants("ProjectStartDate").FirstOrDefault()!
        .Value == default ? null : XDocument.Load(@"..\xml\data-config.xml").Descendants("ProjectStartDate").FirstOrDefault()!.Value);

    DateTime? IDal.ProjectEndDate => Convert.ToDateTime(XDocument.Load(@"..\xml\data-config.xml").Descendants("ProjectEndDate").FirstOrDefault()!
        .Value == default ? null : XDocument.Load(@"..\xml\data-config.xml").Descendants("ProjectEndDate").FirstOrDefault()!.Value);

    public void Reset()
    {
        XDocument doc = XDocument.Load(@"..\xml\tasks.xml");
        var tasksToDelete = doc.Descendants("ArrayOfTask").Elements();
        while (tasksToDelete.Any())
            tasksToDelete.First().Remove();
        doc.Save(@"..\xml\tasks.xml");
        doc = XDocument.Load(@"..\xml\dependencies.xml");
        var dependenciesToDelete = doc.Descendants("ArrayOfDependency").Elements();
        while (dependenciesToDelete.Any())
            dependenciesToDelete.First().Remove();
        doc.Save(@"..\xml\dependencies.xml");
        doc = XDocument.Load(@"..\xml\engineers.xml");
        var engineersToDelete = doc.Descendants("ArrayOfEngineer").Elements();
        while (engineersToDelete.Any())
            engineersToDelete.First().Remove();
        doc.Save(@"..\xml\engineers.xml");
    }
}

