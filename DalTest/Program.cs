using DO;
using DalTest;
using DalApi;


public class Program
{
    static readonly IDal s_dal = Factory.Get; //stage 4

    static void Main(string[] args)
    {
        Program program = new();//Creating an instance of the program class so that we can access through the Main the fields and methods of the class that are not static  
        Console.Write("Would you like to create Initial data? (Y/N)"); //stage 3
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
        if (ans == "Y") //stage 3
                        //Initialization.Do(s_dal); //stage 2
            Initialization.Do(); //stage 4
        program.mainMenu();
    }

    private void mainMenu()// show the user the main menu of the program
    {
        int choice = -1;
        while (choice != 0)//every diffrent choich will call another Sub menu
        {
            try
            {
                Console.WriteLine("Select an entity you want to check:\r\n0 - Exit\r\n1- Task\r\n2- Dependency\r\n3- Engineer");
                int.TryParse(Console.ReadLine(), out choice);
                switch (choice)
                {
                    case 1:
                        taskMenu();
                        break;
                    case 2:
                        dependencyMenu();
                        break;
                    case 3:
                        engineerMenu();
                        break;
                    case 0:
                        break;
                    default:
                        throw new DalOutOfRangeChoiceException("Choose integer number between 0-3");
                }
            }
            catch (DalDeletionIsProhibitedException exception)
            {
                Console.WriteLine(exception);
            }
            catch (DalDoesNotExistException exception)
            {
                Console.WriteLine(exception);
            }
            catch (DalExistException exception)
            {
                Console.WriteLine(exception);
            }
            catch (DalOutOfRangeChoiceException exception)
            {
                Console.WriteLine(exception);
            }
            catch (DalDataWasNotReceived exception)
            {
                Console.WriteLine(exception);
            }
        }
    }

    #region Tasks Functions
    private void taskMenu()//The task menu, options :0- exit 1- create new task , 2- show a specific task's details, 3- show all the tasks details, 4- update a specific task, 5- remove a task from the active tasks, 6- show all the active tasks.
    {
        int choice = -1;
        ITask taskImp = s_dal.Task;
        while (choice != 0)
        {

            Console.WriteLine("Select the action you want to perform:\r\n0 - Exit\r\n1- Adding a task\r\n2- Viewing the task\r\n3- Viewing all tasks\r\n4- Task update\r\n5- Removing the task from the active tasks");
            int.TryParse(Console.ReadLine(), out choice);
            switch (choice)
            {
                case 0:
                    break;
                case 1:
                    taskCreate(taskImp);
                    break;
                case 2:
                    viewTask(taskImp);
                    break;
                case 3:
                    viewAllTasks(taskImp);
                    break;
                case 4:
                    updateTask(taskImp);
                    break;
                case 5:
                    removeTask(taskImp);
                    break;
                default:
                    throw new DalOutOfRangeChoiceException("Choose integer number between 0-6");
            }
        }
    }

