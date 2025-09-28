using Domain.Entities;
using StudyNotionServer.ServiceLayer.Models;

namespace StudyNotionServer.ServiceLayer.Interfaces
{
    public interface IAuthS
    {
        public Task<Response> RegisterUser(RegisterUserRequest request);

        public Task<LoginUserResponse> LoginUser(LoginUserRequest request);

        public Task<bool> UserExist(LoginUserRequest request);

        public Task<User?> GetUser(LoginUserRequest request);
    }
}
