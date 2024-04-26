using System;
using WebApiLab2.Data;
using WebApiLab2.IRepository;
using WebApiLab2.Models;
using WebApiLab2.Repository;

namespace WebApiLab2.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ITIContext _context;
        private IGenericRepository<Department> _department;
        private IGenericRepository<Student> _student;
        public UnitOfWork(ITIContext context)
        {
            _context = context;
        }
        public IGenericRepository<Department> DepartmentRepo 
        {
            get
            {
                if (_department == null)
                {
                    _department = new GenericRepository<Department>(_context);
                }
                return _department;
            }
        }

        public IGenericRepository<Student> StudentRepo
        {
            get
            {
                if (_student == null)
                {
                    _student = new GenericRepository<Student>(_context);
                }
                return _student;
            }
        }
        
        void IUnitOfWork.Save()
        {
            _context.SaveChanges();
        }
    }
}
