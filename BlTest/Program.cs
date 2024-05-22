using BO;

namespace BlTest;

public class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    static void Main(string[] args)
    {
        try
        {
            Program program = new();
            Console.Write("Would you like to create Initial data? (Y/N)");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            if (ans == "Y")
                DalTest.Initialization.Do();
            program.mainMenu();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
            if (exception.InnerException != null)
                Console.WriteLine("inner:\n" + exception.InnerException.Message);
            else
            {
                Console.WriteLine("not inner");
            }
        }
    }
    #region Main Menu
    private void mainMenu()// show the user the main menu of the program
    {
        try
        {
            int choice = -1;
            while (choice != 0)//every diffrent choich will call another Sub menu
            {
                try
                {
                    Console.WriteLine("Select an entity you want to check:\r\n0 - Exit\r\n1- Task\r\n2- Milestone\r\n3- Engineer");
                    int.TryParse(Console.ReadLine(), out choice);
                    switch (choice)
                    {
                        case 1:
                            taskMenu();
                            break;
                        case 2:
                            milestoneMenu();
                            break;
                        case 3:
                            engineerMenu();
                            break;
                        case 0:
                            break;
                        default:
                            throw new BO.BlOutOfRangeChoiceException("Choose integer number between 0-3");
                    }
                }
                catch (BO.BlDeletionIsProhibitedException exception)
                {
                    Console.WriteLine(exception);
                }
                catch (BO.BlDoesNotExistException exception)
                {
                    Console.WriteLine(exception);
                }
                catch (BO.BlExistException exception)
                {
                    Console.WriteLine(exception);
                }
                catch (BO.BlOutOfRangeChoiceException exception)
                {
                    Console.WriteLine(exception);
                }
                catch (BO.BlDataWasNotReceived exception)
                {
                    Console.WriteLine(exception);
                }
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
            if (exception.InnerException != null)
                Console.WriteLine("inner:\n" + exception.InnerException.Message);
            else
            {
                Console.WriteLine("not inner");
            }
        }
    }
    #endregion
    #region Engineer Menu
    private void engineerMenu()//The Engineer menu,  0- exit, 1- add a Enginner, 2- show a specific Enginner's details, 3- show all the Engineers details, 4 - update a specific Engineer details, 5- remove an Engineer from the list.
    {
        int choice = -1;
        BlApi.IEngineer EngineerImp = s_bl.Engineer;
        while (choice != 0)
        {

            Console.WriteLine("Select the action you want to perform:\r\n0 - Exit\r\n1- Adding an engineer\r\n2- Displaying the engineer's details\r\n3- Displaying the details of all engineers\r\n4- Updating engineer details\r\n5- Deleting an engineer");
            int.TryParse(Console.ReadLine(), out choice);
            try
            {
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
                        throw new BO.BlOutOfRangeChoiceException("Choose integer number between 0-5");
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                if (exception.InnerException != null)
                    Console.WriteLine("inner:\n" + exception.InnerException.Message);
                else
                {
                    Console.WriteLine("not inner");
                }
            }
        }

    }

    private void addEngineer(BlApi.IEngineer engineerImp)//add a Enginner by receiving data from the user
    {
        Console.WriteLine("Please enter the details for the engineer:");
        Console.WriteLine("Id: ");
        int id;
        string input = Console.ReadLine() ?? "";
        if (input == "") throw new BO.BlDataWasNotReceived("Engineer Id is not accepted");
        else if (!(int.TryParse(input, out id)))
            throw new BO.BlDataWasNotReceived("The entered value is invalid");


        Console.WriteLine("Name:");
        string name = Console.ReadLine() ?? "";
        if (name == "") throw new BO.BlDataWasNotReceived("Engineer name is not accepted");


        Console.WriteLine("email: ");
        string email = Console.ReadLine() ?? "";
        if (email == "") throw new BO.BlDataWasNotReceived("email is not accept");

        Console.WriteLine("Engineer level (Beginner/AdvancedBeginner/Intermediate/Advanced/Expert): ");
        input = Console.ReadLine() ?? "";
        BO.EngineerExperience level;
        if (input == "") throw new BO.BlDataWasNotReceived("engineer Level is not accept");
        else if (!BO.EngineerExperience.TryParse(input!, out level))
            throw new BO.BlDataWasNotReceived("The entered value is invalid");


        Console.WriteLine("Engineer salary per hour: ");
        input = Console.ReadLine() ?? "";
        if (input == "") throw new BO.BlDataWasNotReceived("not accept salary Per Hour");
        double cost = double.Parse(input);

        Console.WriteLine("Do you want to assign a task to the engineer? (Y/N)");
        input = Console.ReadLine() ?? throw new BO.BlDataWasNotReceived("not accepted any choice");
        BO.TaskInEngineer? taskInEngineer = null;
        switch (input)
        {
            case "y":
            case "Y":
                Console.WriteLine("enter task Id:");
                input = Console.ReadLine() ?? "";
                int taskId = default;
                if (input == "") throw new BO.BlDataWasNotReceived("task id isn't accepted");
                else if (!(int.TryParse(input, out taskId)))
                    throw new BO.BlDataWasNotReceived("The entered value is invalid");
                taskInEngineer = new BO.TaskInEngineer(taskId, "");
                break;
            case "n":
            case "N":
                break;
            default:
                throw new BO.BlDataIsInvalidException("The entered value is invalid");
        }

        s_bl.Engineer.Create(id, name, email, level, cost, taskInEngineer);
        Console.WriteLine($"Engineer Id number = {id}, added successfully!");
    }
    private void viewEngineerDetails(BlApi.IEngineer engineerImp)//show a specific Enginner's details
    {
        Console.WriteLine("Enter the Engineer ID");
        string input = Console.ReadLine() ?? "";
        int engineerId;
        if (input == "") throw new BO.BlDataWasNotReceived("Must enter the id of the Engineer to see his details:");
        else if (!(int.TryParse(input, out engineerId)))
            throw new BO.BlDataIsInvalidException("The entered value is invalid");
        BO.Engineer engineer = engineerImp.Read(engineerId) ?? throw new BO.BlDoesNotExistException($"The Engineer with ID {engineerId} is not found");
        Console.WriteLine(engineer);
    }
    private void viewAllEngineersDetails(BlApi.IEngineer engineerImp)//show all the Engineers details
    {
        IEnumerable<BO.Engineer> engineers = engineerImp.ReadAll()!;
        foreach (BO.Engineer engineer in engineers)
        {
            Console.WriteLine(engineer);
        }
    }
    private void updateEngineerDetails(BlApi.IEngineer engineerImp)//update a specific Engineer details by receiving data from the user To skip (if they dont want to update any field) they press enter
    {
        Console.WriteLine("Please enter the id of the engineer you would like to update");
        string input = Console.ReadLine() ?? "";
        int engineerId;
        if (input == "") throw new BO.BlDataWasNotReceived("Must enter the id of the engineer you want to update");
        else if (!(int.TryParse(input, out engineerId)))
            throw new BO.BlDataIsInvalidException("The entered value is invalid");
        BO.Engineer engineerToUpdate = engineerImp.Read(engineerId) ?? throw new BO.BlDoesNotExistException("The requested engineer was not found");
        Console.WriteLine(engineerToUpdate);

        Console.WriteLine("Please enter the update details for the engineer:(To skip press enter)");

        Console.WriteLine("Name");
        string name = Console.ReadLine() ?? "";
        if (name == "") name = engineerToUpdate.Name;

        Console.WriteLine("email: ");
        string email = Console.ReadLine() ?? "";
        if (email == "") email = engineerToUpdate.Email;

        Console.WriteLine("Engineer level (Beginner/AdvancedBeginner/Intermediate/Advanced/Expert): ");
        BO.EngineerExperience level = engineerToUpdate.Level;
        input = Console.ReadLine() ?? "";
        if (input != "")
        {
            if (!(BO.EngineerExperience.TryParse(input, out level)))
                throw new BO.BlDataIsInvalidException("The entered value is invalid");
        }


        Console.WriteLine("Engineer salary per hour: ");

        double cost = engineerToUpdate.Cost;
        input = Console.ReadLine() ?? "";
        if (input != "")
        {
            if (!(double.TryParse(input, out cost)))
                throw new BO.BlDataIsInvalidException("The entered value is invalid");
        }
        Console.WriteLine("Do you want to update the task assign? (Y/N)");
        input = Console.ReadLine() ?? throw new BO.BlDataWasNotReceived("not accepted any choice");
        BO.TaskInEngineer? taskInEngineer = engineerToUpdate.Task;
        int id = default;
        switch (input)
        {
            case "y":
            case "Y":
                Console.WriteLine("enter task Id:");
                if (input == "") throw new BO.BlDataWasNotReceived("task id isn't accepted");
                else if (!(int.TryParse(input, out id)))
                    throw new BO.BlDataWasNotReceived("The entered value is invalid");
                BO.Task task = s_bl.Task.Read(id) ?? throw new BO.BlDoesNotExistException($"Faild to load task {id}");
                taskInEngineer = new BO.TaskInEngineer(task.Id, task.Alias);
                break;
            case "n":
            case "N":
                break;
            default:
                throw new BO.BlDataIsInvalidException("The entered value is invalid");
        }
        BO.Engineer newEngineer = new BO.Engineer(engineerToUpdate.Id, name, email, level, cost, taskInEngineer);
        engineerImp.Update(newEngineer);
    }
    private void deleteEngineer(BlApi.IEngineer engineerImp)//remove an Engineer from the list
    {
        Console.WriteLine("Enter the engineer ID");
        int engineerId = 0;
        if (!(int.TryParse(Console.ReadLine(), out engineerId)))
            throw new BO.BlDataIsInvalidException("The entered value is invalid");
        engineerImp.Delete(engineerId);
    }


    #endregion
    #region Milestone Menu
    private void milestoneMenu()
    {
        int choice = -1;
        while (choice != 0)
        {
            BlApi.IMilestone taskImp = s_bl.Milestone;
            Console.WriteLine("Select the action you want to perform:\r\n0 - Exit\r\n1- reading an Milestone\r\n2- update an Milestone\r\n 3- Create the project Schedule");
            int.TryParse(Console.ReadLine(), out choice);
            try
            {
                switch (choice)
                {
                    case 0:
                        break;
                    case 1:
                        readMilestone(taskImp);
                        break;
                    case 2:
                        updateMilestone(taskImp);
                        break;
                    case 3:
                        creatingTheProjectSchedule();
                        break;

                    default:
                        throw new BO.BlOutOfRangeChoiceException("Choose integer number between 0-6");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                if (exception.InnerException != null)
                    Console.WriteLine("inner:\n" + exception.InnerException.Message);
                else
                {
                    Console.WriteLine("not inner");
                }
            }
        }
    }
    private void readMilestone(BlApi.IMilestone milestoneImp)
    {
        Console.WriteLine("Enter the Milestone ID");
        string input = Console.ReadLine() ?? "";
        int milestoneId;
        if (input == "") throw new BO.BlDataWasNotReceived("Must enter the id of the Milestone you want to see");
        if (!(int.TryParse(input, out milestoneId)))
            throw new BO.BlDataWasNotReceived("The entered value is invalid");
        BO.Milestone? checkingMilestoneId = milestoneImp.Read(milestoneId);
        BO.Milestone showTask = checkingMilestoneId == null ? throw new BO.BlDoesNotExistException($"The Task {milestoneId} is not found") : checkingMilestoneId;
        Console.WriteLine(showTask);
    }
    private void updateMilestone(BlApi.IMilestone milestoneImp)
    {
        Console.WriteLine("Please enter the id of the milestone you would like to update(To skip press enter)");
        string input = Console.ReadLine() ?? "";
        if (input == "") throw new BO.BlDataWasNotReceived("Must enter the id of the milestone you want to update");
        if (!(int.TryParse(input, out int milestoneId)))
            throw new BO.BlDataWasNotReceived("The entered value is invalid");
        BO.Milestone? milestoneToUpdate = milestoneImp.Read(milestoneId);
        if (milestoneToUpdate != null)
        {
            Console.WriteLine(milestoneToUpdate);
        }
        else
        {
            throw new BO.BlDoesNotExistException("The requested milestone was not found");
        }
        Console.WriteLine("Please enter the update details for the milestone:(To skip press enter)");

        Console.WriteLine("Enter the update Alias");
        input = Console.ReadLine() ?? "";
        string alias = input == "" ? milestoneToUpdate.Alias : input;


        Console.WriteLine("Enter the update Description:");
        input = Console.ReadLine() ?? "";
        string description = input == "" ? milestoneToUpdate.Description : input;


        Console.WriteLine("Enter the update Comments:");
        input = Console.ReadLine() ?? "";
        string? comments = input == "" ? milestoneToUpdate.Remarks : input;

        milestoneImp.Update(milestoneToUpdate.Id, alias, description, comments);
    }
    private void creatingTheProjectSchedule()
    {
        s_bl.Milestone.creatingTheProjectSchedule();
    }

    #endregion
    #region Task Menu

    private void taskMenu()
    {
        int choice = -1;
        while (choice != 0)
        {
            BlApi.ITask taskImp = s_bl.Task;
            Console.WriteLine("Select the action you want to perform:\r\n0 - Exit\r\n1- Adding a task\r\n2- Viewing the task\r\n3- Viewing all tasks\r\n4- Task update\r\n5- Removing the task from the active tasks");
            int.TryParse(Console.ReadLine(), out choice);
            try
            {
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
                        throw new BO.BlOutOfRangeChoiceException("Choose integer number between 0-6");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                if (exception.InnerException != null)
                    Console.WriteLine("inner:\n" + exception.InnerException.Message);
                else
                {
                    Console.WriteLine("not inner");
                }
            }
        }

    }

    public void taskCreate(BlApi.ITask taskImp)
    {
        string? input;
        Console.WriteLine("Please enter the details for the Task:");//Receiving data from the user


        Console.Write("Alias: ");
        string alias = Console.ReadLine() ?? "";
        if (alias == "") throw new BO.BlDataWasNotReceived("alias is not accepted");

        Console.Write("Description: ");
        string description = Console.ReadLine() ?? "";
        if (description == "") throw new BO.BlDataWasNotReceived("description is not accepted");

        List<TaskInList>? dependencies = null;
        int taskInListId;
        TaskInList taskInList;
        BO.Task task;
        Console.WriteLine("Add Dependencies?(Y/N)");
        input = Console.ReadLine();
        switch (input)
        {
            case "Y":
            case "y":
                dependencies = new List<TaskInList>();
                while (input == "Y" || input == "y")
                {
                    Console.WriteLine("enter Dependency id");
                    input = Console.ReadLine() ?? throw new BO.BlDoesNotExistException("id is not accepted");
                    if (!int.TryParse(input, out taskInListId))
                        throw new BO.BlDataIsInvalidException("The entered value is invalid");
                    task = s_bl.Task.Read(taskInListId) ?? throw new BO.BlDoesNotExistException($"not exist task with id:{taskInListId}");
                    taskInList = new TaskInList(taskInListId, task.Description, task.Alias, task.Status);
                    dependencies.Add(taskInList);
                    Console.WriteLine("Add Dependencies?(Y/N)");
                    input = Console.ReadLine();
                }
                break;
            case "N":
            case "n":
            case "":
                break;
            default:
                throw new BO.BlDataWasNotReceived("The entered value is invalid");
        }



        Console.Write("Required Effort Time: ");
        TimeSpan requiredEffortTime = default;
        input = Console.ReadLine() ?? "";
        if (input == "") throw new BO.BlDataWasNotReceived("Required Effort Time is not accepted");
        if (!(TimeSpan.TryParse(input, out requiredEffortTime)))
            throw new BO.BlDataIsInvalidException("The entered value is invalid");


        Console.Write("Complexity (Beginner/AdvancedBeginner/Intermediate/Advanced/Expert):  ");
        BO.EngineerExperience complexity;
        input = Console.ReadLine() ?? "";
        if (input == "") throw new BO.BlDataWasNotReceived("Complexity is not accepted");
        if (!(Enum.TryParse(input, out complexity)))
            throw new BO.BlDataIsInvalidException("The entered value is invalid");

        Console.Write("DeadLine (yyyy-mm-dd hh:mm:ss): ");
        string? doThereDeadLine = Console.ReadLine();
        DateTime deadLine = default;
        if (!string.IsNullOrWhiteSpace(doThereDeadLine))
        {
            if (!(DateTime.TryParse(doThereDeadLine, out deadLine)))
            {
                throw new BO.BlDataIsInvalidException("The entered value is invalid");

            }

        }


        Console.Write("Scheduled Date (optional - leave blank if not applicable) (yyyy-mm-dd hh:mm:ss): ");
        string? doThereScheduledDate = Console.ReadLine();
        DateTime scheduledDate = default;
        if (!string.IsNullOrWhiteSpace(doThereScheduledDate))
        {
            if (!(DateTime.TryParse(doThereScheduledDate, out scheduledDate)))
                throw new BO.BlDataIsInvalidException("The entered value is invalid");
        }



        Console.Write("Remarks (optional - leave blank if not applicable): ");
        string? remarks = Console.ReadLine();

        Console.Write("Deliverables: ");
        string? deliverables = Console.ReadLine() ?? "";

        taskImp.Create(default, alias, description, dependencies, scheduledDate, requiredEffortTime, deadLine, deliverables, remarks, complexity);

    }
    private void viewTask(BlApi.ITask taskImp)///show a specific task's details
    {
        Console.WriteLine("Enter the task ID");
        string input = Console.ReadLine() ?? "";
        if (input == "") throw new BO.BlDoesNotExistException("Must enter the id of the task you want to see");
        if (!(int.TryParse(input, out int taskId)))
            throw new BO.BlDataWasNotReceived("The entered value is invalid");
        BO.Task? checkingTaskId = taskImp.Read(taskId);
        BO.Task showTask = checkingTaskId == null ? throw new BO.BlDoesNotExistException($"The Task {taskId} is not found") : checkingTaskId;
        Console.WriteLine(showTask);
    }
    private void viewAllTasks(BlApi.ITask taskImp)///show all the tasks details
    {
        IEnumerable<BO.Task> tasks = taskImp.ReadAll()!;
        foreach (var task in tasks)
        {
            Console.WriteLine(task);
        }
    }
    private void updateTask(BlApi.ITask taskImp)///update the task by receiving data from the user To skip (if they dont want to update any field) they press enter
    {
        Console.WriteLine("Please enter the id of the task you would like to update");
        string input = Console.ReadLine() ?? "";
        if (input == "") throw new BO.BlDoesNotExistException("Must enter the id of the task you want to update");
        if (!(int.TryParse(input, out int taskId)))
            throw new BO.BlDataWasNotReceived("id is not accepted");
        BO.Task? taskToUpdate = taskImp.Read(taskId) ?? throw new BO.BlDoesNotExistException("The requested task was not found"); ;
        Console.WriteLine(taskToUpdate);
        Console.WriteLine("Please enter updated details for the task (To skip press enter):");

        Console.Write("Alias: ");
        string alias = Console.ReadLine() ?? "";
        alias = alias == "" ? taskToUpdate.Alias : alias;


        Console.Write("Description: ");
        string description = Console.ReadLine() ?? "";
        description = description == "" ? taskToUpdate.Description : description;


        Console.Write("Status (Unscheduled,Scheduled,OnTrack,InJeopardy,Done): ");
        input = Console.ReadLine() ?? "";
        BO.Status statusTask = default;
        BO.Status? status = taskToUpdate.Status;
        if (input != "")
        {
            if (!(BO.Status.TryParse(input, out statusTask)))
                throw new BO.BlDataWasNotReceived("The entered value is invalid");
            status = statusTask == default ? null : statusTask;
        }




        Console.Write("Required Effort Time (yyyy-mm-dd hh:mm:ss): ");
        input = Console.ReadLine() ?? "";
        TimeSpan requiredEffortTimeSpan = default;
        TimeSpan? requiredEffortTime = taskToUpdate.RequiredEffortTime;
        if (input != "")
        {
            if (!(TimeSpan.TryParse(input, out requiredEffortTimeSpan)))
                throw new BO.BlDataWasNotReceived("The entered value is invalid");
            requiredEffortTime = requiredEffortTimeSpan == default ? null : requiredEffortTimeSpan;
        }




        Console.Write("Start Date (yyyy-mm-dd hh:mm:ss): ");
        input = Console.ReadLine() ?? "";

        DateTime startDateNN = default;
        DateTime? startDate = taskToUpdate.StartDate;
        if (input != "")
        {
            if (!(DateTime.TryParse(input, out startDateNN)))
                throw new BO.BlDataWasNotReceived("The entered value is invalid");
            startDate = startDateNN == default ? null : startDateNN;
        }





        Console.Write("Scheduled Date (yyyy-mm-dd hh:mm:ss): ");
        input = Console.ReadLine() ?? "";
        DateTime scheduledDateNN = default;
        DateTime? scheduledDate = taskToUpdate.DeadlineDate;
        if (input != "")
        {
            if (!(DateTime.TryParse(input, out scheduledDateNN)))
                throw new BO.BlDataWasNotReceived("The entered value is invalid");
            scheduledDate = scheduledDateNN == default ? null : scheduledDateNN;
        }



        Console.Write("Forecast Date (yyyy-mm-dd hh:mm:ss): ");
        input = Console.ReadLine() ?? "";
        DateTime forecastDateNN = default;
        DateTime? forecastDate = taskToUpdate.ForecastDate;
        if (input != "")
        {
            if (!(DateTime.TryParse(input, out forecastDateNN)))
                throw new BO.BlDataWasNotReceived("The entered value is invalid");
            forecastDate = forecastDateNN == default ? null : forecastDateNN;
        }


        Console.Write("DeadLine (yyyy-mm-dd hh:mm:ss): ");
        input = Console.ReadLine() ?? "";
        DateTime deadLine = default;
        DateTime? DeadlineDate = taskToUpdate.DeadlineDate;
        if (input != "")
        {
            if (!(DateTime.TryParse(input, out deadLine)))
                throw new BO.BlDataWasNotReceived("The entered value is invalid");
            DeadlineDate = deadLine == default ? null : DeadlineDate;
        }


        Console.Write("Complete Date (yyyy-mm-dd hh:mm:ss): ");
        input = Console.ReadLine() ?? "";
        DateTime completeDateNN = default;
        DateTime? completeDate = taskToUpdate.CompleteDate;
        if (input != "")
        {
            if (!(DateTime.TryParse(input, out completeDateNN)))
                throw new BO.BlDataWasNotReceived("The entered value is invalid");
            completeDate = completeDateNN == default ? null : completeDateNN;
        }

        Console.Write("Deliverables: ");
        string? deliverables = Console.ReadLine() ?? "";
        deliverables = deliverables == "" ? taskToUpdate.Deliverables : deliverables;

        Console.Write("Remarks: ");
        string? remarks = Console.ReadLine() ?? "";
        remarks = remarks == "" ? taskToUpdate.Remarks : remarks;




        Console.Write("Difficulty Level (Novice/AdvancedBeginner/Competent/Proficient/Expert): ");
        input = Console.ReadLine() ?? "";
        BO.EngineerExperience difficultyLevel = default;
        BO.EngineerExperience? level = taskToUpdate.Copmlexity;
        if ((input != "") && (!(BO.EngineerExperience.TryParse(input, out difficultyLevel))))
            throw new BO.BlDataWasNotReceived("The entered value is invalid");
        level = difficultyLevel;


        BO.Task task = new BO.Task(taskToUpdate.Id, alias, description, taskToUpdate.CreatedAtDate, status, taskToUpdate.Dependencies, taskToUpdate.Milestone
            , requiredEffortTime, startDate, scheduledDate, forecastDate, DeadlineDate, completeDate, deliverables,
            remarks, taskToUpdate.Engineer, level);
        taskImp.Update(task);
        s_bl.Task.Update(task);
    }
    private void removeTask(BlApi.ITask taskImp)// remove a task from the active tasks
    {
        Console.WriteLine("Enter the task ID");
        int taskId;
        string input = Console.ReadLine() ?? "";
        if (input == "") throw new BO.BlDataWasNotReceived("Must enter the id of the task you want to see");
        if (!(int.TryParse(input, out taskId)))
            throw new BO.BlDataWasNotReceived("The entered value is invalid");
        taskImp.Delete(taskId);
    }

    #endregion
}

