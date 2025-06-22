using System.ComponentModel.DataAnnotations.Schema;

namespace wabApi.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age {  get; set; }

        [ForeignKey("Department")]
        public int DeptId {  get; set; }
        public virtual Department? Department { get; set; }
    }
}
