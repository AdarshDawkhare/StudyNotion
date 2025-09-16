using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudyNotionServer.Data
{
    public class Section
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public string SectionName { get; set; } = string.Empty;

        //Reference to subsection
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> SubSectionsIds { get; set; } = new();
        public List<SubSection> SubSections { get; set; }
    }
}
