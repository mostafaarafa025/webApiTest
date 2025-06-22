using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wabApi.DTOs.DepartmentDtos;
using wabApi.Models;
using wabApi.Repositories;
using wabApi.UnitOfWorks;

namespace wabApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        //IGenericRepository<Department> deptRepo;
        UnitOfWork unitOfWork;
        IMapper map;
        public DepartmentController(UnitOfWork _unitOfWork,IMapper _map) 
        {
            this.map = _map;
           //this.deptRepo = _deptRepo;
           this.unitOfWork = _unitOfWork;
        }
        [HttpGet]
        [Authorize]
        [EndpointSummary("get All Departments")]
        public IActionResult displayAll()
        {
            List<Department>Deptlist=unitOfWork.DepartmentRepo.GetAll();
            List<DisplayDepartmentDto>dto=map.Map<List<DisplayDepartmentDto>>(Deptlist);
            return Ok(dto);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult getDepartmentById(int id)
        {
            Department dpt = unitOfWork.DepartmentRepo.getById(id);
            if (dpt == null)
                return BadRequest("Department not found");
            DisplayDepartmentDto dto=map.Map<DisplayDepartmentDto>(dpt);
            return Ok(dto);
        }

        [HttpPost]
        public IActionResult postDepartment(AddDepartmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid data");
            if (dto == null)
                return BadRequest("invalid data");
            Department dpt=map.Map<Department>(dto);
            unitOfWork.DepartmentRepo.addItem(dpt);
            return Ok(dpt);
        }
        [HttpPut]
        [Route("{id:int}")]
        public IActionResult UpdateDepartment(UpdateDepartmentDto Newdto,int id)
        {
            if (Newdto == null)
                return BadRequest("invalid data");
            if (id != Newdto.Id)
                return BadRequest("ids don't match");
            Department oldDept = unitOfWork.DepartmentRepo.getById(id);
            if (oldDept == null)
                return BadRequest("department NotFound");
            map.Map(Newdto, oldDept);
            unitOfWork.DepartmentRepo.updateItem(oldDept);
            return Ok(Newdto);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteDepartment(int id)
        {
            Department dpt = unitOfWork.DepartmentRepo.getById(id);
            if (dpt == null)
                return BadRequest("Department NotFound");
           unitOfWork.DepartmentRepo.deleteEntity(dpt);
            return Ok("department deleted successfully");
        }

        [HttpGet]
        [Route("{name:alpha}")]
        public IActionResult GetDepartmentByName(string name)
        {
            Department dpt=unitOfWork.DepartmentRepo.getByName(name);
            if (dpt == null)
                return BadRequest("department not found");
            DisplayDepartmentDto dto = map.Map<DisplayDepartmentDto>(dpt);
            return Ok(dto);
        }
    }
}
