using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudyNotionServer.Data
{
    public class User
    {
        // MongoDB primary key
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public accountType AccountType { get; set; }

        [Required]
        public string Image { get; set; } = string.Empty;


        // Reference to Profile
        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProfileId { get; set; } = string.Empty;
        public Profile? Profile { get; set; }


        // Reference to Courses (Foreign Key style)
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> CourseIds { get; set; } = new();
        public List<Course> Courses { get; set; }



        [BsonRepresentation(BsonType.ObjectId)]
        public List<ObjectId> CourseProgressIds { get; set; }
        public List<CourseProgress> CourseProgresses    { get; set; }
    }

    public enum accountType
    {
        Admin,
        Student,
        Instructor
    }
}
