using System.ComponentModel.DataAnnotations;

namespace StudyNotionServer.ServiceLayer.Models
{
    public class LoginUserRequest
    {
        [Required]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+])[A-Za-z\d!@#$%^&*()_+]{8,}$")]
        public string Password { get; set; }
    }

    public class LoginUserResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }
    }
}
