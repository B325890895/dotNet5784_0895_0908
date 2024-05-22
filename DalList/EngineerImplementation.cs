namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;

internal class EngineerImplementation : IEngineer//The class that implements the IEngineer interface
{
    public int Create(Engineer item)//Creates new Engineer object in DAL
    {

        if (DataSource.Engineers!.Exists(current => current.Id == item.Id))
            throw new DalExistException($"Engineer with ID={item.Id} already exist");
        else
        {
            DataSource.Engineers.Add(item);
        }
        return item.Id;
    }
    public void Delete(int id)//Deletes an Engineer object by its Id
    {
        Engineer? removeItem = DataSource.Engineers!.Find(current => current.Id == id);
        if (removeItem != null)
        {
            DataSource.Engineers.Remove(removeItem);
        }
        else
        {
            throw new DalDoesNotExistException($"Engineer with ID={id} does Not exist");
        }
    }

    public Engineer? Read(int id)//Reads Engineer object by its ID 
    {
        var engineerItem = from item in DataSource.Engineers
                           where item.Id == id
                           select item;
        return engineerItem.FirstOrDefault();
    }

    public Engineer? Read(Func<Engineer, bool> filter)//stage 2 , Reads engineer object by function
    {
        var engineerItem = from item in DataSource.Engineers
                           where filter(item)
                           select item;
        return engineerItem.FirstOrDefault();
    }

    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null) //stage 2, Reading all Engineers details that establish a certain condition or all of them (if no condition is sent)
    {
        if (filter != null)
        {
            return from item in DataSource.Engineers
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Engineers
               select item;
    }

    public void Update(Engineer item)//Updates Engineer object
    {
        Engineer? removeItem = DataSource.Engineers!.Find(current => current.Id == item.Id);
        if (removeItem != null)
        {
            DataSource.Engineers.Remove(removeItem);
            DataSource.Engineers.Add(item);
        }
        else
        {
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does Not exist");
        }
    }
}

