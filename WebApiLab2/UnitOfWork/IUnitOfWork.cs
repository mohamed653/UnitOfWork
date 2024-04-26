using WebApiLab2.IRepository;
using WebApiLab2.Models;

namespace WebApiLab2.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IGenericRepository<Department> DepartmentRepo { get; }
        public IGenericRepository<Student> StudentRepo { get; }

        public void Save();
    }
}
