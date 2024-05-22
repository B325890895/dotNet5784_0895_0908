using BO;
using System.ComponentModel;

namespace BlApi
{
    public interface IMilestone
    {
        public void creatingTheProjectSchedule();
        public Milestone Read(int id);
        public Milestone Update(int id,string alias,string description, string? comments);
       
    }
}
