using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudyNotionServer.Data
{
    public class Profile
    {
        // MongoDB primary key
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public string DateOfBirth { get; set; } = string.Empty;

        public string About { get; set; } = string.Empty;

        public int  phoneNumber { get; set; }
    }
}
