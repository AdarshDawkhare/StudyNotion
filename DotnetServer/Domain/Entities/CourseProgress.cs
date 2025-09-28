using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities
{
    public class CourseProgress
    {
        [BsonId]   // MongoDB primary key
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }

        // Foreign Key to Course
        [BsonRepresentation(BsonType.ObjectId)]
        public string CourseId { get; set; } = string.Empty;
        public Course? Course { get; set; }


        // Foreign Key to User
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        public User User { get; set; }


        // Completed SubSections
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> CompletedVideosIds {  get; set; }
        public List<SubSection> SubSections { get; set; }

    }
}
