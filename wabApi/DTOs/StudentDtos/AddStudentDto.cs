using System.ComponentModel.DataAnnotations.Schema;

namespace wabApi.DTOs.StudentDtos
{
    public class AddStudentDto
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public int DeptId { get; set; }
    }
}