    private void taskCreate(ITask taskImp)//create new task
    {
        Console.WriteLine("Please enter the details for the Task:");//Receiving data from the user

        Console.Write("Alias: ");
        string? alias = Console.ReadLine() ?? "";
        alias = alias == "" ? throw new DalDataWasNotReceived("alias is not accepted") : alias;

        Console.Write("Description: ");
        string description = Console.ReadLine() ?? "";
        description = description == "" ? throw new DalDataWasNotReceived("description is not accepted") : description;

        Console.Write("Is Milestone: ");
        string isMilestoneExist = Console.ReadLine() ?? "";
        bool isMilestone = default;
        if (isMilestoneExist == "") throw new DalDataWasNotReceived("Is Mileston is not accepted");
        else if (!(bool.TryParse(isMilestoneExist, out  isMilestone)))throw new DalInvalidInputException("is Milstone input is not valid");

        Console.Write("How many time the task required: ");
        string? input = Console.ReadLine() ?? "";
        TimeSpan requiredEffortTime = default;
        if (input == "") throw new DalDataWasNotReceived("Time required for the task is not accepted");
        if(!(TimeSpan.TryParse(input, out  requiredEffortTime))) throw new DalInvalidInputException("required Effort Time  input is not valid");

        Console.Write("Copmlexity (Novice/AdvancedBeginner/Intermediate/Advanced/Expert): ");
        input = Console.ReadLine() ?? "";
        EngineerExperience copmlexity = default;
        if (input == "") throw new DalDataWasNotReceived("Complexity is not accepted");
        if(!(Enum.TryParse(input, out copmlexity))) throw new DalInvalidInputException("Complexity input is not valid");

        Console.Write("Start Date (yyyy-mm-dd hh:mm:ss): ");
        string? doThereStartDate = Console.ReadLine();
        DateTime startDate = default;
        if (!string.IsNullOrWhiteSpace(doThereStartDate))
        {
            DateTime.TryParse(doThereStartDate, out startDate);
        }

        Console.Write("Scheduled Date (yyyy-mm-dd hh:mm:ss): ");
        string? doThereScheduledDate = Console.ReadLine();
        DateTime scheduledDate = default;
        if (!string.IsNullOrWhiteSpace(doThereScheduledDate))
        {
            DateTime.TryParse(doThereStartDate, out scheduledDate);
        }
        Console.Write("Deadline Date (yyyy-mm-dd hh:mm:ss): ");
        string? doThereDeadlineDate = Console.ReadLine();
        DateTime deadlineDate = default;
        if (!string.IsNullOrWhiteSpace(doThereDeadlineDate))
        {
            DateTime.TryParse(doThereStartDate, out deadlineDate);
        }

        Console.Write("Complete Date (yyyy-mm-dd hh:mm:ss): ");
        string? doThereCompleteDate = Console.ReadLine();
        DateTime completeDate = default;
        if (!string.IsNullOrWhiteSpace(doThereCompleteDate))
        {
            DateTime.TryParse(doThereStartDate, out completeDate);
        }
        Console.Write("Remarks (optional - leave blank if not applicable): ");
        string? remarks = Console.ReadLine();

        Console.Write("Assign To Engineer (enter engineer's id): ");
        input = Console.ReadLine() ?? "";
        int engineerId = default;
        if (!(int.TryParse(input, out  engineerId)) && engineerId <= 0) throw new DalInvalidInputException($"engineer Id={input} isnt valid");
        if (s_dal.Engineer.Read(engineerId) == null) throw new DalDoesNotExistException($"engineer Id={engineerId} isnt exist"); 
    
        //create the task:
        DO.Task task = new DO.Task(default,alias,description,DateTime.Now,isMilestone,requiredEffortTime,copmlexity,startDate,scheduledDate,deadlineDate,completeDate,null,remarks,engineerId);

        int taskId = taskImp.Create(task);
        Console.WriteLine($"{taskId} Task added successfully!");
    }

