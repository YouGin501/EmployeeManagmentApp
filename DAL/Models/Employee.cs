using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public string LastName { get; set; }
        [MaxLength(30)]
        public string FirstName { get; set; }
        [MaxLength(30)]
        public string? MiddleName { get; set; }
        [MaxLength(100)]
        public string Address { get; set; }
        [MaxLength(20)]
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
        public string? CompanyInfo { get; set; }

        public int DepartmentId { get; set; }
        public virtual Department? Department { get; set; }
        public int PositionId { get; set; }
        public virtual Position? Position { get; set; }
    }
}
