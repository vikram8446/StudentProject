using System.ComponentModel.DataAnnotations;


namespace StudentManagementSystem.Models
{
    public class StudentInfo
    {
        [Key]
        public int StudentId { get; set; }

        [Required(ErrorMessage ="First Name is Required")]

        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public int Age { get; set; }
        public string ContactNo { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Pincode { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public decimal Amount { get; set; }
        public bool Status { get; set; }
        public string? ImagePath { get; set; }
    }
}
