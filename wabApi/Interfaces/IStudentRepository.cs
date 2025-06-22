using wabApi.Models;

namespace wabApi.Interfaces
{
    public interface IStudentRepository:IGenericRepository<Student>
    {
        Student getByName(string name);
    }
}
