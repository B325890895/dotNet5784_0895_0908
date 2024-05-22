namespace BO;

public class MilestoneInTask
{
    public int MilestoneId { get; set; }
    public string MilestoneAlias { get; set; }
    
    public MilestoneInTask(int MilestoneId, string MilestoneAlias) {  
        this.MilestoneId = MilestoneId; 
        this.MilestoneAlias = MilestoneAlias;
    }
    
    public override string ToString()
    {
        return ($"   Id:{MilestoneId} Alias:{MilestoneAlias}");
    }

}
