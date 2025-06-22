using wabApi.Interfaces;
using wabApi.Models;

namespace wabApi.Repositories
{
    public class DepartmentRepository:GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ItiDbContext context):base(context) 
        {

        }
        public Department getByName(string name)
        {
            return context.Departments.FirstOrDefault(d => d.Name == name);
        }
    }
}