    private void viewTask(ITask taskImp)//show a specific task's details
    {
        Console.WriteLine("Enter the task ID");
        string input = Console.ReadLine() ?? "";
        if (input == "") throw new DalDataWasNotReceived("Must enter the id of the task you want to see");
        int.TryParse(input, out int taskId);
        DO.Task? checkingTaskId = taskImp.Read(taskId);
        DO.Task showTask = checkingTaskId == null ? throw new DalDoesNotExistException($"The Task {taskId} is not found") : checkingTaskId;
        Console.WriteLine(showTask);
    }
    private void viewAllTasks(ITask taskImp)//show all the tasks details
    {
        IEnumerable<DO.Task> tasks = taskImp.ReadAll()!;
        foreach (DO.Task task in tasks)
        {
            Console.WriteLine(task);
        }
    }
    private void updateTask(ITask taskImp)//update the task by receiving data from the user To skip (if they dont want to update any field) they press enter
    {
        Console.WriteLine("Please enter the id of the task you would like to update");
        string input = Console.ReadLine() ?? "";
        if (input == "") throw new DalDataWasNotReceived("Must enter the id of the task you want to update");
        int.TryParse(input, out int taskId);
        DO.Task? taskToUpdate = taskImp.Read(taskId);
        if (taskToUpdate != null)
        {
            Console.WriteLine(taskToUpdate);
        }
        else
        {
            throw new DalDoesNotExistException("The requested task was not found");
        }
        Console.WriteLine("Please enter updated details for the task (To skip press enter):");

        Console.Write("Description: ");
        string description = Console.ReadLine() ?? "";
        description = description == "" ? taskToUpdate.Description : description;

        Console.Write("Alias: ");
        string alias = Console.ReadLine() ?? "";
        alias = alias == "" ? taskToUpdate.Alias : alias;

        Console.Write("Milestone (true/false): ");
        input = Console.ReadLine() ?? "";
        bool milestone = input == "" ? taskToUpdate.IsMilestone : Convert.ToBoolean(input);

        Console.Write("Assign To Engineer (enter engineer's id): ");
        input = Console.ReadLine() ?? "";
        int? assignTo = input == "" ? taskToUpdate.EngineerId : Convert.ToInt32(input);

        Console.Write("Complexity (Novice/AdvancedBeginner/Competent/Proficient/Expert): ");
        input = Console.ReadLine() ?? "";
        EngineerExperience? complexity = input == "" ? taskToUpdate.Copmlexity : Enum.Parse<EngineerExperience>(input!);

        Console.Write("Time required to the task");
        input = Console.ReadLine() ?? "";
        TimeSpan? timeRequired = input == "" ? taskToUpdate.RequiredEffortTime : TimeSpan.Parse(input);


        Console.Write("Deadline (yyyy-mm-dd hh:mm:ss): ");
        input = Console.ReadLine() ?? "";
        DateTime? deadLine = input == "" ? taskToUpdate.DeadlineDate : Convert.ToDateTime(input);

        Console.Write("Scheduled Date (yyyy-mm-dd hh:mm:ss): ");
        input = Console.ReadLine() ?? "";
        DateTime? scheduledDate = input == "" ? taskToUpdate.ScheduledDate : Convert.ToDateTime(input);

        Console.Write("Start Date (yyyy-mm-dd hh:mm:ss): ");
        input = Console.ReadLine() ?? "";
        DateTime? startDate = input == "" ? taskToUpdate.StartDate : Convert.ToDateTime(input);

        Console.Write("Complete Date (yyyy-mm-dd hh:mm:ss): ");
        input = Console.ReadLine() ?? "";
        DateTime? completeDate = input == "" ? taskToUpdate.CompleteDate : Convert.ToDateTime(input);

      
        Console.Write("Remarks: ");
        string? remarks = Console.ReadLine() ?? "";
        remarks = remarks == "" ? taskToUpdate.Remarks : remarks;

        Console.Write("Deliverables: ");
        string? deliverables = Console.ReadLine() ?? "";
        deliverables = deliverables == "" ? taskToUpdate.Deliverables : deliverables;


        DO.Task task = new DO.Task(taskToUpdate.Id,alias,description, taskToUpdate.CreatedAtDate,milestone,timeRequired,complexity,startDate,scheduledDate,deadLine,completeDate,deliverables,remarks, taskToUpdate.EngineerId);
        taskImp.Update(task);
    }
    private void removeTask(ITask taskImp)// remove a task from the active tasks
    {
        Console.WriteLine("Enter the task ID");
        int taskId = 0;
        int.TryParse(Console.ReadLine(), out taskId);
        taskImp.Delete(taskId);
    }

    #endregion

    #region Dependencies Functions
  private void dependencyMenu()//The dependency menu, 0- exit, 1- add a dependency between 2 tasks, 2- show a specific dependency's details, 3- show all the dependencies details, 4 - update a specific dependency.
  {

      int choice = -1;
      IDependency dependencyImp = s_dal.Dependency;
      while (choice != 0)
      {

          Console.WriteLine("Select the action you want to perform:\r\n0 - Exit\r\n1- Adding dependencies\r\n2- Presentation of dependency\r\n3- Showing all dependencies\r\n4- Dependency update");
          int.TryParse(Console.ReadLine(), out choice);
          switch (choice)
          {
              case 0:
                  break;
              case 1:
                  dependencyCreate(dependencyImp);
                  break;
              case 2:
                  viewDependency(dependencyImp);
                  break;
              case 3:
                  viewAllDependency(dependencyImp);
                  break;
              case 4:
                  updateDependency(dependencyImp);
                  break;
              default:
                  throw new DalOutOfRangeChoiceException("Choose integer number between 0-4");
          }
      }
  }

