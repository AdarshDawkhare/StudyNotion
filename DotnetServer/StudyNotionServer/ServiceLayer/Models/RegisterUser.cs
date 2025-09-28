using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace StudyNotionServer.ServiceLayer.Models
{
    public class RegisterUserRequest
    {
        [Required(ErrorMessage ="Firstname must be mentioned")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Lastname must be mentioned")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email must be mentioned")]
        public string Email {  get; set; }

        [Required(ErrorMessage = "Password must be mentioned")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm-Password must be mentioned")]
        public string ConfirmPassword { get; set; }

        public string Image {  get; set; }

        public string? ProfileId { get; set; } = "none";    // optional during registration

        [Required(ErrorMessage = "AccountType must be mentioned")]
        public string AccountType { get; set; }     // "Admin" | "Student" | "Instructor"
    }

    public class RegisterUserResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public User? RegisteredUser { get; set; }
    }
}
