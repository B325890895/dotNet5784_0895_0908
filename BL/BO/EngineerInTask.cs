namespace BO;

public class EngineerInTask
{
    public int EngineerId {  get; set; }
    public string EngineerName { get; set;}
    public EngineerInTask(int engineerId, string engineerName)
    {
        EngineerId = engineerId;
        EngineerName = engineerName;
    }
    public override string ToString()
    {
        return ($"   Id:{EngineerId} Name:{EngineerName}");
    }

}
