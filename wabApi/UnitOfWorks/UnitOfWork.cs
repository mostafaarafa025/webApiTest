using wabApi.Interfaces;
using wabApi.Models;
using wabApi.Repositories;

namespace wabApi.UnitOfWorks
{
    public class UnitOfWork
    {
        IStudentRepository studentRepo;
        IDepartmentRepository departmentRepo;
        IAccountRpository accountRpository;
        ItiDbContext context;
        public UnitOfWork(ItiDbContext _context)
        {
            this.context = _context;
        }   
        public IStudentRepository StudentRepo 
        { get
            {
                if(studentRepo == null) 
                    studentRepo=new StudentRepository(context);
                return studentRepo;
            }
        }

        public IDepartmentRepository DepartmentRepo
        { get
            {
                if(departmentRepo == null)
                    departmentRepo=new DepartmentRepository(context);
                return departmentRepo;
            } 
        }

        public IAccountRpository AccountRpository
        {
            get
            {
                if(accountRpository == null)
                    accountRpository=new AccountRepository(context);
                return accountRpository;
            }
        }
    }
}
