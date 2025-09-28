using Domain.Entities;
using Domain.Interface;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly StudyNotionDbContext _db;

        public AuthRepository(StudyNotionDbContext db)
        {
            _db = db;
        }
        public async Task<Response> AddUser(User request)
        {
            Response response = new Response
            {
                IsSuccess = false,
                Message = string.Empty,
            };

            if (request != null)
            {
                if (string.IsNullOrEmpty(request.FirstName) || string.IsNullOrEmpty(request.LastName) || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.PasswordHash))
                {
                    response.Message = "missing attribute in request";
                }
                else
                {
                    try
                    {
                        _db.Users.Add(request);
                        await _db.SaveChangesAsync();
                        response.IsSuccess = true;
                        response.Message = "user added";
                    }
                    catch (Exception ex)
                    {
                        response.Message = ex.ToString();
                    }
                }
            }
            else
            {
                response.Message = "missing attribute in request";
            }

            return response;
        }

        public async Task<bool> UserExist(string Email)
        {
            var exists = await _db.Users.AnyAsync(user => user.Email == Email);

            if (exists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<User?> GetUser(string Email)
        {
            User? user = await _db.Users.FirstOrDefaultAsync(u => u.Email == Email);

            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }
    }
}
