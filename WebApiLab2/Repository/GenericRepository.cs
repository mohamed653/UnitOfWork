using Microsoft.EntityFrameworkCore;
using WebApiLab2.Data;
using WebApiLab2.IRepository;

namespace WebApiLab2.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ITIContext _context;
        public GenericRepository(ITIContext context)
        {
            _context = context;
        }
        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);    
        }


        public void Delete(int id)
        {
            var entity = _context.Set<TEntity>().Find(id);
            _context.Set<TEntity>().Remove(entity);
        }

        public IEnumerable<TEntity> GetAll()
        {
            var entities = _context.Set<TEntity>().ToList();
            return entities;
        }
        public DbSet<TEntity> GetDbSet()
        {
            return _context.Set<TEntity>();
        }


        public TEntity GetById(int id)
        {
            if (id==null)
            {
                return null;
            }
            var entity = _context.Set<TEntity>().Find(id);
            return entity;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
           _context.Update(entity);
        }
    }
}
