using wabApi.Interfaces;
using wabApi.Models;

namespace wabApi.Repositories
{
    public class StudentRepository:GenericRepository<Student>,IStudentRepository
    {

        public StudentRepository(ItiDbContext context):base(context) 
        {

        }

        public Student getByName(string name)
        {
            return context.Students.FirstOrDefault(s => s.Name == name);
        }
    }
}
