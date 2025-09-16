using StudyNotionServer.Data;
using StudyNotionServer.ServiceLayer.Models;

namespace StudyNotionServer.RepositoryLayer
{
    public interface IStudyNotionRepo
    {
        public Task<RegisterUserResponse> RegisterUser(User request);

        public Task<LoginUserResponse> LoginUser(LoginUserRequest request);

        public Task<bool> UserExist(LoginUserRequest request);

    }
}
