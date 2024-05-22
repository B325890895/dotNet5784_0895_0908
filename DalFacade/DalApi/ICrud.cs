namespace DalApi;

public interface ICrud<T> where T : class//Generic Interface for all the entities methods.
{
    int Create(T item); //Creates new item from type T object in DAL
    T? Read(int id); //Reads item from type T object by its ID 
    T? Read(Func<T, bool> filter); // stage 2, Reads item by any of its parameters.
    IEnumerable<T> ReadAll(Func<T, bool>? filter = null); // stage 2
    void Update(T item); //Updates item from type T object
    void Delete(int id); //Deletes a item from type T object by its Id
}