  private void dependencyCreate(IDependency dependencyImp)//create new dependency by receiving data from the user
  {
      Console.WriteLine("Enter the pending task id:");
      string input = Console.ReadLine() ?? "";
      if (input == "") throw new DalDataWasNotReceived("pending task id is not accepted");
      int.TryParse(input, out int pandingTaskId);
      Console.WriteLine("Enter the previous task id:");
      input = Console.ReadLine() ?? "";
      if (input == "") throw new DalDataWasNotReceived("previous task id is not accepted");
      int.TryParse(input, out int previousTaskId);
      Dependency newDependency = new Dependency(0, pandingTaskId, previousTaskId);
      int dependencyId = dependencyImp.Create(newDependency);
      Console.WriteLine($"{dependencyId} Dependency added successfully!");
  }
  private void viewDependency(IDependency dependencyImp)//show a specific dependency's details
  {
      Console.WriteLine("Enter the Dependency ID");
      string input = Console.ReadLine() ?? "";
      if (input == "") throw new DalDataWasNotReceived("Must enter the id of the dependency you want to see");
      int.TryParse(input, out int dependencyId);
      DO.Dependency? checkingDependencyId = dependencyImp.Read(dependencyId);
      DO.Dependency showDependency = checkingDependencyId == null ? throw new DalDoesNotExistException($"The Dependency {dependencyId} is not found") : checkingDependencyId;
      Console.WriteLine(showDependency);
  }
  private void viewAllDependency(IDependency dependencyImp)//show all the dependencies details
  {
      IEnumerable<DO.Dependency> dependencies = dependencyImp.ReadAll()!;
      foreach (DO.Dependency dependency in dependencies)
      {
          Console.WriteLine(dependency);
      }
  }
  private void updateDependency(IDependency dependencyImp)//update the dependency by receiving data from the user To skip (if they dont want to update any field) they press enter
  {
      Console.WriteLine("Please enter the id of the dependency you would like to update(To skip press enter)");
      string input = Console.ReadLine() ?? "";
      if (input == "") throw new DalDataWasNotReceived("Must enter the id of the dependency you want to update");
      int.TryParse(input, out int dependencyId);
      Dependency? dependencyToUpdate = dependencyImp.Read(dependencyId);
      if (dependencyToUpdate != null)
      {
          Console.WriteLine(dependencyToUpdate);
      }
      else
      {
          throw new DalDoesNotExistException("The requested dependency was not found");
      }
      Console.WriteLine("Please enter the update details for the dependency:(To skip press enter)");
      Console.WriteLine("Enter the update pending task id:");
      input = Console.ReadLine() ?? "";
      int pandingTaskId = dependencyToUpdate.DependentTask;
      if (input != "")
      {
          int.TryParse(input, out pandingTaskId);
      }

      Console.WriteLine("Enter the previous task id:");
      input = Console.ReadLine() ?? "";
      int previousTaskId = dependencyToUpdate.DependsOnTask;
      if (input != "")
      {
          int.TryParse(input, out previousTaskId);
      }

      Dependency newDependency = new Dependency(dependencyToUpdate.Id, pandingTaskId, previousTaskId);
      dependencyImp.Update(newDependency);
  }
  #endregion

    #region Engineers Functions
  private void engineerMenu()//The Engineer menu,  0- exit, 1- add a Enginner, 2- show a specific Enginner's details, 3- show all the Engineers details, 4 - update a specific Engineer details, 5- remove an Engineer from the list.
  {
      int choice = -1;
      IEngineer EngineerImp = s_dal.Engineer;
      while (choice != 0)
      {

          Console.WriteLine("Select the action you want to perform:\r\n0 - Exit\r\n1- Adding an engineer\r\n2- Displaying the engineer's details\r\n3- Displaying the details of all engineers\r\n4- Updating engineer details\r\n5- Deleting an engineer");
          int.TryParse(Console.ReadLine(), out choice);
          switch (choice)
          {
              case 0:
                  break;
              case 1:
                  addEngineer(EngineerImp);
                  break;
              case 2:
                  viewEngineerDetails(EngineerImp);
                  break;
              case 3:
                  viewAllEngineersDetails(EngineerImp);
                  break;
              case 4:
                  updateEngineerDetails(EngineerImp);
                  break;
              case 5:
                  deleteEngineer(EngineerImp);
                  break;
              default:
                  throw new DalOutOfRangeChoiceException("Choose integer number between 0-5");
          }
      }

  }

