using Microsoft.EntityFrameworkCore;

namespace StudyNotionServer.Data
{
    public class StudyNotionDbContext : DbContext
    {
        public DbSet<User> Users { get; init; }
        public DbSet<Profile> Profiles { get; init; }
        public DbSet<Course> Courses { get; init; }
        public DbSet<CourseProgress> CourseProgresses { get; init; }
        public DbSet<Section> Section { get; init; }
        public DbSet<SubSection> SubSection { get; init; }
        public DbSet<RatingAndReview> RatingAndReview { get; init; }
        public DbSet<Tag> Tag { get; init; }
        public DbSet<OTP> OTP { get; init; }


        public StudyNotionDbContext(DbContextOptions<StudyNotionDbContext> options) : base(options) 
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>();
            modelBuilder.Entity<Profile>();
            modelBuilder.Entity<Course>();
            modelBuilder.Entity<CourseProgress>();
            modelBuilder.Entity<Section>();
            modelBuilder.Entity<SubSection>();
            modelBuilder.Entity<RatingAndReview>();
            modelBuilder.Entity<Tag>();
            modelBuilder.Entity<OTP>();
        }
    }
}
