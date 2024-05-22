using BlApi;
using System.ComponentModel.DataAnnotations;
namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

///<summary>
/// Creates a new engineer with the given details.
/// </summary>
/// <param name="id">The ID of the engineer.</param>
/// <param name="name">The name of the engineer.</param>
/// <param name="email">The email of the engineer.</param>
/// <param name="level">The experience level of the engineer.</param>
/// <param name="cost">The cost of the engineer.</param>
/// <param name="task">The task the engineer is assigned to (optional).</param>
public void Create(int id, string name, string email, BO.EngineerExperience level, double cost, BO.TaskInEngineer? task)
    {
        var emailValidation = new EmailAddressAttribute();

        // Validate the input parameters
        if (id > 0 || (name != "" || name != default) || emailValidation.IsValid(email) || cost > 0)
        {
            // Check if the engineer is assigned to a task
            if (task != null)
            {
                // Retrieve the task data and ensure it exists
                DO.Task taskData = _dal.Task.Read(task.Id) ?? throw new BO.BlDoesNotExistException($"Failed to find task {task.Id}");

                // Check if the engineer is already assigned to another task
                if (taskData.EngineerId != null)
                    throw new BO.BlCanNotUseTheEntityException($"The engineer {taskData.EngineerId} has already been assigned to another task");

                // Check if the engineer's level is suitable for the assigned task
                if (Convert.ToInt32(taskData.Copmlexity) > Convert.ToInt32(level))
                    throw new BO.BlCanNotUseTheEntityException($"The engineer {taskData.EngineerId} you selected is not at the assigned task level.");

                // Update the task with the engineer's ID
                DO.Task newTask = taskData with { EngineerId = id };
                _dal.Task.Update(newTask);
            }

            // Create a new engineer based on the provided details
            DO.EngineerExperience doLevel = (DO.EngineerExperience)Enum.Parse(typeof(DO.EngineerExperience), level.ToString());
            DO.Engineer doEngineer = new DO.Engineer(id, email, cost, name!, doLevel);

            try
            {
                _dal.Engineer.Create(doEngineer);
            }
            catch (Exception ex)
            {
                throw new BO.BlExistException("Couldn't add the Engineer!", ex);
            }
        }
        else
        {
            throw new BO.BlDataIsInvalidException("Can't add Engineer without valid details");
        }
    }

    /// <summary>
    /// Deletes the engineer with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the engineer to delete.</param>
    public void Delete(int id)
    {
        // Check if the engineer is assigned to any tasks
        var engineer = _dal.Engineer.Read(id);
        var engineerTask = _dal.Task.ReadAll().Where(eng => eng != null&& eng.EngineerId == id);
        if (engineerTask.Any())
        {
            throw new BO.BlDeletionIsProhibitedException($"Deletion  engineer with ID:{id} Is Prohibited!");
        }

        try
        {
            // Delete the engineer from the data access layer
            _dal.Engineer.Delete(id);
        }
        catch (Exception ex)
        {
            throw new BO.BlDoesNotExistException($"Failed to delete Engineer with Id = {id}", ex);
        }
    }


    /// <summary>
    /// Retrieves the engineer with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the engineer to retrieve.</param>
    /// <returns>The engineer with the specified ID.</returns>
    public BO.Engineer? Read(int id)
    {
        DO.Engineer? doEngineer = null;

        try
        {
            // Read the engineer from the data access layer
            doEngineer = _dal.Engineer.Read(id);
        }
        catch (Exception ex)
        {
            throw new BO.BlDoesNotExistException($"Failed to find Engineer with Id = {id}", ex);
        }

        // Read all tasks from the data access layer
        IEnumerable<DO.Task> tasks = _dal.Task.ReadAll();

        BO.TaskInEngineer? importEngineerTask = null;

        if (tasks.Any())
        {
            // Find the tasks assigned to the engineer and create a BO.TaskInEngineer object
            importEngineerTask = tasks
                .Where(task => task.EngineerId is not null && task.EngineerId == id)
                .Select(task => new BO.TaskInEngineer(task!.Id, task.Alias))
                .FirstOrDefault() ?? null;
        }

        // Create a new BO.Engineer object based on the retrieved engineer and assigned task (if any)
        BO.Engineer engineer = new BO.Engineer(
            id,
            doEngineer!.Name,
            doEngineer.Email,
            (BO.EngineerExperience)doEngineer.Level,
            doEngineer.Cost,
            importEngineerTask
        );

        return engineer;
    }


    /// <summary>
    /// Retrieves a list of all engineers, optionally filtered by a provided filter function.
    /// </summary>
    /// <param name="filter">Optional filter function to apply to the engineers.</param>
    /// <returns>A list of engineers.</returns>
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        // Read all engineers from the data access layer and create BO.Engineer objects
        IEnumerable<BO.Engineer>? engineersList = _dal.Engineer.ReadAll()
            .Select(doEngineer =>
                new BO.Engineer(
                    doEngineer!.Id,
                    doEngineer.Name,
                    doEngineer.Email,
                    (BO.EngineerExperience)Enum.Parse(typeof(BO.EngineerExperience), doEngineer.Level.ToString()),
                    doEngineer.Cost,
                    _dal.Task.ReadAll()
                        .Where(task => task!.EngineerId == doEngineer.Id)
                        .Select(task => new BO.TaskInEngineer(task!.Id, task.Alias))
                        .FirstOrDefault() ?? null
                )
            );

        // Apply the filter function if provided
        if (filter != null)
        {
            return engineersList.Where(filter);
        }

        return engineersList;
    }

    /// <summary>
    /// Updates the information of an engineer.
    /// </summary>
    /// <param name="engineer">The engineer object containing the updated information.</param>
    public void Update(BO.Engineer engineer)
    {
        var emailValidation = new EmailAddressAttribute();
        if (engineer.Id > 0 || (engineer.Name != "" || engineer.Name != default) || emailValidation.IsValid(engineer.Email) || engineer.Cost > 0)
        {
            DO.Engineer doEngineer = new DO.Engineer(engineer.Id, engineer.Email,engineer.Cost,engineer.Name!, (DO.EngineerExperience)Enum.Parse(typeof(DO.EngineerExperience), engineer.Level.ToString()));
            try {
                _dal.Engineer.Update(doEngineer);
            }
            catch(Exception ex)
            {
                throw new BO.BlDoesNotExistException($"Failed to update engineer with ID:{engineer.Id}", ex);
            }
        }
        else
        {
            throw new BO.BlDataIsInvalidException($"Failed to update engineer with ID:{engineer.Id}");
        }

    }

 

}

