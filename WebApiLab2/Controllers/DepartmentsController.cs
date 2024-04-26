using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApiLab2.Data;
using WebApiLab2.DTOs;
using WebApiLab2.IRepository;
using WebApiLab2.Models;
using WebApiLab2.UnitOfWork;

namespace WebApiLab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IUnitOfWork _UnitOfWork;

        public DepartmentsController(IUnitOfWork UnitOfWork )
        {
            _UnitOfWork = UnitOfWork;
        }

        // GET: api/Departments
        /// <summary>
        /// Get all Departments
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Summary = "Get all Departments")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        public ActionResult<IEnumerable<Department>> GetDepartments()
        {
            //if(_context.Departments.Count() == 0)
            //{
            //    return NotFound();
            //}   

            var Departments = _UnitOfWork.DepartmentRepo.GetAll();
            var DepartmentDtos = Departments.Select(d => new
            {
                d.DeptId,
                d.DeptName,
                d.DeptDesc,
                d.DeptLocation,
                NumOfStudents = CalculateNumOfStudentsInDept(d.DeptId).Result
            });
            return Ok(DepartmentDtos);
        }
        
        private async Task<int> CalculateNumOfStudentsInDept(int deptId)
        {
            return  _UnitOfWork.StudentRepo.GetAll().Where(s => s.DeptId == deptId).Count();
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public ActionResult<Department> GetDepartment(int id)
        {
            var department = _UnitOfWork.DepartmentRepo.GetById(id);

            if (department == null)
            {
                return NotFound();
            }
            var DepartmentDto = new DepartmentDTO
            {
                DeptId = department.DeptId,
                DeptName = department.DeptName,
                DepartmentDescription = department.DeptDesc,
                DepartmentLocation = department.DeptLocation,
                NumberOfStudents = CalculateNumOfStudentsInDept(id).Result
            };
            return Ok(DepartmentDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, Department department)
        {
            if (id != department.DeptId)
            {
                return BadRequest();
            }

            _UnitOfWork.DepartmentRepo.Update(department);

            try
            {
                _UnitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartment(Department department)
        {
            _UnitOfWork.DepartmentRepo.Add(department);
            try
            {
               _UnitOfWork.Save();
            }
            catch (DbUpdateException)
            {
                if (DepartmentExists(department.DeptId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDepartment", new { id = department.DeptId }, department);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = _UnitOfWork.DepartmentRepo.GetById(id);
            if (department == null)
            {
                return NotFound();
            }

          _UnitOfWork.DepartmentRepo.Delete(id);
            _UnitOfWork.Save();

            return NoContent();
        }

        private bool DepartmentExists(int id)
        {
            return _UnitOfWork.DepartmentRepo.GetAll().Any(e => e.DeptId == id);
        }
    }
}
