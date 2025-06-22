using Microsoft.EntityFrameworkCore;
using wabApi.Interfaces;
using wabApi.Models;

namespace wabApi.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
       protected ItiDbContext context;
        DbSet<T> dbSet;
        public GenericRepository(ItiDbContext _context)
        {
            this.context = _context;
            dbSet = context.Set<T>();
        }

        public T addItem(T entity)
        {
           dbSet.Add(entity);
            context.SaveChanges();
            return entity;
        }

        public void deleteEntity(T entity)
        {
            dbSet.Remove(entity);
            context.SaveChanges();
        }

        public List<T> GetAll()
        {
           return dbSet.ToList();
        }

        public T getById(int id)
        {
            return dbSet.Find(id);
        }

        public void test()
        {
            throw new NotImplementedException();
        }

        public T updateItem(T entity)
        {
            dbSet.Update(entity);
            context.SaveChanges();
            return entity;
        }
    }
}
