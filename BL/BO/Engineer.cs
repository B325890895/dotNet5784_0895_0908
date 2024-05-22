namespace BO;

public class Engineer
{
    public int Id { get; init; }
    public string Name { get; set; }
    public string Email { get; set; }
    public EngineerExperience Level { get; set; }
    public double Cost { get; set; }
    public TaskInEngineer? Task { get; set; }

    public Engineer(int id,string name,string email,EngineerExperience level,double cost, TaskInEngineer? task)
    {
        Id = id;
        Name = name;
        Email = email;
        Level = level;
        Cost = cost;
        Task = task;
    }

    public override string ToString()
    {
        return($"Id={Id}\n name={Name}\n email={Email}\n engineer level={Level}\n salary per hour={Cost}\n Task={(Task!=null?Task:"no task assign")}");
    }

}
     


 



