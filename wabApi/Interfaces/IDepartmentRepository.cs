using wabApi.Models;

namespace wabApi.Interfaces
{
    public interface IDepartmentRepository:IGenericRepository<Department>
    {
        Department getByName(string name);
    }
}
