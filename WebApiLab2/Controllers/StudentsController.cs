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
using WebApiLab2.Models;
using WebApiLab2.UnitOfWork;

namespace WebApiLab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IUnitOfWork _UnitOfWork;

        public StudentsController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        // GET: api/Students
        /// <summary>
        /// Get all students
        /// </summary>
        /// <returns>List of Students</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Get all students")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            //if (_context.Students.Count() == 0)
            //{
            //    return NotFound();
            //}

             //var students = _UnitOfWork.StudentRepo.GetDbSet().Include(s => s.Dept).Include(s => s.StSuperNavigation).ToList();
             var students = _UnitOfWork.StudentRepo.GetAll();
             var studentDtos = students.Select(s => new StudentDto
             {
                 StId = s.StId,
                 StName = s.StFname,
                 StAddress = s.StAddress,
                 StAge = s.StAge ?? 0,
                 DeptName = _UnitOfWork.DepartmentRepo.GetById(s.DeptId.Value)?.DeptName ?? "",
                 SupervisorName = s.StSuperNavigation?.StFname??""
             });

            return Ok(studentDtos);
        }

        [HttpGet("{pageNumber}/{pageSize}")]
        public IActionResult GetStudentsPagination(int pageNumber = 1, int pageSize = 5)
        {
            var students = _UnitOfWork.StudentRepo.GetDbSet().Include(s => s.Dept).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var studentsDtos = students.Select(s => new StudentDto
            {
                StId = s.StId,
                StName = s.StFname,
                StAddress = s.StAddress,
                StAge = s.StAge ?? 0,
                DeptName = s.Dept?.DeptName ?? "",
                SupervisorName = s.StSuperNavigation?.StFname ?? ""
            });
            return Ok(studentsDtos);
        }

            
        [HttpGet("{name:alpha}")]
        public IActionResult GetStudentsSearch(string name)
        {
            var students = _UnitOfWork.StudentRepo.GetDbSet().Include(s => s.Dept).Where(s => s.StFname.Contains(name)).ToList();
            var studentsDtos = students.Select(s => new StudentDto
            {
                StId = s.StId,
                StName = s.StFname,
                StAddress = s.StAddress,
                StAge = s.StAge ?? 0,
                DeptName = s.Dept?.DeptName ?? "",
                SupervisorName = s.StSuperNavigation?.StFname ?? ""
            });
            return Ok(studentsDtos);
        }
        [HttpGet]
        [Route("GetAllSupervisors")]
        public IActionResult GetAllSupervisors()
        {
            var supervisors = _UnitOfWork.StudentRepo.GetDbSet().Where(s => s.StSuper != null).Distinct().ToList();
            var supervisorsDtos = supervisors.Select(s => new StudentDto
            {
                StId = s.StId,
                StName = s.StFname,
                StAddress = s.StAddress,
                StAge = s.StAge ?? 0,
            });
            return Ok(supervisorsDtos);
        }

        // GET: api/Students/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _UnitOfWork.StudentRepo.GetDbSet().Include(s=>s.Dept).Include(s=>s.StSuperNavigation).FirstOrDefaultAsync(s=>s.StId==id);

            if (student == null)
            {
                return NotFound();
            }

            StudentDto studentDto = new StudentDto
            {
                StId = student.StId,
                StName = student.StFname,
                StAddress = student.StAddress,
                StAge = student.StAge ?? 0,
                DeptName = student.Dept?.DeptName??"",
                SupervisorName = student.StSuperNavigation?.StFname??""
            };

            return Ok(studentDto);
        }

        [HttpPost]
        [Route("AddStudentDto")]
        public async Task<ActionResult<Student>> AddStudentDto(StudentDtoUI studentDto)
        {
            Student student = new Student
            {
                StId = _UnitOfWork.StudentRepo.GetDbSet().Max(s => s.StId) + 1,
                StFname = studentDto.stName,
                StAddress = studentDto.stAddress,
                StAge = studentDto.stAge,
                DeptId = int.Parse(studentDto.deptId),
                StSuper = int.Parse(studentDto.supervisorId)
            };
            _UnitOfWork.StudentRepo.Add(student);
            try
            {
                _UnitOfWork.Save();
            }
            catch (DbUpdateException)
            {
                if (StudentExists(student.StId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStudent", new { id = student.StId }, student);
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _UnitOfWork.StudentRepo.Add(student);
            try
            {
                _UnitOfWork.Save();   
            }
            catch (DbUpdateException)
            {
                if (StudentExists(student.StId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStudent", new { id = student.StId }, student);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.StId)
            {
                return BadRequest();
            }

            _UnitOfWork.StudentRepo.Update(student);    

            try
            {
                _UnitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = _UnitOfWork.StudentRepo.GetById(id);
            if (student == null)
            {
                return NotFound();
            }

            _UnitOfWork.StudentRepo.Delete(id);
            _UnitOfWork.Save();

            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return _UnitOfWork.StudentRepo.GetDbSet().Any(e => e.StId == id);
        }
    }
}
