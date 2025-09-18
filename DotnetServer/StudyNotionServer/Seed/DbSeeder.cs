using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudyNotionServer.Data;

namespace StudyNotionServer.Seed
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StudyNotionDbContext>();

            await dbContext.Database.EnsureCreatedAsync();

            // --------------------------
            // 1. Profiles
            // --------------------------
            if (!await dbContext.Profiles.AnyAsync())
            {
                var profiles = new List<Profile>
                {
                    new Profile { Gender = "Male", DateOfBirth = "1995-01-01", About = "Student interested in backend dev", phoneNumber = 987654321 },
                    new Profile { Gender = "Female", DateOfBirth = "1990-05-05", About = "Senior instructor", phoneNumber = 123456789 },
                    new Profile { Gender = "Male", DateOfBirth = "1998-10-10", About = "Frontend enthusiast", phoneNumber = 555444333 }
                };

                await dbContext.Profiles.AddRangeAsync(profiles);
                await dbContext.SaveChangesAsync();
            }

            var profilesDb = await dbContext.Profiles.ToListAsync();

            // --------------------------
            // 2. Users
            // --------------------------
            if (!await dbContext.Users.AnyAsync())
            {
                var users = new List<User>
                {
                    new User { FirstName = "John", LastName = "Doe", Email = "student1@studynotion.com", PasswordHash = "hashed1", AccountType = accountType.Student, Image = "https://placehold.co/100x100", ProfileId = profilesDb[0].Id },
                    new User { FirstName = "Jane", LastName = "Smith", Email = "instructor1@studynotion.com", PasswordHash = "hashed2", AccountType = accountType.Instructor, Image = "https://placehold.co/100x100", ProfileId = profilesDb[1].Id },
                    new User { FirstName = "Mike", LastName = "Brown", Email = "student2@studynotion.com", PasswordHash = "hashed3", AccountType = accountType.Student, Image = "https://placehold.co/100x100", ProfileId = profilesDb[2].Id }
                };

                await dbContext.Users.AddRangeAsync(users);
                await dbContext.SaveChangesAsync();
            }

            var usersDb = await dbContext.Users.ToListAsync();
            var instructor = usersDb.First(u => u.AccountType == accountType.Instructor);
            var students = usersDb.Where(u => u.AccountType == accountType.Student).ToList();

            // --------------------------
            // 3. Tags
            // --------------------------
            if (!await dbContext.Tag.AnyAsync())
            {
                var tags = new List<Tag>
                {
                    new Tag { Name = "Web Development", Description = "Learn full stack dev" },
                    new Tag { Name = "Data Science", Description = "Machine Learning, AI & Analytics" },
                    new Tag { Name = "Cloud Computing", Description = "AWS, Azure, GCP" }
                };

                await dbContext.Tag.AddRangeAsync(tags);
                await dbContext.SaveChangesAsync();
            }

            var tagsDb = await dbContext.Tag.ToListAsync();

            // --------------------------
            // 4. Sections + SubSections
            // --------------------------
            if (!await dbContext.Section.AnyAsync())
            {
                var introSubsections = new List<SubSection>
                {
                    new SubSection { Title = "Intro to ASP.NET Core", timeDuration = "10 min", Description = "Overview", VideoUrl = "https://video.com/intro" },
                    new SubSection { Title = "Setup Environment", timeDuration = "15 min", Description = "Installing SDK", VideoUrl = "https://video.com/setup" }
                };

                var dsSubsections = new List<SubSection>
                {
                    new SubSection { Title = "Intro to Python", timeDuration = "20 min", Description = "Basics of Python", VideoUrl = "https://video.com/python" },
                    new SubSection { Title = "NumPy & Pandas", timeDuration = "30 min", Description = "Data analysis", VideoUrl = "https://video.com/numpy" }
                };

                var cloudSubsections = new List<SubSection>
                {
                    new SubSection { Title = "Intro to AWS", timeDuration = "25 min", Description = "AWS basics", VideoUrl = "https://video.com/aws" },
                    new SubSection { Title = "Deploy App", timeDuration = "40 min", Description = "Deployment process", VideoUrl = "https://video.com/deploy" }
                };

                var sections = new List<Section>
                {
                    new Section { SectionName = "ASP.NET Core Basics", SubSections = introSubsections },
                    new Section { SectionName = "Data Science Fundamentals", SubSections = dsSubsections },
                    new Section { SectionName = "Cloud Fundamentals", SubSections = cloudSubsections }
                };

                await dbContext.Section.AddRangeAsync(sections);
                await dbContext.SaveChangesAsync();
            }

            var sectionsDb = await dbContext.Section.Include(s => s.SubSections).ToListAsync();

            // --------------------------
            // 5. Courses
            // --------------------------
            if (!await dbContext.Courses.AnyAsync())
            {
                var courses = new List<Course>
                {
                    new Course
                    {
                        Title = "ASP.NET Core Basics",
                        Description = "Learn to build APIs",
                        WhatYouWillLearn = "Controllers, Routing, DI",
                        Price = 49,
                        Thumbnail = "https://placehold.co/600x400",
                        InstructorId = instructor.Id,
                        SectionId = sectionsDb[0].Id,
                        TagId = tagsDb[0].Id,
                        EnrolledStudentsIds = students.Select(s => s.Id).ToList()
                    },
                    new Course
                    {
                        Title = "Data Science with Python",
                        Description = "ML and AI basics",
                        WhatYouWillLearn = "NumPy, Pandas, Scikit-learn",
                        Price = 99,
                        Thumbnail = "https://placehold.co/600x400",
                        InstructorId = instructor.Id,
                        SectionId = sectionsDb[1].Id,
                        TagId = tagsDb[1].Id,
                        EnrolledStudentsIds = new List<string> { students[0].Id }
                    },
                    new Course
                    {
                        Title = "Cloud with AWS",
                        Description = "Deploy apps in cloud",
                        WhatYouWillLearn = "EC2, S3, Lambda",
                        Price = 79,
                        Thumbnail = "https://placehold.co/600x400",
                        InstructorId = instructor.Id,
                        SectionId = sectionsDb[2].Id,
                        TagId = tagsDb[2].Id,
                        EnrolledStudentsIds = new List<string> { students[1].Id }
                    }
                };

                await dbContext.Courses.AddRangeAsync(courses);
                await dbContext.SaveChangesAsync();

                // Update Tags with CourseId
                tagsDb[0].CourseId = courses[0].Id;
                tagsDb[1].CourseId = courses[1].Id;
                tagsDb[2].CourseId = courses[2].Id;
                await dbContext.SaveChangesAsync();
            }

            var coursesDb = await dbContext.Courses.ToListAsync();

            // --------------------------
            // 6. Course Progress
            // --------------------------
            if (!await dbContext.CourseProgresses.AnyAsync())
            {
                var progresses = new List<CourseProgress>
                {
                    new CourseProgress { CourseId = coursesDb[0].Id, CompletedVideosIds = new List<string> { sectionsDb[0].SubSections[0].Id }, SubSections = sectionsDb[0].SubSections },
                    new CourseProgress { CourseId = coursesDb[1].Id, CompletedVideosIds = new List<string> { sectionsDb[1].SubSections[1].Id }, SubSections = sectionsDb[1].SubSections }
                };

                await dbContext.CourseProgresses.AddRangeAsync(progresses);
                await dbContext.SaveChangesAsync();
            }

            // --------------------------
            // 7. Rating & Reviews
            // --------------------------
            if (!await dbContext.RatingAndReview.AnyAsync())
            {
                var reviews = new List<RatingAndReview>
                {
                    new RatingAndReview { Rating = 5, Review = "Excellent course!", UserId = students[0].Id },
                    new RatingAndReview { Rating = 4, Review = "Very useful content", UserId = students[1].Id }
                };

                await dbContext.RatingAndReview.AddRangeAsync(reviews);
                await dbContext.SaveChangesAsync();
            }

            // --------------------------
            // 8. OTPs
            // --------------------------
            if (!await dbContext.OTP.AnyAsync())
            {
                var otps = new List<OTP>
                {
                    new OTP { Email = "student1@studynotion.com", otp = "123456", CreatedAt = DateTime.UtcNow },
                    new OTP { Email = "student2@studynotion.com", otp = "654321", CreatedAt = DateTime.UtcNow }
                };

                await dbContext.OTP.AddRangeAsync(otps);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
