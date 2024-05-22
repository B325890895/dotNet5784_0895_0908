using BO;

namespace BlApi
{
    public interface IEngineer
    {
        public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null);
        public BO.Engineer? Read(int id);
        public void Create(int id, string name, string email, EngineerExperience level, double cost, TaskInEngineer? task);
        public void Delete(int id);
        public void Update(BO.Engineer engineer);       
    }
}
