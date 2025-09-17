using StudyNotionServer.Data;
using StudyNotionServer.ServiceLayer.Models;

namespace StudyNotionServer.RepositoryLayer
{
    public interface IStudyNotionRepo
    {
        public Task<RegisterUserResponse> RegisterUser(User request);

        public Task<bool> UserExist(LoginUserRequest request);

        public Task<User?> GetUser(LoginUserRequest request);

    }
}
