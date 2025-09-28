using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DB
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

            //------ User : Profile( 1 : 1) ------------------------------------------------
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne()
                .HasForeignKey<User>(u => u.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            //------ User : Courses( 1 : N ) ----------- (Instructor --> Courses -------------
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Instructor)
                .WithMany(u => u.Courses)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);


            // User : CourseProgresses (1:N)
            modelBuilder.Entity<CourseProgress>()
                .HasOne<User>()
                .WithMany(u => u.CourseProgresses)
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade);

            // Course : CourseProgress (1:N)
            modelBuilder.Entity<CourseProgress>()
                .HasOne(cp => cp.Course)
                .WithMany()
                .HasForeignKey(cp => cp.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Course : Section (1:1 currently) ---
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Section)
                .WithOne()
                .HasForeignKey<Course>(c => c.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---- Section : SubSections(1: N)
            modelBuilder.Entity<Section>()
                .HasMany(s => s.SubSections)
                .WithOne()
                .HasForeignKey(ss => ss.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Course : RatingAndReview (1:N) ---
            modelBuilder.Entity<Course>()
                 .HasMany(c => c.RatingAndReviews)
                 .WithOne(r => r.Course)
                 .HasForeignKey(r => r.CourseId)
                 .OnDelete(DeleteBehavior.Cascade);

            // --- RatingAndReview : User (N:1) ---
            modelBuilder.Entity<RatingAndReview>()
                .HasOne(r => r.user)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Course : Tag (1:1 as per current model) ---
            modelBuilder.Entity<Course>()
                .HasOne(c => c.tag)
                .WithOne(t => t.course)
                .HasForeignKey<Tag>(t => t.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Course : Students (M:N) ---
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Students)
                .WithMany(); // EF will create a join collection automatically


        }
    }
}
