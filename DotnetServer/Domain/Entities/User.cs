using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities
{
    public class User
    {
        // MongoDB primary key
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = ObjectId.GenerateNewId().ToString();

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


        public string? Image { get; set; }


        // Reference to Profile --> user created but profile filled later), consider removing [Required].
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ProfileId { get; set; }
        public Profile? Profile { get; set; }


        // Reference to Courses (Foreign Key style)
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> CourseIds { get; set; } = new List<string>();
        public List<Course> Courses { get; set; } = new List<Course>();



        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> CourseProgressIds { get; set; } = new List<string>();
        public List<CourseProgress> CourseProgresses { get; set; } = new List<CourseProgress>();
    }

    public enum accountType
    {
        Admin,
        Student,
        Instructor
    }
}
