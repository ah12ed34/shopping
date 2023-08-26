using shopping.Models;

namespace shopping.data.Repositories
{
    public class Repository<T>: IRepository<T>  where T : class
    {
        private readonly shoppingContext _context ;
        public Repository(shoppingContext  context)
        {
            _context = context;
        }
        public bool Delete(T t)
        {
            try
            {
                _context.Set<T>().Remove(t);
                _context.SaveChanges();
                return true;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message); 
            }
           
        }

        public T Get(int id)
        {
           return  _context.Set<T>().Find(id);
        }

        public List<T> GetList(int page, int pageSize)
        {
          return _context.Set<T>().Skip((page-1)*pageSize).Take(pageSize).ToList();
        }
        public IQueryable<T> Get(int page, int pageSize)
        {
          return _context.Set<T>().Skip((page-1)*pageSize).Take(pageSize);
        }

        public bool Update(T entity)
        {
            try {
                _context.Set<T>().Update(entity); 
                _context.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
               
            }
        }

    }
}
