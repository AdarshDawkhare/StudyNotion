using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudyNotionServer.Data
{
    public class SubSection
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
        public string timeDuration { get; set; } = string.Empty ;
        public string Description { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string SectionId { get; set; } = string.Empty;
        public Section? section { get; set; }
    }
}
