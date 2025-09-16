using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudyNotionServer.Data
{
    public class CourseProgress
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CourseId { get; set; } = string.Empty;
        public Course? Course { get; set; }

        //Reference to Subsections
        public List<string> CompletedVideosIds {  get; set; }
        public List<SubSection> SubSections { get; set; }

    }
}
