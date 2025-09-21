using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudyNotionServer.Data
{
    public class Course
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string WhatYouWillLearn { get; set; } = string.Empty;
        public int Price { get; set; }
        public string Thumbnail { get; set; } = string.Empty;

        // Reference to Instructor
        [Required,BsonRepresentation(BsonType.ObjectId)]
        public string InstructorId { get; set; } = string.Empty;
        public User Instructor { get; set; }

        // Reference to section
        [BsonRepresentation(BsonType.ObjectId)]
        public string SectionId { get; set; } = string.Empty ;
        public Section? Section { get; set; }

        //Reference to ratings and reviews
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string>? RatingAndReviewsIds { get; set; } = new();
        public List<RatingAndReview>? RatingAndReviews { get; set; }

        //Reference to tag
        [BsonRepresentation(BsonType.ObjectId)]
        public string TagId { get; set; }
        public Tag? tag { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string>? EnrolledStudentsIds {  get; set; } = new() ;
        public List<User>? Students {  get; set; }
    }
}
