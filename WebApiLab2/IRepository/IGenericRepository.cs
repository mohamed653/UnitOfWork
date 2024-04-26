using Microsoft.EntityFrameworkCore;

namespace WebApiLab2.IRepository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        public IEnumerable<TEntity> GetAll();
        public DbSet<TEntity> GetDbSet();
        public TEntity GetById(int id);

        public void Add(TEntity entity);
        public void Update(TEntity entity);
        public void Delete(int id);
        public void Save();
    }
}
