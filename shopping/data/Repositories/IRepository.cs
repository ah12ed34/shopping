namespace shopping.data.Repositories
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        List<T> GetList(int page, int pageSize);
        bool Update(T entity);
        IQueryable<T> Get(int page, int pageSize);
        bool Delete(T entity);
    }
}
