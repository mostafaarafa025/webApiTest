namespace wabApi.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        List<T> GetAll();
        T getById(int id);
        T addItem(T entity);
        T updateItem(T entity);
        void deleteEntity(T entity);
        void test();
    }
}
