namespace DalTest;
using DalApi;
using DO;

public static class Initialization
{
    private static IDal? s_dal; //stage 2,.

    private static readonly Random s_rand = new();

    private static void createEngineers(IEngineer s_dalEngineer)///Create a new List of Engineers and initialize it(add 5 Engineers)
    {
        string[] Names = { "Smith", "Johnson", "Williams", "Brown", "Jones" };

        EngineerExperience[] level = { EngineerExperience.Beginner,EngineerExperience.AdvancedBeginner,EngineerExperience.Advanced,EngineerExperience.Intermediate,EngineerExperience.Expert };
        for (int i = 0; i < 5; i++) { 
            Engineer newEngineer = new (100000000+i, $"{Names[i]}@gmail.com", (i+1)*100, Names[i],level[i]);
            s_dal!.Engineer.Create(newEngineer); //stage 2

        }
    }
    private static void createTasks(ITask s_dalTask, IEngineer s_dalEngineer)///Create a new List of Tasks and initialize it(add 20 Tasks)
    {
        EngineerExperience[] copmlexity = { EngineerExperience.Beginner, EngineerExperience.AdvancedBeginner, EngineerExperience.Advanced, EngineerExperience.Intermediate, EngineerExperience.Expert };
        IEnumerable<Engineer?> eng = s_dal!.Engineer.ReadAll(); //stage 2
        List<Engineer> engList = eng.ToList()!;
        for (int i = 0; i < 20; i++)
        {
            Task newTask = new(default, $"{i}Alias",$"{i}description",DateTime.Now, false,new TimeSpan(10,0,0,0,0,0),copmlexity[i % 5], null, null, null, null, null, null,engList[i % 5]!.Id);
            s_dal!.Task.Create(newTask); //stage 2
        }
    }

    private static void createDependencies(IDependency s_dalDependency, ITask s_dalTask)//Create a new List of Dependencies and initialize it(add 40 Dependencies)
    {
        IEnumerable<Task?> tasks = s_dal!.Task.ReadAll(); //stage 2
        List<Task> listTask = tasks.ToList()!;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 6; j < 20; j++)
            {
                for (int k = 0; k < 5; k++)
                {
                    Dependency newDep = new(0, listTask.ElementAt(j).Id, listTask.ElementAt(k+j).Id);
                    s_dal!.Dependency.Create(newDep);  //stage 2
                }
            }
        }
    }

    public static void Do() //stage 4
    {
        s_dal = DalApi.Factory.Get; //stage 4
        createEngineers(s_dal.Engineer);
        createTasks(s_dal.Task, s_dal.Engineer);
        createDependencies(s_dal.Dependency, s_dal.Task);
    }

    public static void Reset()
    {
        s_dal = DalApi.Factory.Get; //stage 4
        s_dal.Reset();
    }
}