  private void addEngineer(IEngineer engineerImp)//add a Enginner by receiving data from the user
  {
      Console.WriteLine("Please enter the details for the engineer:");
      Console.WriteLine("Id: ");
      string input = Console.ReadLine() ?? "";
      if (input == "") throw new DalDataWasNotReceived("Engineer Id is not accepted");
      int.TryParse(input, out int id);
      Console.WriteLine("Name");
      string name = Console.ReadLine() ?? "";
      if (name == "") throw new DalDataWasNotReceived("Name is not accept");


      Console.WriteLine("email: ");
      string email = Console.ReadLine() ?? "";
      if (email == "") throw new DalDataWasNotReceived("email is not accept");

      Console.WriteLine("Engineer level (Novice/AdvancedBeginner/Competent/Proficient/Expert): ");
      input = Console.ReadLine() ?? "";
      if (input == "") throw new DO.DalDataWasNotReceived("engineer Level is not accept");
      EngineerExperience engineerLevel = Enum.Parse<EngineerExperience>(input);

      Console.WriteLine("Engineer salary per hour: ");
      input = Console.ReadLine() ?? "";
      if (input == "") throw new DalDataWasNotReceived("not accept salary Per Hour");
      double salaryPerHour = double.Parse(input);

      Engineer newEngineer = new Engineer(id,email, salaryPerHour,name ,engineerLevel);
      id = engineerImp.Create(newEngineer);
      Console.WriteLine($"Engineer Id number = {id}, added successfully!");
  }
  private void viewEngineerDetails(IEngineer engineerImp)//show a specific Enginner's details
  {
      Console.WriteLine("Enter the Engineer ID");
      string input = Console.ReadLine() ?? "";
      if (input == "") throw new DalDataWasNotReceived("Must enter the id of the Engineer to see his details:");
      int.TryParse(input, out int engineerId);
      DO.Engineer? checkingEngineerId = engineerImp.Read(engineerId);
      DO.Engineer showEngineer = checkingEngineerId == null ? throw new DalDoesNotExistException($"The Engineer with ID {engineerId} is not found") : checkingEngineerId;
      Console.WriteLine(showEngineer);
  }
  private void viewAllEngineersDetails(IEngineer engineerImp)//show all the Engineers details
  {
      IEnumerable<DO.Engineer> engineers = engineerImp.ReadAll()!;
      foreach (DO.Engineer engineer in engineers)
      {
          Console.WriteLine(engineer);
      }
  }
  private void updateEngineerDetails(IEngineer engineerImp)//update a specific Engineer details by receiving data from the user To skip (if they dont want to update any field) they press enter
  {
      Console.WriteLine("Please enter the id of the engineer you would like to update");
      string input = Console.ReadLine() ?? "";
      if (input == "") throw new DalDataWasNotReceived("Must enter the id of the engineer you want to update");
      int.TryParse(input, out int engineerId);
      Engineer? engineerToUpdate = engineerImp.Read(engineerId);
      if (engineerToUpdate != null)
      {
          Console.WriteLine(engineerToUpdate);
      }
      else
      {
          throw new DalDoesNotExistException("The requested engineer was not found");
      }
      Console.WriteLine("Please enter the update details for the engineer:(To skip press enter)");

      Console.WriteLine("Name");
      string name = Console.ReadLine() ?? "";
      if (name == "") name = engineerToUpdate.Name;

      Console.WriteLine("email: ");
      string email = Console.ReadLine() ?? "";
      if (email == "") email = engineerToUpdate.Email;

      Console.WriteLine("Engineer level (Novice/AdvancedBeginner/Competent/Proficient/Expert): ");
      EngineerExperience engineerLevel = engineerToUpdate.Level;
      input = Console.ReadLine() ?? "";
      if (input != "")
      {
          engineerLevel = Enum.Parse<EngineerExperience>(input);
      }


      Console.WriteLine("Engineer salary per hour: ");

      double salaryPerHour = engineerToUpdate.Cost;
      input = Console.ReadLine() ?? "";
      if (input != "")
      {
          salaryPerHour = double.Parse(input);
      }


      Engineer newEngineer = new Engineer(engineerToUpdate.Id,email, salaryPerHour,name, engineerLevel);
      engineerImp.Update(newEngineer);
  }
  private void deleteEngineer(IEngineer engineerImp)//remove an Engineer from the list
  {
      Console.WriteLine("Enter the engineer ID");
      int engineerId = 0;
      int.TryParse(Console.ReadLine(), out engineerId);
      engineerImp.Delete(engineerId);
  }
  #endregion
}

