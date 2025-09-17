using Microsoft.EntityFrameworkCore;
using StudyNotionServer.Data;
using StudyNotionServer.ServiceLayer.Models;

namespace StudyNotionServer.RepositoryLayer
{
    public class StudyNotionRepo : IStudyNotionRepo
    {
        private readonly StudyNotionDbContext _db;

        public StudyNotionRepo(StudyNotionDbContext db)
        {
            _db = db;
        }

        public async Task<RegisterUserResponse> RegisterUser(User request)
        {
            RegisterUserResponse response = new RegisterUserResponse
            {
                Success = false,
                Message = string.Empty,
                RegisteredUser = request
            };

            if(request != null)
            {
                if (string.IsNullOrEmpty(request.FirstName) || string.IsNullOrEmpty(request.LastName) || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.PasswordHash))
                {
                    response.Message = "missing attribute in request";
                }
                else
                {
                    _db.Users.Add(request);
                    await _db.SaveChangesAsync();
                    response.Success = true;
                    response.Message = "user added";
                }
            }
            else
            {
                response.Message = "missing attribute in request";
            }

            return response;
        }

        public async Task<bool> UserExist(LoginUserRequest request)
        {
            var exists = await _db.Users.AnyAsync(user => user.Email == request.Email);

            if (exists)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        public async Task<User?> GetUser(LoginUserRequest request)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email && u.PasswordHash == request.Password);
        }

    }
}
