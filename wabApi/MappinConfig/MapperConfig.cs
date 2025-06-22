using AutoMapper;
using wabApi.DTOs.DepartmentDtos;
using wabApi.DTOs.StudentDtos;
using wabApi.Models;

namespace wabApi.MappinConfig
{
    public class MapperConfig:Profile
    {
        public MapperConfig() 
        {
            CreateMap<Department, DisplayDepartmentDto>().AfterMap(
                (src, dest) =>
                {
                    dest.DepartmentId = src.Id;
                    dest.DepartmentName = src.Name;
                    dest.DepartmentManagerName = src.ManagerName;
                    dest.studentNames = src.Students.Select(x => x.Name).ToList();
                }
                ).ReverseMap();
            CreateMap<AddDepartmentDto, Department>().ReverseMap();
            CreateMap<UpdateDepartmentDto, Department>().ReverseMap();

            CreateMap<Student, DisplayStudentDto>().AfterMap(
                (src, dest) =>
                {
                    dest.StudentId = src.Id;
                    dest.StudentName = src.Name;
                    dest.StudentAge = src.Age;
                    dest.DepartmentName=src.Department.Name;
                }
                ).ReverseMap();
                CreateMap<AddStudentDto, Student>().ReverseMap();
                CreateMap<UpdateStudentDto, Student>().ReverseMap();
        }
    }
}
