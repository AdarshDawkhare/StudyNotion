using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities
{
    public class RatingAndReview
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public int Rating { get; set; }

        public string Review { get; set; } = string.Empty;  
        
        //Reference to User
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = string.Empty;
        public User? user { get; set; }


        [BsonRepresentation(BsonType.ObjectId)]
        public string CourseId { get; set; } = string.Empty;
        public Course? Course { get; set; }
    }
}
