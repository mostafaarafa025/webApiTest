namespace wabApi.DTOs.DepartmentDtos
{
    public class DisplayDepartmentDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentManagerName { get; set; }
        public List<string>studentNames { get; set; }=new List<string>();

    }
}
