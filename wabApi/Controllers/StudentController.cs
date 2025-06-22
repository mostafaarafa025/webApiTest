using System.Xml;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wabApi.DTOs.StudentDtos;
using wabApi.Models;
using wabApi.Repositories;
using wabApi.UnitOfWorks;

namespace wabApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        //IGenericRepository<Student> studentRepo;
        //IGenericRepository<Department> deptRepo;
        IMapper map;
        UnitOfWork unitOfWork;
        public StudentController(UnitOfWork _unitOfWork, IMapper _map) 
        {
           this.unitOfWork = _unitOfWork;
            this.map = _map;
        }
        [HttpGet]
        [EndpointSummary("select all Students")]
        public IActionResult displayStudents()
        {
            List<Student> students = unitOfWork.StudentRepo.GetAll();
            List<DisplayStudentDto>dto=map.Map<List<DisplayStudentDto>>(students);
            return Ok(dto);
        }
        [HttpGet]
        [Route("{id:int}")]
        public IActionResult findStudentById(int id)
        {
            Student st=unitOfWork.StudentRepo.getById(id);
            if (st == null)
                return BadRequest("sudent not found");
            DisplayStudentDto dto=map.Map<DisplayStudentDto>(st);
            return Ok(dto);
        }

        [HttpPost]
        public IActionResult AddStudent(AddStudentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid data");
            Department dept = unitOfWork.DepartmentRepo.getById(dto.DeptId);
            if (dept == null)
                return BadRequest("invalid department id");
            Student st = map.Map<Student>(dto);
            unitOfWork.StudentRepo.addItem(st);
            return Ok(dto);
        }
        [HttpPut]
        [Route("{id:int}")]
        public IActionResult UpdateStudent(int id,UpdateStudentDto dto)
        {
            if (dto == null)
                return BadRequest("invalid data");
            if (id != dto.Id)
                return BadRequest("ids don't match");
            Student Oldstudent = unitOfWork.StudentRepo.getById(id);
            if (Oldstudent == null)
                return BadRequest("student not found");
            Department dept =unitOfWork.DepartmentRepo.getById(dto.DeptId);
            if (dept == null)
                return BadRequest("invalid department id");
            map.Map(dto, Oldstudent);
           unitOfWork.StudentRepo.updateItem(Oldstudent);
            return Ok(dto);

        }
        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteStudent(int id)
        {
            Student student = unitOfWork.StudentRepo.getById(id);
            if (student == null) 
                return BadRequest("student not found");
          unitOfWork.StudentRepo.deleteEntity(student);
            return Ok("student Successfully Deleted");
        }


        [HttpGet]
        [Route("{name:alpha}")]
        public IActionResult getStudentByName(string name)
        {
            Student st=unitOfWork.StudentRepo.getByName(name);
            if (st == null)
                return BadRequest("student Not Found");
            DisplayStudentDto dto=map.Map<DisplayStudentDto>(st);
            return Ok(dto);
        }
    }
}
