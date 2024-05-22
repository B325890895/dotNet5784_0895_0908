
namespace Dal;
using DalApi;
using DO;


internal class DependencyImplementation : IDependency//The class that implements the IDependency interface
{
    public int Create(Dependency item)//Creates new Dependency object in DAL
    {

        Dependency newItem = item with { Id = DataSource.Config.NextDependencyId };
        ////Checking that these tasks exist in the system
        //Dependency newItem = (item.Id != 0) ? item : item with { Id = Config.NextDependencyId };
        //if (Read(newItem.DependentTask) == null || Read(newItem.DependsOnTask) == null)
        //    throw new DalDoesNotExistException("It is not possible to add dependencies between non - existing tasks");

        ////A check that the same dependency did not exist in the system
        //Dependency? itemOfDependentTask = Read((item) => { return item.DependentTask == newItem.DependentTask; });
        //Dependency? itemOfNextDependencyId = Read((item) => { return item.DependsOnTask == newItem.DependsOnTask; });
        //if (itemOfDependentTask != null && itemOfNextDependencyId != null &&
        //    itemOfDependentTask.DependentTask == newItem.DependentTask && itemOfNextDependencyId.DependsOnTask == newItem.DependsOnTask)
        //    throw new DalExistException("This dependency already exists in the system");

        DataSource.Dependencies!.Add(newItem);
        return newItem.Id;
    }

    public void Delete(int id)//The delete function throw an exception because Dependencies can't be deleted!!
    {
       throw  new DalDeletionIsProhibitedException("Dependencies cannot be deleted");
        //במקרה שצריך למחוק כי מחקו את המשימה שלו 
        /*
         Dependency? removeItem = DataSource.Dependencies!.Find(current => current.Id == id);
         if (removeItem != null)
         {
             DataSource.Dependencies.Remove(removeItem);
         }
         else
         {
             throw new DalDoesNotExistException($"Dependencies with ID={id} does Not exist");
         }
        */
    }

    public Dependency? Read(int id)//Reads dependency object by its ID 
    {
        var dependencyItem = from item in DataSource.Dependencies
                             where item.Id == id
                             select item;
        return dependencyItem.FirstOrDefault();
    }
 
    public Dependency? Read(Func<Dependency, bool> filter) //stage 2 , Reads dependency object by function
    {
        var dependencyItem = from item in DataSource.Dependencies
                           where filter(item)
                           select item;
        return dependencyItem.FirstOrDefault();
    }

    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null) //stage 2, Reading all dependencies that establish a certain condition or all of them (if no condition is sent)
    {
        if (filter != null)
        {
            return from item in DataSource.Dependencies
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Dependencies
               select item;
    }

    public void Update(Dependency item)////Updates dependency object
    {
        Dependency? removeItem = DataSource.Dependencies!.Find(current => current.Id == item.Id);
        if (removeItem != null)
        {
            DataSource.Dependencies.Remove(removeItem);
            DataSource.Dependencies.Add(item);
        }
        else
        {
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} does Not exist");
        }
    }
}

