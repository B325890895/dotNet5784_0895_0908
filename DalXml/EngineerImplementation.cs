namespace Dal;
using DalApi;
using DO;
using System;
using System.Xml.Linq;

internal class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        XDocument doc = XDocument.Load(@"..\xml\engineers.xml");
        doc.Root!.Add(new XElement("Engineer",
                                new XElement("Id", item.Id),
                                new XElement("Email", item.Email),
                                new XElement("Cost", item.Cost),
                                new XElement("Name", item.Name),
                                new XElement("Level", item.Level)));
        doc.Save(@"..\xml\engineers.xml");
        return item.Id;
    }

    public void Delete(int id)
    {
        XDocument doc = XDocument.Load(@"..\xml\engineers.xml");
        var engineerToDelete = doc.Descendants("Engineer").
            FirstOrDefault(engineer => engineer.Element("Id")!.Value.Equals(Convert.ToString(id))) 
            ?? throw new DalDoesNotExistException($"Engineer with ID={id} does Not exist");
        engineerToDelete.Remove();
        doc.Save(@"..\xml\engineers.xml");
    }

    public Engineer? Read(int id)
    {
        XDocument doc = XDocument.Load(@"..\xml\engineers.xml");
        var engineerItem = doc.Descendants("Engineer").
                    FirstOrDefault(engineer => engineer.Element("Id")!.Value.Equals(Convert.ToString(id)))
                    ?? throw new DalDoesNotExistException($"Engineer with ID={id} does Not exist");
        Enum.TryParse<EngineerExperience>(engineerItem.Element("Level")!.Value, out EngineerExperience level);
        Engineer engineer = new Engineer(Convert.ToInt32(engineerItem.Element("Id")!.Value),
            engineerItem.Element("Email")!.Value, Convert.ToDouble(engineerItem.Element("Cost")!.Value),
            engineerItem.Element("Name")!.Value, level);
        return engineer;
    }

    public Engineer? Read(Func<Engineer, bool> filter)
    {
        List<Engineer>? engineersList = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        return engineersList!.FirstOrDefault(filter);
    }

    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<Engineer> engineersList = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        if (filter != null)
            if(engineersList.Any()) return engineersList.Where(filter);
            else return engineersList;
        else
            return engineersList.Select(item => item);

    }

    public void Update(Engineer item)
    {
        XDocument doc = XDocument.Load(@"..\xml\engineers.xml");
        var engineerToDelete = doc.Descendants("Engineer").
            FirstOrDefault(engineer => engineer.Element("Id")!.Value.Equals(Convert.ToString(item.Id)))
            ?? throw new DalDoesNotExistException($"Engineer with ID={item.Id} does Not exist");
        engineerToDelete.Remove();
        doc.Save(@"..\xml\engineers.xml");
        Create(item);
    }
}

